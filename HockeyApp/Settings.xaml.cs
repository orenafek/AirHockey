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
    public sealed partial class Settings : Page
    {
        public Settings()
        {

            this.InitializeComponent();
        }

        private void btn_BTConnect_Click(object sender, RoutedEventArgs e)
        {
            //if (Bluetooth.isConnected) {
            //    Bluetooth.Disconnect("Disconnected");
            //    btn_BTConnect.Content = "Connect";
            //}

            //if(!Bluetooth.isConnected) {
            //    Bluetooth.ConnectToBoard();
            //    btn_BTConnect.Content = "Disconnect";
            //}
        }

        private void btn_shutMusicDown_Click(object sender, RoutedEventArgs e)
        {
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            vc = e.Parameter as VolumeControl;
        }
    }
}
