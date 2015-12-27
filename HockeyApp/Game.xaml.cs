using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Popups;
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
    public sealed partial class Game : Page
    {
        
        public Game()
        {
            this.InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(1000);
            Timer.Tick += DispatcherTimer_Tick;
            CountDownTimeSpan = TimeSpan.FromSeconds(20);
            tb_Timer.Text = showTime();
            Timer.Start();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            //EndTime = (EndTime == DateTime.MinValue) ? DateTime.Now + (TimeSpan)
        }

        private string showTime()
        {
            return CountDownTimeSpan.ToString().Substring(3); // format is MM:SS
        }

        private void stopTimer()
        {
            if (Timer.IsEnabled && CountDownTimeSpan != TimeSpan.Zero) {Timer.Stop();}
        }

        private void resumeTimer()
        {
            if(!Timer.IsEnabled && CountDownTimeSpan != TimeSpan.Zero) { Timer.Start();}
        }

        private void DispatcherTimer_Tick(object sender, object eo)
        {
            CountDownTimeSpan -= TimeSpan.FromSeconds(1);
            tb_Timer.Text = showTime();
            if (CountDownTimeSpan == TimeSpan.FromSeconds(0))
            {
                //tb_Timer.Foreground = new SolidColorBrush(Color.FromArgb(255,255,0,0));
                Timer.Stop();
                MessageDialog msgDialog = new MessageDialog("Game is Over !");
                UICommand OK = new UICommand("OK");
                OK.Invoked += Popup_OK_Invoked;
                Utils.Show(msgDialog,new List<UICommand> {OK});
                
            }

        }

        private void Popup_OK_Invoked(IUICommand command)
        {
            //TODO: Insert new score to the DB
            Frame.Navigate(typeof(ScoreBoard), null);
        }

        private TimeSpan CountDownTimeSpan { get; set; }
        public DateTime EndTime { get; set; }
        public DispatcherTimer Timer { get; set; }

        private void PauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Timer.IsEnabled)
            {
                stopTimer();
            }

            else
            {
                resumeTimer();
            }
        }
    }
}