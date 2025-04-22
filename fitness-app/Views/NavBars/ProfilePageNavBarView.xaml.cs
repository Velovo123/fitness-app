using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fitness_app.Views.NavBars;

public partial class ProfilePageNavBarView : ContentView
{
    public ProfilePageNavBarView()
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