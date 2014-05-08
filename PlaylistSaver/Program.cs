using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaylistSaver.Core;

namespace PlaylistSaver
{
    class Program
    {
        static void Main(string[] args)
        {
            var instances = Factory<IPlaylistSaver>.CreateInstanceList();

            var entries = new List<PlaylistEntry>();
            foreach (var playlistSaver in instances)
            {
                var times = playlistSaver.GetAvailableTimes().GetTimesForInterval(TimeSpan.FromMinutes(10));
                foreach (var dateTime in times)
                {
                    entries.AddRange(playlistSaver.GetEntrys(dateTime));   
                }
            }
            Console.WriteLine(instances.Count());
            Console.ReadLine();
        } 
    }
}
