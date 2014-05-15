using System;

namespace PlaylistSaver.Radio.Helpers.Playlistbase
{
    public class EmptyResultException : Exception
    {
        public EmptyResultException() : base("no results returned!") { }
    }
}
