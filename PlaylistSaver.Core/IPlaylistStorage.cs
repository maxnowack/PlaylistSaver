﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistSaver.Core
{
    public interface IPlaylistStorage
    {
        long Store(IEnumerable<PlaylistEntry> entries);
        DateTime GetLastEntry(string stationKey);
    }
}
