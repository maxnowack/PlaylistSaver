﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Tool.hbm2ddl;
using PlaylistSaver.Core;
using PlaylistSaver.Storage.NHibernate.Mappings;

namespace PlaylistSaver.Storage.NHibernate
{
    public class NHibernateStorage : IPlaylistStorage
    {
        public static IPlaylistStorage Create()
        {
            return new NHibernateStorage();
        }

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

        public void Store(IEnumerable<PlaylistEntry> entries)
        {
            using (var transaction = session.BeginTransaction())
            {
                foreach (var playlistEntry in entries.Where(playlistEntry => !session.Query<PlaylistEntry>().Any(x=>x.Id == playlistEntry.Id)))
                {
                    session.SaveOrUpdate(playlistEntry);
                }
                transaction.Commit();
            }
        }

        public DateTime GetLastEntry(string stationKey)
        {
            using (session.BeginTransaction())
            {
                return session.QueryOver<PlaylistEntry>()
                    .Where(x => x.Radio == stationKey)
                    .OrderBy(x => x.Time)
                    .Desc.Take(1).List().FirstOrDefault().Time;
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure().Database(MySQLConfiguration.Standard.ConnectionString("Server=localhost;Database=playlistsaver;User ID=root;Password=toor;")).Mappings(x => x.FluentMappings.Add<PlaylistEntryMap>()).BuildSessionFactory();
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