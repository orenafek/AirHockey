using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        public static async void Show(string text, List<UICommand> commands, uint defaultCommandIndex = 0, uint cancelCommandIndex = 0)
        {
            var message = new MessageDialog(text);
            foreach (UICommand command in commands)
            {
                message.Commands.Add(command);
            }
            message.DefaultCommandIndex = defaultCommandIndex;
            message.CancelCommandIndex = cancelCommandIndex;
            await message.ShowAsync();
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
    }
}
