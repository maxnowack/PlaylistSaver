using System;
using System.Collections.Generic;

namespace PlaylistSaver.Core
{
    public interface IPlaylistSaver
    {
        string Name { get; }
        TimeSpan DefaultInterval { get; }
        StartEndSpan GetAvailableTimes();
        List<PlaylistEntry> GetEntrys(DateTime time);
    }
}
