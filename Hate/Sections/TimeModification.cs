using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Threading;

namespace Hate.Sections
{
    internal class TimeModification
    {
        public class CheckInfo
        {
            public CheckInfo(bool result, DateTime previousTime, DateTime newTime, DateTime? generatedAt, long? recordIdentifier)
            {
                this.Result = result;
                this.Previous = previousTime;
                this.New = newTime;
                this.Time = generatedAt;
                this.Id = recordIdentifier;
            }

            public CheckInfo(bool result)
            {
                this.Result = result;
            }

            public bool Result { get; }
            public DateTime Previous { get; }
            public DateTime New { get; }
            public DateTime? Time { get; }
            public long? Id { get; }

        };

        public static void Modification(string[] args, string version)
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Analyzing logs...\n\n");
            CheckInfo info = checkTimeModification();
            Thread.Sleep(2000);
            if (info.Result)
            {
                Console.WriteLine("[!] Find changed dates!");
                Console.WriteLine("Previous time: {0} | New Time: {1}\nGenerated at: {2} | Record ID: {3}\n\n",
                    info.Previous, info.New, info.Time, info.Id);
            }
            else Console.WriteLine("[?] It seems that this person did not change anything!");

            Console.Write("\n\nPress ENTER to go to the menu...");
            Console.ReadLine();
            Console.Clear();
            Program.GUI(args, version);
        }

        static CheckInfo checkTimeModification()
        {
            EventRecord entry;
            string logPath = @"C:\Windows\System32\winevt\Logs\Security.evtx";
            EventLogReader logReader = new EventLogReader(logPath, PathType.FilePath);
            DateTime pcStartTime = Program.startTime();

            while ((entry = logReader.ReadEvent()) != null)
            {
                if (entry.Id != 4616) continue; // Esta id ve la fecha de modificacion del dispositivo.
                if (entry.TimeCreated <= pcStartTime) continue;

                IList<EventProperty> properties = entry.Properties;
                DateTime previousTime = DateTime.Parse(properties[4].Value.ToString());
                DateTime newTime = DateTime.Parse(properties[5].Value.ToString());

                if (Math.Abs((previousTime - newTime).TotalMinutes) > 5)
                    return new CheckInfo(true, previousTime, newTime, entry.TimeCreated, entry.RecordId);
            }
            return new CheckInfo(false);
        }
    }
}
