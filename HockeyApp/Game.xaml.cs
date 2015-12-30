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
        private TimeSpan second = TimeSpan.Zero;
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
            Bluetooth.ConnectToBoard();
            //EndTime = (EndTime == DateTime.MinValue) ? DateTime.Now + (TimeSpan)
        }

        private string showTime()
        {
            return TimeSpaner.ToString().Substring(3); // format is MM:SS
        }

        private void stopTimer()
        {
            if (Timer.IsEnabled && TimeSpaner != TimeSpan.Zero) {Timer.Stop();}
        }

        private void resumeTimer()
        {
            if(!Timer.IsEnabled && TimeSpaner != TimeSpan.Zero) { Timer.Start();}
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
                        stopGame(false);
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
            second += TimeSpan.FromMilliseconds(10);
            if(second < TimeSpan.FromMilliseconds(400))
            {
                return;
            }

            second = TimeSpan.Zero;

            if (Countdown) // by time : 
            {
                TimeSpaner -= TimeSpan.FromSeconds(1);
            }

            if (!Countdown) // by score : 
            {
                TimeSpaner += TimeSpan.FromSeconds(1);
            }

            tb_Timer.Text = showTime();
            if (Countdown && TimeSpaner == TimeSpan.FromSeconds(0))
            {
                stopGame(false);
            }
            
        }

        private void UpdateScore(bool UserScored)
        {
            int userScore = int.Parse(tb_UserScore.Text);
            int robotScore = int.Parse(tb_RobotScore.Text);

            if (UserScored)
            {
                tb_UserScore.Text = (userScore + 1).ToString();
            }

            if (!UserScored)
            {
                tb_RobotScore.Text = (robotScore + 1).ToString();
            }

            if(Params.ByScore && (userScore == SCORE_LIMIT || robotScore == SCORE_LIMIT))
            {
                stopGame(false);
            }
        }

        private void Popup_OK_Invoked(IUICommand command)
        {
            //TODO: Insert new score to the DB
            Frame.Navigate(typeof(ScoreBoard), null);
        }
        private void stopGame(bool navigation)
        {
            Timer.Stop();
            Bluetooth.Write(((char)Command.TERMINATE).ToString());
            if (!navigation)
            {
                MessageDialog msgDialog = new MessageDialog("Game is Over !");
                UICommand OK = new UICommand("OK");
                OK.Invoked += Popup_OK_Invoked;
                Utils.Show(msgDialog, new List<UICommand> { OK });
            }
        }
        private TimeSpan TimeSpaner { get; set; }
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Params = e.Parameter as Session;
            this.InitializeComponent();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(10);
            Timer.Tick += DispatcherTimer_Tick;
            if (Params.ByTime)
            {
                TimeSpaner = TimeSpan.FromMinutes(3);
            }

            if (Params.ByScore)
            {
                TimeSpaner = TimeSpan.Zero;
            }
            tb_Timer.Text = showTime();
            Timer.Start();

        
            tb_player.Text = Params.PlayerName;

            if (!Bluetooth.isConnected)
            {
                Bluetooth.ConnectToBoard();
            }

            //reset Score : 
            tb_UserScore.Text = 0.ToString();
            tb_RobotScore.Text = 0.ToString();

             Bluetooth.Write(((char)Command.START).ToString());
             
             ReceiveStringLoop(Bluetooth.Reader);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            stopGame(true);
        }

        private async void ReceiveStringLoop(DataReader chatReader)
        {
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

                string a = chatReader.ReadString(stringLength);
                Debug.Text = a;

                ReceiveStringLoop(chatReader);
            }
            catch (Exception ex)
            {
                lock (this)
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
}