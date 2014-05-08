using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using PlaylistSaver.Core;

namespace PlaylistSaver.Radio.Njoy
{
    public class NjoySaver : IPlaylistSaver
    {
        public static IPlaylistSaver Create()
        {
            return new NjoySaver();
        }

        public StartEndSpan GetAvailableTimes()
        {

            var doc = CQ.CreateFromUrl("http://www.n-joy.de/radio/titelsuche115.html");
            var items = doc["#playlist_date option"].ToList();

            return new StartEndSpan(DateTime.Parse(items[0].GetAttribute("value")),DateTime.Parse(items[items.Count-1].GetAttribute("value")));
        }

        public List<PlaylistEntry> GetEntrys(DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
