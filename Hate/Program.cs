using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Discord;
using Discord.Webhook;
using Discord.Commands;
using Discord.WebSocket;
using System.Management;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.ServiceProcess;
using System.Reflection;
using Color = System.Drawing.Color;
using BetterConsole;
using System.ComponentModel;

namespace Hate

{
    class Program
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
        static async Task Main(string[] args)

        {
            string user = Environment.UserName;
            string hwid = GetHWID();
            string bootTime = GetBootTime().ToString();
            string os = Environment.OSVersion.Version.ToString();
            string reason = "";


            var url = "https://pastebin.com/raw/h39FPYyY";
            using (var client = new WebClient())
            {
                var content = client.DownloadString(url);
                var lines = content.Split('\n');
                var version = lines[0].Split('=')[1].Trim();
                var download = lines[1].Split('=')[1].Trim();
                var blacklistIndex = Array.FindIndex(lines, x => x.StartsWith("Users blacklisted:"));
                var blacklist = new List<string>(lines.Skip(blacklistIndex + 1).Select(x => x.Trim()));

                foreach (var entry in blacklist)
                {
                    var parts = entry.Split(new[] { ":::" }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2) continue;

                    var entryHwid = parts[0];
                    var entryReason = parts[1];

                    if (entryHwid == hwid)
                    {
                        reason = entryReason;
                        break;
                    }
                }

                if (reason != "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Title = $"Blacklisted!";
                    Console.SetWindowSize(88, 19);
                    Console.WriteLine("");
                    Console.WriteLine(@"██    ██  ██████  ██    ██      █████  ██████  ███████                                 
 ██  ██  ██    ██ ██    ██     ██   ██ ██   ██ ██                                      
  ████   ██    ██ ██    ██     ███████ ██████  █████                                   
   ██    ██    ██ ██    ██     ██   ██ ██   ██ ██                                      
   ██     ██████   ██████      ██   ██ ██   ██ ███████                                 
                                                                                       
                                                                                       
██████  ██       █████   ██████ ██   ██ ██      ██ ███████ ████████ ███████ ██████  ██ 
██   ██ ██      ██   ██ ██      ██  ██  ██      ██ ██         ██    ██      ██   ██ ██ 
██████  ██      ███████ ██      █████   ██      ██ ███████    ██    █████   ██   ██ ██ 
██   ██ ██      ██   ██ ██      ██  ██  ██      ██      ██    ██    ██      ██   ██    
██████  ███████ ██   ██  ██████ ██   ██ ███████ ██ ███████    ██    ███████ ██████  ██ ");
                    Console.WriteLine($"Reason: {reason}");
                    Console.WriteLine($"Your HWID: {hwid}");
                    Console.WriteLine($"Your PC: {Environment.UserName}");
                    Console.WriteLine($"Blacklisted users: {blacklist.Count}");
                    var embedBuilder = new EmbedBuilder()
                    {
                        Title = $":warning: Blacklist :warning:",
                        Timestamp = DateTime.UtcNow
                    };


                    embedBuilder.WithDescription("**Blacklisted user trying enter to Hate!**");
                    embedBuilder.AddField("User Blacklisted:", user);
                    embedBuilder.AddField("Blacklisted for:", reason);
                    embedBuilder.AddField("HWID Blacklisted:", hwid);
                    embedBuilder.WithFooter($"Blacklisted users: {blacklist.Count}");
                    embedBuilder.WithColor((Discord.Color)System.Drawing.Color.Red);
                    var webhook = new DiscordWebhookClient("https://discord.com/api/webhooks/1118065838256295988/-l-n-hyP_nuHD3678Ra2Lq9bk2BUZz5Lr3ELSNDdcb3gNwPbougkH7scmkvinclsXn1k");
                    await webhook.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
                    Console.ReadKey();
                    Environment.Exit(0);
                    return;
                }

                if (version != "2.9")
                {
                    Console.Title = $"Hate | Old version! | New version: {version}";

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"New version available. Please download new version. \n\nNew Version: {version}\n");
                    Console.WriteLine("Do you want to update the program? (Yes/No)");
                    Console.Write("Choose an option: ");
                    string answer = Console.ReadLine().ToLower();

                    if (answer == "yes" || answer == "y")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Clear();
                        Console.WriteLine("");
                        Console.WriteLine($"Downloading new version ({version}) from {download}...");
                        Thread.Sleep(2000);
                        var clientt = new WebClient();
                        clientt.DownloadFile(download, $"Hate-{version}.exe");
                        Console.WriteLine("Download completed.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Console.WriteLine("\nRe-open new version. Thanks for downloading!");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Clear();
                        Console.WriteLine("");
                        Console.WriteLine("Program will continue without updating.");
                        Console.WriteLine("Bye!");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                    }

                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Title = $"Hate | Updated version: {version}!";
                    BConsole.TypeRainbowGradientLine("Hate updated!", 10);

                    //Proxy
                    string ip = new WebClient().DownloadString("https://ifconfig.me/ip").Trim();
                    string urll = "https://proxycheck.io/v2/" + ip + "?vpn=1&asn=1";
                    string response = new WebClient().DownloadString(urll);
                    // Parse el JSON de la respuesta
                    dynamic data = JsonConvert.DeserializeObject(response);
                    string provider = data[ip]["provider"];
                    string country = data[ip]["country"];
                    string city = data[ip]["city"];
                    string region = data[ip]["region"];
                    bool isProxy = data[ip]["proxy"] == "yes";

                    var embedBuilderr = new EmbedBuilder()
                    {
                        Title = $"New Login!",
                        Timestamp = DateTime.UtcNow
                    };
                    embedBuilderr.AddField("User:", user);
                    embedBuilderr.AddField("HWID:", hwid);
                    embedBuilderr.AddField("IP:", ip, inline: true);
                    embedBuilderr.AddField("País:", country, inline: true);
                    embedBuilderr.AddField("Ciudad y Region:", $"{city} | {region}", inline: true);
                    embedBuilderr.AddField("VPN o Proxy:", isProxy ? "Sí" : "No", inline: true);
                    embedBuilderr.AddField("Provider:", provider, inline: true);
                    embedBuilderr.WithColor(Discord.Color.Green);

                    string webhookUrl = "https://discord.com/api/webhooks/1118772176204607538/opW7Q7pVMyZx9UPVTp1fcPDBPN-nY4YntNdnKK3-Pc8IV4N1Mnvkkb1M2QRDksWHYH_9"; // Agrega tu URL de webhook de Discord aquí
                    var webhook1 = new DiscordWebhookClient(webhookUrl);
                    await webhook1.SendMessageAsync(embeds: new[] { embedBuilderr.Build() });


                    var embedBuilder = new EmbedBuilder()
                    {
                        Title = $"New Login!",
                        Timestamp = DateTime.UtcNow
                    };


                    embedBuilder.WithDescription($"**User enter to Hate ({version})!**");
                    embedBuilder.AddField("User:", user);
                    embedBuilder.AddField("HWID:", hwid);
                    embedBuilder.WithColor(Discord.Color.Green);
                    var webhook = new DiscordWebhookClient("https://discord.com/api/webhooks/1118066349881708555/wY2tDRssatEqO9EP08QwNJILlrEcJJAqWP3aTOu0oGf-QdMlli-esfHuzo7nNSjxZ00l");
                    var webhook12 = new DiscordWebhookClient("https://discord.com/api/webhooks/1125624629340409906/jcNdXXAtT0bpW4FOAVJK3ekm8oJfljU-4DUYxbF1To8IUsPbce4HYDNoWbVUM80h80PS");
                    await webhook.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
                    await webhook12.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
                    Console.Clear();
                    PinCheckFirst(args, version);
                }

            }
        }


