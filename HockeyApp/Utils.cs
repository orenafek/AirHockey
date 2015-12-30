using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation;

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

    public class Bluetooth
    {
        private static bool _isConnected = false;
        private static DeviceInformationCollection devices = null;
        private static RfcommDeviceService service = null;
        private static StreamSocket socket = null;
        public static StreamSocketListener Listener = null;
        public static DataWriter writer = null;
        private static DataReader reader = null;
        public static DataReader Reader {
            get
            {
               var r = new DataReader(socket.InputStream);
                r.InputStreamOptions = InputStreamOptions.Partial;
                return r;
            }
        }
        public static bool isSocketOpen { get { return socket != null; } }
        public static bool isConnected { get { return _isConnected; } set { _isConnected = value; } }
        public static bool isWritten { get; set; }
        public static async void ConnectToBoard()
        {
            var devices = await DeviceInformation.FindAllAsync();
            if (devices.Count > 0)
            {
                foreach (var device in devices)
                {
                    if (device.Name == "HC-06")
                    {
                        try
                        {
                            service = await RfcommDeviceService.FromIdAsync(device.Id);
                            socket = new StreamSocket();
                            //Listener = new StreamSocketListener();
                            //Listener.ConnectionReceived += listenerHandler;
                            await socket.ConnectAsync(service.ConnectionHostName,
                                service.ConnectionServiceName);
                            reader = new DataReader(socket.InputStream);
                            writer = new DataWriter(socket.OutputStream);
                            
                            isConnected = true;
                            break;
                        }

                        catch (Exception ex) { }
                    }
                }
            }

            else
            {
                Utils.Show(new MessageDialog("No Devices Were Found :("), null);
            }
        }
        public static async void Write(string Msg)
        {
            isWritten = false;
            if (!isConnected){ return;}
            //writer = new DataWriter(socket.OutputStream);
            writer.WriteString(Msg);
            await writer.StoreAsync();
            isWritten = true;
        }

        public static void Disconnect(string disconnectReason)
        {
            if (writer != null)
            {
                writer.DetachStream();
                writer = null;
            }

            if (reader != null)
            {
                reader.DetachStream();
                reader = null;
            }

            if (service != null)
            {
                service.Dispose();
                service = null;
            }
            
            if (socket != null)
            {
                socket.Dispose();
                socket = null;
            }

            Utils.Show(new MessageDialog(disconnectReason), null);
        }

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

    }
}