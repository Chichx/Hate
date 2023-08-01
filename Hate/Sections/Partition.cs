using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hate.Sections
{
    internal class Partition
    {
        class PartitionInfo
        {
            public PartitionInfo(char letter, bool isMounted)
            {
                this.Letter = letter;
                this.IsMounted = isMounted;
            }

            public char Letter { get; }
            public bool IsMounted { get; }
        }

        class DiskLog
        {
            public DiskLog(string name, DateTime? generatedAt, long? recordIdentifier)
            {
                this.Name = name;
                this.Time = generatedAt;
                this.Id = recordIdentifier;
            }

            public string Name { get; }
            public DateTime? Time { get; }
            public long? Id { get; }
        }

        public static void Partitions(string[] args, string version)
        {
            List<PartitionInfo> partitionsInfo = new List<PartitionInfo>();
            getPartitions().ForEach(p => partitionsInfo.Add(new PartitionInfo(p, isMounted(p))));
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Partitions\n-------------------------------\n");
            partitionsInfo.ForEach(i =>
            {
                Console.WriteLine("Letter: {0}\n => Mounted: {1}", i.Letter, i.IsMounted);
            });

            Console.WriteLine();

            Console.WriteLine("Disks logs\n-------------------------------\n");
            getDisksLogs(args, version).ForEach(l =>
            {
                Console.WriteLine("Disk name: {0}\n => Generated at: {1}\n => Record ID: {2}",
                    l.Name, l.Time, l.Id);
            });

            Console.WriteLine();

            Console.WriteLine("USB storages\n-------------------------------\n");
            getRemovableStorages(args, version).ForEach(s => Console.WriteLine(s));

            Console.Write("\n\nPress ENTER to go to the menu...");
            Console.ReadLine();
            Console.Clear();
            Program.GUI(args, version);
        }

        static List<char> getPartitions()
        {
            List<char> partitions = new List<char>();
            Regex regex = new Regex(@"^\\DosDevices\\(\w):$");
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\MountedDevices"); // Muestra volumenes activos de tu dispositivo
            string[] values = key.GetValueNames();

            foreach (string v in values)
            {
                Match match = regex.Match(v);

                if (!match.Success) continue;

                string partition = match.Groups[1].Value;
                partitions.Add(Convert.ToChar(partition));
            }

            return partitions;
        }

        static List<string> getRemovableStorages(string[] args, string version)
        {
            List<string> storages = new List<string>();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USBSTOR"); // Ver si el usuario tiene o removio algun USB
            if (key != null)
            {
                string[] storagesKeys = key.GetSubKeyNames();


                storagesKeys.ToList().ForEach(k =>
                {
                    RegistryKey storageKey = key.OpenSubKey(k);
                    RegistryKey storageInfoKey = storageKey.OpenSubKey(storageKey.GetSubKeyNames()[0]);
                    string storage = storageInfoKey.GetValue("FriendlyName").ToString();
                    storages.Add(storage);
                });
            }
            else
            {
                Console.WriteLine("USBSTOR key cannot be found in the registry.");
                Console.Write("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Program.GUI(args, version);
            }

            return storages;
        }

        static List<DiskLog> getDisksLogs(string[] args, string version)
        {
            EventRecord entry;
            List<DiskLog> disksLogs = new List<DiskLog>();
            string logPath = @"C:\Windows\System32\winevt\Logs\Microsoft-Windows-StorageSpaces-Driver%4Operational.evtx";
            EventLogReader logReader = new EventLogReader(logPath, PathType.FilePath);
            DateTime pcStartTime = Program.startTime();

            if (logPath != null)
            {
                while ((entry = logReader.ReadEvent()) != null)
                {
                    if (entry.Id != 207) continue; // Esta id muestra los espacios de almacenamiento
                    if (entry.TimeCreated <= pcStartTime) continue;

                    IList<EventProperty> properties = entry.Properties;
                    string driveManufacturer = properties[3].Value.ToString();
                    string driveModelNumber = properties[4].Value.ToString();

                    if (driveManufacturer == "NULL") driveManufacturer = "";
                    else driveManufacturer += " ";

                    disksLogs.Add(new DiskLog($"{driveManufacturer}{driveModelNumber}",
                        entry.TimeCreated, entry.RecordId));
                }
            }
            else
            {
                Console.WriteLine("Microsoft Windows StorageSpaces Driver cannot be found.");
                Console.Write("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Program.GUI(args, version);
            }
            return disksLogs;
        }

        static bool isMounted(char partition)
        {
            return Directory.Exists($"{partition}:");
        }
    }
}
