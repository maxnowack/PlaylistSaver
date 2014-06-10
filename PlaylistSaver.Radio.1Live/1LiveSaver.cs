using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistSaver.Radio._1Live
{
    using PlaylistSaver.Core;
    using PlaylistSaver.Radio.Helpers.Basic;
    using PlaylistSaver.Radio.Helpers.Playlistbase;
    using System.Globalization;

    public class _1LiveSaver : BasicSaver
    {
        public _1LiveSaver() : base("1Live", TimeSpan.FromMinutes(10)) { }
        public static IPlaylistSaver Create()
        {
            return new _1LiveSaver();
        }
        public override StartEndSpan GetAvailableTimes()
        {
            return new StartEndSpan(DateTime.Now.AddYears(-1), DateTime.Now);
        }
        public override List<PlaylistEntry> GetEntrys(DateTime time)
        {
            var days = (DateTime.Now - time).Days;
            var html = GetWebContent(string.Format("http://www.einslive.de/musik/playlists/index.jsp?wday={1}&hour={0:HH}&minute={0:mm}", time, days));

            if (!html.Contains("Es konnten keine Titel gefunden werden."))
            {
                return GetEntrys(time,
                    html,
                    doc => doc["table[summary^='Die am'] tbody tr"].ToList(),
                    cq =>
                        DateTime.ParseExact(cq["td"][0].InnerText, "ddd MMM dd HH:mm:ss CEST yyyy",
                            CultureInfo.InvariantCulture),
                    cq => cq["td"][1].InnerText,
                    cq => cq["td"][2].InnerText);
            }
            return new List<PlaylistEntry>();
        }
    }
}
