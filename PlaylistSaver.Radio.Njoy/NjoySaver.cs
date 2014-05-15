using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using PlaylistSaver.Core;

namespace PlaylistSaver.Radio.Njoy
{
    public class NjoySaver : IPlaylistSaver
    {
        public string Name { get { return "N-Joy"; } }
        public TimeSpan DefaultInterval { get { return TimeSpan.FromMinutes(5); } }
        public static IPlaylistSaver Create()
        {
            return new NjoySaver();
        }

        public StartEndSpan GetAvailableTimes()
        {

            var doc = CQ.CreateFromUrl("http://www.n-joy.de/radio/titelsuche115.html");
            var items = doc["#playlist_date option"].ToList();

            return new StartEndSpan(DateTime.Parse(items[0].GetAttribute("value")),DateTime.Parse(string.Format("{0} {1:HH:mm}", items[items.Count-1].GetAttribute("value"),DateTime.Now)));
        }

        public List<PlaylistEntry> GetEntrys(DateTime time)
        {
            var doc = CQ.CreateFromUrl(
                        string.Format(
                            "http://www.n-joy.de/radio/titelsuche115.html?date={0:yyyy-MM-dd}&time={0:HH}%3A{0:mm}&search_submit=1",
                            time));

            var items = doc["table[summary='Programm'] tbody tr"].ToList();
            var list = new List<PlaylistEntry>();
            foreach (var item in items)
            {
                var tds = CQ.Create(item.InnerHTML)["td"];
                if (!tds.Any()) continue;
                
                var newTime = DateTime.Parse(string.Format("{0:yyyy-MM-dd} {1}", time, tds[0].InnerText));
                //if (list.Any() && list.Last().Time.Hour > newTime.Hour) newTime = newTime.AddDays(1);
                if (time.Hour > newTime.Hour + 6) newTime = newTime.AddDays(1);
                if (newTime.Hour > time.Hour + 6) newTime = newTime.AddDays(-1);
                var entry = new PlaylistEntry
                {
                    Radio = Name,
                    Time = newTime,
                    Artist = tds[1].InnerText,
                    Title = tds[2].InnerText
                };
                list.Add(entry);
            }
            return list;
        }
    }
}
