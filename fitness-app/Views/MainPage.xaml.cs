using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;

namespace fitness_app.Views
{
    public partial class MainPage 
    {
        private const double InitialHeaderHeight = 270;
        private const double FinalHeaderHeight = 110;

        public MainPage()
        {
            InitializeComponent();
            StickyHeader.HeightRequest = InitialHeaderHeight;
        }

        private void MainSearchBar_OnSearchButtonPressed(object? sender, EventArgs e)
        {
            MainSearchBar.Unfocus();
        }

        private async void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
        {
            double newHeight = InitialHeaderHeight;

            if (e.ScrollY < 0)
            {
                newHeight = InitialHeaderHeight - e.ScrollY;
            }
            else
            {
                double collapseThreshold = InitialHeaderHeight;
                if (e.ScrollY <= collapseThreshold)
                {
                    double ratio = e.ScrollY / collapseThreshold;
                    newHeight = InitialHeaderHeight - ((InitialHeaderHeight - FinalHeaderHeight) * ratio);
                }
                else
                {
                    newHeight = FinalHeaderHeight;
                }
            }

            await UpdateHeaderHeight(newHeight);

            bool isCollapsed = newHeight <= FinalHeaderHeight + 20;
            
        }

        private async Task UpdateHeaderHeight(double newHeight)
        {
            await StickyHeader.LayoutTo(new Rect(StickyHeader.X, StickyHeader.Y, StickyHeader.Width, newHeight), 50, Easing.Linear);
            StickyHeader.HeightRequest = newHeight;
        }
    }
}