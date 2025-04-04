using System.Threading.Tasks;

namespace fitness_app.Services;

public interface IDialogService
{
    Task ShowMessageAsync(string title, string message);
}