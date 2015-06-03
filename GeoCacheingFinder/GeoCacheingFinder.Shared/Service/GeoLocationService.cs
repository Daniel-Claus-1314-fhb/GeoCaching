using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GeoCacheingFinder.Service
{
    class GeoLocationService
    {
        private Geolocator _geolocator = null;
        private CancellationTokenSource _cts = null;

        public GeoLocationService()
        {
            this._geolocator = new Geolocator();
            // Desired Accuracy needs to be set
            // before polling for desired accuracy.
            _geolocator.DesiredAccuracyInMeters = 25;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accuracy"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        public async Task<Geoposition> FindGeoLocation(uint accuracy, CancellationTokenSource cts) 
        {
            Geoposition pos = null;

            try
            {
                if (cts != null)
                {
                    _cts = cts;
                }
                else
                {
                    _cts = new CancellationTokenSource();
                }
                CancellationToken token = _cts.Token;
                // Carry out the operation
                _geolocator.DesiredAccuracyInMeters = accuracy;
                pos = await _geolocator.GetGeopositionAsync().AsTask(token);
            }
            catch (System.UnauthorizedAccessException)
            {

            }
            catch (TaskCanceledException)
            {

            }
            finally
            {
                _cts = null;
            }
            return pos;
        }

    }
}
