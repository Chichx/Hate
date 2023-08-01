using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Webhook;
using System.Management;
using Newtonsoft.Json;
using System.ServiceProcess;
using BetterConsole;
using System.ComponentModel;

namespace Hate

{
    class Program
    {
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

                if (version != "3.2")
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
                    await webhook.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
                    Console.Clear();
                    PinCheckFirstAsync(args, version);
                }
            }
        }


        public static Task GUI(string[] args, string version)
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
                        Sections.SimpleDetects.SimpleThings(args, version);
                        break;
                    case 2:
                        Console.Title = $"Hate | Programs";
                        Console.Clear();
                        Sections.Programas.Programs(args, version);
                        break;
                    case 3:
                        Console.Title = $"Hate | Time Modification";
                        Console.Clear();
                        Sections.TimeModification.Modification(args, version);
                        break;
                    case 4:
                        Console.Title = $"Hate | Partition Disk";
                        Console.Clear();
                        Sections.Partition.Partitions(args, version);
                        break;
                    case 5:
                        Console.Title = $"Hate | Executed Programs";
                        Console.Clear();
                        Sections.ExecutedProgram.ExecutedPrograms(args, version);
                        break;
                    case 6:
                        Console.Title = $"Hate | PcaClient Viewer";
                        Console.Clear();
                        Sections.Pca.PcaSvc(args, version);
                        break;
                    case 7:
                        Console.Title = $"Hate | Amcache Hash Detector";
                        Console.Clear();
                        Sections.AmCache.Amcache(args, version);
                        break;
                    case 8:
                        Console.Title = $"Hate | Better detection file";
                        Console.Clear();
                        Sections.BetterDetectFile.BetterFileAsync(args, version).Wait();
                        break;
                    case 9:
                        Console.Title = $"Hate | Prefetch Filter";
                        Console.Clear();
                        Sections.WinPrefetch.WinPrefetchView(args, version);
                        break;
                    case 10:
                        Console.Title = $"Hate | String Scanner";
                        Console.Clear();
                        Sections.ManualStringScanner.StringScanner(args, version).Wait();
                        break;
                    case 11:
                        Console.Title = $"Hate | String Scanner (Automatic)";
                        Console.Clear();
                        Sections.AutomaticStringScanner.DNSCache(args, version);
                        break;
                    case 12:
                        Console.Title = $"Hate | Exit and Credits";
                        Console.Clear();
                        Sections.SelfDestruct.ExitAndCredits();
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

        public static string GetHWID()
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

        public static DateTime GetBootTime()
        {
            var mc = new ManagementClass("Win32_OperatingSystem");
            foreach (var mo in mc.GetInstances())
            {
                var lastBootUpTime = mo.Properties["LastBootUpTime"].Value.ToString();
                return ManagementDateTimeConverter.ToDateTime(lastBootUpTime);
            }
            return DateTime.MinValue;
        }

        public static DateTime GetExplorerStartTime()
        {
            Process[] explorerProcesses = Process.GetProcessesByName("explorer");

            if (explorerProcesses.Length > 0)
            {
                Process explorerProcess = explorerProcesses[0];
                return explorerProcess.StartTime;
            }

            return DateTime.MinValue;
        }

        public static DateTime GetCsrssStartTime()
        {
            Process[] csrssProcesses = Process.GetProcessesByName("csrss");

            if (csrssProcesses.Length > 0)
            {
                Process csrss1Processes = csrssProcesses[0];
                return csrss1Processes.StartTime;
            }

            return DateTime.MinValue;
        }

        public static DateTime GetSCStartTime()
        {
            Process[] SMCProcesses = Process.GetProcessesByName("smartscreen");

            if (SMCProcesses.Length > 0)
            {
                Process smartProcesses = SMCProcesses[0];
                return smartProcesses.StartTime;
            }

            return DateTime.MinValue;
        }

        public static DateTime startTime()
        {
            return DateTime.Now.AddMilliseconds(-Environment.TickCount);
        }
        public static string BootTime()
        {
            DateTime bootTime = DateTime.MinValue;
            using (var uptime = new PerformanceCounter("System", "System Up Time"))
            {
                uptime.NextValue(); 
                bootTime = DateTime.Now - TimeSpan.FromSeconds(uptime.NextValue());
            }

            return bootTime.ToString();
        }

        //static void Hola(string[] args, string version)
        //{
        //    string sielmodulo = new WebClient().DownloadString("https://pastebin.com/raw/bS9Ym4vF");

        //    Console.WriteLine("scaning amigo!");

        //    for (int i = 0; i < Process.GetProcesses().Length; i++)
        //    {
        //        Process procesowtf = Process.GetProcesses()[i];

        //        if (procesowtf.ProcessName.Contains("GTAProcess"))
        //        {
        //            Console.WriteLine($"jaja se encontro gta id: {procesowtf.Id}");

        //            for (int i2 = 0; i2 < procesowtf.Modules.Count; i2++)
        //            {
        //                ProcessModule modulo = procesowtf.Modules[i2];

        //                if (!sielmodulo.Contains(modulo.ModuleName) && !modulo.ModuleName.EndsWith(".exe"))
        //                {
        //                    Console.WriteLine($"Se encontro un modulo desconocido {modulo.ModuleName}");
        //                }
        //            }
        //        }
        //    }
        //    Console.WriteLine("scaning terminado!");
        //    Console.ForegroundColor = ConsoleColor.White;
        //    Console.WriteLine("\nPress ENTER to go to the menu...");
        //    Console.ReadLine();
        //    Console.Clear();
        //    GUI(args, version).Wait();
        //}

        public static void CheckUser(string[] args, string version)
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

        public static async Task SendWebhook()
        {
            string webhookUrl = "https://discordapp.com/api/webhooks/1121269735787597894/WdpT5dhSz5mLoZEVmRg_Vomi-2UGY-BG_2O-zFV_yQ4D33zIoAKtKCaAoRBwEwUS32OR";
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
                await webhook.SendMessageAsync(embeds: new[] { legitEmbedBuilder.Build() });
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
                await webhook.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
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
                await webhook.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
            }
        }

    //Pin check
    public static void PinCheckFirstAsync(string[] args, string version)
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
            Sections.PinCheck.PinChecks(args, version).Wait();
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