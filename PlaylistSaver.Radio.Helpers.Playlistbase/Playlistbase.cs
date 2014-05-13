using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistSaver.Radio.Helpers.Playlistbase
{
    using System.Net;

    using CsQuery;

    using PlaylistSaver.Core;

    public class Playlistbase
    {
        private const string GetEntriesUrl =
            "http://de.playlistbase.com/de/playlist/get-playlist-songs/station/{0}/date/{1:yyyy-MM-dd}";

        public static List<PlaylistEntry> GetEntries(string stationKey, string stationName, DateTime day)
        {
            var web = new WebClient();
            var html = web.DownloadString(string.Format(GetEntriesUrl, stationKey, day));

            if (string.IsNullOrEmpty(html.Trim())) throw new UnknownStationException();
            if (html.Contains("Dieser online Radiosender hat keine Playlist-Informationen verf")) return new List<PlaylistEntry>();

            var doc = CQ.Create(html);
            var items = doc["tr"].ToList();

            return (from item in items
                    select CQ.Create(item.InnerHTML)
                    into itemQuery
                    let artist = itemQuery["span.artist-name"][0].InnerText
                    let title = itemQuery["span.song-name"][0].InnerText
                    let time = DateTime.Parse(string.Format("{0:yyyy-MM-dd} {1}", day, itemQuery["td.time-cell"][0].InnerText.Trim()))
                    select new PlaylistEntry() { Artist = artist, Title = title, Time = time, Radio = stationName })
                .ToList();
        }
    }
}
