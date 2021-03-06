﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using PlaylistSaver.Core;
using PlaylistSaver.Storage.NHibernate.Mappings;

namespace PlaylistSaver.Storage.NHibernate
{
    public class NHibernateStorage : IPlaylistStorage
    {

        private FixedSizedQueue<PlaylistEntry> queue = new FixedSizedQueue<PlaylistEntry>(100);  
        public static IPlaylistStorage Create()
        {
            return new NHibernateStorage();
        }
#if DEBUG
        private const string ConnectionString = "Server=192.168.2.105;Database=playlistsaver;User ID=root;Password=dPfaSiMy5ql?;";
#else
        private const string ConnectionString = "Server=localhost;Database=playlistsaver;User ID=root;Password=dPfaSiMy5ql?;";
#endif
        private ISessionFactory sessionFactory = null;
        private ISession session = null;

        public NHibernateStorage()
        {
            sessionFactory = CreateSessionFactory();
            session = sessionFactory.OpenSession();
        }

        ~NHibernateStorage()
        {
            session.Dispose();
        }

        public long Store(IEnumerable<PlaylistEntry> entries)
        {
            long retVal = 0;
            foreach (var playlistEntry in entries.Where(playlistEntry => !queue.Contains(playlistEntry)))
            {
                try
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(playlistEntry);
                        transaction.Commit();
                    }
                }
                catch (NonUniqueObjectException)
                {
                    
                }
                queue.Enqueue(playlistEntry);
                retVal++;
            }
            return retVal;
        }

        public DateTime GetLastEntry(string stationKey)
        {
            using (session.BeginTransaction())
            {
                queue = new FixedSizedQueue<PlaylistEntry>(100);
                foreach (var entry in session.QueryOver<PlaylistEntry>().Where(x=>x.Radio==stationKey).OrderBy(x=>x.Time).Desc.Take(100).List())
                {
                    queue.Enqueue(entry);
                }

                return session.QueryOver<PlaylistEntry>()
                    .Where(x => x.Radio == stationKey)
                    .OrderBy(x => x.Time)
                    .Desc.Take(1).List().FirstOrDefault().Time;
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure().Database(MySQLConfiguration.Standard.ConnectionString(ConnectionString)).Mappings(x => x.FluentMappings.Add<PlaylistEntryMap>()).BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
           /* new SchemaExport(config)
              .Create(false, true);*/
        }
    }
}
