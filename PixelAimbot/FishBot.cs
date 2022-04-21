using AutoItX3Lib;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Emgu.CV.CvEnum;
using Telegram.Bot;
using WindowsInput;
using WindowsInput.Native;

namespace PixelAimbot
{
    public partial class FishBot : Form
    {
        ///                                                                                                                                               ///
        private bool _start = false;

        private bool _stop = false;
        private bool _REPAIR = false;
        private bool _LOGOUT = false;
        private bool _Fishing = false;
        private bool _Buff = false;
        private bool _canFish = true;
        private bool _Restart = false;
        private int _swap = 0;
        private int x, y, width, height = 0;
        private int rodCounter = 0;
        private bool telegramBotRunning = false;

        PrintScreen screenPrinter = new PrintScreen();

        private Image rawScreen;
        private Bitmap bitmapImage;
        Image<Bgr, byte> screenCapture;

        //SKILL AND COOLDOWN//

        public frmMinimized formMinimized = new frmMinimized();
        public static FishBot _FishBot;

        public Config conf = new Config();

        ///                                                                                                                                                 ///
        public string resourceFolder = "";


        private static readonly Random random = new Random();

        /////
        ///
        // 2. Import the RegisterHotKey Method
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id); 

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();

                switch (id)
                {
                    case 1:
                        btnStart_Click(null, null);

                        break;

                    case 2:
                        btnPause_Click(null, null);

                        break;
                }
            }

            base.WndProc(ref m);
        }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        private IntPtr handle;
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
            int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);


        private const UInt32 SWP_NOSIZE = 0x0001;

        private const UInt32 SWP_NOMOVE = 0x0002;

        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            uint uFlags);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }


        static int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        static int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

        public static int recalc(int value, bool horizontal = true)
        {
            decimal oldResolution;
            decimal newResolution;
            if (horizontal)
            {
                oldResolution = 1920;
                newResolution = screenWidth;
            }
            else
            {
                oldResolution = 1080;
                newResolution = screenHeight;
            }


            decimal normalized = (decimal) value * newResolution;
            decimal rescaledPosition = (decimal) normalized / oldResolution;

            int returnValue = Decimal.ToInt32(rescaledPosition);
            return returnValue;
        }

        public FishBot()
        {
            InitializeComponent();
            conf = Config.Load();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(recalc(0), recalc(842, false));
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string applicationFolder = Path.Combine(folder, "cb_res");
            _FishBot = this;
            resourceFolder = applicationFolder;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = RandomString(15);

            if (conf.telegram != "" && !telegramBotRunning)
            {
                textBoxTelegramAPI.Text = conf.telegram;
                try
                {
                    _ = RunBotAsync(conf.telegram);
                }
                catch
                {
                    // ignored
                }
            }

            label15.Text = Config.version;
            int FirstHotkeyId = 1;
            int FirstHotKeyKey = (int) Keys.F9;
            // Register the "F9" hotkey
            UnregisterHotKey(this.Handle, FirstHotkeyId);
            
            Boolean F9Registered = RegisterHotKey(
                this.Handle, FirstHotkeyId, 0x0000, FirstHotKeyKey
            );

            // Repeat the same process but with F10
            int SecondHotkeyId = 2;
            int SecondHotKeyKey = (int) Keys.F10;
            UnregisterHotKey( this.Handle, SecondHotKeyKey);

            Boolean F10Registered = RegisterHotKey(
                this.Handle, SecondHotkeyId, 0x0000, SecondHotKeyKey
            );
            // 4. Verify if both hotkeys were succesfully registered, if not, show message in the console
            if (!F9Registered)
            {
                btnStart_Click(null, null);
            }

            if (!F10Registered)
            {
                btnPause_Click(null, null);
                _cts.Cancel();
            }
        }


        public async Task RunBotAsync(string token)
        {
            telegramBotRunning = true;
            var bot = new TelegramBotClient(token);
            int offset = -1;
            var botIsRun = true;
            while (botIsRun)
            {
                Telegram.Bot.Types.Update[] updates;

                try
                {
                    updates = await bot.GetUpdatesAsync(offset);
                    telegramBotRunning = true;
                }
                catch (Exception ex)
                {
                    botIsRun = false;
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
                            .AppendLine("Runtime: " + formMinimized.sw.Elapsed.Hours.ToString("D2") + ":" +
                                        formMinimized.sw.Elapsed.Minutes.ToString("D2") + ":" +
                                        formMinimized.sw.Elapsed.Seconds.ToString("D2"));

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
                                cropImage(screen,
                                    new Rectangle(FishBot.recalc(1322), PixelAimbot.FishBot.recalc(189, false),
                                        FishBot.recalc(544), FishBot.recalc(640, false))), ImageFormat.Png);
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

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        public Stream ToStream(Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }


        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private void btnPause_Click(object sender, EventArgs e)

        {
            if (_stop == true)
            {
                _cts.Cancel();
                _start = false;
                _stop = false;

                formMinimized.Hide();
                formMinimized.sw.Reset();
                this.Show();
                this.TopMost = true;
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "STOPPED!"));
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_start == false)
            {
                try
                {
                    formMinimized.StartPosition = FormStartPosition.Manual;
                    formMinimized.updateLabel("Gatheringbot");
                    formMinimized.Location = new Point(0, recalc(28, false));
                    formMinimized.timerRuntimer.Enabled = true;
                    formMinimized.sw.Reset();
                    formMinimized.sw.Start();
                    formMinimized.Show();
                    formMinimized.Location = new Point(0, recalc(28, false));
                    formMinimized.Size = new Size(594, 28);

                    this.Hide();

                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Bot is starting..."));
                    _start = true;
                    _stop = true;
                    _cts = new CancellationTokenSource();
                    var token = _cts.Token;

                    var t1 = Task.Run(() => START(token));
                    if (chBoxAutoBuff.Checked == true)
                    {
                        _Buff = true;
                    }
                    else
                    {
                        _Buff = false;
                    }

                    if (chBoxLOGOUT.Checked == true && _start == true)
                    {
                        var logout = Task.Run(() => LOGOUTTIMER(token));
                    }
                    else
                    {
                        _LOGOUT = false;
                    }

                    await Task.WhenAny(new[] {t1});
                }
                catch (OperationCanceledException)
                {
                    // Handle canceled
                }
                catch (Exception)
                {
                    // Handle other exceptions
                }
            }
        }


        public async void LOGOUTTIMER(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay((int.Parse(txtLOGOUT.Text) * 1000) * 60, token);
                _LOGOUT = true;
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch
            {
            }
        }

        public async void BUFFTIMER(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay(15 * 1000 * 60 * 15, token); // 15 Minutes
                _Buff = true;
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task START(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                Process[] processName = Process.GetProcessesByName("LostArk");
                if (processName.Length == 1)
                {
                    handle = processName[0].MainWindowHandle;
                    SetForegroundWindow(handle);
                }

                await Task.Delay(1500, token);
                rodCounter = 0;

                var t12 = Task.Run(() => CheckGathering(token));
                await Task.Delay(1, token);
                var t14 = Task.Run(() => REPAIRCHECK(token));
                var t1 = Task.Run(() => CheckEnergy(token));

                await Task.WhenAny(new[] {t1, t12, t14});
            }
            catch
            {
            }
        }


        private async Task CheckEnergy(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    var template = new Image<Bgr, byte>(resourceFolder + "/energy_fish.png");
                    var mask = new Image<Bgr, byte>(resourceFolder + "/energy_fish.png");


                    var Detector = new ScreenDetector(template, mask, 0.9f, ChaosBot.recalc(683),
                        ChaosBot.recalc(979, false), ChaosBot.recalc(45), ChaosBot.recalc(33, false));


                    using (screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                    {
                        var item = Detector.GetBest(screenCapture, true);
                        if (item.HasValue)
                        {
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "No more Energy, Stopping"));
                            btnPause_Click(null, null);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        ///    FIGHT SEQUENCES    ///
        private async Task CheckGathering(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                var template = new Image<Bgr, byte>(resourceFolder + "/gathering.png");
                var mask = new Image<Bgr, byte>(resourceFolder + "/gathering.png");


                var Detector = new ScreenDetector(template, null, 0.80f, ChaosBot.recalc(550),
                    ChaosBot.recalc(997, false), ChaosBot.recalc(56), ChaosBot.recalc(54, false));
                using (screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                {


                    var item = Detector.GetBest(screenCapture, true);
                    if (item.HasValue)
                    {
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Switch to Gathering"));
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_B);
                    }
                }

                if (chBoxAutoBuff.Checked && _Buff == true)
                {
                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Use Buff..."));
                    VirtualMouse.MoveTo(x + (width / 2), y + (height / 2), 5);

                    await Task.Delay(1000, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_W);
                    await Task.Delay(3000, token);
                    _Buff = false;
                    var bufftimer = Task.Run(() => BUFFTIMER(token));
                }

                var t3 = Task.Run(() => ThrowFishingRod(token));
                await Task.WhenAny(new[] {t3});
            }
            catch
            {
            }
        }

        private async Task ThrowFishingRod(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                if (_LOGOUT)
                {
                    var t3 = Task.Run(() => LOGOUT(token));
                    await Task.WhenAny(new[] {t3});
                }

                rodCounter++;
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Fishing (" + rodCounter + ")..."));

                VirtualMouse.MoveTo(x + (width / 2), y + (height / 2), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_Q);
                await Task.Delay(3000, token);
                var fishing = true;
                int failCounter = 0;

                var template = new Image<Bgr, byte>(resourceFolder + "/attention2.png");

                var detector = new ScreenDetector(template, null, 0.91f, ChaosBot.recalc(950),
                    ChaosBot.recalc(465, false), ChaosBot.recalc(20), ChaosBot.recalc(44, false));
                detector.setMatchingMethod(TemplateMatchingType.SqdiffNormed);
                while (fishing)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        rawScreen = screenPrinter.CaptureScreen();
                        bitmapImage = new Bitmap(rawScreen);
                        using (screenCapture = bitmapImage.ToImage<Bgr, byte>())
                        {
                            var item = detector.GetClosest(screenCapture, true);
                            if (item.HasValue)
                            {
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_Q);
                                await Task.Delay(500, token);
                                _swap++;
                                fishing = false;
                                lbStatus.Invoke((MethodInvoker) (() =>
                                    lbStatus.Text = "Fishing (" + rodCounter + ") success..."));
                            }
                            else
                            {
                                failCounter++;
                                if (failCounter > 80)
                                {
                                    fishing = false;
                                    lbStatus.Invoke((MethodInvoker) (() =>
                                        lbStatus.Text = "Fishing (" + rodCounter + ") failed..."));
                                }
                            }
                        }
                    }
                    catch
                    {
                        fishing = false;
                    }

                    await Task.Delay(100, token);
                }

                Random rnd = new Random();

                await Task.Delay((5500 + rnd.Next(10, 200)), token);
                if (_REPAIR)
                {
                    var t3 = Task.Run(() => RepairTask(token));
                    await Task.WhenAny(new[] {t3});
                }
                else if (_Buff)
                {
                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Use Buff..."));
                    VirtualMouse.MoveTo(x + (width / 2), y + (height / 2), 5);

                    await Task.Delay(1000, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_W);
                    await Task.Delay(5000, token);
                    _Buff = false;
                    var t3 = Task.Run(() => RESTART(token));
                    await Task.WhenAny(new[] {t3});
                }
                else
                {
                    var t3 = Task.Run(() => RESTART(token));
                    await Task.WhenAny(new[] {t3});
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        ///    BOT COMES TO THE END    ///
        private async Task RESTART(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                _Restart = true;
                if (chBoxChannelSwap.Checked == true)
                {
                    if (_swap == 15)
                    {
                        VirtualMouse.MoveTo(recalc(1875), recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(1000);
                        VirtualMouse.MoveTo(recalc(1875), recalc(43, false), 10);
                        KeyboardWrapper.HoldKey(KeyboardWrapper.VK_LBUTTON, 2000);
                        _swap++;
                        _Restart = false;
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(2000);
                        var t9 = Task.Run(() => RESTART(token));
                        await Task.WhenAny(new[] {t9});
                    }
                    else if (_swap == 30)
                    {
                        VirtualMouse.MoveTo(recalc(1875), recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(1000);
                        VirtualMouse.MoveTo(recalc(1875), recalc(63, false), 10);
                        KeyboardWrapper.HoldKey(KeyboardWrapper.VK_LBUTTON, 2000);
                        _swap++;
                        _Restart = false;
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(2000);
                        var t9 = Task.Run(() => RESTART(token));
                        await Task.WhenAny(new[] {t9});
                    }
                    else if (_swap == 45)
                    {
                        VirtualMouse.MoveTo(recalc(1875), recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(1000);
                        VirtualMouse.MoveTo(recalc(1875), recalc(83, false), 10);
                        KeyboardWrapper.HoldKey(KeyboardWrapper.VK_LBUTTON, 2000);
                        _swap = 0;
                        _Restart = false;
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(2000);
                        var t9 = Task.Run(() => RESTART(token));
                        await Task.WhenAny(new[] {t9});
                    }
                }

                if (_Restart == true)
                {
                    await Task.Delay(1000, token);
                    var t1 = Task.Run(() => CheckGathering(token));
                    await Task.WhenAny(new[] {t1});
                }
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task LOGOUT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                for (int i = 0; i < 1; i++)
                {
                    await Task.Delay(2000, token);

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "LOGOUT Process starts..."));
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                    await Task.Delay(2000, token);
                    VirtualMouse.MoveTo(recalc(1238), recalc(728, false), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    await Task.Delay(2000, token);
                    VirtualMouse.MoveTo(recalc(906), recalc(575, false), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    await Task.Delay(1000, token);

                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "You are logged out!"));
                    _start = false;
                    _cts.Cancel();
                }
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task REPAIRCHECK(CancellationToken token)

        {
            while (true)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    var template = new Image<Bgr, byte>(resourceFolder + "/gatheringRepair.png");
                    var mask = new Image<Bgr, byte>(resourceFolder + "/gatheringRepair.png");


                    var Detector = new ScreenDetector(template, mask, 0.90f, ChaosBot.recalc(1456),
                        ChaosBot.recalc(65, false), ChaosBot.recalc(13), ChaosBot.recalc(11, false));
                    using (screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                    {
                        var item = Detector.GetBest(screenCapture, true);
                        if (item.HasValue)
                        {
                            // Found

                            _REPAIR = true;
                        }
                    }
                }
                catch
                {
                }

                await Task.Delay(2000, token);
            }
        }

        private async Task RepairTask(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Auto-Repair starts ..."));
            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            // KLICK UNTEN RECHTS (RATGEBER)
            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);
            VirtualMouse.MoveTo(recalc(1741), recalc(1040, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
            await Task.Delay(500, token);

            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            // KLICK AUF BEGLEITER
            await Task.Delay(500, token);
            VirtualMouse.MoveTo(recalc(1684), recalc(823, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            // KLICK AUF AMBOSS
            await Task.Delay(500, token);
            VirtualMouse.MoveTo(recalc(1291), recalc(693, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            // KLICK AUF REPARIEREN
            await Task.Delay(500, token);
            VirtualMouse.MoveTo(recalc(717), recalc(745, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            await Task.Delay(500, token);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

            // 2x ESCAPE REPARATUR UND BEGLEITER FENSTER SCHLIEßEN
            await Task.Delay(1500, token);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
            
            await Task.Delay(3000, token);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
            
            await Task.Delay(1500, token);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);

            _REPAIR = false;
            var t3 = Task.Run(() => ThrowFishingRod(token));
            await Task.WhenAny(new[] {t3});
        }

        private void lbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        public void FishBot_Load(object sender, EventArgs e)
        {
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);


            txPQ.Text = Properties.Settings.Default.txPQ;
            txPW.Text = Properties.Settings.Default.txPW;
            txPE.Text = Properties.Settings.Default.txPE;
            txPR.Text = Properties.Settings.Default.txPR;
            txPA.Text = Properties.Settings.Default.txPA;
            txPS.Text = Properties.Settings.Default.txPS;
            txPD.Text = Properties.Settings.Default.txPD;
            txPF.Text = Properties.Settings.Default.txPF;


            chBoxAutoRepair.Checked = Properties.Settings.Default.chBoxAutoRepair;

            chBoxChannelSwap.Checked = Properties.Settings.Default.chBoxChannelSwap;
            chBoxAutoBuff.Checked = Properties.Settings.Default.chBoxSaveAll;
        }

        private void FishBot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }


        private void checkIsDigit(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }


        private void chBoxAutoRepair_CheckedChanged(object sender, EventArgs e)
        {
            if (!chBoxAutoRepair.Checked)
            {
                _REPAIR = false;
            }
        }

        private void chBoxLOGOUT_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxLOGOUT.Checked)
            {
                txtLOGOUT.ReadOnly = false;
            }
            else if (!chBoxLOGOUT.Checked)
            {
                txtLOGOUT.ReadOnly = true;
                _LOGOUT = false;
            }
        }


        private void btnInstructions_Click(object sender, EventArgs e)
        {
            frmGuide Form = new frmGuide();
            Form.Show();
        }


        private void lbStatus_TextChanged(object sender, EventArgs e)
        {
            formMinimized.labelMinimizedState.Text = lbStatus.Text;
        }

        private void textBoxTelegramAPI_TextChanged(object sender, EventArgs e)
        {
            conf.telegram = textBoxTelegramAPI.Text;
            conf.Save();
            if (!telegramBotRunning)
            {
                _ = RunBotAsync(textBoxTelegramAPI.Text);
            }
        }

        private void buttonSelectArea_Click(object sender, EventArgs e)
        {
            this.Hide();
            SelectArea form1 = new SelectArea(false);
            form1.InstanceRef = this;
            form1.Show();
        }

        public void updateArea(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            btnStart.Enabled = true;
            btnPause.Enabled = true;
        }

        private void labelSwap_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            
            if (Application.OpenForms.OfType<PixelAimbot.ChaosBot>().Count() == 1)
                Application.OpenForms.OfType<PixelAimbot.ChaosBot>().First().Close();

            ChaosBot Form = new ChaosBot();
            Form.Show();
            Application.OpenForms.OfType<PixelAimbot.FishBot>().First().Hide();
            Application.OpenForms.OfType<PixelAimbot.FishBot>().First().Close();
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Setup Bot"));

            var processName = Process.GetProcessesByName("LostArk");
            if (processName.Length == 1)
            {
                handle = processName[0].MainWindowHandle;
                SetForegroundWindow(handle);
            }

            Thread.Sleep(1500);
            var template = new Image<Bgr, byte>(resourceFolder + "/gathering.png");
            var mask = new Image<Bgr, byte>(resourceFolder + "/gathering.png");


            var detector = new ScreenDetector(template, mask, 0.75f, ChaosBot.recalc(550),
                ChaosBot.recalc(997, false), ChaosBot.recalc(56), ChaosBot.recalc(54, false));
            using (screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
            {
                var item = detector.GetBest(screenCapture, true);
                if (item.HasValue)
                {
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_B);
                }
            }

            Thread.Sleep(1000);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_L);
            VirtualMouse.MoveTo(ChaosBot.recalc(666), ChaosBot.recalc(489, false), 10);
            KeyboardWrapper.KeyDown(KeyboardWrapper.VK_LBUTTON);
            VirtualMouse.MoveTo(ChaosBot.recalc(698), ChaosBot.recalc(998, false), 10);
            KeyboardWrapper.KeyUp(KeyboardWrapper.VK_LBUTTON);
            VirtualMouse.MoveTo(ChaosBot.recalc(666), ChaosBot.recalc(588, false), 10);
            KeyboardWrapper.KeyDown(KeyboardWrapper.VK_LBUTTON);
            VirtualMouse.MoveTo(ChaosBot.recalc(744), ChaosBot.recalc(998, false), 10);
            KeyboardWrapper.KeyUp(KeyboardWrapper.VK_LBUTTON);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Setup Done"));


            handle = Process.GetCurrentProcess().MainWindowHandle;
            SetForegroundWindow(handle);
        }

        private void btnInstructions_Click_1(object sender, EventArgs e)
        {
            frmGuideFishbot fishbotGuide = new frmGuideFishbot();
            fishbotGuide.Show();
        }
    }
}