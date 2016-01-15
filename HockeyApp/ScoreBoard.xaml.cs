using HockeyApp.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public enum ButtonState
        {
            BY_TIME,
            BY_SCORE,
            NONE
        }
        private ButtonState BtnState { get; set; }
        private bool ByTimeIsReady = false;
        private bool ByScoreIsReady = false;

        public ScoreBoard()
        {
            BtnState = ButtonState.NONE;
            viewModel.PropertyChanged += ShowTable;
            this.InitializeComponent();
            //viewModel.GetAllTimeLimitedGamesAsync();
            this.DataContext = viewModel;
        }

        /*async protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            //await viewModel.GetAllTimeLimitedGamesAsync();
            //await viewModel.GetAllScoreLimitedGamesAsync();
        }*/

        private void btn_changeBy_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn?.Name)
            {
                case "btn_ByTime":
                    BtnState = ButtonState.BY_TIME;
                    viewModel.GetAllTimeLimitedGamesAsync();
                    break;

                case "btn_ByScore":
                    BtnState = ButtonState.BY_SCORE;
                    viewModel.GetAllScoreLimitedGamesAsync();
                    break;
                default:
                    break;
            }
        }

        private void ShowTable(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ScoreLimitedGames":
                    ByScoreIsReady = true;
                    if (BtnState == ButtonState.BY_SCORE)
                    {
                        ByScoreListView.Visibility = Visibility.Visible;
                        ByTimeListView.Visibility = Visibility.Collapsed;
                    }
                    break;
                case "TimeLimitedGames":
                    ByTimeIsReady = true;
                    if (BtnState == ButtonState.BY_TIME)
                    {
                        ByTimeListView.Visibility = Visibility.Visible;
                        ByScoreListView.Visibility = Visibility.Collapsed;
                    }
                    break;
                default:
                    break;
            }
        }

    
    }
}