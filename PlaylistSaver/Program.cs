using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            var storage = Factory<IPlaylistStorage>.CreateInstance();
            var entries = new List<PlaylistEntry>();

            var startTime = DateTime.Now;

            foreach (var playlistSaver in instances)
            {
                var times = new List<DateTime>();
                try
                {
                    var availTimes = playlistSaver.GetAvailableTimes();
                    if (Properties.Settings.Default.LastRun > availTimes.Start) availTimes.Start = Properties.Settings.Default.LastRun;

                    times = availTimes.GetTimesForInterval(playlistSaver.DefaultInterval);
                }
                catch (NotImplementedException nie)
                {
                    Debug.WriteLine(nie);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("cannot get available times for {0}", playlistSaver.Name);
                }

                foreach (var dateTime in times)
                {
                    Console.WriteLine("{0}: {1:yyyy-MM-dd HH:mm}", playlistSaver.Name, dateTime);
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
                    catch (Exception e)
                    {
                        Debug.WriteLine(
                            "cannot get entries in {1:yyyy-MM-dd HH:mm} for {0}",
                            playlistSaver.Name,
                            dateTime);
                    }
                }
            }
            Properties.Settings.Default.LastRun = startTime;
            Properties.Settings.Default.Save();

            var unique = entries.Distinct().ToList();
            if(storage!=null) storage.Store(unique);

        } 
    }
}
