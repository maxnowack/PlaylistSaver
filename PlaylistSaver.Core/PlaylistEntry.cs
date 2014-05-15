using System;

namespace PlaylistSaver.Core
{
    public class PlaylistEntry
    {
        public virtual string Radio { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual string Artist { get; set; }
        public virtual string Title { get; set; }

        protected bool Equals(PlaylistEntry other)
        {
            return string.Equals(Radio, other.Radio) && Time.Equals(other.Time) && string.Equals(Artist, other.Artist) && string.Equals(Title, other.Title);
        }

        public virtual string Id
        {
            get { return string.Format("{0}|{1:yyyy-MM-dd HH:mm}", Radio, Time); }
            set
            {
                var tmp = value.Split('|');
                Radio = tmp[0];
                Time = DateTime.Parse(tmp[1]);
            }
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
            if (obj.GetType() != GetType()) return false;
            return Equals((PlaylistEntry) obj);
        }

        public override string ToString()
        {
            return string.Format("{1} - {2} at {3:yyyy-MM-dd HH:mm} {0}", Radio, Artist, Title, Time);
        }
    }
}
