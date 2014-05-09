using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PlaylistSaver.Core;

namespace PlaylistSaver
{
    class Program
    {
        /*static void LoadAdditionalAssemblies()
        {
            var folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (var file in Directory.GetFiles(folderPath, "PlaylistSaver.Radio.*.dll"))
            {
                Debug.WriteLine(file);
                Assembly.LoadFile(file);
            }
        }*/

        static void Main(string[] args)
        {
            //LoadAdditionalAssemblies();

            var instances = Factory<IPlaylistSaver>.CreateInstanceList().ToList();
            var entries = new List<PlaylistEntry>();
            foreach (var playlistSaver in instances)
            {
                var times = new List<DateTime>();
                try
                {
                    times = playlistSaver.GetAvailableTimes().GetTimesForInterval(playlistSaver.DefaultInterval);
                }
                catch (NotImplementedException nie)
                {
                    Debug.WriteLine(nie);
                }

                foreach (var dateTime in times)
                {
                    try
                    {
                        entries.AddRange(playlistSaver.GetEntrys(dateTime));
                    }
                    catch (WebException we)
                    {
                        Debug.WriteLine(we);
                    }
                    catch (NotImplementedException nie)
                    {
                        Debug.WriteLine(nie);
                    }
                }
            }

            var unique = entries.Distinct();



            Console.WriteLine("{0}/{1}", unique.Count(), entries.Count());
            Console.ReadLine();
        } 
    }
}
