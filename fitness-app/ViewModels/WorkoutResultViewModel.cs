using System.Collections.ObjectModel;
using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Models.Supabase;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]
public class WorkoutResultViewModel : BaseViewModel, IInitializeAsyncAware
{
    private readonly IWorkoutExerciseService _workoutExerciseService;
    private readonly INavigationService _navigationService;
    public Workout? Workout { get; set; }
    public double WorkoutDuration { get; set; }
    public int CompletedExercises { get; set; }
    public int TotalExercises { get; set; }
    
    public ICommand BackCommand { get; set; }
    public ObservableCollection<Exercise> Exercises { get; set; } = new();

    public WorkoutResultViewModel(
        IWorkoutExerciseService workoutExerciseService, 
        INavigationService navigationService)
    {
        _workoutExerciseService = workoutExerciseService;
        _navigationService = navigationService;
        BackCommand = CreateAsyncCommand(NavigateBackCommand);
    }
    
    public async Task InitializeAsync(INavigationParameters parameters)
    {
        if (parameters.ContainsKey(NavigationParametersConstants.Workout))
            Workout = parameters.GetValue<Workout>(NavigationParametersConstants.Workout);
        
        if (parameters.ContainsKey(NavigationParametersConstants.Exercises))
            Exercises = parameters.GetValue<ObservableCollection<Exercise>>(
                NavigationParametersConstants.Exercises);
        
        if (parameters.ContainsKey(NavigationParametersConstants.CompletedExercises))
            CompletedExercises = parameters.GetValue<int>(
                NavigationParametersConstants.CompletedExercises);
        
        TotalExercises = Exercises?.Count ?? 0;
        
        WorkoutDuration = await GetWorkoutDurationValueAsync();
    }
    
    private async Task NavigateBackCommand()
    {
        var result = await _navigationService.NavigateAsync(
            NavigationUriConstants.MainPageRoute);
        if (!result.Success)
        {
            //log
        }
    }
    
    private async Task<double> GetWorkoutDurationValueAsync()
    {
        if (Workout == null)
        {
            return 0;
        }

        double duration = await 
            _workoutExerciseService.GetWorkoutDurationAsync(Workout.Id!);
        return duration;
    }
    
}