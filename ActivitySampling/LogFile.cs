using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;

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

        public LogEntry CreateLogEntry(string text)
        {
            return new LogEntry
            {
                TimeStamp =  DateTime.Now,
                Message = text
            };
        }

        public LogEntry Write(string message)
        {
            var entry = CreateLogEntry(message);

            WritePlain(entry.ToString());

            return entry;
        }

        public void WritePlain(string message)
        {
            using (StreamWriter file = new StreamWriter(_filename, true))
            {
                file.WriteLine(message);
            }
        }

        public IEnumerable<LogEntry> Load()
        {
            var list = new List<LogEntry>();

            foreach (var line in File.ReadLines(_filename).Reverse())
            {
                var msg = line.Split(';')[1].Substring(1);
                var date = line.Substring(0, 10).Replace(".", "");
                var lastDate = DateTime.Now.AddDays(-6).ToString("yyyyMMdd");

                date = $"{date.Substring(4, 4)}{date.Substring(2, 2)}{date.Substring(0, 2)}";

                if (int.Parse(date) >= int.Parse(lastDate))
                {
                    var lines = line.Split(':');
                    var dates = lines[0].Split('.');
                    var entry = new LogEntry { Message = msg, TimeStamp = new DateTime(
                        int.Parse(dates[2].Substring(0, 4)), 
                        int.Parse(dates[1]), 
                        int.Parse(dates[0]),
                        int.Parse(lines[0].Substring(11,2)),
                        int.Parse(lines[1]),
                        int.Parse(lines[2].Substring(0,2)))
                    };
                    list.Add(entry);
                }
            }

            return list;
        }
    }
}
