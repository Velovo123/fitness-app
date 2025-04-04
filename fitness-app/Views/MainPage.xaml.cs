using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
namespace fitness_app.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void MainSearchBar_OnSearchButtonPressed(object? sender, EventArgs e)
    {
        MainSearchBar.Unfocus();
    }
}