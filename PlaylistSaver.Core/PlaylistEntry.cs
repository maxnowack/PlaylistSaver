using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaylistSaver.Core
{
    public class PlaylistEntry
    {
        public string Radio { get; set; }
        public DateTime Time { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }

        protected bool Equals(PlaylistEntry other)
        {
            return string.Equals(Radio, other.Radio) && Time.Equals(other.Time) && string.Equals(Artist, other.Artist) && string.Equals(Title, other.Title);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Radio != null ? Radio.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Time.GetHashCode();
                hashCode = (hashCode*397) ^ (Artist != null ? Artist.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Title != null ? Title.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlaylistEntry) obj);
        }
    }
}
