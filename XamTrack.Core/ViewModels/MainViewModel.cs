using System.Threading.Tasks;
using TinyMvvm;
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
        #endregion

        public MainViewModel()
        {
        }

        public async override Task Initialize()
        {
            await base.Initialize();
            Name = "BenBtg";
            Lat = "-1.46";
            Lon = "52.23";
            Country = "United Kingdom";
            City = "Chippenham";
        }

    }
}