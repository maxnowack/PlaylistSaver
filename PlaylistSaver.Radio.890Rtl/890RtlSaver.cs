using System;
using System.Linq;
using System.Collections.Generic;
using PlaylistSaver.Core;
using PlaylistSaver.Radio.Helpers.Basic;
using System.Globalization;

namespace PlaylistSaver.Radio._890Rtl
{
    public class _890RtlSaver : BasicSaver
    {
        public _890RtlSaver() : base("89.0 RTL", TimeSpan.FromMinutes(60)) { }

        public static IPlaylistSaver Create()
        {
            return new _890RtlSaver();
        }

        public override StartEndSpan GetAvailableTimes()
        {
            return new StartEndSpan(DateTime.Now.AddYears(-2), DateTime.Now);
        }

        public override List<PlaylistEntry> GetEntrys(DateTime time)
        {
            return base.GetEntrys(
                time,
                GetWebContent("http://sites.89.0rtl.de/playlist/playlist_request2014.php",
                              new Dictionary<string,string>(){
                                  {"tag",string.Format("{0:dd.MM.yyyy}",time)},
                                  {"stunde",string.Format("{0:HH}",time)}
                              }),
                doc => doc["div.result ul li"].ToList(),
                cq => DateTime.Parse(string.Format("{0:yyyy-MM-dd} {1}", time, cq["div.date"][0].InnerText)),
                cq => cq["div.interpret"][0].InnerText,
                cq => cq["div.title"][0].InnerText);
        }
    }
}
