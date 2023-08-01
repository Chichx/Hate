using System;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace Hate.Sections
{
    internal class WinPrefetch
    {
        public static void WinPrefetchView(string[] args, string version)
        {
            Console.Write("Enter a date (MM-dd-yyyy) to filter the Prefetch files: ");
            string dateString = Console.ReadLine();
            Console.WriteLine("Date entered: " + dateString);
            DateTime date = DateTime.ParseExact(dateString, "MM-dd-yyyy", null);
            string prefetchPath = @"C:\Windows\Prefetch";
            string outputPath = $@"C:\Users\{Environment.UserName}\Hate\WinPrefetchView\";

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            DirectoryInfo di = new DirectoryInfo(prefetchPath);
            foreach (FileInfo fileInfo in di.GetFiles("*.pf"))
            {
                Console.WriteLine("File creation date " + fileInfo.Name + ": " + fileInfo.CreationTime.ToString());
                if (fileInfo.LastWriteTime.Date == date.Date)
                {
                    string outputFile = Path.Combine(outputPath, fileInfo.Name);
                    fileInfo.CopyTo(outputFile, true);
                }
            }

            string zipUrl = "https://chicho.fun/downloads/WinPrefetchView.exe";
            string zipPath = Path.Combine(outputPath, "WinPrefetchView.exe");
            using (var client = new WebClient())
            {
                client.DownloadFile(zipUrl, zipPath);
            }

            string winPrefetchViewPath = Path.Combine(outputPath, "WinPrefetchView.exe");
            Process.Start(winPrefetchViewPath, $"/folder \"{outputPath}\"");
            Console.WriteLine("\n\nPress ENTER to go to the menu...");
            Console.ReadLine();
            Console.Clear();
            Program.GUI(args, version);
        }
    }
}
