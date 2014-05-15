using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistSaver.Radio.Hr3
{
    using PlaylistSaver.Core;
    using PlaylistSaver.Radio.Helpers.Playlistbase;

    public class Hr3Saver : IPlaylistSaver
    {
        private const string StationName = "HR3";

        private string StationKey
        {
            get
            {
                return StationName.ToLower();
            }
        }

        public string Name
        {
            get { return StationName; }
        }

        public TimeSpan DefaultInterval
        {
            get { return TimeSpan.FromDays(1); }
        }

        public static IPlaylistSaver Create()
        {
            return new Hr3Saver();
        }

        public StartEndSpan GetAvailableTimes()
        {
            return new StartEndSpan(DateTime.Now.AddMonths(-1), DateTime.Now);
        }

        public List<PlaylistEntry> GetEntrys(DateTime time)
        {
            return Playlistbase.GetEntries(StationKey, Name, time);
        }
    }
}
