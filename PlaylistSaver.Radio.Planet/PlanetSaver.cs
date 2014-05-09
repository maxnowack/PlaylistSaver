using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaylistSaver.Core;

namespace PlaylistSaver.Radio.Planet
{
    public class PlanetSaver : IPlaylistSaver
    {
        public string Name
        {
            get { return "Planet Radio"; }
        }

        public TimeSpan DefaultInterval
        {
            get { return TimeSpan.FromMinutes(10); }
        }

        public StartEndSpan GetAvailableTimes()
        {
            throw new NotImplementedException();
        }

        public List<PlaylistEntry> GetEntrys(DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
