using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using TinyMvvm;
using Xamarin.Essentials;
using XamTrack.Core.Models;
using XamTrack.Core.Services;

namespace XamTrack.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        private string _messageText;
        public string MessageText
        {
            get => _messageText;
            set => Set(ref _messageText, value);
        }

        private string _connectionStatus;
        public string ConnectionStatus
        {
            get => _connectionStatus;
            set => Set(ref _connectionStatus, value);
        }

        private string _deviceId;
        public string DeviceId
        {
            get => _deviceId;
            set => Set(ref _deviceId, value);
        }

        private string _city;
        public string City
        {
            get => _city;
            set => Set(ref _city, value);
        }

        private string _connected;
        public string Connected
        {
            get => _connected;
            set => Set(ref _connected, value);
        }

        private string _trackButtonText;
        public string TrackButtonText
        {
            get => _trackButtonText;
            set => Set(ref _trackButtonText, value);
        }

        private Location _currentLocation;
        public Location CurrentLocation
        {
            get => _currentLocation;
            set => Set(ref _currentLocation, value);
        }
        #endregion

        #region Commands
        private ICommand _connectCommand;
        public ICommand ConnectCommand => _connectCommand = new TinyCommand(async () =>
        {
            if (_ioTDeviceClientService.ConnectionStatus != "Connected")
            {
                await StartTrackingAsync();
            }
            else
            {
                await StopTrackingAsync();
            }
        });


        #endregion

        private IGeolocationService _geolocationService;
        private IDeviceInfoService _deviceInfoService;
        private IIoTDeviceClientService _ioTDeviceClientService;


        Timer _messageTimer;

        readonly int MessageTimerPeriod = 5000;

        public MainViewModel(IGeolocationService geolocationService, IIoTDeviceClientService ioTDeviceClientService, IDeviceInfoService deviceInfoService)
        {
            _geolocationService = geolocationService;
            _deviceInfoService = deviceInfoService;
            _ioTDeviceClientService = ioTDeviceClientService;
            _ioTDeviceClientService.ConnectionStatusChanged += _ioTDeviceClientService_ConnectionStatusChanged;

            TrackButtonText = "Start Tracking";
            ConnectionStatus = "Disconnected";
        }

        private void _ioTDeviceClientService_ConnectionStatusChanged(object sender, string e)
        {
            ConnectionStatus = e;
        }

        public async override Task OnAppearing()
        {
            DeviceId = _deviceInfoService.GetDeviceId();

            CurrentLocation = await _geolocationService?.GetLastKnownLocationAsync();
            City = await _geolocationService?.GetCityName(CurrentLocation);
        }

        private async Task StartTrackingAsync()
        {
            IsBusy = true;
            ConnectionStatus = "Connecting";
            await _ioTDeviceClientService?.ConnectAsync();
            TrackButtonText = "Stop Tracking";

            _messageTimer = new Timer(MessageTimerPeriod);
            _messageTimer.Elapsed += async (o,e) => await SendMesaageAsync();
            _messageTimer.AutoReset = true;
            _messageTimer.Start();
        }

        private async Task StopTrackingAsync()
        {
            IsBusy = false;
            _messageTimer.Stop();
            ConnectionStatus = "Disconnecting";
            await _ioTDeviceClientService?.DisconnectAsync();
            TrackButtonText = "Start Tracking";
        }

        private async Task SendMesaageAsync()
        {
            Debug.WriteLine("MessageTimerElapsed");

            await UpdateCurrentLocationAsync();

            var message = new IoTMessage(CurrentLocation, MessageText,
                _deviceInfoService.GetDeviceId(),
                _deviceInfoService.GetDeviceName(),
                _deviceInfoService.GetDeviceModel()
                );
            var messagejson = JsonConvert.SerializeObject(message);

            await _ioTDeviceClientService.SendEventAsync(messagejson);
        }

        private async Task UpdateCurrentLocationAsync()
        {
            CurrentLocation = await _geolocationService.GetLocationAsync();
            City = await _geolocationService?.GetCityName(CurrentLocation);
        }
    }
}