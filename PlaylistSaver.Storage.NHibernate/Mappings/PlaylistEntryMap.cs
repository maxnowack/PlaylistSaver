using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using PlaylistSaver.Core;

namespace PlaylistSaver.Storage.NHibernate.Mappings
{
    public class PlaylistEntryMap : ClassMap<PlaylistEntry>
    {
        public PlaylistEntryMap()
        {
            Id(x => x.Id);
            Map(x => x.Radio);
            Map(x => x.Time);
            Map(x => x.Artist);
            Map(x => x.Title);
        }
    }
}
