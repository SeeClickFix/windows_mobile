using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Common
{
    public static class GeoCoordinateWatcherUtil
    {
        public static Task<GeoCoordinateWatchResponse> GetCoordinateAsync()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            var taskCompletionSource = new TaskCompletionSource<GeoCoordinateWatchResponse>();

            EventHandler<GeoPositionStatusChangedEventArgs> statusChanged = null;
            statusChanged = (s, e) =>
            {
                if (e.Status == GeoPositionStatus.Disabled || e.Status == GeoPositionStatus.NoData || e.Status == GeoPositionStatus.Ready)
                {
                    watcher.StatusChanged -= statusChanged;
                    taskCompletionSource.SetResult(
                        new GeoCoordinateWatchResponse() { Status = e.Status, Position = watcher.Position.Location });
                }
            };

            watcher.StatusChanged += statusChanged;
            watcher.Start();

            return taskCompletionSource.Task;
        }
    }

    public class GeoCoordinateWatchResponse
    {
        public GeoCoordinate Position { get; set; }
        public GeoPositionStatus Status { get; set; }
    }
}
