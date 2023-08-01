using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.ServiceProcess;

using static Hate.Program;

namespace Hate.Sections
{
    internal class AutomaticStringScanner
    {
        private static Detect detect = new Detect();
        private static Dictionary<string, List<string>> jsonData = new Dictionary<string, List<string>>();
        public class Detect
        {
            public void Add(string value, Dictionary<string, List<string>> data, string category)
            {
                data.TryGetValue(category, out List<string> detectionList);
                if (detectionList == null)
                {
                    detectionList = new List<string>();
                    data[category] = detectionList;
                }
                detectionList.Add(value);
            }
        }
        public static void DNSCache(string[] args, string version)

        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nCheck #1:");
                Console.OutputEncoding = Encoding.UTF8;
                string outputPath = $@"C:\Users\{Environment.UserName}\Hate\Strings\";

                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                WebClient client = new WebClient();
                byte[] fileData = client.DownloadData("https://anticheat.site/strings/downloads/");
                string fileDirectory = Path.Combine(outputPath, "xxstrings.exe");
                File.WriteAllBytes(fileDirectory, fileData);

                string url = "https://pastebin.com/raw/XxgJHQ54";
                string content;
                using (WebClient webClient = new WebClient())
                {
                    content = webClient.DownloadString(url);
                }

                Dictionary<string, List<string>> detectionsJson = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(content);


                HashSet<string> valueSet = new HashSet<string>();
                foreach (List<string> values in detectionsJson.Values)
                {
                    foreach (string value in values)
                    {
                        valueSet.Add(value);
                    }
                }

                ServiceController dpsService = ServiceController.GetServices().FirstOrDefault(service => service.ServiceName == "Dnscache");
                if (dpsService != null && dpsService.Status == ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("WARNING: The Check #1 process is stopped and cannot be scanned.");
                    detect.Add($"DNSCache has stopped.", jsonData, "Stopped Services");
                    Thread.Sleep(500);
                    DPSScan(args, version);
                    return;
                }

                int pid = GetServicePID("Dnscache");

                Process process = new Process();
                process.StartInfo.FileName = Path.Combine($"C:\\Users\\{Environment.UserName}\\Hate\\Strings\\xxstrings.exe");
                process.StartInfo.Arguments = $"-p {pid} -l 6";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                string resultado = process.StandardOutput.ReadToEnd().ToLower();
                process.WaitForExit();

