using System;
using System.Collections.Generic;
using System.Linq;
using PlaylistSaver.Core;
using PlaylistSaver.Radio.Helpers.Basic;

namespace PlaylistSaver.Radio.Njoy
{
    public class NjoySaver : BasicSaver
    {
        public NjoySaver() : base("N-Joy", TimeSpan.FromMinutes(10)) { }
        public static IPlaylistSaver Create()
        {
            return new NjoySaver();
        }

        public override StartEndSpan GetAvailableTimes()
        {
            return GetAvailableTimes(GetWebContent("http://www.n-joy.de/radio/titelsuche115.html"),
                doc => doc["#playlist_date option"].ToList(),
                items => DateTime.Parse(items[0].GetAttribute("value")),
                items => DateTime.Parse(string.Format("{0} {1:HH:mm}", items[items.Count - 1].GetAttribute("value"), DateTime.Now)));
        }

        public override List<PlaylistEntry> GetEntrys(DateTime time)
        {
            return GetEntrys(time, 
                GetWebContent(string.Format("http://www.n-joy.de/radio/titelsuche115.html?date={0:yyyy-MM-dd}&time={0:HH}%3A{0:mm}&search_submit=1", time)),
                doc => doc["table[summary='Programm'] tbody tr"].ToList(),
                cq => DateTime.Parse(string.Format("{0:yyyy-MM-dd} {1}", time, cq["td"][0].InnerText)),
                cq => cq["td"][1].InnerText,
                cq => cq["td"][2].InnerText);
        }
    }
}
