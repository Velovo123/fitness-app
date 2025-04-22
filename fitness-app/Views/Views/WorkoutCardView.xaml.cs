using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using fitness_app.Models.Supabase;

namespace fitness_app.Views.Views;

public partial class WorkoutCardView : ContentView
{
    public WorkoutCardView()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty WorkoutProperty =
        BindableProperty.Create(nameof(Workout),
            typeof(Workout),
            typeof(WorkoutCardView),
            null);

    public Workout Workout
    {
        get => (Workout)GetValue(WorkoutProperty);
        set => SetValue(WorkoutProperty, value);
    }
    
    public static readonly BindableProperty CardTappedCommandProperty =
        BindableProperty.Create(nameof(CardTappedCommand),
            typeof(ICommand),
            typeof(WorkoutCardView),
            null);

    public ICommand CardTappedCommand
    {
        get => (ICommand)GetValue(CardTappedCommandProperty);
        set => SetValue(CardTappedCommandProperty, value);
    }
}