        static Task GUI(string[] args, string version)
        {
            Console.Title = $"Hate | Version {version}";
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(85, 23);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@"
                   ▄█    █▄       ▄████████     ███        ▄████████ 
                  ███    ███     ███    ███ ▀█████████▄   ███    ███ 
                  ███    ███     ███    ███    ▀███▀▀██   ███    █▀  
                 ▄███▄▄▄▄███▄▄   ███    ███     ███   ▀  ▄███▄▄▄     
                ▀▀███▀▀▀▀███▀  ▀███████████     ███     ▀▀███▀▀▀     
                  ███    ███     ███    ███     ███       ███    █▄  
                  ███    ███     ███    ███     ███       ███    ███ 
                  ███    █▀      ███    █▀     ▄████▀     ██████████
                                    ");
            BConsole.RainbowGradientLine("           ╔═════════════════════════════════╦═══════════════════════════╗");
            BConsole.RainbowGradientLine($"           ║ [1] Simple detects              ║ [7] Amcache hash detector ║");
            BConsole.RainbowGradientLine($"           ║ [2] Programs                    ║ [8] Better detect file    ║");
            BConsole.RainbowGradientLine($"           ║ [3] Time Modification           ║ [9] Prefetch Filter       ║");
            BConsole.RainbowGradientLine($"           ║ [4] Partition Disks             ║ [10] String Scanner       ║");
            BConsole.RainbowGradientLine($"           ║ [5] Executed Programs           ║ [11] Auto String Scanner  ║");
            BConsole.RainbowGradientLine($"           ║ [6] Pcasvc Viewer               ║                           ║");
            BConsole.RainbowGradientLine("           ╚═════════════════════════════════╩═══════════════════════════╝");
            BConsole.RainbowGradientLine($"           ║                          [12] Destruct                      ║");
            BConsole.RainbowGradientLine("           ╚═════════════════════════════════════════════════════════════╝");
            Console.WriteLine("");
            BConsole.RainbowGradient("Choose an option » ");
            string input = Console.ReadLine();
            int option;
            if (int.TryParse(input, out option))
            {
                switch (option)
                {
                    default:
                        Console.WriteLine("Invalid input. Please enter a valid option.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        GUI(args, version).Wait();
                        break;
                    case 1:
                        Console.Title = $"Hate | Simple detects";
                        Console.Clear();
                        SimpleThings(args, version);
                        break;
                    case 2:
                        Console.Title = $"Hate | Programs";
                        Console.Clear();
                        Programas(args, version);
                        break;
                    case 3:
                        Console.Title = $"Hate | Time Modification";
                        Console.Clear();
                        Modification(args, version);
                        break;
                    case 4:
                        Console.Title = $"Hate | Partition Disk";
                        Console.Clear();
                        Partition(args, version);
                        break;
                    case 5:
                        Console.Title = $"Hate | Executed Programs";
                        Console.Clear();
                        ExecutedPrograms(args, version);
                        break;
                    case 6:
                        Console.Title = $"Hate | PcaClient Viewer";
                        Console.Clear();
                        PcaSvc(args, version);
                        break;
                    case 7:
                        Console.Title = $"Hate | Amcache Hash Detector";
                        Console.Clear();
                        Amcache(args, version);
                        break;
                    case 8:
                        Console.Title = $"Hate | Better detection file";
                        Console.Clear();
                        BetterFileAsync(args, version).Wait();
                        break;
                    case 9:
                        Console.Title = $"Hate | Prefetch Filter";
                        Console.Clear();
                        WinPrefetch(args, version);
                        break;
                    case 10:
                        Console.Title = $"Hate | String Scanner";
                        Console.Clear();
                        StringScanner(args, version).Wait();
                        break;
                    case 11:
                        Console.Title = $"Hate | String Scanner (Automatic)";
                        Console.Clear();
                        DNSCache(args, version);
                        break;
                    case 12:
                        Console.Title = $"Hate | Exit and Credits";
                        Console.Clear();
                        ExitAndCredits(args, version);
                        break;

                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid option.");
                Thread.Sleep(1000);
                Console.Clear();
                GUI(args, version).Wait();
            }

            return Task.CompletedTask;
        }

        private static string GetHWID()
        {
            var mc = new ManagementClass("win32_computersystemproduct");
            var moc = mc.GetInstances();

            string idD = "";
            foreach (var mo in moc)
            {
                idD += mo.Properties["UUID"].Value.ToString();
                break;
            }
            return idD;
        }

        private static DateTime GetBootTime()
        {
            var mc = new ManagementClass("Win32_OperatingSystem");
            foreach (var mo in mc.GetInstances())
            {
                var lastBootUpTime = mo.Properties["LastBootUpTime"].Value.ToString();
                return ManagementDateTimeConverter.ToDateTime(lastBootUpTime);
            }
            return DateTime.MinValue;
        }

        private static void SimpleThings(string[] args, string version)
        {
            string username = Environment.UserName;


            // Modification de la papelera
            string sid = "";
            Process process = new Process();
            process.StartInfo.FileName = "wmic";
            process.StartInfo.Arguments = $"useraccount where name=\"{username}\" get sid";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            sid = output.Split(new string[] { "\r\r\n" }, StringSplitOptions.None)[1];
            string recycleBinPath = $"C:/$Recycle.Bin/{sid}";

            DateTime modTime = File.GetLastWriteTime(recycleBinPath);

            // Ver grabadores de pantalla
            Dictionary<string, string> recordingSoftwares = new Dictionary<string, string>()
    {
        {"bdcam", "Bandicam"},
        {"action", "Action"},
        {"action_svc", "Action"},
        {"obs32", "OBS 32Bits"},
        {"obs64", "OBS 64Bits"},
        {"dxtory", "Dxtory"},
        {"nvidia share", "Geforce Experience"},
        {"nvcontainer", "Shadowplay"},
        {"radeonsettings", "Radeon Settings"},
        {"radeonsoftware", "Radeon Software"},
        {"camtasia", "Camtasia"},
        {"fraps", "Fraps"},
        {"xsplit.core", "XSplit Core"},
        {"CamRecorder", "Cam Recorder"},
        {"screencast", "Screencast"},
        {"sharex", "Share X"},
        {"playclaw.exe", "PlayClaw"},
        {"mirillis.exe", "Mirillis Action"},
        {"wmcap.exe", "Bandicam"},
        {"lightstream.exe", "Lightstream"},
        {"streamlabs.exe", "Streamlabs OBS"},
        {"webrtcvad.exe", "Audacity (recording)"},
        {"openbroadcastsoftware.exe", "Open Broadcaster Software"},
        {"movavi.screen.recorder.exe", "Movavi Screen Recorder"},
        {"icecreamscreenrecorder.exe", "Icecream Screen Recorder"},
        {"smartpixel.exe", "Smartpixel"},
        {"d3dgear.exe", "D3DGear"},
        {"gadwinprintscreen.exe", "Gadwin PrintScreen"},
        {"apowersoftfreescreenrecorder.exe", "Apowersoft Free Screen Recorder"},
        {"bandicamlauncher.exe", "Bandicam (launcher)"},
        {"hypercam.exe", "HyperCam"},
        {"loiloilgamerecorder.exe", "LoiLo Game Recorder"},
        {"nchexpressions.exe", "Express Animate (recording)"},
        {"captura.exe", "Captura"},
        {"vokoscreenNG", "VokoscreenNG"},
        {"simple.screen.recorder", "SimpleScreenRecorder"},
        {"recordmydesktop", "RecordMyDesktop"},
        {"kazam", "Kazam"},
        {"gtk-recordmydesktop", "Gtk-RecordMyDesktop"},
        {"screenstudio", "ScreenStudio"},
        {"screenkey", "Screenkey"},
        {"pycharm64.exe", "PyCharm (recording)"},
        {"jupyter-notebook.exe", "Jupyter Notebook (recording)"}
    };
            string tasks = "";
            Process processs = new Process();
            processs.StartInfo.FileName = "tasklist";
            processs.StartInfo.UseShellExecute = false;
            processs.StartInfo.RedirectStandardOutput = true;
            processs.Start();
            tasks = processs.StandardOutput.ReadToEnd().ToLower();
            processs.WaitForExit();

            var found = Array.FindAll(recordingSoftwares.Keys.ToArray(), software => tasks.Contains(software));

            //Proxy
            string ip = new WebClient().DownloadString("https://ifconfig.me/ip").Trim();
            string url = "https://proxycheck.io/v2/" + ip + "?vpn=1&asn=1";
            string response = new WebClient().DownloadString(url);
            string path1 = $@"C:\Users\{Environment.UserName}\Hate\Proxy\";

            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("VPN or Proxy Detector.");

            File.WriteAllText(Path.Combine(path1, "proxy.json"), response);

            string[] lines = File.ReadAllLines(Path.Combine(path1, "proxy.json"));
            foreach (string line in lines)
            {
                if (line.Contains("\"proxy\": \"yes\""))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("VPN or Proxy: Yes");
                }
                else if (line.Contains("\"proxy\": \"no\""))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("VPN or Proxy: No");
                }
            }

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Times check.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Recycle Bin last modified: {modTime.ToString("dd/MM/yyyy HH:mm:ss")}");

            DateTime explorerStartTime = GetExplorerStartTime();
            string explorerStartTime1 = explorerStartTime != DateTime.MinValue ? explorerStartTime.ToString() : "Time not found.";
            Console.WriteLine($"Explorer: {explorerStartTime1}");

            DateTime csrssStartTime = GetCsrssStartTime();
            string csrssStartTime1 = csrssStartTime != DateTime.MinValue ? csrssStartTime.ToString() : "Time not found.";
            Console.WriteLine($"Csrss: {csrssStartTime1}");

            DateTime smartStartTime = GetSCStartTime();
            string smartStartTime1 = smartStartTime != DateTime.MinValue ? smartStartTime.ToString() : "Time not found.";
            Console.WriteLine($"SmartScreen: {smartStartTime1}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("Services check.");
            Regex regex = new Regex(@"CDPUserSvc_[a-zA-Z0-9]{5}");
            bool founde = false;
            List<string> serviceNames = new List<string> { "DPS", "Pcasvc", "Diagtrack", "Sysmain", "SgrmBroker", "Appinfo", "Dusmsvc", "DcomLaunch", "BFE", "MpsSvc" };


            foreach (var service in ServiceController.GetServices())
            {
                string nombre = service.ServiceName;
                Match match = regex.Match(nombre);
                if (match.Success)
                {
                    founde = true;
                    serviceNames.Add(nombre);
                }
            }

            if (!founde)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CDPUserSvc not found");
            }

     

            // Define los mensajes en inglés y español
            Dictionary<string, string[]> messages = new Dictionary<string, string[]>();
            messages.Add("stopped", new string[] { "stopped", "detenido" });
            messages.Add("running", new string[] { "running", "en ejecución" });
            messages.Add("unknown", new string[] { "unknown", "desconocido" });

            // Recorre la lista de nombres de servicio y verifica el estado de cada uno
            foreach (string serviceName in serviceNames)
            {
                string queryCommand = "sc query " + serviceName;

                Process processdps = new Process();
                processdps.StartInfo.FileName = "cmd.exe";
                processdps.StartInfo.Arguments = "/c " + queryCommand;
                processdps.StartInfo.UseShellExecute = false;
                processdps.StartInfo.RedirectStandardOutput = true;
                processdps.Start();

                string outputs = processdps.StandardOutput.ReadToEnd();
                processdps.WaitForExit();

                // Busca el estado del servicio en inglés y español
                Regex stateRegex = new Regex("STATE|ESTADO");
                Match stateMatch = stateRegex.Match(outputs);
                if (!stateMatch.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("The state of the " + serviceName + " service is " + messages["unknown"][0] + ".");
                }
                else
                {
                    string stateString = outputs.Substring(stateMatch.Index + stateMatch.Length);
                    if (stateString.Contains("STOPPED") || stateString.Contains("DETENIDO"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The " + serviceName + " service is " + messages["stopped"][0] + ".");
                    }
                    else if (stateString.Contains("RUNNING") || stateString.Contains("EN EJECUCIÓN"))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("The " + serviceName + " service is " + messages["running"][0] + ".");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("The state of the " + serviceName + " service is " + messages["unknown"][0] + ".");
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            if (found.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Recording software(s) found.");
                foreach (var software in found)
                {
                    if (recordingSoftwares.TryGetValue(software, out string softwareName))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"    {softwareName} found.");
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("No recording software found.");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("Plugged USB Detector.");
            Process process11 = new Process();
            process11.StartInfo.FileName = "wmic";
            process11.StartInfo.Arguments = "path Win32_USBHub get DeviceID";
            process11.StartInfo.UseShellExecute = false;
            process11.StartInfo.RedirectStandardOutput = true;
            process11.Start();

            string output10 = process11.StandardOutput.ReadToEnd();

            string[] filteredOutput = Array.FindAll(output10.Split('\n'), item => item.Contains("VID_") || item.Contains("PID_"));

            string[][] vidPidValues = new string[filteredOutput.Length][];
            for (int i = 0; i < filteredOutput.Length; i++)
            {
                string[] splitItem = filteredOutput[i].Split('\\');
                string vid = Regex.Match(splitItem[splitItem.Length - 2], @"VID_([\w\d]+)&").Groups[1].Value;
                string pid = Regex.Match(splitItem[splitItem.Length - 2], @"PID_([\w\d]+)").Groups[1].Value;
                vidPidValues[i] = new string[] { vid, pid };
            }

            foreach (string[] item in vidPidValues)
            {
                try
                {
                    string url10 = $"https://devicehunt.com/view/type/usb/vendor/{item[0]}/device/{item[1]}";
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    string content = client.DownloadString(url10);
                    string deviceLine = Regex.Match(content, @"class=""details --type-device --auto-link""><h3 class='details__heading'>([\w\d\s\-]+)</h3><table").Groups[1].Value;
                    string vendorLine = Regex.Match(content, @"class=""details --type-vendor --auto-link""><h3 class='details__heading'>([\w\d\s\-\(\)]+)</h3><table").Groups[1].Value;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[+]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($" Detected {item[0]} + {item[1]} as: {deviceLine.Trim()} - {vendorLine.Trim()}");
                    Console.WriteLine($"Please wait...\n");
                    Console.ResetColor();
                }
                catch (WebException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[!]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($" Unable to get {item[0]} + {item[1]} device.");
                    Console.WriteLine($"Please wait...\n");
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[!]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($" Unable to get {item[0]} + {item[1]} device.");
                    Console.WriteLine($"Please wait...\n");
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nPress ENTER to go to the menu...");
            Console.ReadLine();
            Console.Clear();
            GUI(args, version).Wait();

        }

        // Time Modification
        class CheckInfo
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

        private static void Modification(string[] args, string version)
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
            GUI(args, version).Wait();
        }



        static CheckInfo checkTimeModification()
        {
            EventRecord entry;
            string logPath = @"C:\Windows\System32\winevt\Logs\Security.evtx";
            EventLogReader logReader = new EventLogReader(logPath, PathType.FilePath);
            DateTime pcStartTime = startTime();

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

        private static void Partition(string[] args, string version)
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
            GUI(args, version).Wait();
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
                GUI(args, version).Wait();
            }

            return storages;
        }

        static List<DiskLog> getDisksLogs(string[] args, string version)
        {
            EventRecord entry;
            List<DiskLog> disksLogs = new List<DiskLog>();
            string logPath = @"C:\Windows\System32\winevt\Logs\Microsoft-Windows-StorageSpaces-Driver%4Operational.evtx";
            EventLogReader logReader = new EventLogReader(logPath, PathType.FilePath);
            DateTime pcStartTime = startTime();

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
                GUI(args, version).Wait();
            }
            return disksLogs;
        }

        static bool isMounted(char partition)
        {
            return Directory.Exists($"{partition}:");
        }

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

        static void ExecutedPrograms(string[] args, string version)
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
            GUI(args, version).Wait();
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
                GUI(args, version).Wait();
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

        // PcaClient Viewer
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
    int dwDesitedAccess,
    bool bInheritHandle,
    int dwProcessID);

        [DllImport("kernel32.dll")]
        static extern int VirtualQueryEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            out MEMORY_BASIC_INFORMATION lpBuffer,
            uint dwLength);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            IntPtr dwSize,
            ref int lpNumberOfBytesRead);

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        const int PROCESS_ALL_ACCESS = (0x1F0FFF);
        const int MEM_COMMIT = (0x00001000);
        const int MEM_FREE = (0x00010000);
        const int MEM_PRIVATE = (0x00020000);
        const int MEM_IMAGE = (0x01000000);
        const int MEM_MAPPED = (0x00040000);
        const int PAGE_NOACCESS = (0x01);

