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
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HockeyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Game : Page
    {
        private const int SCORE_LIMIT = 5;
        private enum Command
        {
            START = 'S',
            TERMINATE = 'T',
            GOAL_ROBOT = 'R',
            GOAL_PLAYER = 'P',
            EMPTY = 'E'
        }

        private Command command = Command.EMPTY;

        private Session Params { get; set; }
        private bool Countdown { get { return Params.ByTime; } }
        
        public Game()
        {
            this.InitializeComponent();
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
            if(command != Command.EMPTY)
            {
                switch (command)
                {
                    case Command.START: //souldn't get here
                        break;
                    case Command.TERMINATE:
                        stopGame();
                        break;
                    case Command.GOAL_ROBOT:
                        UpdateScore(false);           
                        break;
                    case Command.GOAL_PLAYER:
                        UpdateScore(true);
                        break;
                    case Command.EMPTY: //souldn't get here
                        break;
                }

                command = Command.EMPTY;
            }
            if (CountDownTimeSpan.Milliseconds == 0)
            {
                if (Countdown) // by time : 
                {
                    CountDownTimeSpan -= TimeSpan.FromSeconds(1);
                }

                if (!Countdown) // by score : 
                {
                    CountDownTimeSpan += TimeSpan.FromSeconds(1);
                }

                tb_Timer.Text = showTime();
                if (Countdown && CountDownTimeSpan == TimeSpan.FromSeconds(0))
                {
                    stopGame();
                }
            }
        }

        private void UpdateScore(bool UserScored)
        {
            int userScore = int.Parse(UserScore.Text);
            int robotScore = int.Parse(RobotScore.Text);

            if (UserScored)
            {
                UserScore.Text = (userScore + 1).ToString();
            }

            if (!UserScored)
            {
                RobotScore.Text = (robotScore + 1).ToString();
            }

            if(Params.ByScore && (userScore == SCORE_LIMIT || robotScore == SCORE_LIMIT))
            {
                stopGame();
            }
        }

        private void Popup_OK_Invoked(IUICommand command)
        {
            //TODO: Insert new score to the DB
            Frame.Navigate(typeof(ScoreBoard), null);
        }
        private void stopGame()
        {
            Timer.Stop();
            MessageDialog msgDialog = new MessageDialog("Game is Over !");
            UICommand OK = new UICommand("OK");
            OK.Invoked += Popup_OK_Invoked;
            Utils.Show(msgDialog, new List<UICommand> { OK });
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Params = e.Parameter as Session;
            if (!Bluetooth.isConnected)
            {
                Bluetooth.ConnectToBoard();
            }

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(10);
            Timer.Tick += DispatcherTimer_Tick;
            CountDownTimeSpan = TimeSpan.FromSeconds(20);
            tb_Timer.Text = showTime();
            Timer.Start();
           

        }

        private async void  ReceiveStringLoop()
        {
            DataReader chatReader = Bluetooth.reader;
            try
            {
                uint size = await chatReader.LoadAsync(sizeof(uint));
                if (size < sizeof(uint))
                {
                    Bluetooth.Disconnect("Remote device terminated connection");
                    return;
                }

                uint stringLength = chatReader.ReadUInt32();
                uint actualStringLength = await chatReader.LoadAsync(stringLength);
                if (actualStringLength != stringLength)
                {
                    // The underlying socket was closed before we were able to read the whole data
                    return;
                }

                command = (Command)(char.Parse(chatReader.ReadString(stringLength)));

                ReceiveStringLoop();
            }
            catch (Exception ex)
            {
                if (!Bluetooth.isSocketOpen)
                {
                    // Do not print anything here -  the user closed the socket.
                    // HResult = 0x80072745 - catch this (remote device disconnect) ex = {"An established connection was aborted by the software in your host machine. (Exception from HRESULT: 0x80072745)"}
                }
                else
                {
                    Bluetooth.Disconnect("Read stream failed with error: " + ex.Message);
                }
            }
        }
    }
}