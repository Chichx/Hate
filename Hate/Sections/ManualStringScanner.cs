using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Webhook;

namespace Hate.Sections
{
    internal class ManualStringScanner
    {
        public static async Task StringScanner(string[] args, string version)
        {
            // inicializar HttpClient
            var httpClient = new HttpClient();

            // leer el archivo adjunto
            var xd = new List<string>();
            string filePath;
            while (true)
            {
                Console.Write("Enter file path (.txt): ");
                filePath = Console.ReadLine();

                if (string.IsNullOrEmpty(filePath))
                {
                    Console.WriteLine("No file path entered.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    continue;
                }
                else if (!Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Only .txt files are supported.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    continue;
                }
                else if (!File.Exists(filePath))
                {
                    Console.WriteLine("File does not exist.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    continue;
                }
                else
                {
                    break;
                }
            }
            var fileContent = File.ReadAllText(filePath);
            xd = fileContent.Split('\n').Select(line => line.Split(new string[] { "): " }, StringSplitOptions.None).Last().Trim()).ToList();

            Console.Clear();
            Console.WriteLine("Choose a scan type:");
            Console.WriteLine(" ");
            Console.WriteLine("Recommend:");
            Console.WriteLine("In DPS Scan: Put `^!![^!]+!.*$` in Regex (Case-Insensitive) for faster results.");
            Console.WriteLine(" ");
            Console.WriteLine("1. DPS Scan (with default strings)");
            Console.WriteLine("2. PCASvc Scan (with default strings)");
            Console.WriteLine("3. Lsass Scan (with default strings)");
            Console.WriteLine("4. Browser Scan (with default strings)");
            Console.WriteLine("5. Custom Strings (provide a Pastebin link)");
            Console.WriteLine(" ");
            Console.Write("Choose an option » ");
            string scanTypeInput = Console.ReadLine();

            int scanType;
            if (!int.TryParse(scanTypeInput, out scanType) || scanType < 1 || scanType > 5)
            {
                Console.WriteLine("Invalid option. Please enter a valid option (1, 2, 3, 4  or 5).");
                Thread.Sleep(1000);
                Console.Clear();
                StringScanner(args, version).Wait();
            }

            // Obtener la URL del archivo de detecciones
            string url = "";
            string scanTypeString = "";
            if (scanType == 1)
            {
                url = "https://pastebin.com/raw/RDvat1kW";
                scanTypeString = "DPS";
            }
            else if (scanType == 2)
            {
                url = "https://pastebin.com/raw/GZPdK9iY";
                scanTypeString = "PcaSvc";
            }
            else if (scanType == 3)
            {
                url = "https://pastebin.com/raw/HvvE82z5";
                scanTypeString = "LSASS";
            }
            else if (scanType == 4)
            {
                url = "https://pastebin.com/raw/vSWp6m2q";
                scanTypeString = "Browser";
            }
            else if (scanType == 5)
            {
                Console.Clear();
                Console.WriteLine("Guide:");
                Console.WriteLine("If you use a custom URL, enter the pastebin URL (example: pastebin.com/raw/xxxxxxxx)");
                Console.WriteLine("with custom string syntax: Client:::String");
                Console.Write("Enter the Pastebin URL for custom strings: ");
                url = Console.ReadLine();
                scanTypeString = $"Custom URL (||{url}||)";
            }

            // Si no se proporcionó una URL personalizada, utilizar una URL predeterminada para DPSScan
            if (string.IsNullOrEmpty(url) && scanType == 5)
            {
                url = "https://pastebin.com/raw/RDvat1kW";
                scanTypeString = "DPS (Custom URL enter)";
            }

            // obtener las detecciones
            var response = await httpClient.GetAsync(url);
            var contentLines = (await response.Content.ReadAsStringAsync()).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var found = new List<(string, string)>();
            foreach (var line in contentLines)
            {
                var splitLine = line.Split(new[] { ":::" }, StringSplitOptions.RemoveEmptyEntries);
                var client = splitLine[0];
                var detecc = splitLine[1];

                foreach (var xdLine in xd)
                {
                    if (xdLine.Contains(detecc))
                    {
                        found.Add((client, xdLine));
                    }
                }
            }

            if (found.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("Strings detected:");
                foreach (var (client, xdLine) in found)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{client}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($" has been detected");
                }
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("No detections found.");
                Console.ForegroundColor = ConsoleColor.White;
            }


            // Mostrar resultados
            if (found.Count > 10)
            {
                string hwid = Program.GetHWID();
                string userName = Environment.UserName;
                string webhookUrl = "https://discord.com/api/webhooks/1120158378455482520/ASoVTOjRSiPexzxDxEnTdAZFHS7NuwAJlCgPCSjElTU7caE0jmZyG4QeGeSKyKGFDt8W";
                string cheatFilePath = $@"C:\Users\{Environment.UserName}\Hate\Strings\{userName}.txt";

                // Crear y escribir en el archivo de trampas
                using (var cheatFile = File.CreateText(cheatFilePath))
                {
                    foreach (var (client, xdLine) in found)
                    {
                        cheatFile.WriteLine($"{client} has been detected with {xdLine}.");
                    }
                }

                var embedBuilder = new EmbedBuilder()
                    .WithTitle("User detected! :x:")
                    .WithTimestamp(DateTime.UtcNow)
                    .AddField("User:", userName, inline: true)
                    .AddField("HWID:", hwid, inline: true)
                    .AddField("Scan Type:", scanTypeString, inline: true)
                    .AddField("Detected:", $"In the {userName}-cheats.txt", inline: true)
                    .WithColor(Discord.Color.Red)
                    .WithFooter($"Cheats detected: {found.Count}");

                var webhook = new DiscordWebhookClient(webhookUrl);
                using (var fileStream = File.OpenRead(cheatFilePath))
                {
                    await webhook.SendFileAsync(fileStream, $"{userName}-cheats.txt", "", false, embeds: new[] { embedBuilder.Build() });
                }
                File.Delete(cheatFilePath);
            }
            else if (found.Count > 0)
            {
                string hwid = Program.GetHWID();
                string userName = Environment.UserName;
                var embedBuilder = new EmbedBuilder()
                    .WithTitle("User detected! :x:")
                    .WithTimestamp(DateTime.UtcNow)
                    .AddField("User:", userName, inline: true)
                    .AddField("HWID:", hwid, inline: true)
                    .AddField("Scan Type:", scanTypeString, inline: true)
                    .WithFooter($"Cheats detected: {found.Count}");
                var detectedField = new StringBuilder();
                foreach (var (client, xdLine) in found)
                {
                    detectedField.AppendLine($"{client} has been detected with {xdLine}.");
                }
                embedBuilder.AddField("Detected:", "```" + detectedField.ToString() + "```");
                embedBuilder.WithColor(Discord.Color.Red);
                var webhook = new DiscordWebhookClient("https://discord.com/api/webhooks/1120158378455482520/ASoVTOjRSiPexzxDxEnTdAZFHS7NuwAJlCgPCSjElTU7caE0jmZyG4QeGeSKyKGFDt8W");
                await webhook.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
            }
            else
            {
                string hwid = Program.GetHWID();
                string userName = Environment.UserName;

                var embedBuilder = new EmbedBuilder()
                {
                    Title = "User Legit!",
                    Timestamp = DateTime.UtcNow
                };
                embedBuilder.AddField("User:", userName, inline: true);
                embedBuilder.AddField("HWID:", hwid, inline: true);
                embedBuilder.AddField("Scan Type:", scanTypeString, inline: true);

                embedBuilder.WithColor(Discord.Color.Green);

                var webhook = new DiscordWebhookClient("https://discord.com/api/webhooks/1120158378455482520/ASoVTOjRSiPexzxDxEnTdAZFHS7NuwAJlCgPCSjElTU7caE0jmZyG4QeGeSKyKGFDt8W");
                await webhook.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
            }

            Console.Write("\n\nPress ENTER to go to the menu...");
            Console.ReadLine();
            Console.Clear();
            Program.GUI(args, version).Wait();
        }
    }
}
