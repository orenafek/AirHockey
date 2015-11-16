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
using AirHockeyApp.ViewModel;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AirHockeyApp
{ 
    public sealed partial class ScoreBoard : Page
    {
        ScoreBoardViewModel viewModel = new ScoreBoardViewModel(App.MobileService);
        public ScoreBoard()
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
