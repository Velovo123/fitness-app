using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace fitness_app.Views.OnboardingWizard.Views;

public partial class StatsInputView : ContentView
{
    public StatsInputView()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty OptionOneTextProperty =
        BindableProperty.Create(nameof(OptionOneText), typeof(string), typeof(StatsInputView), "KG");

    public string OptionOneText
    {
        get => (string)GetValue(OptionOneTextProperty);
        set => SetValue(OptionOneTextProperty, value);
    }

    public static readonly BindableProperty OptionTwoTextProperty =
        BindableProperty.Create(nameof(OptionTwoText), typeof(string), typeof(StatsInputView), "LBS");

    public string OptionTwoText
    {
        get => (string)GetValue(OptionTwoTextProperty);
        set => SetValue(OptionTwoTextProperty, value);
    }
    
    public static readonly BindableProperty SegmentedControlChangedCommandProperty =
        BindableProperty.Create(nameof(SegmentedControlChangedCommand), typeof(ICommand), typeof(StatsInputView), default(ICommand));

    public ICommand SegmentedControlChangedCommand
    {
        get => (ICommand)GetValue(SegmentedControlChangedCommandProperty);
        set => SetValue(SegmentedControlChangedCommandProperty, value);
    }
    
}