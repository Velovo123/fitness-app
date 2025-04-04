using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using fitness_app.Services;

namespace fitness_app.Views;

public partial class MainFlyoutPage 
{
    public MainFlyoutPage(IFlyoutService flyoutService)
    {
        flyoutService.SetFlyout(this);
        InitializeComponent();
    }
    
}