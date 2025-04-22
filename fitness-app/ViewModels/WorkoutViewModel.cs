using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using fitness_app.Constants;
using fitness_app.Models.Supabase;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using MPowerKit;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;


namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]
public class WorkoutViewModel : BaseViewModel, IInitializeAsyncAware, INavigationAware
{
    private readonly IWorkoutExerciseService _workoutExerciseService;
    private readonly INavigationService _navigationService;

    public string CurrentVideoTime { get; set; } = 
        TimeFormatConstants.DefaultTime;
    public bool IsPaused { get; set; } 
    
    public int CompletedExercises { get; set; }
    public double Progress { get; set; }
    public ICommand ToggleVideoCommand { get; }
    public ICommand VideoPositionChangedCommand { get; }
    public ICommand VideoEndedCommand { get; }
    public ICommand SkipExerciseCommand { get; }
    
    public ICommand FreeResourcesCommand { get; }

    public Workout? Workout { get; set; }
    public List<string> Playlist { get; set; } = new();
    public int CurrentVideoIndex { get; set; } 
    public string? CurrentVideo { get; set; }

    public ObservableCollection<Exercise> Exercises { get; set; } = new();
    public ObservableCollection<Exercise> NextExercises { get; set; } = new();

    public int CurrentExerciseNumber { get; set; }
    public int TotalExerciseCount { get; set; }

    [DependsOn(nameof(CurrentVideoIndex), nameof(Exercises))]
    public string CurrentExerciseName =>
        Exercises?.ElementAtOrDefault(CurrentVideoIndex)?.ExerciseName ?? string.Empty;

    public WorkoutViewModel(IWorkoutExerciseService workoutExerciseService, 
        INavigationService navigationService)
    {
        _workoutExerciseService = workoutExerciseService;
        _navigationService = navigationService;

        ToggleVideoCommand = CreateCommand<MediaElement>(ToggleVideo);
        VideoPositionChangedCommand = CreateCommand<MediaElement>(OnVideoPositionChanged);
        VideoEndedCommand = CreateAsyncCommand<MediaElement>(OnVideoEnded);
        SkipExerciseCommand = CreateAsyncCommand<MediaElement>(SkipExercise);
        FreeResourcesCommand = CreateCommand<MediaElement>(FreeResources);
    }

    public async Task InitializeAsync(INavigationParameters parameters)
    {
        if (parameters.ContainsKey(NavigationParametersConstants.Workout))
            Workout = parameters.GetValue<Workout>(NavigationParametersConstants.Workout);

        if (Workout != null && !string.IsNullOrEmpty(Workout.Id))
            await LoadExercisesAsync();

        SetupPlaylist();

        TotalExerciseCount = Exercises.Count;
        CompletedExercises = 0;

        if (Playlist.Any())
            InitializeVideoPlayback();

        UpdateNextExercises();
    }

    private void FreeResources(MediaElement mediaElement)
    {
        mediaElement.Handler?.DisconnectHandler();
    }

    private async Task LoadExercisesAsync()
    {
        if (Workout?.Id != null)
        {
            var exercisesList = await _workoutExerciseService.GetExercisesForWorkoutAsync(
                Workout.Id);
            Exercises = new ObservableCollection<Exercise>(exercisesList);
        }
    }

    private void SetupPlaylist()
    {
        Playlist.Clear();

        foreach (var exercise in Exercises)
            if (!string.IsNullOrEmpty(exercise.Video))
                Playlist.Add(exercise.Video);
    }

    private void InitializeVideoPlayback()
    {
        CurrentVideoIndex = 0;
        CurrentVideo = Playlist[CurrentVideoIndex];
        CurrentExerciseNumber = CurrentVideoIndex + 1;
    }

    private async Task OnVideoEnded(MediaElement mediaElement)
    {
        if (Playlist.Count == 0)
            return;

        CompletedExercises++;
        
        var navParams = new NavigationParameters
        {
            { NavigationParametersConstants.Workout, Workout! },
            { NavigationParametersConstants.Exercises, Exercises },
            { NavigationParametersConstants.CompletedExercises, CompletedExercises }
        };

        if (CurrentVideoIndex < Playlist.Count - 1) 
            MoveToNextVideo(mediaElement);
        else
        {
            var result = await _navigationService.NavigateAsync(
                nameof(WorkoutResultPage), 
                navParams);
            if (!result.Success)
            {
                //log
            }
        }
            
        
    }

    private void MoveToNextVideo(MediaElement mediaElement)
    {
        CurrentVideoIndex++;
        CurrentVideo = Playlist[CurrentVideoIndex];
        CurrentExerciseNumber = CurrentVideoIndex + 1;

        mediaElement.Source = CurrentVideo;
        mediaElement.Play();

        UpdateNextExercises();
    }

    private void OnVideoPositionChanged(MediaElement mediaElement)
    {
        var current = mediaElement.Position;
        CurrentVideoTime = current.ToString(TimeFormatConstants.TimeFormat);

        if (mediaElement.Duration.TotalSeconds > 0)
            Progress = current.TotalSeconds / mediaElement.Duration.TotalSeconds;
    }

    private void ToggleVideo(MediaElement mediaElement)
    {
        if (mediaElement == null)
            return;

        if (mediaElement.CurrentState == MediaElementState.Playing)
        {
            mediaElement.Pause();
            IsPaused = true;
        }
        else
        {
            mediaElement.Play();
            IsPaused = false;
        }
    }
    
    private async Task SkipExercise(MediaElement mediaElement)
    {
        if (Playlist.Count == 0)
            return;

        if (CurrentVideoIndex < Playlist.Count - 1)
        {
            MoveToNextVideo(mediaElement);
        }
        else
        {
            var navParams = new NavigationParameters
            {
                { NavigationParametersConstants.Workout, Workout! },
                { NavigationParametersConstants.Exercises, Exercises },
                { NavigationParametersConstants.CompletedExercises, CompletedExercises }
            };
            var result = await _navigationService.NavigateAsync(
                nameof(WorkoutResultPage), 
                navParams);
            if (!result.Success)
            {
                // Log or handle navigation error.
            }
        }
    }

    private void UpdateNextExercises()
    {
        if (Exercises != null && Exercises.Any())
            NextExercises = new ObservableCollection<Exercise>(
                Exercises.Skip(CurrentVideoIndex + 1));
    }

    public async void OnNavigatedTo(INavigationParameters navigationParameters)
    {
        if (navigationParameters.ContainsKey(NavigationParametersConstants.Workout))
            ;
        else
        {
            var result = await _navigationService.NavigateAsync(
                NavigationUriConstants.MainPageRoute);
            if (!result.Success)
            {
                //log
            }
        }
    }

    public void OnNavigatedFrom(INavigationParameters navigationParameters)
    {
        // not needed
        Console.WriteLine(123);
    }
}