                List<string> resultadoList = new List<string>(resultado.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
                resultadoList = new List<string>(new HashSet<string>(resultadoList));
                string contentString = string.Join("\n", resultadoList);

                bool stringsEncontradas = false;

                foreach (string value in valueSet)
                {
                    if (contentString.Contains(value))
                    {
                        foreach (KeyValuePair<string, List<string>> entry in detectionsJson)
                        {
                            if (entry.Value.Contains(value))
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"{entry.Key}");
                                detect.Add($"{entry.Key} due to string: {value}", jsonData, "Dnscache");
                                stringsEncontradas = true;
                                continue;
                            }
                        }
                    }
                }
                if (!stringsEncontradas)
                {
                    Thread.Sleep(500);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDone!");
                Thread.Sleep(500);
                DPSScan(args, version);
            }
            catch
            {
                Console.WriteLine($"Unable to scan Check #1.");
                Thread.Sleep(500);
                DPSScan(args, version);
            }
        }
        public static void DPSScan(string[] args, string version)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nCheck #2:");
                Console.OutputEncoding = Encoding.UTF8;
                string outputPath = $@"C:\Users\{Environment.UserName}\Hate\Strings\";

                WebClient client = new WebClient();

                string url = "https://pastebin.com/raw/RXYCGiKZ";
                string content;
                using (WebClient webClient = new WebClient())
                {
                    content = webClient.DownloadString(url);
                }

                Dictionary<string, List<string>> detectionsJson = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(content);


                HashSet<string> valueSet = new HashSet<string>();
                foreach (List<string> values in detectionsJson.Values)
                {
                    foreach (string value in values)
                    {
                        valueSet.Add(value);
                    }
                }

                ServiceController dpsService = ServiceController.GetServices().FirstOrDefault(service => service.ServiceName == "DPS");
                if (dpsService != null && dpsService.Status == ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("WARNING: The Check #2 process is stopped and cannot be scanned.");
                    detect.Add($"DPS has stopped.", jsonData, "Stopped Services");
                    Thread.Sleep(500);
                    PcaSvcs(args, version);
                    return;
                }

                int pid = GetServicePID("DPS");

                Process process = new Process();
                process.StartInfo.FileName = Path.Combine($"C:\\Users\\{Environment.UserName}\\Hate\\Strings\\xxstrings.exe");
                process.StartInfo.Arguments = $"-p {pid} -l 6";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                string resultado = process.StandardOutput.ReadToEnd().ToLower();
                process.WaitForExit();

                List<string> resultadoList = new List<string>(resultado.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
                resultadoList = new List<string>(new HashSet<string>(resultadoList));
                string contentString = string.Join("\n", resultadoList);

                bool stringsEncontradas = false;

                foreach (string value in valueSet)
                {
                    if (contentString.Contains(value))
                    {
                        foreach (KeyValuePair<string, List<string>> entry in detectionsJson)
                        {
                            if (entry.Value.Contains(value))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"{entry.Key} ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"has been detected!");
                                detect.Add($"{entry.Key} due to string: {value}", jsonData, "DPS");
                                stringsEncontradas = true;
                                continue;
                            }
                        }
                    }
                }
                if (!stringsEncontradas)
                {
                    Thread.Sleep(500);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDone!");
                Thread.Sleep(500);
                PcaSvcs(args, version);
            }
            catch
            {
                Console.WriteLine("Unable to scan Check #2.");
                Thread.Sleep(500);
                PcaSvcs(args, version);
            }
        }

        public static void PcaSvcs(string[] args, string version)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nCheck #3:");
                Console.OutputEncoding = Encoding.UTF8;
                string outputPath = $@"C:\Users\{Environment.UserName}\Hate\Strings\";

                WebClient client = new WebClient();

                string url = "https://pastebin.com/raw/T61UVnC9";
                string content;
                using (WebClient webClient = new WebClient())
                {
                    content = webClient.DownloadString(url);
                }

                Dictionary<string, List<string>> detectionsJson = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(content);


                HashSet<string> valueSet = new HashSet<string>();
                foreach (List<string> values in detectionsJson.Values)
                {
                    foreach (string value in values)
                    {
                        valueSet.Add(value);
                    }
                }

                ServiceController dpsService = ServiceController.GetServices().FirstOrDefault(service => service.ServiceName == "PcaSvc");
                if (dpsService != null && dpsService.Status == ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("WARNING: The Check #3 process is stopped and cannot be scanned.");
                    detect.Add($"PcaSvc has stopped.", jsonData, "Stopped Services");
                    Thread.Sleep(500);
                    LsassScan(args, version);
                    return;
                }

                int pid = GetServicePID("PcaSvc");

                Process process = new Process();
                process.StartInfo.FileName = Path.Combine($"C:\\Users\\{Environment.UserName}\\Hate\\Strings\\xxstrings.exe");
                process.StartInfo.Arguments = $"-p {pid} -l 6";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                string resultado = process.StandardOutput.ReadToEnd().ToLower();
                process.WaitForExit();

                List<string> resultadoList = new List<string>(resultado.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
                resultadoList = new List<string>(new HashSet<string>(resultadoList));
                string contentString = string.Join("\n", resultadoList);

                bool stringsEncontradas = false;

                foreach (string value in valueSet)
                {
                    if (contentString.Contains(value))
                    {
                        foreach (KeyValuePair<string, List<string>> entry in detectionsJson)
                        {
                            if (entry.Value.Contains(value))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"{entry.Key} ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"has been detected!");
                                detect.Add($"{entry.Key} due to string: {value}", jsonData, "PcaSvc");
                                stringsEncontradas = true;
                                continue;
                            }
                        }
                    }
                }
                if (!stringsEncontradas)
                {
                    Thread.Sleep(500);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDone!");
                Thread.Sleep(500);
                LsassScan(args, version);
            }
            catch
            {
                Console.WriteLine($"Unable to scan Check #3.");
                Thread.Sleep(500);
                LsassScan(args, version);
            }
        }

        public static void LsassScan(string[] args, string version)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nCheck #4:");
                Console.OutputEncoding = Encoding.UTF8;
                string outputPath = $@"C:\Users\{Environment.UserName}\Hate\Strings\";

                WebClient client = new WebClient();

                string url = "https://pastebin.com/raw/KetmuxGt";
                string content;
                using (WebClient webClient = new WebClient())
                {
                    content = webClient.DownloadString(url);
                }

                Dictionary<string, List<string>> detectionsJson = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(content);

                HashSet<string> valueSet = new HashSet<string>();
                foreach (List<string> values in detectionsJson.Values)
                {
                    foreach (string value in values)
                    {
                        valueSet.Add(value);
                    }
                }

                Process lsassProcess = Process.GetProcessesByName("lsass").FirstOrDefault();
                if (lsassProcess == null)
                {
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("WARNING: The Check #4 process cannot be scanned.");
                    detect.Add($"Lsass has stopped.", jsonData, "Stopped Services");
                    Thread.Sleep(500);
                    return;
                }

                int pid = lsassProcess.Id;

                Process process = new Process();
                process.StartInfo.FileName = Path.Combine($"C:\\Users\\{Environment.UserName}\\Hate\\Strings\\xxstrings.exe");
                process.StartInfo.Arguments = $"-p {pid} -l 6";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                string resultado = process.StandardOutput.ReadToEnd().ToLower();
                process.WaitForExit();

                List<string> resultadoList = new List<string>(resultado.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
                resultadoList = new List<string>(new HashSet<string>(resultadoList));
                string contentString = string.Join("\n", resultadoList);

                bool stringsEncontradas = false;

                foreach (string value in valueSet)
                {
                    if (contentString.Contains(value))
                    {
                        foreach (KeyValuePair<string, List<string>> entry in detectionsJson)
                        {
                            if (entry.Value.Contains(value))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"{entry.Key} ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"has been detected!");
                                detect.Add($"{entry.Key} due to string: {value}", jsonData, "Lsass");
                                stringsEncontradas = true;
                                continue;
                            }
                        }
                    }
                }
                if (!stringsEncontradas)
                {
                    Thread.Sleep(500);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDone!");
                Console.WriteLine("Please wait for result!");
                Thread.Sleep(500);
            }
            catch
            {
                Console.WriteLine($"Unable to scan Check #4.");
                Console.ReadLine();
            }
            string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            string filePath = $@"C:\Users\{Environment.UserName}\Hate\Strings\detections.json";
            File.WriteAllText(filePath, json);
            SendWebhook().Wait();
            CheckUser(args, version);
        }
    }
}
