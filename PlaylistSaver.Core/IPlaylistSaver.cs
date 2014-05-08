using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistSaver.Core
{
    public interface IPlaylistSaver
    {
        StartEndSpan GetAvailableTimes();
        List<PlaylistEntry> GetEntrys(DateTime time);
    }
}
