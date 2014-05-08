using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaylistSaver.Core
{
    public class PlaylistEntry
    {
        public string Radio { get; set; }
        public DateTime Time { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
    }
}
