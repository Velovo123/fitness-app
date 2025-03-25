using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace fitness_app.Services;

public class DialogService : IDialogService
{
    public async Task ShowMessageAsync(string title, string message)
    {
        await Application.Current!.MainPage!.DisplayAlert(title, message, "OK");
    }
}