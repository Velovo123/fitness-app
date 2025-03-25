using fitness_app.Constants;
using fitness_app.Models.Supabase;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;
using Supabase.Gotrue;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]
public class SelectFavoriteViewModel : BaseViewModel, IInitializeAsyncAware
{
    private readonly IUserFitnessDataService _userFitnessDataService;
    //private Session? _session;

    public SelectFavoriteViewModel(IUserFitnessDataService userFitnessDataService)
    {
        _userFitnessDataService = userFitnessDataService;
        
    }

    public async Task InitializeAsync(INavigationParameters parameters)
    {
        //_session = parameters.GetValue<Session>(NavigationParametersConstants.Session);

       // var result = await _userFitnessDataService.UpdateUserFitnessDataAsync(_session, new UserFitnessData { DesiredWeight = 124 });
    }
}