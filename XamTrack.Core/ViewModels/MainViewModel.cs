using System;
using System.Timers;
using System.Threading.Tasks;
using TinyMvvm;
using TinyMvvm.IoC;
using Xamarin.Essentials;
using XamTrack.Core.Services;
using System.Windows.Input;
using Newtonsoft.Json;

namespace XamTrack.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        #region Properties
        private double _timerProgress;
        public double TimerProgress
        {
            get => _timerProgress;
            set => Set(ref _timerProgress, value);
        }

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

        private string name;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private string _country;
        public string Country
        {
            get => _country;
            set => Set(ref _country, value);
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
            await _ioTDeviceClientService.Connect();
        });

        private ICommand? _disconnect;
        public ICommand Disconnect => _disconnect ??= new TinyCommand(async () =>
        {
            await _ioTDeviceClientService.Disconnect();
        });

        #endregion


        IGeolocationService _geolocationService;
        IIoTDeviceClientService _ioTDeviceClientService;
         
        Timer _messageTimer;
        //Timer _progressTimer;

        readonly int MessageTimerPeriod = 5000;
        //readonly int ProgressTimerPeriod = 10;
        //int _progressTimerCount = 0;

        System.Threading.CancellationToken _cancellationToken;

        public MainViewModel(IGeolocationService geolocationService, IIoTDeviceClientService ioTDeviceClientService)
        {
           _cancellationToken = new System.Threading.CancellationToken();
            _geolocationService = geolocationService;
            _ioTDeviceClientService = ioTDeviceClientService;
            _ioTDeviceClientService.ConnectionStatusChanged += _ioTDeviceClientService_ConnectionStatusChanged;

            ConnectionStatus = "Disconnected";
        }

        private void _ioTDeviceClientService_ConnectionStatusChanged(object sender, string e)
        {
            ConnectionStatus = e;
        }

        public async override Task Initialize()
        {
            await base.Initialize();
            Name = "BenBtg";
            Country = "United Kingdom";
            City = "Chippenham";

            CurrentLocation = await _geolocationService?.GetLastKnownLocationAsync();

            TimerProgress = 1.0;

            _messageTimer = new Timer(MessageTimerPeriod);
            _messageTimer.Elapsed += _timer_ElapsedAsync;
            _messageTimer.AutoReset = true;
            _messageTimer.Start();

            //_progressTimer = new Timer(ProgressTimerPeriod);
            //_progressTimer.Elapsed += _progressTimer_Elapsed;
            //_progressTimer.Start();
        }

        //private void _progressTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    _progressTimerCount++;
        //    var ProgressTimerRatio = (double)ProgressTimerPeriod / (double)MessageTimerPeriod;
        //    //System.Diagnostics.Debug.WriteLine("ProgressTimerElapsed");
        //    TimerProgress = ProgressTimerRatio * _progressTimerCount;
        //}

        private async void _timer_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            //TimerProgress = 0.0;
            //_progressTimerCount = 0;
            //_progressTimer.Start(); 
            //System.Diagnostics.Debug.WriteLine("MessageTimerElapsed");
            
            await UpdateCurrentLocationAsync();
            var locationMessage = JsonConvert.SerializeObject(CurrentLocation);
            await _ioTDeviceClientService.SendEventAsync(locationMessage, _cancellationToken);
            TimerProgress = 1.0;
        }

        private async Task UpdateCurrentLocationAsync()
        {
            CurrentLocation = await _geolocationService.GetLocationAsync();
        }
    }
}