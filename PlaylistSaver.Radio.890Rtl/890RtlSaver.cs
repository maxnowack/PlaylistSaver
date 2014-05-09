using System;
using System.Collections.Generic;
using PlaylistSaver.Core;

namespace PlaylistSaver.Radio._890Rtl
{
    public class _890RtlSaver : IPlaylistSaver
    {
        public string Name { get { return "89.0 RTL"; } }
        public TimeSpan DefaultInterval { get { return TimeSpan.FromMinutes(60); } }

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
