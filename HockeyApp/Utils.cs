using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HockeyApp
{
    public class Session
    {
        public Session(bool byScore, string playerName)
        {
            ByScore = byScore;
            PlayerName = playerName;
        }

        public Session(Session other)
        {
            ByScore = other.ByScore;
            PlayerName = other.PlayerName;
        }

        public bool ByScore { get; set; }

        public bool ByTime
        {
            get { return !ByScore; }
            set { ByScore = !value; }
        }

        public string PlayerName { get; set; }
    }

    
    public class Utils
    {
        private static Windows.UI.Popups.IUICommand msgResponseAsyncOperation { get; set; }
        public static async void Show(MessageDialog msgDialog, List<UICommand> commands, uint defaultCommandIndex = 0,
            uint cancelCommandIndex = 0)
        {
            var message = msgDialog;
            foreach (UICommand command in commands)
            {
                message.Commands.Add(command);
            }
            message.DefaultCommandIndex = defaultCommandIndex;
            message.CancelCommandIndex = cancelCommandIndex;
            msgResponseAsyncOperation = await message.ShowAsync();
        }

        public static Windows.UI.Popups.IUICommand getMsgResponse()
        {
            return msgResponseAsyncOperation;
        }
        public static void EnableNavigateButton()
        {
            Frame rootFrame = Window.Current.Content as Frame;

            string myPages = "";
            foreach (PageStackEntry page in rootFrame.BackStack)
            {
                myPages += page.SourcePageType.ToString() + "\n";
            }
            //stackCount.Text = myPages;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private static MediaElement BackgroundSoundPlayer
        {
            get
            {
                var rootGrid = VisualTreeHelper.GetChild(Window.Current.Content, 0);
               return  VisualTreeHelper.GetChild(rootGrid, 0) as MediaElement;
            }
        }

        private static double backgroundVolume { get; set; }
        public static void Mute()
        {
            backgroundVolume = BackgroundSoundPlayer.Volume;
            BackgroundSoundPlayer.Volume = 0;
           
        }

        public static void UnMute()
        {
            BackgroundSoundPlayer.Volume = backgroundVolume;
            
        }
    }
}