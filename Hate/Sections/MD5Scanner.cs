using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Hate.Sections
{
    internal class MD5Scanner
    {
        static HttpClient client = new HttpClient();

        public static void MD5Scan()
        {
            string pastebinUrl = "https://pastebin.com/raw/mjTE12x0"; // Reemplazar con la URL del Pastebin
            Dictionary<string, List<string>> md5List = FetchMd5ListFromPastebin(pastebinUrl);

            List<string> matchedFiles = new List<string>();
            foreach (var entry in md5List)
            {
                foreach (var targetMD5 in entry.Value)
                {
                    string[] drivesToScan = { "C:", "D:", "E:", "A:", "F:", "G:" };
                    foreach (string drive in drivesToScan)
                    {
                        ScanDirectory(drive, targetMD5, matchedFiles);
                    }
                }

                Console.WriteLine($"Archivos encontrados con MD5 '{entry.Key}' with MD5: {entry.Value[0]}");
                foreach (string matchedFile in matchedFiles)
                {
                    Console.WriteLine(matchedFile);
                }

                matchedFiles.Clear();
                Console.WriteLine();
            }

            Console.WriteLine("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        static Dictionary<string, List<string>> FetchMd5ListFromPastebin(string url)
        {
            string content = client.GetStringAsync(url).Result;
            return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(content);
        }

        static void ScanDirectory(string path, string targetMD5, List<string> matchedFiles)
        {
            try
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    try
                    {
                        using (var md5 = MD5.Create())
                        {
                            using (var stream = File.OpenRead(file))
                            {
                                byte[] hash = md5.ComputeHash(stream);
                                string currentMD5 = BitConverter.ToString(hash).Replace("-", "").ToLower();
                                if (currentMD5.Equals(targetMD5))
                                {
                                    matchedFiles.Add(Path.GetFullPath(file)); 
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Ignorar y continuar con otros archivos
                    }
                }

                foreach (string directory in Directory.GetDirectories(path))
                {
                    try
                    {
                        ScanDirectory(directory, targetMD5, matchedFiles);
                    }
                    catch (Exception)
                    {
                        // Ignorar y continuar con otros directorios
                    }
                }
            }
            catch (Exception)
            {
                // Ignorar si no se puede acceder al directorio
            }
        }
    }
}
