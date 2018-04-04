using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivitySampling
{
    public class LogEntry
    {
        public DateTime TimeStamp { get; set; }
        public String Message { get; set; }
        public int Duration { get; set; }

        public override string ToString()
        {
            var duration = Duration > 0 ? Duration.ToString() : string.Empty;
            return $"{TimeStamp}; {Message} {duration}";
        }
    }
}
