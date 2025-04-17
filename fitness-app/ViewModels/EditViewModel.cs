using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Extensions;
using fitness_app.Models;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels;

//TODO if changes are not saved - they should be discarded

[AddINotifyPropertyChangedInterface]
public class EditViewModel : BaseViewModel, IInitializeAsyncAware
{
    private readonly ISupabaseService _supabaseService;
    private readonly IUserFitnessDataService _userFitnessDataService;
    private readonly IAuthService _authService;
    private User? _originalUser;
    public User? EditableUser { get; set; }
    public ICommand HandleEditImageCommand { get; set; }
    public ICommand SaveUserCommand { get; set; }

    public EditViewModel(
        ISupabaseService supabaseService,
        IAuthService authService,
        IUserFitnessDataService userFitnessDataService)
    {
        _supabaseService = supabaseService;
        _userFitnessDataService = userFitnessDataService;
        _authService = authService;
        
        HandleEditImageCommand = CreateAsyncCommand(HandleEditImage);
        SaveUserCommand = CreateAsyncCommand(SaveUser);
    }

    private async Task SaveUser()
    {
        _originalUser!.CopyFrom(EditableUser!);
        
        var session = _authService.CurrentSession;
        await _userFitnessDataService.UpdateUserFitnessDataAsync(session!, _originalUser!);
        await _supabaseService.UpdateUserAvatarMetadataAsync(session!, _originalUser!.Photo!);
    }
    
    public Task InitializeAsync(INavigationParameters parameters)
    {
        if (parameters.ContainsKey(NavigationParametersConstants.User))
        {
            _originalUser = parameters.GetValue<User>(NavigationParametersConstants.User);
        }
        
        EditableUser = _originalUser!.DeepClone();
        
        return Task.CompletedTask;
    }

    public async Task HandleEditImage()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult? photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo != null)
            {
                var url = await _supabaseService.UploadFileToBucket(
                    "avatars",
                    photo.FullPath,
                    photo.FileName);

                if (url != null)
                {
                    EditableUser!.Photo = url;
                    var session = _authService.CurrentSession;
                }
            }
        }
    }
}