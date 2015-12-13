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
            int isOn = (ts_led.IsOn) ? 1 : 2;
            writer = new DataWriter(socket.OutputStream);
            writer.WriteInt32(isOn);
            await writer.StoreAsync();
        }

        private async void PairAppToArduino()
        {
            Status.Text = "Connecting To Arduino...";
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
            var pairedDevices = await PeerFinder.FindAllPeersAsync();

            if (pairedDevices.Count == 0)
            {
                Utils.Show(new MessageDialog("No Pairing Devices Were Found :("),new List<UICommand>() {new UICommand("OK")} );
                return;
            }

            foreach (var device in pairedDevices)
            {
                if (device.DisplayName == "HC-06")
                {
                    BTConnectionManager.Connect(device.HostName);
                    Status.Text = "Connected to Arduino !";
                    continue;
                }
            }
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
                            Status.Text = "Connected";
                            await socket.ConnectAsync(service.ConnectionHostName,
                                service.ConnectionServiceName);
                            ts_led.IsEnabled = true;
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
    }
}
