using HockeyApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HockeyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScoreBoard : Page
    {
        ScoreboardViewModel viewModel = new ScoreboardViewModel(App.MobileService);

        public ScoreBoard()
        {
            this.InitializeComponent();
            //viewModel.GetAllTimeLimitedGamesAsync();
            this.DataContext = viewModel;
        }

        async protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            await viewModel.GetAllTimeLimitedGamesAsync();
            await viewModel.GetAllScoreLimitedGamesAsync();
        }

        
        private void btn_changeBy_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn?.Name)
            {
                case "btn_ByTime":

                    break;

                case "btn_ByScore":

                    break;
                default:
                    break;
            }
        }

    
    }
}