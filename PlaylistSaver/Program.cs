using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                try
                {
                    var times = playlistSaver.GetAvailableTimes().GetTimesForInterval(playlistSaver.DefaultInterval);
                    foreach (var dateTime in times)
                    {
                        entries.AddRange(playlistSaver.GetEntrys(dateTime));
                    }
                }
                catch (NotImplementedException nie)
                {
                    Debug.WriteLine(nie);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            Console.WriteLine(instances.Count());
            Console.ReadLine();
        } 
    }
}
