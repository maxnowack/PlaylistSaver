using System;
using System.Collections.Generic;
using PlaylistSaver.Core;

namespace PlaylistSaver.Radio.Ffn
{
    public class FfnSaver : IPlaylistSaver
    {
        public string Name { get { return "FFN"; } }
        public TimeSpan DefaultInterval { get { return TimeSpan.FromMinutes(5); } }
        public static IPlaylistSaver Create()
        {
            return new FfnSaver();
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
