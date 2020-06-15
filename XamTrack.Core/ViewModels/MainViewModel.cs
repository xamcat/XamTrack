using System;
using System.Timers;
using System.Threading.Tasks;
using TinyMvvm;
using TinyMvvm.IoC;
using Xamarin.Essentials;
using XamTrack.Core.Services;

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

        private string _lat;
        public string Lat
        {
            get => _lat;
            set => Set(ref _lat, value);
        }

        private string _lon;
        public string Lon
        {
            get => _lon;
            set => Set(ref _lon, value);
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

        IGeolocationService _geolocationService;
         
        Timer _timer;

        readonly int TimerPeriod = 5000;

        public MainViewModel(IGeolocationService GeolocationService)
        {
            _geolocationService = GeolocationService;
        }

        public async override Task Initialize()
        {
            await base.Initialize();
            Name = "BenBtg";
            Lat = "-1.46";
            Lon = "52.23";
            Country = "United Kingdom";
            City = "Chippenham";

            CurrentLocation = await _geolocationService.GetLastKnownLocationAsync();

            _timer = new Timer(TimerPeriod);
            _timer.Elapsed += _timer_ElapsedAsync;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private async void _timer_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            await UpdateCurrentLocationAsync();
        }

        private async Task UpdateCurrentLocationAsync()
        {
            CurrentLocation = await _geolocationService.GetLocationAsync();
        }


    }
}