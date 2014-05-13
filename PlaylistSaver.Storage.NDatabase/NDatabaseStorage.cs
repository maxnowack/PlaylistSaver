using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistSaver.Storage.NDatabase
{
    using System.Security.Cryptography;

    using global::NDatabase;

    using PlaylistSaver.Core;

    public class NDatabaseStorage : IPlaylistStorage
    {
        public static IPlaylistStorage Create()
        {
            return new NDatabaseStorage();
        }

        private const string DbFileName = "playlist.db";
        public void Store(IEnumerable<PlaylistEntry> entries)
        {
            using (var odb = OdbFactory.Open(DbFileName))
            {
                foreach (var entry in from entry in entries let results = (from e in odb.AsQueryable<PlaylistEntry>() where e.Equals(entry) select e) where !results.Any() select entry)
                {
                    odb.Store(entry);
                }
            }
        }
    }
}
