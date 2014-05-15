using System;

namespace PlaylistSaver.Radio.Helpers.Playlistbase
{
    public class UnknownStationException : Exception
    {
        public UnknownStationException() : base("The provided Station is unknown!") {}
    }
}
