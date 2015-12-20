using BluetoothConnectionManager;
using System;
using System.Collections.Generic;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HockeyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BTConnecting : Page
    {
        private DeviceInformationCollection devices = null;
        private RfcommDeviceService service = null;
        private StreamSocket socket = null;
        private StreamSocketListener listener = null;
        public static DataWriter writer = null;

        public BTConnecting()
        {
            this.InitializeComponent();
            BTConnectionManager = new ConnectionManager();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BTConnectionManager.Initialize();
           
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            BTConnectionManager.Terminate();
        }

        private ConnectionManager BTConnectionManager { get; set; }
        private async void Ts_led_OnToggled(object sender, RoutedEventArgs e)
        {
            string isOn = (ts_led.IsOn) ? "1" : "2";
            writer = new DataWriter(socket.OutputStream);
            writer.WriteString(isOn);
            await writer.StoreAsync();
        }

        private void Btn_Connect_OnClick(object sender, RoutedEventArgs e)
        {
            ConnectToBoard();
            
        }

        private RfcommDeviceService Service { get; set; }
        private StreamSocket Socket { get; set; }

        async void Init()
        {
            var services = await DeviceInformation.FindAllAsync(
                RfcommDeviceService.GetDeviceSelector(RfcommServiceId.ObexObjectPush));

            if (services.Count == 0)
            {
                Status.Text = "No Pairing Devices Were Found :(";
            }
            if (services.Count > 0)
            {
                var service = await RfcommDeviceService.FromIdAsync(services[0].Id);

                Service = service;
                Socket = new StreamSocket();
                
                await
                    Socket.ConnectAsync(Service.ConnectionHostName, Service.ConnectionServiceName,
                        SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

                Status.Text = "Connected to Arduino !";
            }
        }

        async void Pair()
        {
            var selector = "HC-06";/*BluetoothDevice.GetDeviceSelector();*/
            var devices = await DeviceInformation.FindAllAsync(selector);

        }

        private async void ConnectToBoard()
        {
            var devices = await DeviceInformation.FindAllAsync();
            if(devices.Count > 0)
            {
                foreach(var device in devices)
                {
                    if(device.Name == "HC-06")
                    {
                        try
                        {
                            service = await RfcommDeviceService.FromIdAsync(device.Id);
                            socket = new StreamSocket();
                            listener = new StreamSocketListener();
                            listener.ConnectionReceived += Listener_ConnectionReceived;
                            await socket.ConnectAsync(service.ConnectionHostName,
                                service.ConnectionServiceName);
                            ts_led.IsEnabled = true;
                            Status.Text = "Connected";
                            break;
                        }

                        catch (Exception ex) { }
                    }
                }
            }

            else
            {
                Utils.Show(new MessageDialog("No Devices Were Found :("),null);
            }
        }

        private void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            
        }

        private async void btn_ON_OFF_Click(object sender, RoutedEventArgs e)
        {
            /*btn_ON_OFF.Content = "SENDING";
            writer = new DataWriter(socket.OutputStream);
            string isOn = btn_ON_OFF.Content.ToString() == "ON" ? "1" : "2";
            writer.WriteString("1");
            await writer.StoreAsync();
            btn_ON_OFF.Content = isOn == "ON" ? "OFF" : "ON";*/
        }
    }
}
