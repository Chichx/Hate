using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Security.Cryptography;
using Color = System.Drawing.Color;
using BetterConsole;

namespace Hate.Sections
{
    internal class PinCheck
    {
        public static async Task PinChecks(string[] args, string version)
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
                    string hwid = Program.GetHWID();

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
                            await PinChecks(args, version);
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
            Program.GUI(args, version).Wait();
        }
    }
}
