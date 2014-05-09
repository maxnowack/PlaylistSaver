﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaylistSaver.Core;

namespace PlaylistSaver.Radio.Ffn
{
    public class FfnSaver : IPlaylistSaver
    {
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
