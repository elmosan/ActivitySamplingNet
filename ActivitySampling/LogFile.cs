using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;

namespace ActivitySampling
{
    public class LogFile
    {
        private string _filename = @"ActivitySampling.log";

        public void Init()
        {
            var file = new FileInfo(_filename);

            if (file.Exists)
                file.CopyTo($"{_filename.Replace(file.Extension, "")}-{DateTime.Now:yyyyMMddHHmmss}.log.bkp");
        }

        public void Write(string message)
        {
            using (StreamWriter file = new StreamWriter(_filename, true))
            {
                file.WriteLine(message);
            }
        }

        public IEnumerable<string> Load()
        {
            var logs = "";
            var list = new List<string>();

            foreach (var line in File.ReadLines(_filename).Reverse())
            {
                list.Add(line);
            }

            return list;
        }
    }
}
