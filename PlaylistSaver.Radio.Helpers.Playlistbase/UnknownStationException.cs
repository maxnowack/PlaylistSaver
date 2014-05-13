using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaylistSaver.Radio.Helpers.Playlistbase
{
    public class UnknownStationException : Exception
    {
        public UnknownStationException() : base("The provided Station is unknown!") {}
    }
}
