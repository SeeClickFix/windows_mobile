using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SeeClickFix.WP8.Services
{
    public class GeoLocatorService
    {
        public async Task<GetGeoCoordinateResponse> GetCurrentPositionAsync()
        {
            GetGeoCoordinateResponse res = null;
            Geolocator geolocator = new Geolocator()
            {
                //DesiredAccuracy = PositionAccuracy.High
               // DesiredAccuracyInMeters = 50
            };

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    TimeSpan.FromMinutes(5), //age
                    TimeSpan.FromSeconds(5)); // timeout

                res = new GetGeoCoordinateResponse(
                    new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude),
                    null, 
                    false);
            }
            catch (Exception ex)
            {
                res = new GetGeoCoordinateResponse(null, ex, (uint)ex.HResult == 0x80004004);
            }
            return res;
        }

        public bool IsLocationDisabledError(Exception ex)
        {
            return (uint)ex.HResult == 0x80004004;
        }
    }
}
