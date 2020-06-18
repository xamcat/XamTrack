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
            _ioTDeviceClientService.Disconnect();
        });

        #endregion

        IGeolocationService _geolocationService;
        IIoTDeviceClientService _ioTDeviceClientService;
         
        Timer _timer;

        readonly int TimerPeriod = 5000;

        System.Threading.CancellationToken _cancellationToken;

        public MainViewModel(IGeolocationService GeolocationService, IIoTDeviceClientService ioTDeviceClientService)
        {
           _cancellationToken = new System.Threading.CancellationToken();
            _geolocationService = GeolocationService;
            _ioTDeviceClientService = ioTDeviceClientService;
        }

        public async override Task Initialize()
        {
            await base.Initialize();
            Name = "BenBtg";
            Country = "United Kingdom";
            City = "Chippenham";

            CurrentLocation = await _geolocationService?.GetLastKnownLocationAsync();

            _timer = new Timer(TimerPeriod);
            _timer.Elapsed += _timer_ElapsedAsync;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private async void _timer_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            await UpdateCurrentLocationAsync();
            var locationMessage = JsonConvert.SerializeObject(CurrentLocation);
            await _ioTDeviceClientService.SendEventAsync(locationMessage, _cancellationToken);
        }

        private async Task UpdateCurrentLocationAsync()
        {
            CurrentLocation = await _geolocationService.GetLocationAsync();
        }


    }
}