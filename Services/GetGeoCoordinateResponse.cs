using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SeeClickFix.WP8.Services
{
    public class GetGeoCoordinateResponse
    {
        public bool IsLocationServicesDisabled { get; private set; }
        public GeoCoordinate Coordinate { get; private set; }
        public Exception Error { get; private set; }

        public GetGeoCoordinateResponse(GeoCoordinate coordinate, Exception error, bool isLocationServicesDisabled)
        {
            this.Coordinate = coordinate;
            this.Error = error;
            this.IsLocationServicesDisabled = isLocationServicesDisabled;
        }

    }
}
