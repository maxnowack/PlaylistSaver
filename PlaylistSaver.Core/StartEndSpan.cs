using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return this.End.Subtract(this.Start);
            }
        }

        public StartEndSpan(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }

        public List<DateTime> GetTimesForInterval(TimeSpan interval)
        {
            var retVal = new List<DateTime>();
            for (var current = this.Start; current <= this.End; current += interval)
            {
                retVal.Add(current);
            }
            return retVal;
        }
    }
}
