using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using PlaylistSaver.Core;

namespace PlaylistSaver.Radio.Ffn
{
    public class FfnSaver : IPlaylistSaver
    {
        public string Name { get { return "FFN"; } }
        public TimeSpan DefaultInterval { get { return TimeSpan.FromMinutes(5); } }
        public static IPlaylistSaver Create()
        {
            return new FfnSaver();
        }

        public StartEndSpan GetAvailableTimes()
        {

            var doc = CQ.CreateFromUrl("http://www.ffn.de/musik/playlist.html");
            var items = doc[".playlist-selector-datepicker option"].ToList();

            return new StartEndSpan(DateTime.Parse(string.Format("{0} {1:HH:mm}", items.First().GetAttribute("value"), DateTime.Now)), DateTime.Parse(items.Last().GetAttribute("value")));
        }

        public List<PlaylistEntry> GetEntrys(DateTime time)
        {
            var doc = CQ.CreateFromUrl(
                        string.Format(
                            "http://www.n-joy.de/radio/titelsuche115.html?date={0:yyyy-MM-dd}&time={0:HH}%3A{0:mm}&search_submit=1",
                            time));

            var items = doc["li.playlist-list-item"].ToList();
            var list = new List<PlaylistEntry>();
            foreach (var item in items)
            {
                var cq = CQ.Create(item.InnerHTML);
                var hh = cq[".time-container span"].ToList().FirstOrDefault().InnerText;
                var mm = cq[".time-container sub"].ToList().FirstOrDefault().InnerText;

                var newTime = DateTime.Parse(string.Format("{0:yyyy-MM-dd} {1}:{2}", time, hh, mm));
                
                if (time.Hour > newTime.Hour + 6) newTime = newTime.AddDays(1);
                if (newTime.Hour > time.Hour + 6) newTime = newTime.AddDays(-1);
                var entry = new PlaylistEntry
                {
                    Radio = Name,
                    Time = newTime,
                    Artist = cq["h3.artist"].ToList().FirstOrDefault().InnerText,
                    Title = cq["h3.title"].ToList().FirstOrDefault().InnerText
                };
                list.Add(entry);
            }
            return list;
        }
    }
}
