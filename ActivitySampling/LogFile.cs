using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivitySampling
{
    public class LogFile
    {
        private string _filename = @"ActivitySampling.log";

        public void Write(string message)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(_filename, true))
            {
                file.WriteLine(message);
            }
        }
    }
}
