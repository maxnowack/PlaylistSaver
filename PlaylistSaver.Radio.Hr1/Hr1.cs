using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistSaver.Radio.Hr1
{
    using PlaylistSaver.Core;
    using PlaylistSaver.Radio.Helpers.Playlistbase;

    public class Hr1Saver : IPlaylistSaver
    {
        public string Name
        {
            get { return "HR1"; }
        }

        public TimeSpan DefaultInterval
        {
            get { return TimeSpan.FromDays(1); }
        }

        public static IPlaylistSaver Create()
        {
            return new Hr1Saver();
        }

        public StartEndSpan GetAvailableTimes()
        {
            return new StartEndSpan(DateTime.Now.AddMonths(-2),DateTime.Now);
        }

        public List<PlaylistEntry> GetEntrys(DateTime time)
        {
            return Playlistbase.GetEntries("hr1", Name, time);
        }
    }
}
