using System.Windows.Input;
using fitness_app.Controls;
using fitness_app.Helpers;
using fitness_app.Resources.Localization;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.Views;
using MPowerKit;
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
            SkipCommand = CreateAsyncCommand<bool>(NavigateToNextPageAsync);
            FocusEntryCommand = CreateCommand<BorderlessEntry>(FocusEntry);
        }

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            var onboardingParams = OnboardingNavigationParameters.From(parameters);
            Session = onboardingParams.Session;
            RemainingPages = onboardingParams.RemainingPages;

            SetInitialPageCount();
            UpdateStepsAndButtonText();

            await Task.CompletedTask;
        }
        
        private void SetInitialPageCount()
        {
            if (OnboardingNavigationParameters.InitalPageCount == 0)
            {
                OnboardingNavigationParameters.InitalPageCount = RemainingPages.Count + 1;
            }
            TotalSteps = OnboardingNavigationParameters.InitalPageCount;
        }

        private void UpdateStepsAndButtonText()
        {
            CurrentStep = TotalSteps - RemainingPages.Count;
            ButtonText = (CurrentStep == TotalSteps)
                ? AppResources.FinishStepText
                : AppResources.NextStepText;
        }
        
        private void FocusEntry(BorderlessEntry entry)
        {
            entry.Focus();
        }
        
        protected async Task NavigateToNextPageAsync(bool animated = false)
        {
            var newRemaining = RemainingPages.Skip(1).ToList();
            var navParams = new OnboardingNavigationParameters
            {
                Session = Session,
                RemainingPages = newRemaining
            };
        
            if (RemainingPages.Any())
            {
                await NavigateToNextOnboardingPageAsync(navParams);
            }
            else
            {
                await NavigateToMainPageAsync(Session);
            }
        }
        
        private async Task NavigateToNextOnboardingPageAsync(OnboardingNavigationParameters navParams)
        {
            var nextPage = RemainingPages.First();
            var result = await _navigationService.NavigateAsync(nextPage, navParams.ToNavigationParameters(), animated: false);
            if (!result.Success)
            {
                // Log
            }
        }
        
        private async Task NavigateToMainPageAsync(Session session)
        {
            var user = await _userFitnessDataService.GetUserFromSessionAsync(session);
            if (user == null)
            {
                return;
            }

            var result = await _navigationService.NavigateAsync(
                $"/{nameof(MainFlyoutPage)}/{nameof(MainPage)}",
                new NavigationParameters { { "user", user } });

            if (!result.Success)
            {
                // Log 
            }
        }

    }
}
