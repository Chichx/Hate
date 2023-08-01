using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using Color = System.Drawing.Color;
using BetterConsole;

namespace Hate.Sections
{
    internal class SelfDestruct
    {
        public static void ExitAndCredits()
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
    }
}
