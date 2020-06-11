using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TinyMvvm;

namespace XamTrack.ViewModels
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
            Name = "Ben";
        }

    }
}
