using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixelAimbot.Classes.Misc;
using Telegram.Bot;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        /*
         * Telegram
         */
        public async Task RunBotAsync(string apikey, CancellationToken token)
        {
            _telegramBotRunning = true;
            var bot = new TelegramBotClient(apikey);
            int offset = -1;
            _botIsRun = true;
            buttonConnectTelegram.Text = "disconnect";

            while (_botIsRun)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    Telegram.Bot.Types.Update[] updates;

                    try
                    {
                        updates = await bot.GetUpdatesAsync(offset);
                        _telegramBotRunning = true;
                    }
                    catch (Exception ex)
                    {
                        _botIsRun = false;
                        buttonConnectTelegram.Text = "connect";
                        continue;
                    }

                    foreach (var update in updates)
                    {
                        offset = update.Id + 1;

                        if (update.Message == null)
                        {
                            continue;
                        }

                        if (update.Message.Date < DateTime.Now.AddHours(-2))
                        {
                            continue;
                        }

                        string text = update.Message.Text.ToLower();
                        var chatId = update.Message.Chat.Id;
                        if (text.Contains("/help"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("Currently supported Commands")
                                .AppendLine("/start - Starts the Bot doing Chaosdungeon")
                                .AppendLine("/stop - Stops the Bot doing anything")
                                .AppendLine("/info - Returns currently runtime and state of Bot")
                                .AppendLine("/unstuck - Leaves Chaosdungeon and Restarts everything")
                                .AppendLine("/inv - Send a screenshot of your inventory")
                                .AppendLine("/screen - Send a Screenshot of Game");

                            await bot.SendTextMessageAsync(chatId, sb.ToString());
                        }

                        if (text.Contains("/start"))
                        {
                            if (_start == false)
                            {
                                btnStart_Click(null, null);
                                await bot.SendTextMessageAsync(chatId, "Bot started");
                            }
                            else
                            {
                                await bot.SendTextMessageAsync(chatId, "Bot already running!");
                            }
                        }

                        if (text.Contains("/stop"))
                        {
                            if (_stop)
                            {
                                btnPause_Click(null, null);
                                cts.Cancel();
                                await bot.SendTextMessageAsync(chatId, "Bot stopped!");
                            }
                            else
                            {
                                await bot.SendTextMessageAsync(chatId, "Bot isnt running!");
                            }
                        }

                        if (text.Contains("/info"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("State: " + lbStatus.Text)
                                .AppendLine("Runtime: " + FormMinimized.sw.Elapsed.Hours.ToString("D2") + ":" +
                                            FormMinimized.sw.Elapsed.Minutes.ToString("D2") + ":" +
                                            FormMinimized.sw.Elapsed.Seconds.ToString("D2"));

                            await bot.SendTextMessageAsync(chatId, sb.ToString());
                        }

                        if (text.Contains("/unstuck"))
                        {
                            if (_stop)
                            {
                                cts.Cancel();
                                await bot.SendTextMessageAsync(chatId, "Stopped current Process");
                                var t12 = Task.Run(() => Leavedungeon(cts.Token));

                                await bot.SendTextMessageAsync(chatId,
                                    "Leave Dungeon and send Screenshot in a few seconds");
                                await Task.WhenAny(new[] {t12});

                                await Task.Delay(humanizer.Next(10, 240) + 5000);

                                var picture = new PrintScreen();
                                Stream stream = ToStream(picture.CaptureScreen(), ImageFormat.Png);
                                await bot.SendPhotoAsync(chatId, stream);
                            }
                            else
                            {
                                await bot.SendTextMessageAsync(chatId, "Bot isnt running!");
                            }
                        }

                        if (text.Contains("/inv"))
                        {
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                            await Task.Delay(humanizer.Next(10, 240) + 100);
                            var picture = new PrintScreen();
                            var screen = picture.CaptureScreen();

                            Stream stream =
                                ToStream(
                                    CropImage(screen,
                                        new Rectangle(ChaosBot.Recalc(1322), PixelAimbot.ChaosBot.Recalc(189, false),
                                            ChaosBot.Recalc(544), ChaosBot.Recalc(640, false))), ImageFormat.Png);
                            await bot.SendPhotoAsync(chatId, stream);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                        }

                        if (text.Contains("/screen"))
                        {
                            var picture = new PrintScreen();
                            Stream stream = ToStream(picture.CaptureScreen(), ImageFormat.Png);
                            await bot.SendPhotoAsync(chatId, stream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                    Debug.WriteLine("[" + line + "]" + ex.Message);
                }
            }
        }
        private void buttonTestTelegram_Click_1(object sender, EventArgs e)
        {
            var bot = new TelegramBotClient(textBoxTelegramAPI.Text);
            try
            {
                bot.GetMeAsync().Wait();
                _telegramBotRunning = true;
                labelTelegramState.Text = "State = success!";
                labelTelegramState.ForeColor = Color.Green;
                conf.telegram = textBoxTelegramAPI.Text;
                conf.Save();
            }
            catch (Exception ex)
            {
                labelTelegramState.Text = "State = error!";
                labelTelegramState.ForeColor = Color.Red;
            }
        }

        private void textBoxTelegramAPI_TextChanged(object sender, EventArgs e)
        {
            conf.telegram = textBoxTelegramAPI.Text;
            conf.Save();
        }
        private void buttonConnectTelegram_Click(object sender, EventArgs e)
        {
            if (_botIsRun)
            {
                _botIsRun = false;
                labelTelegramState.Text = "State = disconnected";
                labelTelegramState.ForeColor = Color.White;
                buttonConnectTelegram.Text = "connect";
            }
            else
            {
                TelegramTask = RunBotAsync(textBoxTelegramAPI.Text, telegramToken.Token);
                buttonConnectTelegram.Text = "disconnect";
                buttonTestTelegram_Click_1(null, null);
            }
        }
    }
}