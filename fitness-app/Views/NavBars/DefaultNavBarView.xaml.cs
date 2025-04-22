using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;

namespace fitness_app.Views.NavBars;

public partial class DefaultNavBarView : ContentView
{
    public DefaultNavBarView()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            propertyName: nameof(Title),
            returnType: typeof(string),
            declaringType: typeof(DefaultNavBarView),
            defaultValue: "",
            defaultBindingMode: BindingMode.TwoWay);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}