using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hate.Sections
{
    internal class BetterDetectFile
    {
        public static async Task BetterFileAsync(string[] args, string version)
        {
            // Variables
            string username = Environment.UserName;
            string BetterFilePath = $@"C:\users\{username}\Hate\BetterFilesDetect";
            string api_key;
            string file;
            if (Directory.Exists(BetterFilePath))
            {
                Directory.Delete(BetterFilePath, true);
            }

            Directory.CreateDirectory($@"C:\users\{username}\Hate\BetterFilesDetect");


            Console.Write("/!\\ VirusTotal API [Enter to skip and use a public API]: ");
            api_key = Console.ReadLine();


            if (string.IsNullOrEmpty(api_key))
            {
                api_key = "55f3072a9e14add3e2641fa6340c5d0494119c46c8b2d124cf56155ae52d5dc0";
            }

            Console.Clear();

            using (var client = new WebClient())
            {
                client.DownloadFile("https://chicho.fun/downloads/ST.exe", Path.Combine(BetterFilePath, "ST.exe"));
            }

            Console.Clear();
            string textt = "Better file detection system.";
            var ft = textt;
            Console.WriteLine(ft);
            Console.WriteLine("\n  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n");
            Console.Write("[+] Enter your custom strings file: ");
            file = Console.ReadLine();
            Console.Clear();
            Console.WriteLine(ft);
            Console.WriteLine("\n  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n");
            Console.WriteLine("[+] Please wait.\n");

            List<string> lines = File.ReadAllLines(file).ToList();
            List<string> unique_lines = lines.Distinct().ToList();

            List<string> new_lines = new List<string>();
            foreach (string line in unique_lines)
            {
                string[] parts = line.Split(new string[] { "): " }, StringSplitOptions.None);
                string content = parts[parts.Length - 1];
                if (Regex.IsMatch(content, "^[a-zA-Z]:"))
                {
                    new_lines.Add(content.Trim());
                }
            }

            File.WriteAllLines(file, new_lines);

            string carpeta_files = $@"C:\Users\{username}\Hate\BetterFilesDetect\files";
            Directory.CreateDirectory(carpeta_files);

            List<string> file_lines = File.ReadAllLines(file).ToList();
            foreach (string linea in file_lines)
            {
                string filePath = linea.Trim();
                try
                {
                    File.Copy(filePath, Path.Combine(carpeta_files, Path.GetFileName(filePath)));
                }
                catch { }
            }

            string folder_path = $@"C:\Users\{username}\Hate\BetterFilesDetect\files";
            foreach (string archivo in Directory.GetFiles(folder_path, "*.exe"))
            {
                Process proceso = new Process();
                proceso.StartInfo.FileName = $@"C:\Users\{username}\Hate\BetterFilesDetect\ST.exe";
                proceso.StartInfo.Arguments = $@"verify /pa C:\Users\{username}\Hate\BetterFilesDetect\files\{Path.GetFileName(archivo)}";
                proceso.StartInfo.UseShellExecute = false;
                proceso.StartInfo.RedirectStandardOutput = true;
                proceso.StartInfo.RedirectStandardError = true;
                proceso.Start();

                string stdout = proceso.StandardOutput.ReadToEnd();
                string stderr = proceso.StandardError.ReadToEnd();
                proceso.WaitForExit();

                if (proceso.ExitCode == 0)
                {
                    File.Delete($@"C:\Users\{username}\Hate\BetterFilesDetect\files\{Path.GetFileName(archivo)}");
                }
            }
            string folderPath = $@"C:\Users\{username}\Hate\BetterFilesDetect\files\";
            List<string> nonMaliciousFiles = new List<string>();

            int count = 0;
            foreach (string filename in Directory.GetFiles(folderPath))
            {
                count++;
                string filePath = Path.Combine(folderPath, filename);
                if (File.Exists(filePath))
                {
                    byte[] fileContent = File.ReadAllBytes(filePath);

                    string md5Hash;
                    using (var md5 = MD5.Create())
                    {
                        byte[] hash = md5.ComputeHash(fileContent);
                        md5Hash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }

                    string url = $"https://www.virustotal.com/api/v3/files/{md5Hash}";
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("x-apikey", api_key);
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic jsonData = JsonConvert.DeserializeObject(result);

                        if (jsonData.data.attributes.last_analysis_stats.malicious < 1)
                        {
                            nonMaliciousFiles.Add(filePath);
                        }
                        else
                        {
                            dynamic lastAnalysisStats = jsonData.data.attributes.last_analysis_stats;
                            string[] names = ((JArray)jsonData.data.attributes.names).ToObject<string[]>();
                            int positives = lastAnalysisStats.malicious;
                            if (positives > 4)
                            {
                                string text = $" Suspicious File\n > Name: {filename}\n > Engines: {positives}\n > Global Names: {string.Join(", ", names)}\n\n - - - - - - - - - - - - - - - - - - - - - - \n\n";
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("[!]");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(text);
                                Console.ResetColor();

                                string text2 = $"[!] Suspicious File\n > Name: {filename}\n > Engines: {positives}\n > Global Names: {string.Join(", ", names)}\n\n - - - - - - - - - - - - - - - - - - - - - - \n\n";
                                File.AppendAllText($@"C:\Users\{username}\Hate\BetterFilesDetect\results.txt", text2);


                            }
                        }
                    }
                    else
                    {
                        string text = $" {filename} isn't uploaded to VirusTotal [Rare, or api ratelimit]\n";
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[!]");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(text);
                        Console.ResetColor();

                        string text2 = $"[!] {filename} isn't uploaded to VirusTotal [Rare, or api ratelimit]\n";
                        File.AppendAllText($@"C:\Users\{username}\Hate\BetterFilesDetect\results.txt", text2);

                    }

                }
            }

            foreach (string filePath in nonMaliciousFiles)
            {
                try
                {
                    File.Delete(filePath);
                }
                catch { }
            }

            Process.Start("explorer.exe", $@"C:\Users\{username}\Hate\BetterFilesDetect\files\");
            Process.Start("notepad.exe", $@"C:\Users\{username}\Hate\BetterFilesDetect\results.txt");
            Console.Write("\n\nPress ENTER to go to the menu...");
            Console.ReadLine();
            Console.Clear();
            Program.GUI(args, version).Wait();
        }
    }
}
