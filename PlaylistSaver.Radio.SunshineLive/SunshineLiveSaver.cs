using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistSaver.Radio.SunshineLive
{
    using PlaylistSaver.Core;
    using PlaylistSaver.Radio.Helpers.Playlistbase;

    public class SunshineLiveSaver : IPlaylistSaver
    {
        private const string StationName = "Sunshine Live";

        private string StationKey
        {
            get
            {
                return "sunshine-live";
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
            return new SunshineLiveSaver();
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
