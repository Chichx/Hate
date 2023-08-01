using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hate.Sections
{
    internal class ExecutedProgram
    {
        // ExecutedPrograms
        class ProgramInfo
        {
            public ProgramInfo(string fileName, DateTime? lastModified,
                DateTime? createdOn, double size)
            {
                this.FileName = fileName;
                this.LastModified = lastModified;
                this.CreatedOn = createdOn;
                this.Size = size;
            }

            public string FileName { get; }
            public DateTime? LastModified { get; }
            public DateTime? CreatedOn { get; }
            public double Size { get; }
        }

        enum Order
        {
            FileName,
            LastModified,
            CreatedOn,
            Size,
            Random
        }

        static HashSet<string> programs = new HashSet<string>();

        public static void ExecutedPrograms(string[] args, string version)
        {
            Console.ForegroundColor = ConsoleColor.White;
            List<ProgramInfo> programsInfo = new List<ProgramInfo>();
            Order orderFlag = Order.Random;
            bool save = false; string savePath = string.Empty;

            FileStream fstream = new FileStream(@"C:\tmpout.txt",
                FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fstream);
            TextWriter oldOut = Console.Out;

            if (args.Length > 0)
            {
                if (args.Contains("-orderby"))
                {
                    int index = Array.IndexOf(args, "-orderby");
                    string argValue = args[index + 1].ToLower();
                    if (argValue != "filename" && argValue != "lastmodified"
                        && argValue != "createdon" && argValue != "size")
                    {
                        Console.WriteLine("[!] Missing argument value: " +
                        "(filename|lastmodified|createdon|size");
                        Environment.Exit(-1);
                    }
                    if (argValue == "filename")
                        orderFlag = Order.FileName;
                    else if (argValue == "lastmodified")
                        orderFlag = Order.LastModified;
                    else if (argValue == "createdon")
                        orderFlag = Order.CreatedOn;
                    else if (argValue == "size")
                        orderFlag = Order.Size;
                    else
                    {
                        Console.WriteLine("[!] Invalid argument value");
                        Environment.Exit(-1);
                    }
                }

                if (args.Contains("-save"))
                {
                    int index = Array.IndexOf(args, "-save");
                    save = true; savePath = args[index + 1];
                }
            }

            getStore(args, version); getAppSwitched(args, version); getMuiCache(args, version);

            foreach (string p in programs)
                programsInfo.Add(getProgramInfo(p));

            if (orderFlag == Order.FileName)
                programsInfo = programsInfo.OrderBy(p => p.FileName).ToList();
            if (orderFlag == Order.LastModified)
                programsInfo = programsInfo.OrderBy(p => p.LastModified).ToList();
            if (orderFlag == Order.CreatedOn)
                programsInfo = programsInfo.OrderBy(p => p.CreatedOn).ToList();
            if (orderFlag == Order.Size)
                programsInfo = programsInfo.OrderBy(p => p.Size).ToList();

            if (save)
            {
                fstream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(fstream);
                Console.SetOut(writer);
            }

            Console.WriteLine("Hate | ExecutedPrograms\n\n");

            foreach (ProgramInfo info in programsInfo)
            {
                string lastModified, createdOn, size;

                if (info.LastModified == null) lastModified = string.Empty;
                else lastModified = info.LastModified.ToString();

                if (info.CreatedOn == null) createdOn = string.Empty;
                else createdOn = info.CreatedOn.ToString();

                if (info.Size == 0) size = string.Empty;
                else size = info.Size.ToString() + "MB";

                Console.WriteLine("File: {0}\n => Last modified: {1}\n" +
                    " => Created on: {2}\n => Size: {3}",
                    info.FileName, lastModified, createdOn, size);
                Console.WriteLine("\n\n");
            }

            Console.SetOut(oldOut); writer.Close(); fstream.Close();

            Console.Write("\n\nPress ENTER to go to the menu...");
            if (!save) Console.ReadLine();
            Console.Clear();
            Program.GUI(args, version); 
           }


        static void getStore(string[] args, string version)
        {
            string registryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store";

            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);
            if (key == null)
            {
                Console.WriteLine($"The specified path was not found. ({registryPath})");
                Thread.Sleep(2000);
                Console.Clear();
                getAppSwitched(args, version);
                return;
            }

            Regex rgx = new Regex(@"^\w:\\.+.exe$");
            string[] values = key.GetValueNames();

            foreach (string v in values)
            {
                if (!Char.IsUpper(v[0])) continue;

                Match match = rgx.Match(v);
                if (!match.Success) continue;

                string program = match.Groups[0].Value;
                programs.Add(program);
            }
        }

        static void getAppSwitched(string[] args, string version)
        {
            string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FeatureUsage\AppSwitched";

            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);
            if (key == null)
            {
                Console.WriteLine($"The specified path was not found. ({registryPath})");
                Thread.Sleep(2000);
                Console.Clear();
                getMuiCache(args, version);
                return;
            }

            Regex rgx = new Regex(@"^\w:\\.+.exe$");
            string[] values = key.GetValueNames();

            foreach (string v in values)
            {
                if (!Char.IsUpper(v[0])) continue;

                Match match = rgx.Match(v);
                if (!match.Success) continue;

                string program = match.Groups[0].Value;
                programs.Add(program);
            }
        }
        static void getMuiCache(string[] args, string version)
        {
            string registryPath = @"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache";

            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);

            if (key == null)
            {
                Console.WriteLine($"No se encontró la ruta especificada. ({registryPath})");
                Thread.Sleep(2000);
                Console.Clear();
                Program.GUI(args, version);
                return;
            }

            Regex rgx = new Regex(@"^(\w:\\.+.exe)(.FriendlyAppName|.ApplicationCompany)$"); // MuiCache muestra aplicaciones.
            string[] values = key.GetValueNames();

            foreach (string v in values)
            {
                Match match = rgx.Match(v);
                if (!match.Success) continue;

                string program = match.Groups[1].Value;
                programs.Add(program);
            }
        }

        static ProgramInfo getProgramInfo(string fileName)
        {
            DateTime? lastModified = null, createdOn = null;
            double megaBytes = 0;
            FileInfo program = new FileInfo(fileName);

            if (program.Exists)
            {
                lastModified = program.LastWriteTime;
                createdOn = program.CreationTime;
                megaBytes = program.Length / 1048576d;
            }

            return new ProgramInfo(fileName, lastModified, createdOn, Math.Round(megaBytes, 2));
        }
    }
}
