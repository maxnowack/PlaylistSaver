using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
