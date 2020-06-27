using System;
using System.Collections.Generic;
using System.Text;
using TinyIoC;
using Xamarin.Forms.Maps;
using XamTrack.Core.Services;

namespace XamTrack
{
    public class TrackingMapView :  Map
    {
        private IGeolocationService _geolocationService;
        

        public TrackingMapView()
        {
            var container = TinyIoCContainer.Current;
            
            _geolocationService = container.Resolve<IGeolocationService>();

            _geolocationService.LocationUpatedHandler += _geolocationService_LocationUpatedHandler;            
        }

        private void _geolocationService_LocationUpatedHandler(object sender, Xamarin.Essentials.Location e)
        {
            var position = new Position(e.Latitude, e.Longitude);
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(20));
            this.MoveToRegion(mapSpan);
        }
    }
}
