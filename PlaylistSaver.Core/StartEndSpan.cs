using System;
using System.Collections.Generic;

namespace PlaylistSaver.Core
{
    public class StartEndSpan
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TimeSpan TimeSpan
        {
            get
            {
                return End.Subtract(Start);
            }
        }

        public StartEndSpan(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public List<DateTime> GetTimesForInterval(TimeSpan interval)
        {
            var retVal = new List<DateTime>();
            for (var current = Start; current <= End; current += interval)
            {
                retVal.Add(current);
            }
            return retVal;
        }
    }
}
