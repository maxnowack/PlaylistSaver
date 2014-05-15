using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDatabase.Api;
using NDatabase.Exceptions;

namespace PlaylistSaver.Storage.NDatabase
{

    using global::NDatabase;

    using PlaylistSaver.Core;

    public class NDatabaseStorage : IPlaylistStorage
    {
        private IOdb odb = null;
        public static IPlaylistStorage Create()
        {
            return new NDatabaseStorage();
        }

        public NDatabaseStorage()
        {
            //OdbConfiguration.EnableBTreeValidation();
            odb = OdbFactory.Open(DbFileName);
        }

        private const string DbFileName = "playlist.db";
        public void Store(IEnumerable<PlaylistEntry> entries)
        {
            foreach (var entry in from entry in entries let results = (from e in odb.AsQueryable<PlaylistEntry>() where e.Equals(entry) select e) where !results.Any() select entry)
            {
                odb.Store(entry);
            }
        }
        //public void Store(IEnumerable<PlaylistEntry> entries)
        //{
        //    if (!odb.IndexManagerFor<PlaylistEntry>().ExistIndex("playlistIndex"))
        //    {
        //        odb.IndexManagerFor<PlaylistEntry>()
        //            .AddUniqueIndexOn("playlistIndex", new[] {"Radio", "Time", "Artist", "Title"});
        //    }

        //    foreach (var entry in entries)
        //    {
        //        try
        //        {
        //            odb.Store(entry);
        //        }
        //        catch (DuplicatedKeyException duplicatedKeyException)
        //        {
        //            odb.Dispose();
        //            odb = OdbFactory.Open(DbFileName);
        //        }
        //    }
        //}

        ~NDatabaseStorage()
        {
            odb.Dispose();
        }

        public DateTime GetLastEntry(string stationKey)
        {
            return (from e in odb.QueryAndExecute<PlaylistEntry>()
                    where e.Radio == stationKey
                    orderby e.Time descending
                    select e.Time).FirstOrDefault();
        }
    }
}
