using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace Hate.Sections
{
    internal class Programas
    {
        public static void Programs(string[] args, string version)
        {
            Console.Clear();
            Console.WriteLine("Choose a program:");
            Console.WriteLine(" ");
            Console.WriteLine("1. Process Hacker");
            Console.WriteLine("2. System Informer");
            Console.WriteLine("3. Previous Files Recovery");
            Console.WriteLine("4. Recuva");
            Console.WriteLine("5. Everything");
            Console.WriteLine("6. WinLiveInfo");
            Console.WriteLine("7. JournalTrace");
            Console.WriteLine("8. RegScanner");
            Console.WriteLine("9. Executed Programs list");
            Console.WriteLine("10. Autopsy");
            Console.WriteLine("11. Journal Files");
            Console.WriteLine("12. OsForensics");
            Console.WriteLine("13. Skript file presence");
            Console.WriteLine("14. Menu");
            Console.WriteLine(" ");
            Console.Write("Choose an option » ");
            string programInput = Console.ReadLine();
            string outputPath = $@"C:\Users\{Environment.UserName}\Hate\Programs\";

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            int programType;
            if (!int.TryParse(programInput, out programType) || programType < 1 || programType > 14)
            {
                Console.WriteLine("Invalid option. Please enter a valid option (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 or 14).");
                Thread.Sleep(1000);
                Console.Clear();
                Programs(args, version);
            }

            if (programType == 1)
            {
                string zipUrl = "https://github.com/processhacker/processhacker/releases/download/v2.39/processhacker-2.39-setup.exe";
                string zipName = "ProcessHacker.exe";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 2)
            {
                string zipUrl = "https://github.com/winsiderss/si-builds/releases/download/3.0.6772/systeminformer-3.0.6772-setup.exe";
                string zipName = "SystemInformer.exe";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 3)
            {
                string zipUrl = "https://www.nirsoft.net/utils/previousfilesrecovery-x64.zip";
                string zipName = "PreviousFileRecovery.zip";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 4)
            {
                string zipUrl = "https://download.ccleaner.com/rcsetup153.exe";
                string zipName = "Recuva.exe";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 5)
            {
                string zipUrl = "https://www.voidtools.com/Everything-1.4.1.1005.x64.zip";
                string zipName = "Everything.zip";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 6)
            {
                string zipUrl = "https://chicho.fun/downloads/WinLiveInfo.exe";
                string zipName = "WinLiveInfo.exe";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 7)
            {
                string zipUrl = "https://chicho.fun/downloads/JournalTrace.exe";
                string zipName = "JournalTrace.exe";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 8)
            {
                string zipUrl = "https://www.nirsoft.net/utils/regscanner-x64.zip";
                string zipName = "RegScanner.zip";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 9)
            {
                string zipUrl = "https://www.nirsoft.net/utils/executedprogramslist.zip";
                string zipName = "ExecutedProgramsList.zip";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 10)
            {
                string zipUrl = "https://github.com/sleuthkit/autopsy/releases/download/autopsy-4.20.0/autopsy-4.20.0-64bit.msi";
                string zipName = "Autopsy.msi";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    Console.WriteLine($"Plesea wait, {zipName} weighs 1GB...");
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 11)
            {
                string zipUrl = "https://chicho.fun/downloads/Journal%20Files.exe";
                string zipName = "JournalFiles.exe";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 12)
            {
                string zipUrl = "https://osforensics.com/downloads/osf.exe";
                string zipName = "OsForensics.exe";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 13)
            {
                string zipUrl = "https://chicho.fun/downloads/SkriptDetector.exe";
                string zipName = "SkriptDetector.exe";
                string zipPath = Path.Combine(outputPath, zipName);
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipUrl, zipPath);
                }

                string ProgramPath = Path.Combine(outputPath, zipName);
                Process.Start(ProgramPath, $"/folder \"{outputPath}\"");
                Console.Clear();
                Thread.Sleep(1000);
                Console.WriteLine($"{zipName} downloaded successfully!");
                Console.WriteLine("\n\nPress ENTER to go to the menu...");
                Console.ReadLine();
                Console.Clear();
                Programs(args, version);
            }
            else if (programType == 14)
            {
                Console.Clear();
                Thread.Sleep(1000);
                Program.GUI(args, version);
            }
        }
    }
}
