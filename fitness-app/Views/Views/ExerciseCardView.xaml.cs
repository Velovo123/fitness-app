using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fitness_app.Models.Supabase;

namespace fitness_app.Views.Views;

public partial class ExerciseCardView : ContentView
{
    public ExerciseCardView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ExerciseProperty =
        BindableProperty.Create(nameof(Exercise), typeof(Exercise), typeof(ExerciseCardView), null);

    public Exercise Exercise
    {
        get => (Exercise)GetValue(ExerciseProperty);
        set => SetValue(ExerciseProperty, value);
    }

    public static readonly BindableProperty ExerciseDurationFormattedProperty =
        BindableProperty.Create(nameof(ExerciseDurationFormatted), typeof(string), typeof(ExerciseCardView), "0 min");

    public string ExerciseDurationFormatted
    {
        get => (string)GetValue(ExerciseDurationFormattedProperty);
        set => SetValue(ExerciseDurationFormattedProperty, value);
    }
}