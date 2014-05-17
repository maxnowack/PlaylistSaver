using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using PlaylistSaver.Core;

namespace PlaylistSaver.Radio.Helpers.Basic
{
    public class BasicSaver<T> where T : class, IPlaylistSaver, new()
    {
        private readonly TimeSpan defaultInterval;
        private readonly string radioName;
        public string Name
        {
            get { return radioName; }
        }

        public static IPlaylistSaver Create()
        {
            return new T();
        }

        public BasicSaver(string name,TimeSpan interval)
        {
            radioName = name;
            defaultInterval = interval;
        }

        public TimeSpan DefaultInterval
        {
            get { return defaultInterval; }
        }


        public StartEndSpan GetAvailableTimes(string html, string optionSelector, Func<List<IDomObject>, DateTime> startTime, Func<List<IDomObject>, DateTime> endTime)
        {
            var doc = CQ.Create(html);
            var items = doc[optionSelector].ToList();

            return new StartEndSpan(startTime.Invoke(items), endTime.Invoke(items));
        }

        public List<PlaylistEntry> GetEntrys(DateTime time, string html, Func<CQ, List<IDomObject>> getItems,
            Func<CQ, DateTime> getTime, Func<CQ, string> getArtist, Func<CQ, string> getTitle)
        {
            var doc = CQ.Create(html);

            var items = getItems.Invoke(doc);
            var list = new List<PlaylistEntry>();
            foreach (var item in items)
            {
                var cq = CQ.Create(item.InnerHTML);

                var newTime = getTime.Invoke(cq);

                if (time.Hour > newTime.Hour + 6) newTime = newTime.AddDays(1);
                if (newTime.Hour > time.Hour + 6) newTime = newTime.AddDays(-1);
                var entry = new PlaylistEntry
                {
                    Radio = Name,
                    Time = newTime,
                    Artist = getArtist.Invoke(cq),
                    Title = getTitle.Invoke(cq)
                };
                list.Add(entry);
            }
            return list;
        }
    }
}