        static void PcaSvc(string[] args, string version)
        {
            List<string> dump = new List<string>();
            int pid = Process.GetProcessesByName("explorer").FirstOrDefault().Id;
            MEMORY_BASIC_INFORMATION memInfo = new MEMORY_BASIC_INFORMATION();
            IntPtr hProc = OpenProcess(PROCESS_ALL_ACCESS, true, pid);
            int memInfoSize = Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION));
            byte first = 0, second = 0;
            bool uFlag = true, isUnicode = false;

            Console.ForegroundColor = ConsoleColor.White;

            for (IntPtr p = IntPtr.Zero;
                VirtualQueryEx(hProc, p, out memInfo,
                (uint)memInfoSize) == memInfoSize;
                p = new IntPtr(p.ToInt64() + memInfo.RegionSize.ToInt64()))
            {

                if (memInfo.Protect == PAGE_NOACCESS) continue;

                if (memInfo.State == MEM_COMMIT
                    && memInfo.Type == MEM_PRIVATE)
                {
                    byte[] buffer = new byte[memInfo.RegionSize.ToInt64()];
                    int bytesRead = 0;

                    if (ReadProcessMemory(hProc, p, buffer, memInfo.RegionSize, ref bytesRead))
                    {
                        Array.Resize(ref buffer, bytesRead);
                        StringBuilder builder = new StringBuilder();

                        for (int i = 0; i < bytesRead; i++)
                        {
                            bool cFlag = isChar(buffer[i]);

                            if (cFlag && uFlag && isUnicode && first > 0)
                            {
                                isUnicode = false;
                                if (builder.Length > 0) builder.Remove(builder.Length - 1, 1);
                                builder.Append((char)buffer[i]);
                            }
                            else if (cFlag) builder.Append((char)buffer[i]);
                            else if (uFlag && buffer[i] == 0 && isChar(first) && isChar(second))
                                isUnicode = true;
                            else if (uFlag && buffer[i] == 0 && isChar(first)
                                && isChar(second) && builder.Length < 5)
                            {
                                isUnicode = true;
                                builder = new StringBuilder();
                                builder.Append((char)first);
                            }
                            else
                            {
                                if (builder.Length >= 5 && builder.Length <= 1500)
                                {
                                    int l = builder.Length;
                                    if (isUnicode) l *= 2;
                                    dump.Add(builder.ToString());
                                }

                                isUnicode = false;
                                builder = new StringBuilder();
                            }
                        }
                    }
                }

            }

            Regex rgx = new Regex(@"^TRACE,.+,PcaClient,.+,(\w:\\.+.exe).+$", RegexOptions.Multiline);

            Console.WriteLine("PcaClient\n-----------------------\n");
            foreach (string d in dump)
            {
                MatchCollection matches = rgx.Matches(d);
                foreach (Match match in matches)
                    Console.WriteLine(match.Groups[1].Value);
            }

            Console.Write("\n\nPress ENTER to go to the menu...");
            Console.ReadLine();
            Console.Clear();
            GUI(args, version).Wait();

        }

        static bool isChar(byte b)
        {
            return (b >= 32 && b <= 126) || b == 10 || b == 13 || b == 9;
        }

        static void Amcache(string[] args, string version)
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
            GUI(args, version).Wait();
        }

        static void Amcache()
        {
            using (WebClient client = new WebClient())
            {
                string url = "https://chicho.lol/downloads/AM.exe";
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
                GUI(args, version).Wait();
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
            GUI(args, version).Wait();
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



        private static void WinPrefetch(string[] args, string version)
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

            string zipUrl = "https://chicho.lol/downloads/WinPrefetchView.exe";
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
            GUI(args, version).Wait();
        }

        private static async Task BetterFileAsync(string[] args, string version)
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
                client.DownloadFile("https://chicho.lol/downloads/ST.exe", Path.Combine(BetterFilePath, "ST.exe"));
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
            GUI(args, version).Wait();
        }


        private static async Task StringScanner(string[] args, string version)
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
            } else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("No detections found.");
                }
            

            // Mostrar resultados
            if (found.Count > 10)
            {
                string hwid = GetHWID();
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
                var webhook12 = new DiscordWebhookClient("https://discord.com/api/webhooks/1125625173597495417/YGQamvg7zDgTjE9Ed7RFXzKt2W23D7CEaPAcbytt8h8qKaJ6vYjitShse41nAnCLUG6a");
                using (var fileStream = File.OpenRead(cheatFilePath))
                {
                    await webhook.SendFileAsync(fileStream, $"{userName}-cheats.txt", "", false, embeds: new[] { embedBuilder.Build() });
                }
                using (var fileStream = File.OpenRead(cheatFilePath))
                {
                    await webhook12.SendFileAsync(fileStream, $"{userName}-cheats.txt", "", false, embeds: new[] { embedBuilder.Build() });
                }
                File.Delete(cheatFilePath);
            }
            else
            {
                string hwid = GetHWID();
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
                var webhook123 = new DiscordWebhookClient("https://discord.com/api/webhooks/1125625173597495417/YGQamvg7zDgTjE9Ed7RFXzKt2W23D7CEaPAcbytt8h8qKaJ6vYjitShse41nAnCLUG6a");
                await webhook.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
                await webhook123.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
            }            

            Console.Write("\n\nPress ENTER to go to the menu...");
            Console.ReadLine();
            Console.Clear();
            GUI(args, version).Wait();
        }

        private static void Programas(string[] args, string version)
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
                Programas(args, version);
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
                Programas(args, version);
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
                Programas(args, version);
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
                Programas(args, version);
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
                Programas(args, version);
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
                Programas(args, version);
            }
            else if (programType == 6)
            {
                string zipUrl = "https://chicho.lol/downloads/WinLiveInfo.exe";
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
                Programas(args, version);
            }
            else if (programType == 7)
            {
                string zipUrl = "https://chicho.lol/downloads/JournalTrace.exe";
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
                Programas(args, version);
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
                Programas(args, version);
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
                Programas(args, version);
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
                Programas(args, version);
            }
            else if (programType == 11)
            {
                string zipUrl = "https://chicho.lol/downloads/Journal%20Files.exe";
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
                Programas(args, version);
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
                Programas(args, version);
            }
            else if (programType == 13)
            {
                string zipUrl = "https://chicho.lol/downloads/SkriptDetector.exe";
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
                Programas(args, version);
            }
            else if (programType == 14)
            {
                Console.Clear();
                Thread.Sleep(1000);
                GUI(args, version).Wait();
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
                byte[] fileData = client.DownloadData("https://cdn.discordapp.com/attachments/916928290449662003/1073342691229847592/xxstrings64.exe");
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
                    Thread.Sleep(1000);
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
                    Thread.Sleep(1000);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDone!");
                Thread.Sleep(1000);
                DPSScan(args, version);
            }
            catch
            {
                Console.WriteLine("Unable to scan Check #1.");
                Thread.Sleep(1000);
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
                    Thread.Sleep(1000);
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
                    Thread.Sleep(1000);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDone!");
                Thread.Sleep(1000);
                PcaSvcs(args, version);
            }
            catch
            {
                Console.WriteLine("Unable to scan Check Check #2.");
                Thread.Sleep(1000);
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
                    Thread.Sleep(1000);
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
                    Thread.Sleep(1000);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDone!");
                Thread.Sleep(1000);
                LsassScan(args, version);
            }
            catch
            {
                Console.WriteLine("Unable to scan Check Check #3.");
                Thread.Sleep(1000);
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
                    Thread.Sleep(1000);
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
                    Thread.Sleep(1000);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDone!");
                Console.WriteLine("Please wait for result!");
                Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("Unable to scan Check #4.");
                Console.ReadLine();
            }
            string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            string filePath = $@"C:\Users\{Environment.UserName}\Hate\Strings\detections.json";
            File.WriteAllText(filePath, json);
            SendWebhook().Wait();
            CheckUser(args, version);
        }

        private static void ExitAndCredits(string[] args, string version)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetWindowSize(74, 24);
            Console.WriteLine("              ╔═══════════════════════════════════════════╗");
            Console.WriteLine($"              ║                  Hate Tool                ║");
            Console.WriteLine($"              ║                 Created by:               ║");
            Console.WriteLine($"              ║                 Chicho#5279               ║");
            Console.WriteLine($"              ║                      &                    ║");
            Console.WriteLine($"              ║                   agu#1615                ║");
            Console.WriteLine("              ╚═══════════════════════════════════════════╝");
            Console.WriteLine($"\n                   Thanks for using Hate, {Environment.UserName} :)!");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("Deleting Hate files.");
            BConsole.Progressbar("", Color.Green);
            Thread.Sleep(2000);
            Console.Clear();
            BConsole.TypeRainbowGradient($"\nDone! Bye.", 10);
            Thread.Sleep(2000);
            string folderPath = $@"C:\Users\{Environment.UserName}\Hate\";

            // Eliminar la carpeta al salir del programa
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true); // Borra la carpeta y su contenido
                }
            };
            InitiateSelfDestructSequence();
            Thread.Sleep(3000);
        }
    static void InitiateSelfDestructSequence()
        {
            string batchScriptName = $@"C:\Users\{Environment.UserName}\Hate.bat";
            using (StreamWriter writer = File.AppendText(batchScriptName))
            {
                writer.WriteLine(":Loop");
                writer.WriteLine("Tasklist /fi \"PID eq " + Process.GetCurrentProcess().Id.ToString() + "\" | find \":\"");
                writer.WriteLine("if Errorlevel 1 (");
                writer.WriteLine("  Timeout /T 1 /Nobreak");
                writer.WriteLine("  Goto Loop");
                writer.WriteLine(")");
                writer.WriteLine("Del \"" + (new FileInfo((new Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath)).Name + "\"");
            }
            Process.Start(new ProcessStartInfo() { Arguments = "/C " + batchScriptName + " & Del " + batchScriptName, WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true, FileName = "cmd.exe" });
        }

        private static DateTime GetExplorerStartTime()
        {
            Process[] explorerProcesses = Process.GetProcessesByName("explorer");

            if (explorerProcesses.Length > 0)
            {
                Process explorerProcess = explorerProcesses[0];
                return explorerProcess.StartTime;
            }

            return DateTime.MinValue;
        }

        private static DateTime GetCsrssStartTime()
        {
            Process[] csrssProcesses = Process.GetProcessesByName("csrss");

            if (csrssProcesses.Length > 0)
            {
                Process csrss1Processes = csrssProcesses[0];
                return csrss1Processes.StartTime;
            }

            return DateTime.MinValue;
        }

        private static DateTime GetSCStartTime()
        {
            Process[] SMCProcesses = Process.GetProcessesByName("smartscreen");

            if (SMCProcesses.Length > 0)
            {
                Process smartProcesses = SMCProcesses[0];
                return smartProcesses.StartTime;
            }

            return DateTime.MinValue;
        }


        static DateTime startTime()
        {
            return DateTime.Now.AddMilliseconds(-Environment.TickCount);
        }

        //  Scanner auto
        private static string BootTime()
        {
            DateTime bootTime = DateTime.MinValue;
            using (var uptime = new PerformanceCounter("System", "System Up Time"))
            {
                uptime.NextValue(); // Se requiere una primera lectura para inicializar el contador
                bootTime = DateTime.Now - TimeSpan.FromSeconds(uptime.NextValue());
            }

            return bootTime.ToString();
        }
        static void CheckUser(string[] args, string version)
        {
            string jsonFilePath = $@"C:\Users\{Environment.UserName}\Hate\Strings\detections.json";
            string jsonData = File.ReadAllText(jsonFilePath);
            Dictionary<string, List<string>> detections = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData);
            if (detections.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ");
                Console.WriteLine("Result:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("User is legit!");
                Console.WriteLine(" ");
            }
            else if (detections.ContainsKey("DPS") || detections.ContainsKey("Dnscache") || detections.ContainsKey("PcaSvc") || detections.ContainsKey("Lsass"))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ");
                Console.WriteLine("Result:");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("User is cheating!");
                Console.WriteLine(" ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ");
                Console.WriteLine("Result:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("User is suspicious!");
                Console.WriteLine(" ");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nPress ENTER to go to the menu...");
            Console.ReadLine();
            Console.Clear();
            GUI(args, version).Wait();
        }

        static async Task SendWebhook()
        {
            string webhookUrl = "https://discordapp.com/api/webhooks/1121269735787597894/WdpT5dhSz5mLoZEVmRg_Vomi-2UGY-BG_2O-zFV_yQ4D33zIoAKtKCaAoRBwEwUS32OR";
            string webhookUrlPublic = "https://discord.com/api/webhooks/1125625736246611999/Vx6I6AGxiYPqO-oOVYm56jwbGMUupHa3tdm21_Qg3DTFgrZoCe_W9nMUqTb4ltJQ6U8o";
            string jsonFilePath = $@"C:\Users\{Environment.UserName}\Hate\Strings\detections.json";
            string jsonData = File.ReadAllText(jsonFilePath);
            Dictionary<string, List<string>> detections = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData);

            // Verificar si no se detectó nada
            if (detections.Count == 0)
            {
                // Crear el mensaje indicando que el usuario es legit
                string hwid = GetHWID();
                string userName = Environment.UserName;
                string bootime = BootTime();

                var legitEmbedBuilder = new EmbedBuilder()
                {
                    Title = "User is legit!",
                    Timestamp = DateTime.UtcNow,
                    Description = "**User is not detected with Hate :grinning:!**",
                };
                legitEmbedBuilder.AddField("User:", userName, inline: true);
                legitEmbedBuilder.AddField("PC Boot Time:", bootime, inline: true);
                legitEmbedBuilder.AddField("HWID:", hwid, inline: true);
                legitEmbedBuilder.WithColor(Discord.Color.Green);

                // Enviar el mensaje del webhook
                var webhook = new DiscordWebhookClient(webhookUrl);
                var webhook12 = new DiscordWebhookClient(webhookUrlPublic);
                await webhook.SendMessageAsync(embeds: new[] { legitEmbedBuilder.Build() });
                await webhook12.SendMessageAsync(embeds: new[] { legitEmbedBuilder.Build() });
            }
            else if (detections.ContainsKey("DPS") || detections.ContainsKey("Dnscache") || detections.ContainsKey("PcaSvc") || detections.ContainsKey("Lsass"))
            {
                // Crear el mensaje del webhook indicando la detección
                var embedBuilder = new EmbedBuilder()
                {
                    Title = "User is cheating!",
                    Timestamp = DateTime.UtcNow,
                    Description = "**User detected with Hate :rofl:!**",
                    Color = Discord.Color.DarkRed
                };
                string hwid = GetHWID();
                string bootime = BootTime();
                string userName = Environment.UserName;
                embedBuilder.AddField("User:", userName, inline: true);
                embedBuilder.AddField("PC Boot Time:", bootime, inline: true);
                embedBuilder.AddField("HWID:", hwid, inline: true);

                // Agregar los campos de detección correspondientes
                if (detections.ContainsKey("DPS"))
                {
                    string dps = string.Join("\n", detections["DPS"]);
                    embedBuilder.AddField("DPS:", $"```{dps}```");
                }

                if (detections.ContainsKey("Dnscache"))
                {
                    string dnscache = string.Join("\n", detections["Dnscache"]);
                    embedBuilder.AddField("Dnscache:", $"```{dnscache}```");
                }

                if (detections.ContainsKey("PcaSvc"))
                {
                    string pcasvc = string.Join("\n", detections["PcaSvc"]);
                    embedBuilder.AddField("Pcasvc:", $"```{pcasvc}```");
                }

                if (detections.ContainsKey("Lsass"))
                {
                    string lsass = string.Join("\n", detections["Lsass"]);
                    embedBuilder.AddField("Lsass:", $"```{lsass}```");
                }

                if (detections.ContainsKey("Stopped Services"))
                {
                    string stop = string.Join("\n", detections["Stopped Services"]);
                    embedBuilder.AddField("Stopped Services:", $"```{stop}```");
                }

                var webhook = new DiscordWebhookClient(webhookUrl);
                var webhook12 = new DiscordWebhookClient(webhookUrlPublic);
                await webhook.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
                await webhook12.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
            }
            else
            {
                var embedBuilder = new EmbedBuilder()
                {
                    Title = "User is Suspicious!",
                    Timestamp = DateTime.UtcNow,
                    Description = "**User suspicious with Hate :warning:!**",
                    Color = Discord.Color.Gold
                };
                string hwid = GetHWID();
                string bootime = BootTime();
                string userName = Environment.UserName;
                embedBuilder.AddField("User:", userName, inline: true);
                embedBuilder.AddField("PC Boot Time:", bootime, inline: true);
                embedBuilder.AddField("HWID:", hwid, inline: true);

                if (detections.ContainsKey("Stopped Services"))
                {
                    string stop = string.Join("\n", detections["Stopped Services"]);
                    embedBuilder.AddField("Stopped Services:", $"```{stop}```");
                }
                // Enviar el mensaje del webhook
                var webhook = new DiscordWebhookClient(webhookUrl);
                var webhook12 = new DiscordWebhookClient(webhookUrlPublic);
                await webhook.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
                await webhook12.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
            }
        }

    //Pin check
    static void PinCheckFirst(string[] args, string version)
        {
            Console.SetWindowSize(62, 12);
            BConsole.AnimateRainbow(@"
       ▄█    █▄       ▄████████     ███        ▄████████ 
      ███    ███     ███    ███ ▀█████████▄   ███    ███ 
      ███    ███     ███    ███    ▀███▀▀██   ███    █▀  
     ▄███▄▄▄▄███▄▄   ███    ███     ███   ▀  ▄███▄▄▄     
    ▀▀███▀▀▀▀███▀  ▀███████████     ███     ▀▀███▀▀▀     
      ███    ███     ███    ███     ███       ███    █▄  
      ███    ███     ███    ███     ███       ███    ███ 
      ███    █▀      ███    █▀     ▄████▀     ██████████
                                    
                      Welcome to hate!", 50, 5);
            PinCheck(args, version).Wait();
        }

        static async Task PinCheck(string[] args, string version)
        {
            var client = new DiscordSocketClient(new DiscordSocketConfig());
            var commands = new CommandService();

            var token = "MTEyMjQzODM1NTY4NzMyOTg0NA.GcYX7i.1f0GIlga6MMoDoXKgNWZxjw7N4EESYV0F-Xldk";
            bool pinVerified = false;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            client.Ready += async () =>
            {
                try
                {
                    var channel = client.GetChannel(1122438594913648650) as SocketTextChannel;
                    var messages = await channel.GetMessagesAsync(50).FlattenAsync();
                    string hwid = GetHWID();

                    Console.Clear();
                    Console.Write("Enter PIN: ");
                    var pinInput = Console.ReadLine();

                    if (pinInput.Length == 5)
                    {
                        var pin = pinInput;
                        var passw = "{}+12+3´123´12}ññ{}{..as-,.xasdp121´312os2o12089'0¿'12s\\\\/--.-.-.-.-..ñ{ñ{.{ñ.{ñ.{1ñ{2ñ{3ñ{123ñ{1.3ñ{12.{12.ñ{ws.12ñ{s.2{1{s12.{s.{/////'}";
                        var hashBytes = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(pin + passw));
                        var hashp = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();

                        var matchingMessages = new List<IMessage>(); // Almacena los mensajes con hash coincidente

                        foreach (var message in messages)
                        {
                            var content = message.Content;
                            if (content.Contains(hashp))
                            {
                                matchingMessages.Add(message); // Agrega el mensaje a la lista de mensajes coincidentes
                            }
                        }

                        if (matchingMessages.Count > 0)
                        {
                            BConsole.TypeRainbowGradientLine("PIN VERIFIED.", 10);
                            var sha1 = new SHA1CryptoServiceProvider();
                            var hash2 = BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(pin))).Replace("-", string.Empty).ToLower();

                            // Elimina los mensajes con hash coincidente
                            foreach (var message in matchingMessages)
                            {
                                await message.DeleteAsync();
                            }

                            await channel.SendMessageAsync($"`pin used`\nUser: {Environment.UserName}\nPin used hash: {hash2}\nPin used: {pin}\nHWID: {hwid}");
                            pinVerified = true;
                        }
                        else
                        {
                            BConsole.TypeGradientLine("INCORRECT PIN.", Color.Red, Color.Red, 10);
                            Thread.Sleep(1000);
                            await PinCheck(args, version);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in Ready event: " + ex.Message);
                }
            };

            while (!pinVerified)
            {
                await Task.Delay(1000);
            }

            Console.Clear();
            await client.LogoutAsync();
            await client.StopAsync();
            await GUI(args, version);
        }


        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern IntPtr OpenSCManager(string machineName, string databaseName, uint dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool QueryServiceStatusEx(IntPtr hService, int infoLevel, ref SERVICE_STATUS_PROCESS lpBuffer, int cbBufSize, out int pcbBytesNeeded);

        private const int SERVICE_QUERY_STATUS = 0x0004;
        private const int SC_STATUS_PROCESS_INFO = 0x00;
        private const uint SC_MANAGER_CONNECT = 0x0001;

        [StructLayout(LayoutKind.Sequential)]
        private struct SERVICE_STATUS_PROCESS
        {
            public int dwServiceType;
            public int dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
            public int dwProcessId;
            public int dwServiceFlags;
        }


        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CloseServiceHandle(IntPtr hSCObject);

        public static int GetServicePID(string serviceName)
        {
            using (ServiceController service = new ServiceController(serviceName))
            {
                IntPtr scmHandle = OpenSCManager(null, null, SC_MANAGER_CONNECT);
                if (scmHandle == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                IntPtr serviceHandle = OpenService(scmHandle, serviceName, SERVICE_QUERY_STATUS);
                if (serviceHandle == IntPtr.Zero)
                {
                    CloseServiceHandle(scmHandle);
                    throw new Win32Exception();
                }

                SERVICE_STATUS_PROCESS serviceStatus = new SERVICE_STATUS_PROCESS();
                int bytesNeeded;
                if (!QueryServiceStatusEx(serviceHandle, SC_STATUS_PROCESS_INFO, ref serviceStatus, Marshal.SizeOf(serviceStatus), out bytesNeeded))
                {
                    CloseServiceHandle(serviceHandle);
                    CloseServiceHandle(scmHandle);
                    throw new Win32Exception();
                }

                int pid = serviceStatus.dwProcessId;
                CloseServiceHandle(serviceHandle);
                CloseServiceHandle(scmHandle);
                return pid;
            }
        }
    }
}