using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using log4net;
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
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger(typeof(Program));
            logger.Info("PlaylistSaver started");
            //LoadAdditionalAssemblies();

            var instances = Factory<IPlaylistSaver>.CreateInstanceList().ToList();
            logger.Info(string.Format("{0} savers found", instances.Count));

            var storage = Factory<IPlaylistStorage>.CreateInstance();
            logger.Info(string.Format("use storage engine {0}" , storage.ToString()));

            foreach (var playlistSaver in instances)
            {
                logger.Info(string.Format("{0} starting...", playlistSaver.Name));
                var times = new List<DateTime>();
                try
                {
                    var availTimes = playlistSaver.GetAvailableTimes();
                    try
                    {
                        var lastRun = storage.GetLastEntry(playlistSaver.Name);
                        if(lastRun!=null && lastRun!=DateTime.MinValue) availTimes.Start = lastRun;
                        logger.Info(string.Format("LastRun for {0} is {1:yyyy-MM-dd HH:mm}", playlistSaver.Name, lastRun));
                    }
                    catch
                    {
                        logger.Info(string.Format("Cannot get the last entry for {0}. Maybe there are no entries...",
                            playlistSaver.Name));
                    }

                    times = availTimes.GetTimesForInterval(playlistSaver.DefaultInterval);
                }
                catch (NotImplementedException nie)
                {
                    logger.Warn(string.Format("GetAvailableTime() is not implemented in {0}", playlistSaver.Name), nie);
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }

                foreach (var dateTime in times)
                {
                    //logger.Info(string.Format("{0}: {1:yyyy-MM-dd HH:mm}", playlistSaver.Name, dateTime));
                    try
                    {
                        var entries = playlistSaver.GetEntrys(dateTime);
                        logger.Info(string.Format("found {0} entries for {2} on {1:yyyy-MM-dd HH:mm}", entries.Count,
                            dateTime, playlistSaver.Name));
                        storage.Store(entries);
                        logger.Info(string.Format("{0} entries stored for {2} on {1:yyyy-MM-dd HH:mm}", entries.Count,
                            dateTime, playlistSaver.Name));
                    }
                    catch (WebException we)
                    {
                        logger.Warn(we);
                        break;
                    }
                    catch (NotImplementedException nie)
                    {
                        logger.Warn(nie);
                        break;
                    }
                    catch (Exception e)
                    {
                        logger.Error(e);
                        break;
                    }
                }
            }
        } 
    }
}
