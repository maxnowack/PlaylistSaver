﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaylistSaver.Core;

namespace PlaylistSaver.Radio.Antenne
{
    public class AntenneSaver : IPlaylistSaver
    {
        public static IPlaylistSaver Create()
        {
            return new AntenneSaver();
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
