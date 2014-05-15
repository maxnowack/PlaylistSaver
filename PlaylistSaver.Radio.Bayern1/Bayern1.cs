using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistSaver.Radio.Bayern1
{
    using PlaylistSaver.Core;
    using PlaylistSaver.Radio.Helpers.Playlistbase;

    public class Bayern1 : IPlaylistSaver
    {
        private const string StationName = "Bayern 1";

        private string StationKey
        {
            get
            {
                return "bayern1";
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
            return new Bayern1();
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
