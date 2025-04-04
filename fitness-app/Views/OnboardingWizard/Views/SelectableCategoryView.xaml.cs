using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace fitness_app.Views.OnboardingWizard.Views;

public partial class SelectableCategoryView : ContentView
{
    public SelectableCategoryView()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(
            nameof(ImageSource),
            typeof(ImageSource),
            typeof(SelectableCategoryView),
            default(ImageSource));

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(SelectableCategoryView),
            default(string));
    
    public static readonly BindableProperty CategoryCommandProperty =
        BindableProperty.Create(nameof(CategoryCommand), typeof(ICommand), typeof(SelectableCategoryView), null);
    
    public static readonly BindableProperty HasShadowProperty =
        BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(SelectableCategoryView), false);
    
    
    
    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public ICommand CategoryCommand
    {
        get => (ICommand)GetValue(CategoryCommandProperty);
        set => SetValue(CategoryCommandProperty, value);
    }
    
    public bool HasShadow
    {
        get => (bool)GetValue(HasShadowProperty);
        set => SetValue(HasShadowProperty, value);
    }
}