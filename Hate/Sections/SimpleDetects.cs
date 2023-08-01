using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.ServiceProcess;

namespace Hate.Sections
{
    internal class SimpleDetects
    {
        public static void SimpleThings(string[] args, string version)
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

            DateTime explorerStartTime = Program.GetExplorerStartTime();
            string explorerStartTime1 = explorerStartTime != DateTime.MinValue ? explorerStartTime.ToString() : "Time not found.";
            Console.WriteLine($"Explorer: {explorerStartTime1}");

            DateTime csrssStartTime = Program.GetCsrssStartTime();
            string csrssStartTime1 = csrssStartTime != DateTime.MinValue ? csrssStartTime.ToString() : "Time not found.";
            Console.WriteLine($"Csrss: {csrssStartTime1}");

            DateTime smartStartTime = Program.GetSCStartTime();
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
            Program.GUI(args, version);

        }
    }
}
