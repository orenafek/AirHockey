﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
            if (!System.Text.RegularExpressions.Regex.IsMatch(nameInput.Text, "^[a-zA-Z]+([ ][a-zA-Z]+)*$") ||
                nameInput.Text.Length > 12)
            {
                MessageDialog msg = new MessageDialog("Invalid or Too Long name, please enter again");
                Utils.Show(msg, new List<UICommand> {new UICommand("Close")});
            }
            else
            {
                NamePanel.Visibility = Visibility.Collapsed;
                tb_greetingOutput.Text = "Hello, " + nameInput.Text + "!";
                ChooseTimeOrScore.Visibility = Visibility.Visible;
            }
        }
        
        private void NameInput_OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            inputButton.IsEnabled = ((TextBox) sender).Text != string.Empty;
        }

        private void BtnOption_OnClick(object sender, RoutedEventArgs e)
        {
            Button btnSender = sender as Button;
            bool byScore = btnSender == btn_byScore;

            Session session = new Session(byScore, nameInput.Text);
            Frame.Navigate(typeof (Game), session);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            vc = e.Parameter as VolumeControl;
        }
    }
}