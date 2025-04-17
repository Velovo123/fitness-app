using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Models;
using fitness_app.Models.Supabase;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using MPowerKit;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : BaseViewModel, IInitializeAsyncAware
    {
        private readonly IFlyoutService _flyoutService;
        private readonly IWorkoutService _workoutService;
        private readonly INavigationService _navigationService;
        private readonly IAuthService _authService;
        private readonly IUserFitnessDataService _userFitnessDataService;

        public ObservableCollection<ToggleButtonItem> Items { get; }
        public ObservableCollection<CategoryItem> CategoryItems { get; }
        public ObservableCollection<Workout> Workouts { get; set; } = new ObservableCollection<Workout>();
        public User? User { get; set; }
        public ICommand OpenFlyoutCommand { get; set; }
        public ICommand CloseFlyoutCommand { get; set; }
        public ICommand ItemTappedCommand { get; }
        public ICommand CategoryCommand { get; set; }
        public ICommand WorkoutTappedCommand { get; }
        
        public ICommand NavigateToProfilePageCommand { get; }

        public MainViewModel(
            IFlyoutService flyoutService, 
            IWorkoutService workoutService, 
            INavigationService navigationService,
            IAuthService authService,
            IUserFitnessDataService userFitnessDataService)
        {
            _flyoutService = flyoutService;
            _workoutService = workoutService;
            _navigationService = navigationService;
            _authService = authService;
            _userFitnessDataService = userFitnessDataService;
            
            OpenFlyoutCommand = CreateCommand(OpenFlyout);
            CloseFlyoutCommand = CreateCommand(CloseFlyout);
            CategoryCommand = CreateCommand<CategoryItem>(ChangeCategoryAsync);
            ItemTappedCommand = new Command<ToggleButtonItem>(OnItemTapped);
            NavigateToProfilePageCommand = CreateAsyncCommand(NavigateToProfilePageAsync);

            Items = InitializeToggleButtonItems();
            CategoryItems = InitializeCategoryItems();

            WorkoutTappedCommand = CreateAsyncCommand<Workout>(OnWorkoutTappedAsync);
        }

        private ObservableCollection<ToggleButtonItem> InitializeToggleButtonItems()
        {
            return new ObservableCollection<ToggleButtonItem>
            {
                new ToggleButtonItem { Text = "Lose Weight" },
                new ToggleButtonItem { Text = "Gain Weight" },
                new ToggleButtonItem { Text = "Body Building" },
                new ToggleButtonItem { Text = "Healthy Lifestyle" }
            };
        }

        private ObservableCollection<CategoryItem> InitializeCategoryItems()
        {
            return new ObservableCollection<CategoryItem>
            {
                new CategoryItem { Title="Yoga", 
                    ImageSource=ImageNameConstants.YogaCategory, IsSelected=false },
                new CategoryItem { Title="Gym", 
                    ImageSource=ImageNameConstants.GymCategory, IsSelected=false },
                new CategoryItem { Title="Cardio", 
                    ImageSource=ImageNameConstants.CardioCategory, IsSelected=false },
                new CategoryItem { Title="Stretch", 
                    ImageSource=ImageNameConstants.StretchCategory, IsSelected=false },
                new CategoryItem { Title="Full Body",
                    ImageSource=ImageNameConstants.FullbodyCategory, IsSelected=false },
            };
        }

        private async Task NavigateToProfilePageAsync()
        {
            var navParams = new NavigationParameters
            {
                { NavigationParametersConstants.User, User! }
            };
            await _navigationService.NavigateThrougFlyoutPageAsync(
                $"{nameof(NavigationPage)}/{nameof(ProfilePage)}",
                navParams);
            
            CloseFlyout();
        }

        private async Task LoadWorkoutsAsync()
        {
            var workoutsList = await _workoutService.GetAllWorkoutsAsync();
            
            Workouts.Clear();
            foreach (var workout in workoutsList.Take(2))
            {
                Workouts.Add(workout);
            }
        }

        private void OpenFlyout()
        {
            _flyoutService.OpenFlyout();
        }
        
        private void CloseFlyout()
        {
            _flyoutService.CloseFlyout();
        }
        
        private void OnItemTapped(ToggleButtonItem tappedItem)
        {
            if (tappedItem.IsSelected)
            {
                tappedItem.IsSelected = false;
            }
            else
            {
                foreach (var item in Items)
                {
                    item.IsSelected = false;
                }
                tappedItem.IsSelected = true;
            }
        }
        
        private void ChangeCategoryAsync(CategoryItem category)
        {
            foreach (var item in CategoryItems)
                item.IsSelected = false;
            if (category != null)
                category.IsSelected = true;
        }

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(NavigationParametersConstants.User))
            {
                User = parameters.GetValue<User>(NavigationParametersConstants.User);
            }
            else
            {
                await _authService.RefreshSessionAsync();
                var user = await _userFitnessDataService.GetUserFromSessionAsync(
                    _authService.CurrentSession!);
                if(user != null)
                    User = user;
                else
                {
                    //log that cant retrieve user from session
                }
            }

            var workoutsList = await _workoutService.GetAllWorkoutsAsync();
            Workouts = new ObservableCollection<Workout>(workoutsList.Take(2));
        }

        private async Task OnWorkoutTappedAsync(Workout selectedWorkout)
        {
            if (selectedWorkout == null)
                return;

            var navParams = new NavigationParameters
            {
                { NavigationParametersConstants.Workout, selectedWorkout }
            };
            
            var result = await _navigationService.NavigateAsync(
                $"/{nameof(NavigationPage)}/{nameof(WorkoutPage)}", 
                navParams);

            if (!result.Success)
            {
                // Log or handle result.Exception
            }
        }
    }
}
