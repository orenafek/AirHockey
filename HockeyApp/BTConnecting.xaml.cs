using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BluetoothConnectionManager;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HockeyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BTConnecting : Page
    {
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
            string command = (sender as ToggleSwitch).IsOn ? "OFF" : "ON";
            await BTConnectionManager.SendCommand(command);
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
            PairAppToArduino();
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
    }
}
