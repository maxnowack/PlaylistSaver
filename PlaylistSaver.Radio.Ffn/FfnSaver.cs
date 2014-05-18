using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CsQuery;
using PlaylistSaver.Core;
using PlaylistSaver.Radio.Helpers.Basic;

namespace PlaylistSaver.Radio.Ffn
{
    public class FfnSaver : BasicSaver
    {
        public FfnSaver() : base("FFN", TimeSpan.FromMinutes(3)) { }
        public static IPlaylistSaver Create()
        {
            return new FfnSaver();
        }
        public override StartEndSpan GetAvailableTimes()
        {
            return base.GetAvailableTimes(GetWebContent("http://www.ffn.de/musik/playlist.html"),
                doc => doc[".playlist-selector-datepicker option"].ToList(),
                items => DateTime.Parse(items.Last().GetAttribute("value"), CultureInfo.GetCultureInfo("de-DE")),
                items => DateTime.Parse(
                                     string.Format("{0} {1:HH:mm}", items.First().GetAttribute("value"), DateTime.Now),
                                     CultureInfo.GetCultureInfo("de-DE")));
        }

        public override List<PlaylistEntry> GetEntrys(DateTime time)
        {
            return GetEntrys(time,
                GetWebContent(
                    "http://www.ffn.de/musik/playlist.html",
                    new Dictionary<string, string>
                    {
                        {"tx_ffnplaylist_pi1[date]", string.Format("{0:dd.MM.yyyy}",time)},
                        {"tx_ffnplaylist_pi1[hours]", string.Format("{0:HH}",time)},
                        {"tx_ffnplaylist_pi1[minutes]", string.Format("{0:mm}",time)}
                    }
                ),
                doc => doc["ul.playlist-list-search li.playlist-list-item"].ToList(),
                cq =>
                {
                    var hhElm = cq[".time-container span"].ToList().FirstOrDefault();
                    var mmElm = cq[".time-container sup"].ToList().FirstOrDefault();

                    var hh = hhElm!=null ? hhElm.InnerText : "00";
                    var mm = mmElm!=null ? mmElm.InnerText : "00";
                    return DateTime.Parse(string.Format("{0:yyyy-MM-dd} {1}:{2}", time, hh, mm));
                },
                cq => cq["h3.artist"].ToList().FirstOrDefault()!=null ? cq["h3.artist"].ToList().FirstOrDefault().InnerText : string.Empty,
                cq => cq["h3.title"].ToList().FirstOrDefault()!=null ? cq["h3.title"].ToList().FirstOrDefault().InnerText : string.Empty);
        }
    }
}
