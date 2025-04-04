using System.Windows.Input;
using fitness_app.Controls;
using fitness_app.Helpers;
using fitness_app.Resources.Localization;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.Views;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;
using Supabase.Gotrue;

namespace fitness_app.ViewModels.Base
{
    [AddINotifyPropertyChangedInterface]
    public class OnboardingWizardBaseViewModel : BaseViewModel, IInitializeAsyncAware
    {
        protected readonly INavigationService _navigationService;
        protected readonly IUserFitnessDataService _userFitnessDataService;
        protected readonly IDialogService _dialogService;

        public string ButtonText { get; set; } = AppResources.NextStepText;
        protected Session Session { get; set; } = null!;
        protected List<string> RemainingPages { get; set; } = new();
        
        public int SelectedIndex { get; set; } = 0; // to base
        public string SelectedUnit { get; set; } = string.Empty; // to base
    
        public string CurrentStat { get; set; } = string.Empty; //to base 
        public ICommand FocusEntryCommand { get; } // to base

        public int TotalSteps { get; set; }
        public int CurrentStep { get; set; }

        public ICommand SkipCommand { get; set; }

        public OnboardingWizardBaseViewModel(INavigationService navigationService,IDialogService dialogService, IUserFitnessDataService userFitnessDataService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _userFitnessDataService = userFitnessDataService;
            SkipCommand = CreateAsyncCommand(OnSkipAsync);
            FocusEntryCommand = CreateCommand<BorderlessEntry>(FocusEntry);
        }

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            var onboardingParams = OnboardingNavigationParameters.From(parameters);
            Session = onboardingParams.Session;
            RemainingPages = onboardingParams.RemainingPages;

            if (OnboardingNavigationParameters.InitalPageCount == 0)
            {
                OnboardingNavigationParameters.InitalPageCount = RemainingPages.Count + 1;
            }

            TotalSteps = OnboardingNavigationParameters.InitalPageCount;
            CurrentStep = TotalSteps - RemainingPages.Count;
            
            if (CurrentStep == TotalSteps)
            {
                ButtonText = AppResources.FinishStepText;
            }
            else
            {
                ButtonText = AppResources.NextStepText;
            }

            await Task.CompletedTask;
        }
        
        private void FocusEntry(BorderlessEntry entry)
        {
            entry.Focus();
        }

        private async Task OnSkipAsync()
        {
            var newRemaining = RemainingPages.Skip(1).ToList();
            var navParams = new OnboardingNavigationParameters
            {
                Session = Session,
                RemainingPages = newRemaining
            };

            if (RemainingPages.Any())
            {
                var nextPage = RemainingPages.First();
                var result = await _navigationService.NavigateAsync(nextPage, navParams.ToNavigationParameters(), animated: false);
                if (!result.Success)
                {
                    // Log
                }
            }
            else
            {
                await _navigationService.NavigateAsync($"TabbedPage?createTab={nameof(MainFlyoutPage)}");
            }
        }
        
        protected async Task NavigateToNextPageAsync(bool animated)
        {
            var newRemaining = RemainingPages.Skip(1).ToList();
            var navParams = new OnboardingNavigationParameters
            {
                Session = Session,
                RemainingPages = newRemaining
            };
        
            if (RemainingPages.Any())
            {
                var nextPage = RemainingPages.First();
                var result = await _navigationService.NavigateAsync(nextPage, navParams.ToNavigationParameters(), animated: animated);
                if (!result.Success)
                {
                    // log
                }
            }
            else
            {
                var user = _userFitnessDataService.GetUserFromSessionAsync(Session);
                await _navigationService.NavigateAsync(nameof(MainFlyoutPage));
            }
        }

    }
}
