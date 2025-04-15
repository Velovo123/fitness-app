using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Models;
using fitness_app.ViewModels.Base;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]
public class EditViewModel : BaseViewModel, IInitializeAsyncAware
{
    public User? User { get; set; }
    public ICommand HandleEditImageCommand { get; set; }

    public EditViewModel()
    {
        HandleEditImageCommand = CreateAsyncCommand(HandleEditImage);
    }
    
    public Task InitializeAsync(INavigationParameters parameters)
    {
        if (parameters.ContainsKey(NavigationParametersConstants.User))
        {
            User = parameters.GetValue<User>(NavigationParametersConstants.User);
        }
        
        return Task.CompletedTask;
    }

    public Task HandleEditImage()
    {
        return Task.CompletedTask;
    }
}