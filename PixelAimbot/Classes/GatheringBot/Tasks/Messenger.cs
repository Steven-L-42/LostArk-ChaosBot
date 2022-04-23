using System;
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
    partial class GatheringBot
    {
        public async Task RunBotAsync(string telegramToken, CancellationToken token)
        {
            _telegramBotRunning = true;
            var bot = new TelegramBotClient(telegramToken);
            int offset = -1;
            _botIsRun = true;
            buttonConnectTelegram.Text = "disconnect";
            while (_botIsRun)
            {
                Telegram.Bot.Types.Update[] updates;

                try
                {
                    updates = await bot.GetUpdatesAsync(offset);
                    _telegramBotRunning = true;
                }
                catch (Exception ex)
                {
                    _botIsRun = false;
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
                            .AppendLine("/start - Starts the Bot")
                            .AppendLine("/stop - Stops the Bot doing anything")
                            .AppendLine("/info - Returns currently runtime and state of Bot")
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
                            _cts.Cancel();
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

                    if (text.Contains("/inv"))
                    {
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                        await Task.Delay(100);
                        var picture = new PrintScreen();
                        var screen = picture.CaptureScreen();

                        Stream stream =
                            ToStream(
                                CropImage(screen,
                                    new Rectangle(GatheringBot.Recalc(1322),
                                        PixelAimbot.GatheringBot.Recalc(189, false),
                                        GatheringBot.Recalc(544), GatheringBot.Recalc(640, false))), ImageFormat.Png);
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
                _conf.telegram = textBoxTelegramAPI.Text;
                _conf.Save();
            }
            catch (Exception ex)
            {
                labelTelegramState.Text = "State = error!";
                labelTelegramState.ForeColor = Color.Red;
            }
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
                TelegramTask = RunBotAsync(_conf.telegram, _telegramToken.Token);
                buttonConnectTelegram.Text = "disconnect";
                buttonTestTelegram_Click_1(null, null);
            }
        }
    }
}