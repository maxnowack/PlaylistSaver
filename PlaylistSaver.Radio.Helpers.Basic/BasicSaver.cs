using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CsQuery;
using PlaylistSaver.Core;

namespace PlaylistSaver.Radio.Helpers.Basic
{
    public class BasicSaver : IPlaylistSaver
    {
        private readonly TimeSpan _defaultInterval;
        private readonly string _radioName;

        public string Name
        {
            get { return _radioName; }
        }

        public BasicSaver(string name, TimeSpan interval)
        {
            _radioName = name;
            _defaultInterval = interval;
        }

        public TimeSpan DefaultInterval
        {
            get { return _defaultInterval; }
        }


        public StartEndSpan GetAvailableTimes(string html, Func<CQ,List<IDomObject>> getOptions,
            Func<List<IDomObject>, DateTime> startTime, Func<List<IDomObject>, DateTime> endTime)
        {
            var doc = CQ.Create(html);
            var items = getOptions.Invoke(doc);

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

        protected internal string GetWebContent(string url, Dictionary<string,string> data = null, int repeats=0)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    if (data == null)
                    {
                        return wc.DownloadString(url);
                    }
                    var dataString = string.Join("&", data.Select(x => string.Format("{0}{1}{2}", x.Key, "=", x.Value)));
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    return wc.UploadString(url, dataString);
                }
            }
            catch (WebException)
            {
                if (repeats <= 3)
                    return GetWebContent(url, data, repeats);
                throw;
            }
        }


        public virtual StartEndSpan GetAvailableTimes()
        {
            throw new NotImplementedException();
        }

        public virtual List<PlaylistEntry> GetEntrys(DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
