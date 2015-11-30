﻿using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HockeyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartGamePage : Page
    {
        public StartGamePage()
        {
            this.InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            // if text is valid
            //this.Frame.Navigate(typeof(MainMenu), nameInput.Text);
            //else
            if (!System.Text.RegularExpressions.Regex.IsMatch(nameInput.Text, "^[a-zA-Z]+([ ][a-zA-Z]+)*$"))
                Utils.Show("Invalid name, please enter again", new List<UICommand> { new UICommand("Close") });
            else {
                NamePanel.Visibility = Visibility.Collapsed;
                greetingOutput.Text = "Hello, " + nameInput.Text + "! Press the button to start a game.";
                ChooseTimeOrScore.Visibility = Visibility.Visible;
            }
        }

        private void NameInput_OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            inputButton.IsEnabled = ((TextBox) sender).Text != string.Empty;
        }
    }
}
