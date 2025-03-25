using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace fitness_app.ViewModels.Base;

public abstract class BaseViewModel
{
    protected ICommand CreateCommand(Action execute) => new Command(execute);
    
    protected ICommand CreateCommand<T>(Action<T> execute) => new Command<T>(execute);
    
    protected ICommand CreateAsyncCommand(Func<Task> execute) => new Command(async () => await execute());
    
    protected ICommand CreateAsyncCommand<T>(Func<T, Task> execute) => new Command<T>(async param => await execute(param));
}