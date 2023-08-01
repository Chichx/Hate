using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Hate.Sections
{
    internal class AmCache
    {
        public static void Amcache(string[] args, string version)
        {
            string username = Environment.UserName;
            string Hatedir = $@"C:\Users\{username}\Hate\Amcachehash";
            if (Directory.Exists(Hatedir))
            {
                Directory.Delete(Hatedir, true);
            }

            Directory.CreateDirectory($@"C:\Users\{username}\Hate\Amcachehash");

            string virusTotalApiKey = "55f3072a9e14add3e2641fa6340c5d0494119c46c8b2d124cf56155ae52d5dc0";
            Console.WriteLine($"[!] I recommend to use your private api to avoid Public APIs saturation");
            Console.Write("Custom VirusTotal API Key: ");
            string newApiKey = Console.ReadLine().Trim();
            if (!string.IsNullOrEmpty(newApiKey))
            {
                virusTotalApiKey = newApiKey;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Amcache hash detector");
            Amcache();
            Parse(args, version);

            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Query the entire Amcache.");
            Console.WriteLine("2. Query Amcache data since last reboot. [Reduces API ratelimit]");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Clear();
                Read(args, version);
            }
            else if (choice == "2")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Clear();
                ReadBoot(args, version);
            }

            using (StreamReader reader = new StreamReader($@"C:\Users\{username}\Hate\Amcachehash\res.json"))
            {
                using (StreamWriter writer = new StreamWriter($@"C:\Users\{username}\Hate\Amcachehash\res.txt", append: false, encoding: Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        dynamic data = JsonConvert.DeserializeObject(line);
                        string sha1 = data.SHA1;
                        string fullPath = data.FullPath;
                        string name = data.Name;
                        string fileKeyLastWriteTimestamp = data.FileKeyLastWriteTimestamp;
                        string idd = data.ID;
                        string API_KEY = virusTotalApiKey;


                        if (!string.IsNullOrEmpty(sha1))
                        {
                            dynamic response = QueryVirusTotal(sha1, API_KEY);

                            if (response != null)
                            {
                                List<string> names = response.data.attributes.names.ToObject<List<string>>();
                                names = names.Where(x => !string.IsNullOrEmpty(x)).ToList();
                                int positives = response.data.attributes.last_analysis_stats.malicious;
                                int jajahola = response.data.attributes.last_analysis_stats.undetected;

                                if (names.Count > 0)
                                {
                                    writer.WriteLineAsync($"[+] SHA1: {sha1}");
                                    writer.WriteLineAsync($"[+] FullPath: {fullPath}");
                                    writer.WriteLineAsync($"[+] Name: {name}");
                                    writer.WriteLineAsync($"[+] Names: {string.Join(", ", names)}");
                                    writer.WriteLineAsync($"[+] Executed on: {fileKeyLastWriteTimestamp}");
                                    writer.WriteLineAsync($"[+] Scan URL: https://www.virustotal.com/gui/file/{idd}/detection");
                                    writer.WriteLineAsync($"- - - - - - - - - - - - - - - - - - - - - -");
                                }
                            }
                        }
                    }
                }
            }

            Process.Start($@"C:\Users\{username}\Hate\Amcachehash\res.txt");
            Thread.Sleep(2000);
            Console.Clear();
            Program.GUI(args, version);
        }

        static void Amcache()
        {
            using (WebClient client = new WebClient())
            {
                string url = "https://chicho.fun/downloads/AM.exe";
                string filePath = $@"C:\Users\{Environment.UserName}\Hate\Amcachehash\AM.exe";
                client.DownloadFile(url, filePath);
            }
        }

        static void Parse(string[] args, string version)
        {
            string filePath = $@"C:\Users\{Environment.UserName}\Hate\Amcachehash\AM.exe";
            string arguments = $@"-f ""C:\Windows\appcompat\Programs\Amcache.hve"" --csv C:\Users\{Environment.UserName}\Hate\Amcachehash\";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = filePath;
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
        }

        static void ReadBoot(string[] args, string version)
        {
            string folderPath = $@"C:\Users\{Environment.UserName}\Hate\Amcachehash";
            string[] columnas_deseadas = { "SHA1", "IsOsComponent", "FullPath", "FileKeyLastWriteTimestamp" };
            List<Dictionary<string, string>> datos_filtrados_total = new List<Dictionary<string, string>>();
            DateTime boot_time = DateTime.Now.AddMilliseconds(-Environment.TickCount);

            foreach (string filePath in Directory.EnumerateFiles(folderPath, "*Amcache_UnassociatedFileEntries.csv", SearchOption.AllDirectories))
            {
                using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    string[] headers = reader.ReadLine().Split(',');

                    while (!reader.EndOfStream)
                    {
                        string[] values = reader.ReadLine().Split(',');
                        Dictionary<string, string> datos_fila_filtrados = headers.Zip(values, (h, v) => new { Header = h, Value = v })
                                                                               .Where(x => columnas_deseadas.Contains(x.Header))
                                                                               .ToDictionary(x => x.Header, x => x.Value);

                        datos_filtrados_total.Add(datos_fila_filtrados);
                    }
                }
            }

            var datos_filtrados_isoscomponent = datos_filtrados_total.Where(x => x["IsOsComponent"] == "False");

            var datos_filtrados_despues_boot = datos_filtrados_isoscomponent.Where(x => DateTime.Parse(x["FileKeyLastWriteTimestamp"]) >= boot_time);

            var datos_filtrados_ordenados = datos_filtrados_despues_boot.OrderByDescending(x => DateTime.Parse(x["FileKeyLastWriteTimestamp"]));

            if (datos_filtrados_ordenados.Any())
            {
                using (StreamWriter writer = new StreamWriter($@"C:\Users\{Environment.UserName}\Hate\Amcachehash\res.json", append: true))
                {
                    foreach (var datos_fila in datos_filtrados_ordenados)
                    {
                        writer.WriteLine(JsonConvert.SerializeObject(datos_fila));
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("No data has been detected since the last reboot");
                Thread.Sleep(2000);
                Console.Clear();
                Program.GUI(args, version);
            }
        }

        static void Read(string[] args, string version)
        {
            string folderPath = $@"C:\Users\{Environment.UserName}\Hate\Amcachehash";
            string[] columnas_deseadas = { "SHA1", "IsOsComponent", "FullPath", "FileKeyLastWriteTimestamp" };
            List<Dictionary<string, string>> datos_filtrados_total = new List<Dictionary<string, string>>();

            foreach (string filePath in Directory.EnumerateFiles(folderPath, "*Amcache_UnassociatedFileEntries.csv", SearchOption.AllDirectories))
            {
                using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    string[] headers = reader.ReadLine().Split(',');

                    while (!reader.EndOfStream)
                    {
                        string[] values = reader.ReadLine().Split(',');
                        Dictionary<string, string> datos_fila_filtrados = headers.Zip(values, (h, v) => new { Header = h, Value = v })
                                                                               .Where(x => columnas_deseadas.Contains(x.Header))
                                                                               .ToDictionary(x => x.Header, x => x.Value);

                        datos_filtrados_total.Add(datos_fila_filtrados);
                    }
                }
            }

            var datos_filtrados_isoscomponent = datos_filtrados_total.Where(x => x["IsOsComponent"] == "False");

            using (StreamWriter writer = new StreamWriter($@"C:\Users\{Environment.UserName}\Hate\Amcachehash\res.json", append: true))
            {
                foreach (var datos_fila in datos_filtrados_isoscomponent)
                {
                    writer.WriteLine(JsonConvert.SerializeObject(datos_fila));
                }
            }
            Thread.Sleep(2000);
            Console.Clear();
            Program.GUI(args, version);
        }

        static dynamic QueryVirusTotal(string sha1, string API_KEY)
        {
            string url = $"https://www.virustotal.com/api/v3/files/{sha1}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-apikey", API_KEY);
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    dynamic data = JsonConvert.DeserializeObject(result);
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
