using AutoItX3Lib;
using Emgu.CV;
using Emgu.CV.Structure;
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
using Telegram.Bot;
using WindowsInput;

namespace PixelAimbot
{
    public partial class ChaosBot : Form
    {
        ///BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///
        ///                                                                                                                                               ///
        private bool _start = false;

        private bool _STOPP = false;
        private bool _stop = false;
        private bool _Restart = false;

        private bool _Floor1 = false;
        private bool _Floor2 = false;
        private bool _Floor3 = false;
        private bool _FloorFight = false;
        private bool _Searchboss = false;

        private bool PortalIsDetected = false;
        private bool PortalIsNotDetected = false;

        private bool _Revive = false;
        private bool _Portaldetect = false;
        private bool _Ultimate = false;
        private bool _Potions = false;

        private bool _REPAIR = false;
        private bool _Gunlancer = false;
        private bool _Shadowhunter = false;
        private bool _Berserker = false;
        private bool _Paladin = false;
        private bool _Deathblade = false;
        private bool _Sharpshooter = false;
        private bool _Bard = false;
        private bool _Sorcerer = false;
        private bool _Soulfist = false;

        private bool _LOGOUT = false;

        //SKILL AND COOLDOWN//
        private bool _Q = false;
        private bool _W = false;
        private bool _E = false;
        private bool _R = false;
        private bool _A = false;
        private bool _S = false;
        private bool _D = false;
        private bool _F = false;

        private System.Timers.Timer timer;
        private int fightSequence = 0;
        private int fightSequence2 = 0;
        private int searchSequence = 0;
        private int searchSequence2 = 0;
        private int fightOnSecondAbility = 1;
        private int walktopUTurn = 1;
        private int leavetimer = 0;
        private int leavetimer2 = 0;
        private int leavetimer1 = 0;
        private int Floor2 = 1;
        private int Floor3 = 1;
        private int _swap = 0;
        private int _FormExists = 0;

        public frmMinimized formMinimized = new frmMinimized();
        public Config conf = new Config();
        private bool telegramBotRunning = false;
        Random humanizer = new Random();

        public Task TelegramTask;
        ///                                                                                                                                                 ///
        ///BOOLS ENDE////////////BOOLS ENDE////////////////BOOLS ENDE//////////////////BOOLS ENDE///////////////BOOLS ENDE/////////////////////BOOLS ENDE/////
        /// OPENCV START  /// OPENCV START  /// OPENCV START  /// OPENCV START
        public string resourceFolder = "";

        private Priorized_Skills SKILLS = new Priorized_Skills();

        private (int, int) PixelToAbsolute(double x, double y, Point screenResolution)
        {
            int newX = (int) (x);// / screenResolution.X * 65535);
            int newY = (int) (y);// / screenResolution.Y * 65535);
            return (newX, newY);
        }

        private static readonly Random random = new Random();
        public Rotations rotation = new Rotations();

        /////
        ///
        // 2. Import the RegisterHotKey Method
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id); 
        

        protected override void WndProc(ref Message m)
        {
            // 5. Catch when a HotKey is pressed !
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();
                // MessageBox.Show(string.Format("Hotkey #{0} pressed", id));

                // 6. Handle what will happen once a respective hotkey is pressed
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

        private AutoItX3 au3 = new AutoItX3();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
            int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        private static readonly IntPtr HWND_TOP = new IntPtr(0);

        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        private const UInt32 SWP_NOSIZE = 0x0001;

        private const UInt32 SWP_NOMOVE = 0x0002;

        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            uint uFlags);

        public Layout_Keyboard currentLayout;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public static string ConfigPath { get; set; } = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();

        private static int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        private static int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

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

            int returnValue = value;
            if (oldResolution != newResolution)
            {
                decimal normalized = (decimal) value * newResolution;
                decimal rescaledPosition = (decimal) normalized / oldResolution;

                 returnValue = Decimal.ToInt32(rescaledPosition);
            }

            return returnValue;
        }

        public ChaosBot()
        {
            InitializeComponent();
            conf = Config.Load();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(recalc(0), recalc(842, false));
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string applicationFolder = Path.Combine(folder, "cb_res");

            resourceFolder = applicationFolder;
            this.FormBorderStyle = FormBorderStyle.None;
            refreshRotationCombox();
            this.Text = RandomString(15);
            // 3. Register HotKeys
            label15.Text = Config.version;
            // Set an unique id to your Hotkey, it will be used to
            // identify which hotkey was pressed in your code to execute something
            int FirstHotkeyId = 1;
            // Set the Hotkey triggerer the F9 key
            // Expected an integer value for F9: 0x78, but you can convert the Keys.KEY to its int value
            // See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx
            int FirstHotKeyKey = (int) Keys.F9;
            // Register the "F9" hotkey
            UnregisterHotKey(this.Handle, FirstHotkeyId);
            Boolean F9Registered = RegisterHotKey(this.Handle, FirstHotkeyId, 0x0000, FirstHotKeyKey);

            // Repeat the same process but with F10
            int SecondHotkeyId = 2;
            int SecondHotKeyKey = (int) Keys.F10;
            UnregisterHotKey(this.Handle, SecondHotkeyId);
            Boolean F10Registered = RegisterHotKey(this.Handle, SecondHotkeyId, 0x0000, SecondHotKeyKey);
            telegramToken = new CancellationTokenSource();
            if (conf.telegram != "" && !telegramBotRunning)
            {
                
                
                textBoxTelegramAPI.Text = conf.telegram;
                try
                {
                    buttonTestTelegram_Click_1(null, null);
                    TelegramTask = RunBotAsync(conf.telegram, telegramToken.Token);
                    
                }
                catch
                {
                }
            }

            // 4. Verify if both hotkeys were succesfully registered, if not, show message in the console
            if (!F9Registered)
            {
                btnStart_Click(null, null);
            }

            if (!F10Registered)
            {
                btnPause_Click(null, null);
                cts.Cancel();
            }
        }
        public bool botIsRun = true;
        public async Task RunBotAsync(string apikey, CancellationToken token)
        {
            telegramBotRunning = true;
            var bot = new TelegramBotClient(apikey);
            int offset = -1;
            botIsRun = true;
            buttonConnectTelegram.Text = "Verbunden, jetzt Trennen?";
            
            while (botIsRun)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    Telegram.Bot.Types.Update[] updates;
                
                    try
                    {
                        updates = await bot.GetUpdatesAsync(offset);
                        telegramBotRunning = true;
                    }
                    catch (Exception ex)
                    {
                        botIsRun = false;
                        buttonConnectTelegram.Text = "Verbinden";
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
                                .AppendLine("Runtime: " + formMinimized.sw.Elapsed.Hours.ToString("D2") + ":" +
                                            formMinimized.sw.Elapsed.Minutes.ToString("D2") + ":" +
                                            formMinimized.sw.Elapsed.Seconds.ToString("D2"));

                            await bot.SendTextMessageAsync(chatId, sb.ToString());
                        }

                        if (text.Contains("/unstuck"))
                        {
                            if (_stop)
                            {
                                cts.Cancel();
                                await bot.SendTextMessageAsync(chatId, "Stopped current Process");
                                var t12 = Task.Run(() => LEAVEDUNGEON(cts.Token));

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
                                    cropImage(screen,
                                        new Rectangle(ChaosBot.recalc(1322), PixelAimbot.ChaosBot.recalc(189, false),
                                            ChaosBot.recalc(544), ChaosBot.recalc(640, false))), ImageFormat.Png);
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
                } catch {}
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

        public void refreshRotationCombox()
        {
            string[] files = Directory.GetFiles(ConfigPath);
            comboBoxRotations.Items.Clear();
            foreach (string file in files)
            {
                if (Path.GetFileNameWithoutExtension(file) != "main")
                {
                    comboBoxRotations.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationTokenSource telegramToken = new CancellationTokenSource();

        private async void btnPause_Click(object sender, EventArgs e)

        {
            if (_stop == true)
            {
                cts.Cancel();
                _FormExists = 0;
                _start = false;
                _STOPP = false;
                _stop = false;
                _Restart = false;
                _LOGOUT = false;
                _REPAIR = false;

                _Gunlancer = false;
                _Shadowhunter = false;
                _Berserker = false;
                _Paladin = false;
                _Deathblade = false;
                _Sharpshooter = false;
                _Bard = false;
                _Sorcerer = false;
                _Soulfist = false;

                _Floor1 = false;
                _Floor2 = false;
                _Floor3 = false;
                _FloorFight = false;
                _Searchboss = false;

                _Revive = false;
                _Portaldetect = false;
                _Ultimate = false;
                _Potions = false;

                _Q = true;
                _W = true;
                _E = true;
                _R = true;
                _A = true;
                _S = true;
                _D = true;
                _F = true;

                this.Show();
                formMinimized.Hide();
                formMinimized.sw.Reset();
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "STOPPED!"));
                await Task.Delay(humanizer.Next(10, 240) + 1000);
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "READY!"));
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_start == false)
            {
                try
                {
                    _FormExists++;
                    if (_FormExists == 1)
                    {
                        formMinimized.StartPosition = FormStartPosition.Manual;
                        formMinimized.Location = new Point(0, recalc(28, false));
                        formMinimized.timerRuntimer.Enabled = true;
                        formMinimized.sw.Reset();
                        formMinimized.sw.Start();
                        formMinimized.Show();
                        formMinimized.Size = new Size(594, 28);
                        this.Hide();
                    }


                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "READY!"));
                    _start = true;
                    _stop = true;
                    cts = new CancellationTokenSource();
                    var token = cts.Token;

                    var t1 = Task.Run(() => START(token));

                    if (chBoxAutoRepair.Checked == true && _start == true)
                    {
                        var repair = Task.Run(() => REPAIRTIMER());
                    }
                    else
                    {
                        _REPAIR = false;
                    }

                    if (chBoxLOGOUT.Checked == true && _start == true)
                    {
                        var logout = Task.Run(() => LOGOUTTIMER());
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

        public async void REPAIRTIMER()
        {
            try
            {
              
                await Task.Delay(humanizer.Next(10, 240) + (int.Parse(txtRepair.Text) * 1000) * 60);
                _REPAIR = true;
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        public async void LOGOUTTIMER()
        {
            try
            {
                
                await Task.Delay(humanizer.Next(10, 240) + (int.Parse(txtLOGOUT.Text) * 1000) * 60);
                _LOGOUT = true;
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
            // timer.Elapsed += OnTimedEvent2;
            //timer.AutoReset = false;
            //timer.Enabled = true;
            //cts.Cancel();
        }

        
        public (int, int) searchImageAndClick(string templateImage, string templateMask, string foundText,
            float threshold = 0.7f, double softMultiplier = 1, double hardMultiplier = 1.2)
        {
            // Tunable variables
            var enemyTemplate =
                new Image<Bgr, byte>(resourceFolder + templateImage); // icon of the enemy
            var enemyMask =
                new Image<Bgr, byte>(resourceFolder +
                                     templateMask); // make white what the important parts are, other parts should be black
            //var screenCapture = new Image<Bgr, byte>("D:/Projects/bot-enemy-detection/EnemyDetection/screen.png");
            Point myPosition = new Point(recalc(148), recalc(127, false));
            Point screenResolution = new Point(screenWidth, screenHeight);

            // Main program loop
            var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
            var screenPrinter = new PrintScreen();

            var rawScreen = screenPrinter.CaptureScreen();
            Bitmap bitmapImage = new Bitmap(rawScreen);
            var screenCapture = bitmapImage.ToImage<Bgr, byte>();

            var enemy = enemyDetector.GetClosestEnemy(screenCapture, true);
            if (enemy.HasValue)
            {
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = foundText));

                CvInvoke.Rectangle(screenCapture,
                    new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                    new MCvScalar(255));

                double distance_x = (screenWidth - recalc(296)) / 2;
                double distance_y = (screenHeight - recalc(255, false)) / 2;

                var friend_position = ((enemy.Value.X + distance_x), (enemy.Value.Y + distance_y));
                double multiplier = softMultiplier;
                var friend_position_on_minimap = ((enemy.Value.X), (enemy.Value.Y));
                var my_position_on_minimap = ((recalc(296) / 2), (recalc(255, false) / 2));
                var dist = Math.Sqrt(Math.Pow((my_position_on_minimap.Item1 - friend_position_on_minimap.Item1), 2) +
                                     Math.Pow((my_position_on_minimap.Item2 - friend_position_on_minimap.Item2), 2));

                if (dist < 180)
                {
                    multiplier = 1.2;
                }

                double posx;
                double posy;
                if (friend_position.Item1 < (screenWidth / 2))
                {
                    posx = friend_position.Item1 * (2 - multiplier);
                }
                else
                {
                    posx = friend_position.Item1 * multiplier;
                }

                if (friend_position.Item2 < (screenHeight / 2))
                {
                    posy = friend_position.Item2 * (2 - multiplier);
                }
                else
                {
                    posy = friend_position.Item2 * multiplier;
                }

                return PixelToAbsolute(posx, posy, screenResolution);
            }

            return PixelToAbsolute((screenWidth / 2), (screenHeight / 2), screenResolution);
        }


        ///    START SEQUENCES    ///
        private async Task START(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Bot is starting..."));
                _Berserker = true;
                leavetimer = 0;
                leavetimer1 = 0;
                leavetimer2 = 0;
                Floor2 = 1;
                Floor3 = 1;
                searchSequence = 0;
                searchSequence2 = 0;
                fightSequence = 0;
                fightSequence2 = 0;
                

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                Process[] processName = Process.GetProcessesByName("LostArk");
                if (processName.Length == 1)
                {
                    handle = processName[0].MainWindowHandle;
                    SetForegroundWindow(handle);
                }

                /////////////// ANTI KICK ///////////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(recalc(960), recalc(529, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(recalc(960), recalc(529, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                await Task.Delay(humanizer.Next(10, 240) + 500, token);

                /////////////// PRESS G TO ENTER ///////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

                /////////////// CLICK ON ENTER /////////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);
                VirtualMouse.MoveTo(recalc(1467), recalc(858, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                /////////////// CLICK ON ACCEPT ///////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1000);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 9000, token);
                var t3 = Task.Run(() => MOVE(token));
                await Task.WhenAny(new[] {t3});
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task MOVE(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Set Transparency and Scale..."));

                VirtualMouse.MoveTo(recalc(1900), recalc(50, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                VirtualMouse.MoveTo(recalc(1871), recalc(260, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                VirtualMouse.MoveTo(recalc(1902), recalc(87, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                VirtualMouse.MoveTo(recalc(1871), recalc(260, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    var template = new Image<Bgr, byte>(resourceFolder + "/questmarker.png");

                    var detector = new ScreenDetector(template, null, 0.79f, ChaosBot.recalc(1890), ChaosBot.recalc(378, false), ChaosBot.recalc(28), ChaosBot.recalc(31, false));
                    var screenPrinter = new PrintScreen();
                    using(var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>()) {

                        var item = detector.GetBest(screenCapture, true);
                        if (item.HasValue)
                        {
                            VirtualMouse.MoveTo(recalc(1901), recalc(389, false), 10);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        }
                    }

                }
                catch { }


                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Bot moves to start the Dungeon..."));

                VirtualMouse.MoveTo(recalc(960), recalc(529, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                if (chBoxBerserker.Checked == true)
                {
                    KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                    KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));

                    _Berserker = false;
                    await Task.Delay(humanizer.Next(10, 240) + 1000);
                }

                _Floor1 = true;
                _STOPP = false;
                PortalIsNotDetected = true;
                var t16 = Task.Run(() => FLOOR1DETECTIONTIMER(token));
                var t12 = Task.Run(() => FLOORTIME(token));
                await Task.WhenAny(new[] { t12, t16 });
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        ///    FIGHT SEQUENCES    ///
        private async Task FLOORTIME(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                if (_Floor1 == true && _STOPP == false)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    walktopUTurn = 0;

                    // START TASK BOOLS //
                    _FloorFight = true;
                    _Revive = true;
                    _Ultimate = true;
                    _Portaldetect = true;
                    _Potions = true;
                    // CLASSES //
                    _Gunlancer = true;
                    _Shadowhunter = true;
                    _Berserker = true;
                    _Paladin = true;
                    _Deathblade = true;
                    _Sharpshooter = true;
                    _Sorcerer = true;
                    _Soulfist = true;
                    var t11 = Task.Run(() => SearchNearEnemys(token));
                    var t12 = Task.Run(() => FLOORFIGHT(token));
                    var t14 = Task.Run(() => ULTIMATE(token));
                    var t16 = Task.Run(() => REVIVE(token));
                    var t18 = Task.Run(() => PORTALDETECT(token));
                    var t20 = Task.Run(() => POTIONS(token));
                    await Task.WhenAny(new[] {t11, t12, t14, t16, t18, t20});
                }

                if (_Floor2 == true && _STOPP == false)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    fightOnSecondAbility = 1;
                    fightSequence++;
                    leavetimer++;
                    // START TASK BOOLS //
                    _FloorFight = true;
                    _Revive = true;
                    _Ultimate = true;
                    _Potions = true;

                    // PORTAL DETECT BOOL is on a lower place
                    // CLASSES //
                    _Gunlancer = true;
                    _Shadowhunter = true;
                    _Berserker = true;
                    _Paladin = true;
                    _Sharpshooter = true;
                    _Bard = true;
                    _Sorcerer = true;
                    _Soulfist = true;

                    if (leavetimer == 1 && chBoxActivateF3.Checked == false)
                    {
                        var t36 = Task.Run(() => LEAVETIMERFLOOR2(token));
                        await Task.WhenAny(new[] {t36});
                    }

                    if (leavetimer == 1 && chBoxAwakening.Checked == true)
                    {
                        var t39 = Task.Run(() => AWAKENINGSKILL(token));
                        await Task.WhenAny(new[] {t39});
                    }
                    var t11 = Task.Run(() => SearchNearEnemys(token));
                    var t12 = Task.Run(() => FLOORFIGHT(token));
                    var t14 = Task.Run(() => ULTIMATE(token));
                    var t16 = Task.Run(() => REVIVE(token));
                    var t20 = Task.Run(() => POTIONS(token));

                    await Task.Delay(humanizer.Next(10, 240) + int.Parse(txtDungeon2.Text) * 1000);

                    _FloorFight = false;

                    if (fightSequence >= 8 && chBoxActivateF3.Checked == true && _Floor3 == false)
                    {
                        fightSequence2 = 1;
                        _Portaldetect = true;
                        Floor2 = 2;
                        Floor3 = 2;
                        var t18 = Task.Run(() => PORTALDETECT(token));
                        await Task.WhenAny(new[] {t18});
                    }
                    else if (fightSequence < 8 && _STOPP == false)
                    {
                        _FloorFight = false;
                        _Potions = false;
                        _Revive = false;
                        _Searchboss = true;
                        var t13 = Task.Run(() => SEARCHBOSS(token));
                        await Task.WhenAny(new[] {t13});
                    }

                    await Task.WhenAny(new[] {t11, t12, t14, t16, t20});
                }

                if (_Floor3 == true && _STOPP == false)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    leavetimer2++;
                    fightSequence2++;
                    // START TASK BOOLS //
                    _FloorFight = true;
                    _Revive = true;
                    _Ultimate = true;
                    _Potions = true;
                    // CLASSES //
                    _Gunlancer = true;
                    _Shadowhunter = true;
                    _Berserker = true;
                    _Paladin = true;
                    _Deathblade = true;
                    _Sharpshooter = true;
                    _Bard = true;
                    _Sorcerer = true;
                    _Soulfist = true;

                    if (leavetimer2 == 1)
                    {
                        var t36 = Task.Run(() => LEAVETIMERFLOOR3(token));
                        await Task.WhenAny(new[] {t36});
                    }
                    var t11 = Task.Run(() => SearchNearEnemys(token));
                    var t12 = Task.Run(() => FLOORFIGHT(token));
                    var t14 = Task.Run(() => ULTIMATE(token));
                    var t16 = Task.Run(() => REVIVE(token));
                    var t20 = Task.Run(() => POTIONS(token));
                    await Task.Delay(humanizer.Next(10, 240) + int.Parse(txtDungeon3.Text) * 1000, token);

                    _FloorFight = false;

                    if (fightSequence2 < 7 && _STOPP == false)
                    {
                        _FloorFight = false;
                        _Potions = false;
                        _Revive = false;
                        _Searchboss = true;
                        var t13 = Task.Run(() => SEARCHBOSS(token));
                        await Task.WhenAny(new[] {t13});
                    }

                    await Task.WhenAny(new[] {t11, t12, t14, t16, t20});
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private object fight = "1";

        public Point calculateFromCenter(int x, int y)
        {
            
            int centerX = Screen.PrimaryScreen.Bounds.Width / 2;
            int centerY = Screen.PrimaryScreen.Bounds.Height / 2;
            int resultX = centerX;
            int resultY = centerY;

            resultX = centerX - recalc(500) + x;
            resultY = centerY  - recalc(390, false) + y;
            
            return new Point(resultX, resultY);
        }
        private async Task SearchNearEnemys(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                var template = new Image<Bgr, byte>(resourceFolder + "/red_hp.png");
                var detector = new ScreenDetector(template, null, 0.94f, ChaosBot.recalc(460), ChaosBot.recalc(120, false), ChaosBot.recalc(1000), ChaosBot.recalc(780, false));
                detector.setMyPosition(new Point(ChaosBot.recalc(500), ChaosBot.recalc(390, false)));
                var screenPrinter = new PrintScreen();

                while (_FloorFight && _STOPP == false)
                {

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        using(var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>()) {

                            var item = detector.GetClosest(screenCapture, true);
                            if (item.HasValue)
                            {
                                Point position = calculateFromCenter(item.Value.X, item.Value.Y);
                                // correct mouse down
                                int correction = 0;
                                if (item.Value.Y > recalc(383, false) && item.Value.Y < recalc(435, false))
                                {
                                    correction = recalc(80, false);
                                }
                                VirtualMouse.MoveTo(position.X,  position.Y + correction, 10);
                            }
                            else
                            {
                                // Not found Swirl around with Mouse
                                VirtualMouse.MoveTo(new Random().Next(recalc(460), recalc(1000)), new Random().Next(recalc(120, false), recalc(780, false)), 10);
                            }
                        }

                    }
                    catch { }
                }
            }
            catch {}
        }

        private bool isDoubleKey(byte key)
        {
            bool checkboxState = false;
            switch (key)
            {
                case KeyboardWrapper.VK_A:
                    checkboxState = chBoxDoubleA.Checked;
                    break;

                case KeyboardWrapper.VK_S:
                    checkboxState = chBoxDoubleS.Checked;
                    break;

                case KeyboardWrapper.VK_D:
                    checkboxState = chBoxDoubleD.Checked;
                    break;

                case KeyboardWrapper.VK_F:
                    checkboxState = chBoxDoubleF.Checked;
                    break;

                case KeyboardWrapper.VK_Q:
                    checkboxState = chBoxDoubleQ.Checked;
                    break;

                case KeyboardWrapper.VK_W:
                    checkboxState = chBoxDoubleW.Checked;
                    break;

                case KeyboardWrapper.VK_E:
                    checkboxState = chBoxDoubleE.Checked;
                    break;

                case KeyboardWrapper.VK_R:
                    checkboxState = chBoxDoubleR.Checked;
                    break;
            }

            return checkboxState;
        }
        private async Task FLOORFIGHT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                while (_FloorFight && _STOPP == false)
                {
                    try
                    {
                        foreach (KeyValuePair<byte, int> skill in SKILLS.skillset.OrderBy(x => x.Value))
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            if (_FloorFight && !_STOPP)
                            {
                                if (!isKeyOnCooldown(skill.Key))
                                {
                                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Bot is fighting..."));
                                    KeyboardWrapper.AlternateHoldKey(skill.Key, casttimeByKey(skill.Key));
                             
                                    if (isDoubleKey(skill.Key))
                                    {
                                        KeyboardWrapper.PressKey(skill.Key);
                                    }

                                    setKeyCooldown(skill.Key); // Set Cooldown
                                    var td = Task.Run(() => SkillCooldown(token, skill.Key));
                                    fightOnSecondAbility++;
                                }
                                else
                                {
                                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Bot is autoattacking..."));
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    walktopUTurn++;
                                    if (comboBoxAutoAttack.SelectedIndex == 0  || comboBoxAutoAttack.SelectedIndex == 1)
                                    {
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                    }

                                    if (comboBoxAutoAttack.SelectedIndex == 0)
                                    {
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                    }

                                    fightOnSecondAbility = 1;
                                }
                            }
                         
                       
                            if (walktopUTurn == 3 && chBoxAutoMovement.Checked && _Floor1 == true && _STOPP == false)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                VirtualMouse.MoveTo(recalc(960), recalc(240, false), 10);
                                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 2500);
                                VirtualMouse.MoveTo(recalc(960), recalc(566, false), 10);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                                walktopUTurn++;
                            }

                            if (walktopUTurn == 10 && chBoxAutoMovement.Checked && _Floor1 == true && _STOPP == false)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                VirtualMouse.MoveTo(recalc(523), recalc(840, false), 10);
                                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 2400);
                                VirtualMouse.MoveTo(recalc(1007), recalc(494, false), 10);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                await Task.Delay(1, token);

                                walktopUTurn++;
                            }

                            if (walktopUTurn == 17 && chBoxAutoMovement.Checked && _Floor1 == true && _STOPP == false)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                VirtualMouse.MoveTo(recalc(1578), recalc(524, false), 10);
                                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 2800);
                                VirtualMouse.MoveTo(recalc(905), recalc(531, false), 10);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                                walktopUTurn++;
                            }

                            if (walktopUTurn == 23 && chBoxAutoMovement.Checked && _Floor1 == true && _STOPP == false)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                VirtualMouse.MoveTo(recalc(523), recalc(850, false), 10);
                                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 2400);
                                VirtualMouse.MoveTo(recalc(960), recalc(500, false), 10);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                await Task.Delay(1, token);

                                walktopUTurn++;
                            }

                            if (walktopUTurn == 23 && chBoxAutoMovement.Checked && _Floor1 == true && _STOPP == false)
                            {
                                walktopUTurn = 1;
                                await Task.Delay(1, token);
                            }
                            
                       await Task.Delay(humanizer.Next(10, 240) + 400);
                        }
                    }
                    catch (AggregateException)
                    {
                        Debug.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Debug.WriteLine("Bug");
                    }
                    catch
                    {
                    }
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }
        private async Task FLOOR1DETECTIONTIMER(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 180000, token);


                if (PortalIsNotDetected == true)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    PortalIsNotDetected = false;

                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "ChaosDungeon Floor 1 Abort!"));

                    _STOPP = true;
                    PortalIsNotDetected = false;
                    _FloorFight = false;
                    _Searchboss = false;
                    _Revive = false;
                    _Ultimate = false;
                    _Portaldetect = false;
                    _Potions = false;
                    _Floor1 = false;
                    _Floor2 = false;
                    _Floor3 = false;

                    var leave = Task.Run(() => LEAVEDUNGEON(token));
                    await Task.WhenAny(new[] { leave });

                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task PORTALDETECT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                while (_Portaldetect == true && _STOPP == false)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);

                        object fight = au3.PixelSearch(recalc(114), recalc(208, false), recalc(168), recalc(220, false),
                            0xDBC7AC, 7);
                        if (fight.ToString() != "1" && _STOPP == false)
                        {
                            object[] fightCoord = (object[]) fight;
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Portal detected!"));

                            if (chBoxActivateF2.Checked && _STOPP == false)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                _Potions = false;
                                _Revive = false;
                                _Ultimate = false;
                                _Floor1 = false;
                                _FloorFight = false;

                                _STOPP = false;
                                if (Floor2 == 2)
                                {
                                    _Floor2 = false;
                                }

                                leavetimer1++;
                                if (leavetimer1 == 1)
                                {
                                    var t6 = Task.Run(() => LEAVETIMERFLOOR1(token));
                                    await Task.WhenAny(new[] {t6});
                                }

                                PortalIsDetected = true;
                                PortalIsNotDetected = false;
                                var t5 = Task.Run(() => PORTALISDETECTED(token));
                                var t7 = Task.Run(() => SEARCHPORTAL(token));
                                await Task.WhenAny(new[] {t5, t7});
                            }
                            else if (!chBoxActivateF2.Checked && _STOPP == false)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                lbStatus.Invoke(
                                    (MethodInvoker) (() => lbStatus.Text = "ChaosDungeon Floor 1 Complete!"));

                                _STOPP = true;
                                PortalIsNotDetected = false;
                                _FloorFight = false;
                                _Searchboss = false;
                                _Revive = false;
                                _Ultimate = false;
                                _Portaldetect = false;
                                _Potions = false;
                                _Floor1 = false;
                                _Floor2 = false;
                                _Floor3 = false;

                                var leave = Task.Run(() => LEAVEDUNGEON(token));
                                await Task.WhenAny(new[] {leave});
                            }
                        }
                    }
                    catch (AggregateException)
                    {
                        Debug.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Debug.WriteLine("Bug");
                    }
                    catch
                    {
                    }
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task PORTALISDETECTED(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                while (PortalIsDetected == true)
                {
                    try
                    {
                        object health10 = au3.PixelSearch(recalc(1898), recalc(10, false), recalc(1911),
                            recalc(22, false), 0x000000);

                        if (health10.ToString() != "1")
                        {
                            object[] health10Coord = (object[]) health10;
                            PortalIsDetected = false;
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Portalsearch done!"));
                        }

                        await Task.Delay(humanizer.Next(10, 240) + 100, token);
                    }
                    catch (AggregateException)
                    {
                        Debug.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Debug.WriteLine("Bug");
                    }
                    catch
                    {
                    }
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task SEARCHPORTAL(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                _Portaldetect = false;

                while (PortalIsDetected == true)
                    //  for (int i = 0; i <= int.Parse(txtPortalSearch.Text); i++)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(humanizer.Next(10, 240) + 100, token);
                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Search Portal..."));

                    var absolutePositions = searchImageAndClick("/portalenter1.png", "/portalentermask1.png",
                        "Floor 1: Portal found...");

                    VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);

                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 1: Enter Portal..."));

                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

                    Random random = new Random();
                    var sleepTime = random.Next(500, 570);
                    Thread.Sleep(sleepTime);
                }

                searchSequence = 1;

                await Task.Delay(humanizer.Next(10, 240) + 8000);
                _Searchboss = true;
                var t12 = Task.Run(() => SEARCHBOSS(token));
                await Task.WhenAny(new[] {t12});
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task SEARCHBOSS(CancellationToken token)
        {
            try
            {
                if (_Searchboss == true && _FloorFight == false && _STOPP == false)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    if (Floor2 == 1)
                    {
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 2: search enemy..."));
                    }

                    if (Floor2 == 2)
                    {
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 3: search enemy..."));
                    }

                    if (searchSequence == 1)
                    {
                        await Task.Delay(humanizer.Next(10, 240) + 1500);
                        VirtualMouse.MoveTo(recalc(960), recalc(529, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                        searchSequence = 2;
                    }

                    if (chBoxGunlancer.Checked == true && _Gunlancer == false && _Searchboss == true)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);

                        KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);
                        _Gunlancer = true;

                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Deactivate: Gunlancer Ultimate"));
                    }

                    for (int i = 0; i < int.Parse(txtDungeon2search.Text); i++)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(humanizer.Next(10, 240) + 100, token);
                        float threshold = 0.7f;

                        var enemyTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/enemy.png");
                        var enemyMask =
                            new Image<Bgr, byte>(resourceFolder + "/mask.png");
                        var BossTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/boss1.png");
                        var BossMask =
                            new Image<Bgr, byte>(resourceFolder + "/bossmask1.png");
                        var mobTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/mob1.png");
                        var mobMask =
                            new Image<Bgr, byte>(resourceFolder + "/mobmask1.png");
                        var portalTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/portalenter1.png");
                        var portalMask =
                            new Image<Bgr, byte>(resourceFolder + "/portalentermask1.png");

                        Point myPosition = new Point(recalc(148), recalc(127, false));
                        Point screenResolution = new Point(screenWidth, screenHeight);
                        var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                        var BossDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                        var mobDetector = new EnemyDetector(mobTemplate, mobMask, threshold);
                        var portalDetector = new EnemyDetector(portalTemplate, portalMask, threshold);
                        var screenPrinter = new PrintScreen();

                        var rawScreen = screenPrinter.CaptureScreen();
                        Bitmap bitmapImage = new Bitmap(rawScreen);
                        var screenCapture = bitmapImage.ToImage<Bgr, byte>();

                        var enemy = enemyDetector.GetClosestEnemy(screenCapture, true);
                        var Boss = BossDetector.GetClosestEnemy(screenCapture, true);
                        var mob = mobDetector.GetClosestEnemy(screenCapture, true);
                        var portal = portalDetector.GetClosestEnemy(screenCapture, true);

                        if (Boss.HasValue && _Searchboss == true)
                        {
                            CvInvoke.Rectangle(screenCapture,
                                new Rectangle(new Point(Boss.Value.X, Boss.Value.Y), BossTemplate.Size),
                                new MCvScalar(255));
                            double distance_x = (screenWidth - recalc(296)) / 2;
                            double distance_y = (screenHeight - recalc(255, false)) / 2;

                            var boss_position = ((Boss.Value.X + distance_x), (Boss.Value.Y + distance_y));
                            double multiplier = 1;
                            var boss_position_on_minimap = ((Boss.Value.X), (Boss.Value.Y));
                            var my_position_on_minimap = ((recalc(296) / 2), (recalc(255, false) / 2));
                            var dist = Math.Sqrt(
                                Math.Pow((my_position_on_minimap.Item1 - boss_position_on_minimap.Item1), 2) +
                                Math.Pow((my_position_on_minimap.Item2 - boss_position_on_minimap.Item2), 2));

                            if (dist < 180 && _Searchboss == true)
                            {
                                multiplier = 1.2;
                            }

                            double posx;
                            double posy;
                            if (boss_position.Item1 < (screenWidth / 2) && _Searchboss == true)
                            {
                                posx = boss_position.Item1 * (2 - multiplier);
                            }
                            else
                            {
                                posx = boss_position.Item1 * multiplier;
                            }

                            if (boss_position.Item2 < (screenHeight / 2) && _Searchboss == true)
                            {
                                posy = boss_position.Item2 * (2 - multiplier);
                            }
                            else
                            {
                                posy = boss_position.Item2 * multiplier;
                            }

                            var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);

                            if (Floor2 == 1 && _Searchboss == true)
                            {
                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 2: Big-Boss found!"));
                            }

                            if (Floor2 == 2 && _Searchboss == true)
                            {
                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 3: Big-Boss found!"));
                            }

                            VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);

                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 1000);
                        }
                        else
                        {
                            if (enemy.HasValue && _Searchboss == true)
                            {
                                CvInvoke.Rectangle(screenCapture,
                                    new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                    new MCvScalar(255));
                                double distance_x = (screenWidth - recalc(296)) / 2;
                                double distance_y = (screenHeight - recalc(255, false)) / 2;

                                var enemy_position = ((enemy.Value.X + distance_x), (enemy.Value.Y + distance_y));
                                double multiplier = 1;
                                var enemy_position_on_minimap = ((enemy.Value.X), (enemy.Value.Y));
                                var my_position_on_minimap = ((recalc(296) / 2), (recalc(255, false) / 2));
                                var dist = Math.Sqrt(
                                    Math.Pow((my_position_on_minimap.Item1 - enemy_position_on_minimap.Item1), 2) +
                                    Math.Pow((my_position_on_minimap.Item2 - enemy_position_on_minimap.Item2), 2));

                                if (dist < 180 && _Searchboss == true)
                                {
                                    multiplier = 1.2;
                                }

                                double posx;
                                double posy;
                                if (enemy_position.Item1 < (screenWidth / 2) && _Searchboss == true)
                                {
                                    posx = enemy_position.Item1 * (2 - multiplier);
                                }
                                else
                                {
                                    posx = enemy_position.Item1 * multiplier;
                                }

                                if (enemy_position.Item2 < (screenHeight / 2) && _Searchboss == true)
                                {
                                    posy = enemy_position.Item2 * (2 - multiplier);
                                }
                                else
                                {
                                    posy = enemy_position.Item2 * multiplier;
                                }

                                var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                if (Floor2 == 1 && _Searchboss == true)
                                {
                                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 2: Mid-Boss found!"));
                                }

                                if (Floor2 == 2 && _Searchboss == true)
                                {
                                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 3: Mid-Boss found!"));
                                }

                                VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);

                                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 1000);
                            }
                            else
                            {
                                if (mob.HasValue && _Searchboss == true)
                                {
                                    CvInvoke.Rectangle(screenCapture,
                                        new Rectangle(new Point(mob.Value.X, mob.Value.Y), mobTemplate.Size),
                                        new MCvScalar(255));
                                    double distance_x = (screenWidth - recalc(296)) / 2;
                                    double distance_y = (screenHeight - recalc(255, false)) / 2;

                                    var mob_position = ((mob.Value.X + distance_x), (mob.Value.Y + distance_y));
                                    double multiplier = 1;
                                    var mob_position_on_minimap = ((mob.Value.X), (mob.Value.Y));
                                    var my_position_on_minimap = ((recalc(296) / 2), (recalc(255, false) / 2));
                                    var dist = Math.Sqrt(
                                        Math.Pow((my_position_on_minimap.Item1 - mob_position_on_minimap.Item1), 2) +
                                        Math.Pow((my_position_on_minimap.Item2 - mob_position_on_minimap.Item2), 2));

                                    if (dist < 180 && _Searchboss == true)
                                    {
                                        multiplier = 1.2;
                                    }

                                    double posx;
                                    double posy;
                                    if (mob_position.Item1 < (screenWidth / 2) && _Searchboss == true)
                                    {
                                        posx = mob_position.Item1 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posx = mob_position.Item1 * multiplier;
                                    }

                                    if (mob_position.Item2 < (screenHeight / 2) && _Searchboss == true)
                                    {
                                        posy = mob_position.Item2 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posy = mob_position.Item2 * multiplier;
                                    }

                                    var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                    if (Floor2 == 1 && _Searchboss == true)
                                    {
                                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 2: Mob found!"));
                                    }

                                    if (Floor2 == 2 && _Searchboss == true)
                                    {
                                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 3: Mob found!"));
                                    }

                                    VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);

                                    KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 1000);
                                }
                            }
                        }

                        Random random = new Random();
                        var sleepTime = random.Next(100, 150);
                        Thread.Sleep(sleepTime);
                    }

                    if (Floor2 == 1)
                    {
                        _Floor1 = false;
                        _Floor2 = true;
                    }

                    if (Floor3 == 2)
                    {
                        _Floor1 = false;
                        _Floor2 = false;
                        _Floor3 = true;
                    }

                    _Searchboss = false;
                    var t12 = Task.Run(() => FLOORTIME(token));
                    await Task.WhenAny(new[] {t12});
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }


        ///    RUN AT SAME TIME    ///
        private async Task ULTIMATE(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                while (_Ultimate == true && _FloorFight == true && _STOPP == false)
                {
                    try
                    {
                        if (chBoxBard.Checked == true && _Bard == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));

                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Bard try to heal..."));
                        }

                        if (chBoxGunlancer.Checked == true && _Gunlancer == true ||
                            chBoxGunlancer2.Checked == true && _Gunlancer == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);
                            _Gunlancer = false;

                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Gunlancer Ultimate"));
                        }

                        if (chBoxY.Checked == true && _Shadowhunter == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            object d = au3.PixelSearch(recalc(948), recalc(969, false), recalc(968), recalc(979, false),
                                0xBC08F0, 5);

                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[]) d;
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);
                                _Shadowhunter = false;
                                lbStatus.Invoke(
                                    (MethodInvoker) (() => lbStatus.Text = "Activate: Shadowhunter Ultimate"));
                            }
                        }

                        if (chBoxPaladin.Checked == true && _Paladin == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object d = au3.PixelSearch(recalc(892), recalc(1027, false), recalc(934),
                                recalc(1060, false), 0x75D6FF, 10);
                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[]) d;
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);
                                _Paladin = false;

                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Paladin Ultimate"));
                            }
                        }

                        if (chBoxDeathblade.Checked == true && _Deathblade == true ||
                            chBoxDeathblade2.Checked == true && _Deathblade == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object d = au3.PixelSearch(recalc(986), recalc(1029, false), recalc(1017),
                                recalc(1035, false), 0xDAE7F3, 10);
                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[]) d;

                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);
                                _Deathblade = false;

                                if (chBoxDeathblade2.Checked == true)
                                {
                                    await Task.Delay(humanizer.Next(10, 240) + 500);
                                    KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                    KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                }

                                var Deathblade = Task.Run(() => DeathbladeSecondPress(token));
                                lbStatus.Invoke((MethodInvoker) (() =>
                                    lbStatus.Text = "Activate: Deathblade Ultimate"));
                            }
                        }

                        if (chBoxSharpshooter.Checked == true && _Sharpshooter == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object d = au3.PixelSearch(recalc(1006), recalc(1049, false), recalc(1019),
                                recalc(1068, false), 0x09B4EB, 10);
                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[]) d;
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);

                                _Sharpshooter = false;

                                var Sharpshooter = Task.Run(() => SharpshooterSecondPress(token));

                                lbStatus.Invoke(
                                    (MethodInvoker) (() => lbStatus.Text = "Activate: Sharpshooter Ultimate"));
                            }
                        }

                        if (chBoxSorcerer.Checked == true && _Sorcerer == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object d = au3.PixelSearch(recalc(1006), recalc(1038, false), recalc(1010),
                                recalc(1042, false), 0x8993FF, 10);
                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[]) d;
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);

                                _Sorcerer = false;

                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Sorcerer Ultimate"));
                            }
                        }

                        if (chBoxSoulfist.Checked == true && _Soulfist == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);

                            _Soulfist = false;

                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Soulfist Ultimate"));
                        }
                    }
                    catch (AggregateException)
                    {
                        Debug.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Debug.WriteLine("Bug");
                    }
                    catch
                    {
                    }
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task REVIVE(CancellationToken token)
        {
            try
            {
                if (chBoxRevive.Checked)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    while (_Revive == true && _FloorFight == true && _STOPP == false)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            float thresh = int.Parse(txtRevive.Text) * 0.01f;
                            var ReviveDeutschTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/revive1.png");
                            var ReviveDeutschMask =
                                new Image<Bgr, byte>(resourceFolder + "/revivemask1.png");

                            var ReviveEnglishTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/reviveEnglish.png");
                            var ReviveEnglishMask =
                                new Image<Bgr, byte>(resourceFolder + "/reviveEnglishmask.png");

                            Point screenResolution = new Point(screenWidth, screenHeight);
                            var ReviveDeutschDetector =
                                new EnterDetectors(ReviveDeutschTemplate, ReviveDeutschMask, thresh);
                            var ReviveEnglishDetector =
                                new EnterDetectors(ReviveEnglishTemplate, ReviveEnglishMask, thresh);
                            var screenPrinter = new PrintScreen();
                            var rawScreen = screenPrinter.CaptureScreen();
                            Bitmap bitmapImage = new Bitmap(rawScreen);
                            var screenCapture = bitmapImage.ToImage<Bgr, byte>();
                            var ReviveDeutsch = ReviveDeutschDetector.GetClosestEnter(screenCapture);
                            var ReviveEnglish = ReviveEnglishDetector.GetClosestEnter(screenCapture);
                            if (ReviveDeutsch.HasValue || ReviveEnglish.HasValue && _FloorFight == true)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                _FloorFight = false;
                                _Portaldetect = false;
                                _Potions = false;
                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "REVIVE!"));
                                VirtualMouse.MoveTo(recalc(1374), recalc(467, false), 10);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                _FloorFight = true;
                                _Portaldetect = true;
                                _Potions = false;
                            }
                            else
                            {
                            }

                            Random random = new Random();
                            var sleepTime = random.Next(450, 555);
                            Thread.Sleep(sleepTime);
                        }
                        catch (AggregateException)
                        {
                            Debug.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Debug.WriteLine("Bug");
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task POTIONS(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                while (_Potions == true && _FloorFight == true && _STOPP == false)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        object health10 = au3.PixelSearch(recalc(633), recalc(962, false), recalc(651),
                            recalc(969, false), 0x050405, 15);
                        if (health10.ToString() != "1" && checkBoxHeal10.Checked)
                        {
                            object[] health10Coord = (object[]) health10;
                            au3.Send("{" + txtHeal10.Text + "}");
                            au3.Send("{" + txtHeal10.Text + "}");
                            au3.Send("{" + txtHeal10.Text + "}");
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Heal-Potion at 10%"));
                        }
                        else

                            token.ThrowIfCancellationRequested();

                        await Task.Delay(1, token);
                        object health30 = au3.PixelSearch(recalc(633), recalc(962, false), recalc(686),
                            recalc(969, false), 0x050405, 15);
                        if (health30.ToString() != "1" && checkBoxHeal30.Checked)
                        {
                            object[] health30Coord = (object[]) health30;
                            au3.Send("{" + txtHeal30.Text + "}");
                            au3.Send("{" + txtHeal30.Text + "}");
                            au3.Send("{" + txtHeal30.Text + "}");
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Heal-Potion at 30%"));
                        }
                        else

                            token.ThrowIfCancellationRequested();

                        await Task.Delay(1, token);
                        object health70 = au3.PixelSearch(recalc(633), recalc(962, false), recalc(820),
                            recalc(969, false), 0x050405, 15);
                        if (health70.ToString() != "1" && checkBoxHeal70.Checked)
                        {
                            object[] health70Coord = (object[]) health70;
                            au3.Send("{" + txtHeal70.Text + "}");
                            au3.Send("{" + txtHeal70.Text + "}");
                            au3.Send("{" + txtHeal70.Text + "}");
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Heal-Potion at 70%"));
                        }
                    }
                    catch (AggregateException)
                    {
                        Debug.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Debug.WriteLine("Bug");
                    }
                    catch
                    {
                    }
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        ///    BOT COMES TO THE END    ///
        private async Task LEAVEDUNGEON(CancellationToken token)
        {
            try
            {
                _STOPP = true;
                PortalIsNotDetected = false;
                _FloorFight = false;
                _Searchboss = false;
                _Revive = false;
                _Ultimate = false;
                _Portaldetect = false;
                _Potions = false;
                _Floor1 = false;
                _Floor2 = false;
                _Floor3 = false;

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay(humanizer.Next(10, 240) + 500, token);
                /// KLICKT AUF LEAVE BUTTON
                VirtualMouse.MoveTo(recalc(158), recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(recalc(158), recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(recalc(158), recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                var t6 = Task.Run(() => LEAVEACCEPT(token));
                await Task.WhenAny(new[] {t6});
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task
            LEAVEDUNGEONCOMPLETE(
                CancellationToken token) // ITS A DIFFERENT CLICK LOCATION BECAUSE OF SMALLER LEAVE BUTTON FOR FLOOR 3
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                object walk = au3.PixelSearch(recalc(141), recalc(274, false), recalc(245), recalc(294, false),
                    0x29343F, 10);

                if (walk.ToString() != "1")
                {
                    object[] walkCoord = (object[]) walk;
                    VirtualMouse.MoveTo((int) walkCoord[0], (int) walkCoord[1], 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                }

                var t6 = Task.Run(() => LEAVEACCEPT(token));
                await Task.WhenAny(new[] {t6});
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task LEAVEACCEPT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                _STOPP = true;

                _FloorFight = false;
                _Searchboss = false;
                _Revive = false;
                _Ultimate = false;
                _Portaldetect = false;
                _Potions = false;
                _Floor1 = false;
                _Floor2 = false;
                _Floor3 = false;

                _Gunlancer = false;
                _Shadowhunter = false;
                _Berserker = false;
                _Paladin = false;
                _Deathblade = false;
                _Sharpshooter = false;
                _Bard = false;
                _Sorcerer = false;
                _Soulfist = false;

                /// KLICKT ENTER
                await Task.Delay(humanizer.Next(10, 240) + 1000);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

                await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                if (_REPAIR == true)
                {
                    await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                    var t7 = Task.Run(() => REPAIR(token));
                    await Task.WhenAny(new[] {t7});
                }
                else if (_LOGOUT == true)
                {
                    var t11 = Task.Run(() => LOGOUT(token));
                    await Task.WhenAny(new[] {t11});
                }
                else if (_REPAIR == false && _LOGOUT == false)
                {
                    _swap++;

                    await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    var t9 = Task.Run(() => RESTART(token));
                    await Task.WhenAny(new[] {t9});
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
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

                await Task.Delay(humanizer.Next(10, 240) + 20000, token);
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "LOGOUT Process starts..."));
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                VirtualMouse.MoveTo(recalc(1427), recalc(723, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                VirtualMouse.MoveTo(recalc(906), recalc(575, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);

                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "You are logged out!"));
                _start = false;
                cts.Cancel();
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task REPAIR(CancellationToken token)

        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Auto-Repair starts in 20 seconds..."));
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay(humanizer.Next(10, 240) + 25000, token);

                // KLICK UNTEN RECHTS (RATGEBER)
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                VirtualMouse.MoveTo(recalc(1741), recalc(1040, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                await Task.Delay(humanizer.Next(10, 240) + 2000, token);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // KLICK AUF BEGLEITER
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(recalc(1684), recalc(823, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // KLICK AUF AMBOSS
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(recalc(1256), recalc(693, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // KLICK AUF REPARIEREN
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(recalc(1085), recalc(429, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // 2x ESCAPE REPARATUR UND BEGLEITER FENSTER SCHLIEßEN
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);

                _REPAIR = false;
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Auto-Repair done!"));
            

                await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                var t10 = Task.Run(() => RESTART(token));
                await Task.WhenAny(new[] {t10});
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private async Task RESTART(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker) (() =>
                    lbStatus.Text = "Restart in " + int.Parse(txtRestart.Text) + " sekunden."));
                await Task.Delay(humanizer.Next(10, 240) + int.Parse(txtRestart.Text) * 1000);

                _STOPP = true;

                _FloorFight = false;
                _Searchboss = false;
                _Revive = false;
                _Ultimate = false;
                _Portaldetect = false;
                _Potions = false;
                _Floor1 = false;
                _Floor2 = false;
                _Floor3 = false;

                _Restart = true;
                if (chBoxChannelSwap.Checked == true)
                {
                    if (_swap == 3)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        VirtualMouse.MoveTo(recalc(1875), recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(humanizer.Next(10, 240) + 1000);
                        VirtualMouse.MoveTo(recalc(1875), recalc(123, false), 10);
                        KeyboardWrapper.HoldKey(KeyboardWrapper.VK_LBUTTON, 2000);
                        VirtualMouse.MoveTo(recalc(1845), recalc(124, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        _swap++;
                        _Restart = false;
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(humanizer.Next(10, 240) + 2000);
                        var t9 = Task.Run(() => RESTART(token));
                        await Task.WhenAny(new[] {t9});
                    }
                    else if (_swap == 6)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        VirtualMouse.MoveTo(recalc(1875), recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(humanizer.Next(10, 240) + 1000);
                        VirtualMouse.MoveTo(recalc(1875), recalc(123, false), 10);
                        KeyboardWrapper.HoldKey(KeyboardWrapper.VK_LBUTTON, 2000);
                        VirtualMouse.MoveTo(recalc(1845), recalc(103, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        _swap++;
                        _Restart = false;
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(humanizer.Next(10, 240) + 2000);
                        var t9 = Task.Run(() => RESTART(token));
                        await Task.WhenAny(new[] {t9});
                    }
                    else if (_swap == 9)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        VirtualMouse.MoveTo(recalc(1875), recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(humanizer.Next(10, 240) + 1000);
                        VirtualMouse.MoveTo(recalc(1875), recalc(123, false), 10);
                        KeyboardWrapper.HoldKey(KeyboardWrapper.VK_LBUTTON, 2000);
                        VirtualMouse.MoveTo(recalc(1845), recalc(84, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        _swap = 0;
                        _Restart = false;
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(humanizer.Next(10, 240) + 2000);
                        var t9 = Task.Run(() => RESTART(token));
                        await Task.WhenAny(new[] {t9});
                    }
                }

                if (_Restart == true)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        cts.Cancel();
                        _start = false;
                        _STOPP = false;
                        _stop = false;
                        _Restart = false;
                        _LOGOUT = false;
                  

                        _Gunlancer = false;
                        _Shadowhunter = false;
                        _Berserker = false;
                        _Paladin = false;
                        _Deathblade = false;
                        _Sharpshooter = false;
                        _Bard = false;
                        _Sorcerer = false;
                        _Soulfist = false;

                        _Floor1 = false;
                        _Floor2 = false;
                        _Floor3 = false;
                        _FloorFight = false;
                        _Searchboss = false;

                        _Revive = false;
                        _Portaldetect = false;
                        _Ultimate = false;
                        _Potions = false;

                        _Q = false;
                        _W = false;
                        _E = false;
                        _R = false;
                        _A = false;
                        _S = false;
                        _D = false;
                        _F = false;
                        await Task.Delay(humanizer.Next(10, 240) + 1000);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_F9);
                        _stop = true;
                    }
                    catch (AggregateException)
                    {
                        Debug.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Debug.WriteLine("Bug");
                    }

                    /*
                    await Task.Delay(1000, token);
                    var t1 = Task.Run(() => START(token));
                    await Task.WhenAny(new[] { t1 });*/
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

   


        private int casttimeByKey(byte key)
        {
            int cooldownDuration = 500;
            switch (key)
            {
                case KeyboardWrapper.VK_A:
                    cooldownDuration = int.Parse(txA.Text);
                    break;

                case KeyboardWrapper.VK_S:
                    cooldownDuration = int.Parse(txS.Text);
                    break;

                case KeyboardWrapper.VK_D:
                    cooldownDuration = int.Parse(txD.Text);
                    break;

                case KeyboardWrapper.VK_F:
                    cooldownDuration = int.Parse(txF.Text);
                    break;

                case KeyboardWrapper.VK_Q:
                    cooldownDuration = int.Parse(txQ.Text);
                    break;

                case KeyboardWrapper.VK_W:
                    cooldownDuration = int.Parse(txW.Text);
                    break;

                case KeyboardWrapper.VK_E:
                    cooldownDuration = int.Parse(txE.Text);
                    break;

                case KeyboardWrapper.VK_R:
                    cooldownDuration = int.Parse(txR.Text);
                    break;
            }

            return cooldownDuration;
        }

        public byte UltimateKey(string text)
        {
            byte foundkey = 0x0;
            switch (text)
            {
                case "Y":
                    foundkey = KeyboardWrapper.VK_Y;
                    break;

                case "Z":
                    foundkey = KeyboardWrapper.VK_Z;
                    break;
            }

            return foundkey;
        }

        private void setKeyCooldown(byte key)
        {
            switch (key)
            {
                case KeyboardWrapper.VK_A:
                    _A = true;
                    break;

                case KeyboardWrapper.VK_S:
                    _S = true;
                    break;

                case KeyboardWrapper.VK_D:
                    _D = true;
                    break;

                case KeyboardWrapper.VK_F:
                    _F = true;
                    break;

                case KeyboardWrapper.VK_Q:
                    _Q = true;
                    break;

                case KeyboardWrapper.VK_W:
                    _W = true;
                    break;

                case KeyboardWrapper.VK_E:
                    _E = true;
                    break;

                case KeyboardWrapper.VK_R:
                    _R = true;
                    break;
            }
        }

        private bool isKeyOnCooldown(byte key)
        {
            bool returnBoolean = false;
            switch (key)
            {
                case KeyboardWrapper.VK_A:
                    returnBoolean = _A;
                    break;

                case KeyboardWrapper.VK_S:
                    returnBoolean = _S;
                    break;

                case KeyboardWrapper.VK_D:
                    returnBoolean = _D;
                    break;

                case KeyboardWrapper.VK_F:
                    returnBoolean = _F;
                    break;

                case KeyboardWrapper.VK_Q:
                    returnBoolean = _Q;
                    break;

                case KeyboardWrapper.VK_W:
                    returnBoolean = _W;
                    break;

                case KeyboardWrapper.VK_E:
                    returnBoolean = _E;
                    break;

                case KeyboardWrapper.VK_R:
                    returnBoolean = _R;
                    break;
            }

            return returnBoolean;
        }

        private void lbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        public void ChaosBot_Load(object sender, EventArgs e)
        {
            List<Layout_Keyboard> LAYOUT = new List<Layout_Keyboard>();
            Layout_Keyboard QWERTZ = new Layout_Keyboard
            {
                LAYOUTS = "QWERTZ",
                Q = KeyboardWrapper.VK_Q,
                W = KeyboardWrapper.VK_W,
                E = KeyboardWrapper.VK_E,
                R = KeyboardWrapper.VK_R,
                A = KeyboardWrapper.VK_A,
                S = KeyboardWrapper.VK_S,
                D = KeyboardWrapper.VK_D,
                F = KeyboardWrapper.VK_F,
                Y = KeyboardWrapper.VK_Y,
            };
            LAYOUT.Add(QWERTZ);

            Layout_Keyboard QWERTY = new Layout_Keyboard
            {
                LAYOUTS = "QWERTY",
                Q = KeyboardWrapper.VK_Q,
                W = KeyboardWrapper.VK_W,
                E = KeyboardWrapper.VK_E,
                R = KeyboardWrapper.VK_R,
                A = KeyboardWrapper.VK_A,
                S = KeyboardWrapper.VK_S,
                D = KeyboardWrapper.VK_D,
                F = KeyboardWrapper.VK_F,
                Y = KeyboardWrapper.VK_Z,
            };
            LAYOUT.Add(QWERTY);

            Layout_Keyboard AZERTY = new Layout_Keyboard
            {
                LAYOUTS = "AZERTY",
                Q = KeyboardWrapper.VK_A,
                W = KeyboardWrapper.VK_Z,
                E = KeyboardWrapper.VK_E,
                R = KeyboardWrapper.VK_R,
                A = KeyboardWrapper.VK_Q,
                S = KeyboardWrapper.VK_S,
                D = KeyboardWrapper.VK_D,
                F = KeyboardWrapper.VK_F,
                Y = KeyboardWrapper.VK_Z,
            };
            LAYOUT.Add(AZERTY);

            comboBox1.DataSource = LAYOUT;
            comboBox1.DisplayMember = "LAYOUTS";
            currentLayout = comboBox1.SelectedItem as Layout_Keyboard;
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);

            txtPortalSearch.Text = Properties.Settings.Default.txtPortalSearch;

            txPQ.Text = Properties.Settings.Default.txPQ;
            txPW.Text = Properties.Settings.Default.txPW;
            txPE.Text = Properties.Settings.Default.txPE;
            txPR.Text = Properties.Settings.Default.txPR;
            txPA.Text = Properties.Settings.Default.txPA;
            txPS.Text = Properties.Settings.Default.txPS;
            txPD.Text = Properties.Settings.Default.txPD;
            txPF.Text = Properties.Settings.Default.txPF;

            txQ.Text = Properties.Settings.Default.q;
            txW.Text = Properties.Settings.Default.w;
            txE.Text = Properties.Settings.Default.e;
            txR.Text = Properties.Settings.Default.r;
            txA.Text = Properties.Settings.Default.a;
            txS.Text = Properties.Settings.Default.s;
            txD.Text = Properties.Settings.Default.d;
            txF.Text = Properties.Settings.Default.f;
            txCoolQ.Text = Properties.Settings.Default.cQ;
            txCoolW.Text = Properties.Settings.Default.cW;
            txCoolE.Text = Properties.Settings.Default.cE;
            txCoolR.Text = Properties.Settings.Default.cR;
            txCoolA.Text = Properties.Settings.Default.cA;
            txCoolS.Text = Properties.Settings.Default.cS;
            txCoolD.Text = Properties.Settings.Default.cD;
            txCoolF.Text = Properties.Settings.Default.cF;
            txtHeal30.Text = Properties.Settings.Default.instant;
            txtHeal70.Text = Properties.Settings.Default.potion;
            checkBoxHeal30.Checked = Properties.Settings.Default.chboxinstant;
            checkBoxHeal70.Checked = Properties.Settings.Default.chboxheal;
            chBoxAutoRepair.Checked = Properties.Settings.Default.chBoxAutoRepair;
            txtRepair.Text = Properties.Settings.Default.autorepair;
            chBoxY.Checked = Properties.Settings.Default.chBoxShadowhunter;
            chBoxPaladin.Checked = Properties.Settings.Default.chboxPaladin;
            chBoxGunlancer.Checked = Properties.Settings.Default.chBoxGunlancer;
            chBoxBerserker.Checked = Properties.Settings.Default.chBoxBerserker;
            chBoxChannelSwap.Checked = Properties.Settings.Default.chBoxChannelSwap;
            chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxSaveAll;
            chBoxActivateF2.Checked = Properties.Settings.Default.chBoxActivateF2;
            txtDungeon2search.Text = Properties.Settings.Default.txtDungeon2search;
            txtDungeon2.Text = Properties.Settings.Default.txtDungeon2;
            txtDungeon3search.Text = Properties.Settings.Default.txtDungeon3search;
            txtDungeon3.Text = Properties.Settings.Default.txtDungeon3;
            chBoxActivateF3.Checked = Properties.Settings.Default.chBoxActivateF3;
            txLeaveTimerFloor3.Text = Properties.Settings.Default.txLeaveTimerFloor3;
            txLeaveTimerFloor2.Text = Properties.Settings.Default.txLeaveTimerFloor2;
            comboBoxAutoAttack.SelectedIndex = int.Parse(Properties.Settings.Default.chBoxAutoAttack);
            chBoxAwakening.Checked = Properties.Settings.Default.chBoxAwakening;
            txtRevive.Text = Properties.Settings.Default.txtRevive;
            chBoxRevive.Checked = Properties.Settings.Default.chBoxRevive;
        }

        private void ChaosBot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void checkBoxInstant_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHeal30.Checked)
            {
                txtHeal30.ReadOnly = false;
            }
            else if (!checkBoxHeal30.Checked)
            {
                txtHeal30.ReadOnly = true;
                txtHeal30.Text = "";
            }
        }

        private void checkBoxHeal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHeal70.Checked)
            {
                txtHeal70.ReadOnly = false;
            }
            else if (!checkBoxHeal70.Checked)
            {
                txtHeal70.ReadOnly = true;
                txtHeal70.Text = "";
            }
        }

        private void checkIsDigit(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void checkIsLetter(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void chBoxAutoRepair_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAutoRepair.Checked)
            {
                txtRepair.ReadOnly = false;
            }
            else if (!chBoxAutoRepair.Checked)
            {
                txtRepair.ReadOnly = true;
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.chBoxGunlancer = false;
                Properties.Settings.Default.chBoxRevive = false;
                Properties.Settings.Default.txtRevive = "85";
                Properties.Settings.Default.txLeaveTimerFloor2 = "150";
                Properties.Settings.Default.txLeaveTimerFloor3 = "180";

                Properties.Settings.Default.txtPortalSearch = "20";
                Properties.Settings.Default.instant = "";
                Properties.Settings.Default.potion = "";
                Properties.Settings.Default.heal10 = "";
                Properties.Settings.Default.chboxinstant = false;
                Properties.Settings.Default.chboxheal = false;
                Properties.Settings.Default.chBoxAutoRepair = false;
                Properties.Settings.Default.chBoxLOGOUT = false;
                Properties.Settings.Default.txtLOGOUT = "";
                Properties.Settings.Default.autorepair = "10";
                Properties.Settings.Default.chBoxShadowhunter = false;
                Properties.Settings.Default.chBoxSoulfist = false;
                Properties.Settings.Default.chBoxBerserker = false;
                Properties.Settings.Default.chBoxBard = false;
                Properties.Settings.Default.chBoxGunlancer2 = false;
                Properties.Settings.Default.chboxPaladin = false;
                Properties.Settings.Default.chBoxChannelSwap = false;
                Properties.Settings.Default.chBoxSaveAll = false;
                Properties.Settings.Default.chBoxActivateF2 = false;
                Properties.Settings.Default.txtDungeon2 = "15";
                Properties.Settings.Default.txtDungeon2search = "5";
                Properties.Settings.Default.txtDungeon3 = "20";
                Properties.Settings.Default.txtDungeon3search = "10";
                Properties.Settings.Default.chBoxActivateF3 = false;
                Properties.Settings.Default.chBoxAutoMovement = false;
                Properties.Settings.Default.txtRestart = "15";

                Properties.Settings.Default.chBoxGunlancer = false;
                Properties.Settings.Default.chBoxSharpshooter = false;
                Properties.Settings.Default.chBoxSorcerer = false;
                Properties.Settings.Default.chBoxDeathblade = false;
                Properties.Settings.Default.chBoxDeathblade2 = false;

                Properties.Settings.Default.txPQ = "1";
                Properties.Settings.Default.txPW = "2";
                Properties.Settings.Default.txPE = "3";
                Properties.Settings.Default.txPR = "4";
                Properties.Settings.Default.txPA = "5";
                Properties.Settings.Default.txPS = "6";
                Properties.Settings.Default.txPD = "7";
                Properties.Settings.Default.txPF = "8";

                Properties.Settings.Default.cQ = "500";
                Properties.Settings.Default.cW = "500";
                Properties.Settings.Default.cE = "500";
                Properties.Settings.Default.cR = "500";
                Properties.Settings.Default.cA = "500";
                Properties.Settings.Default.cS = "500";
                Properties.Settings.Default.cD = "500";
                Properties.Settings.Default.cF = "500";
                Properties.Settings.Default.q = "500";
                Properties.Settings.Default.w = "500";
                Properties.Settings.Default.e = "500";
                Properties.Settings.Default.r = "500";
                Properties.Settings.Default.a = "500";
                Properties.Settings.Default.s = "500";
                Properties.Settings.Default.d = "500";
                Properties.Settings.Default.f = "500";

                Properties.Settings.Default.chBoxDoubleQ = false;
                Properties.Settings.Default.chBoxDoubleW = false;
                Properties.Settings.Default.chBoxDoubleE = false;
                Properties.Settings.Default.chBoxDoubleR = false;
                Properties.Settings.Default.chBoxDoubleA = false;
                Properties.Settings.Default.chBoxDoubleS = false;
                Properties.Settings.Default.chBoxDoubleD = false;
                Properties.Settings.Default.chBoxDoubleF = false;
                Properties.Settings.Default.chBoxAutoAttack = "0";
                Properties.Settings.Default.chBoxAwakening = false;

                Properties.Settings.Default.Save();
                chBoxGunlancer.Checked = Properties.Settings.Default.chBoxGunlancer;
                txtRestart.Text = Properties.Settings.Default.txtRestart;
                chBoxRevive.Checked = Properties.Settings.Default.chBoxRevive;
                txtRevive.Text = Properties.Settings.Default.txtRevive;
                chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxAutoMovement;
                txLeaveTimerFloor3.Text = Properties.Settings.Default.txLeaveTimerFloor3;
                txLeaveTimerFloor2.Text = Properties.Settings.Default.txLeaveTimerFloor2;
                txtPortalSearch.Text = Properties.Settings.Default.txtPortalSearch;
                txtHeal10.Text = Properties.Settings.Default.instant;
                chBoxLOGOUT.Checked = Properties.Settings.Default.chBoxLOGOUT;
                txtHeal30.Text = Properties.Settings.Default.instant;
                txtHeal70.Text = Properties.Settings.Default.potion;
                checkBoxHeal30.Checked = Properties.Settings.Default.chboxinstant;
                checkBoxHeal70.Checked = Properties.Settings.Default.chboxheal;
                checkBoxHeal10.Checked = Properties.Settings.Default.checkBoxHeal10;
                chBoxAutoRepair.Checked = Properties.Settings.Default.chBoxAutoRepair;
                txtRepair.Text = Properties.Settings.Default.autorepair;
                chBoxY.Checked = Properties.Settings.Default.chBoxShadowhunter;
                chBoxPaladin.Checked = Properties.Settings.Default.chboxPaladin;
                chBoxBerserker.Checked = Properties.Settings.Default.chBoxBerserker;
                chBoxDeathblade.Checked = Properties.Settings.Default.chBoxDeathblade;
                chBoxDeathblade2.Checked = Properties.Settings.Default.chBoxDeathblade2;
                chBoxSorcerer.Checked = Properties.Settings.Default.chBoxSorcerer;
                chBoxSharpshooter.Checked = Properties.Settings.Default.chBoxSharpshooter;
                chBoxSoulfist.Checked = Properties.Settings.Default.chBoxSoulfist;
                chBoxChannelSwap.Checked = Properties.Settings.Default.chBoxChannelSwap;
                chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxSaveAll;
                chBoxActivateF2.Checked = Properties.Settings.Default.chBoxActivateF2;
                txtDungeon2search.Text = Properties.Settings.Default.txtDungeon2search;
                txtDungeon2.Text = Properties.Settings.Default.txtDungeon2;
                txCoolQ.Text = Properties.Settings.Default.cQ;
                txCoolW.Text = Properties.Settings.Default.cW;
                txCoolE.Text = Properties.Settings.Default.cE;
                txCoolR.Text = Properties.Settings.Default.cR;
                txCoolA.Text = Properties.Settings.Default.cA;
                txCoolS.Text = Properties.Settings.Default.cS;
                txCoolD.Text = Properties.Settings.Default.cD;
                txCoolF.Text = Properties.Settings.Default.cF;
                txtLOGOUT.Text = Properties.Settings.Default.txtLOGOUT;
                txQ.Text = Properties.Settings.Default.q;
                txW.Text = Properties.Settings.Default.w;
                txE.Text = Properties.Settings.Default.e;
                txR.Text = Properties.Settings.Default.r;
                txA.Text = Properties.Settings.Default.a;
                txS.Text = Properties.Settings.Default.s;
                txD.Text = Properties.Settings.Default.d;
                txF.Text = Properties.Settings.Default.f;

                txPQ.Text = Properties.Settings.Default.txPQ;
                txPW.Text = Properties.Settings.Default.txPW;
                txPE.Text = Properties.Settings.Default.txPE;
                txPR.Text = Properties.Settings.Default.txPR;
                txPA.Text = Properties.Settings.Default.txPA;
                txPS.Text = Properties.Settings.Default.txPS;
                txPD.Text = Properties.Settings.Default.txPD;
                txPF.Text = Properties.Settings.Default.txPF;
                chBoxDoubleQ.Checked = Properties.Settings.Default.chBoxDoubleQ;
                chBoxDoubleW.Checked = Properties.Settings.Default.chBoxDoubleW;
                chBoxDoubleE.Checked = Properties.Settings.Default.chBoxDoubleE;
                chBoxDoubleR.Checked = Properties.Settings.Default.chBoxDoubleR;
                chBoxDoubleA.Checked = Properties.Settings.Default.chBoxDoubleA;
                chBoxDoubleS.Checked = Properties.Settings.Default.chBoxDoubleS;
                chBoxDoubleD.Checked = Properties.Settings.Default.chBoxDoubleD;
                chBoxDoubleF.Checked = Properties.Settings.Default.chBoxDoubleF;
                txtDungeon3search.Text = Properties.Settings.Default.txtDungeon3search;
                txtDungeon3.Text = Properties.Settings.Default.txtDungeon3;
                chBoxActivateF3.Checked = Properties.Settings.Default.chBoxActivateF3;
                chBoxBard.Checked = Properties.Settings.Default.chBoxBard;
                chBoxGunlancer2.Checked = Properties.Settings.Default.chBoxGunlancer2;
                comboBoxAutoAttack.SelectedIndex = int.Parse(Properties.Settings.Default.chBoxAutoAttack);
                
                chBoxAwakening.Checked = Properties.Settings.Default.chBoxAwakening;
            }
            catch
            {
            }
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            frmGuide Form = new frmGuide();
            Form.Show();
        }

        private string translateKey(int key)
        {
            string translate = "";
            switch (key)
            {
                case 81:
                    translate = "Q";
                    break;

                case 87:
                    translate = "W";
                    break;

                case 69:
                    translate = "E";
                    break;

                case 82:
                    translate = "R";
                    break;

                case 65:
                    translate = "A";
                    break;

                case 83:
                    translate = "S";
                    break;

                case 68:
                    translate = "D";
                    break;

                case 70:
                    translate = "F";
                    break;

                case 89:
                    translate = "Y";
                    break;

                case 90:
                    translate = "Z";
                    break;

                default:
                    translate = key.ToString();
                    break;
            }

            return translate;
        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Layout_Keyboard currentLayout = comboBox1.SelectedItem as Layout_Keyboard;
            lbPQ.Text = lb2Q.Text = lbQ.Text = translateKey(currentLayout.Q);
            lbPW.Text = lb2W.Text = lbW.Text = translateKey(currentLayout.W);
            lbPE.Text = lb2E.Text = lbE.Text = translateKey(currentLayout.E);
            lbPR.Text = lb2R.Text = lbR.Text = translateKey(currentLayout.R);
            lbPA.Text = lb2A.Text = lbA.Text = translateKey(currentLayout.A);
            lbPS.Text = lb2S.Text = lbS.Text = translateKey(currentLayout.S);
            lbPD.Text = lb2D.Text = lbD.Text = translateKey(currentLayout.D);
            lbPF.Text = lb2F.Text = lbF.Text = translateKey(currentLayout.F);
            txBoxUltimateKey.Text = translateKey(currentLayout.Y);
        }

        public async void DeathbladeSecondPress(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 12000, token);
                KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                await Task.Delay(humanizer.Next(10, 240) + 12000, token);
                KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                _Deathblade = true;
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        public async void AWAKENINGSKILL(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                await Task.Delay(humanizer.Next(10, 240) + (int.Parse(txLeaveTimerFloor2.Text) * 1000) - 7000, token);

                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_V, 1000);
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        public async void LEAVETIMERFLOOR1(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 20000, token);

                float threshold = 0.7f;

                var enemyTemplate =
                    new Image<Bgr, byte>(resourceFolder + "/enemy.png");
                var enemyMask =
                    new Image<Bgr, byte>(resourceFolder + "/mask.png");

                Point myPosition = new Point(recalc(148), recalc(127, false));
                Point screenResolution = new Point(screenWidth, screenHeight);
                var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);

                var screenPrinter = new PrintScreen();

                var rawScreen = screenPrinter.CaptureScreen();
                Bitmap bitmapImage = new Bitmap(rawScreen);
                var screenCapture = bitmapImage.ToImage<Bgr, byte>();

                var enemy = enemyDetector.GetClosestEnemy(screenCapture, true);

                if (!enemy.HasValue)
                {
                    _STOPP = true;
                    PortalIsNotDetected = false;
                    _FloorFight = false;
                    _Searchboss = false;
                    _Revive = false;
                    _Ultimate = false;
                    _Portaldetect = false;
                    _Potions = false;
                    _Floor1 = false;
                    _Floor2 = false;
                    _Floor3 = false;
                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Failed to Enter Portal!"));
                    var t12 = Task.Run(() => LEAVEDUNGEON(token));
                    await Task.WhenAny(new[] {t12});
                }
                else
                {
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        public async void LEAVETIMERFLOOR2(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1, token);

                await Task.Delay(humanizer.Next(10, 240) + int.Parse(txLeaveTimerFloor2.Text) * 1000, token);

                _STOPP = true;
                PortalIsNotDetected = false;
                _FloorFight = false;
                _Searchboss = false;
                _Revive = false;
                _Ultimate = false;
                _Portaldetect = false;
                _Potions = false;
                _Floor1 = false;
                _Floor2 = false;
                _Floor3 = false;

                var t12 = Task.Run(() => LEAVEDUNGEON(token));
                await Task.WhenAny(new[] {t12});
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        public async void LEAVETIMERFLOOR3(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + int.Parse(txLeaveTimerFloor3.Text) * 1000, token);
                _STOPP = true;
                PortalIsNotDetected = false;
                _FloorFight = false;
                _Searchboss = false;
                _Revive = false;
                _Ultimate = false;
                _Portaldetect = false;
                _Potions = false;
                _Floor1 = false;
                _Floor2 = false;
                _Floor3 = false;
                var t12 = Task.Run(() => LEAVEDUNGEONCOMPLETE(token));
                await Task.WhenAny(new[] {t12});
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        public async void SharpshooterSecondPress(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 3000, token);

                KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        public async void SkillCooldown(CancellationToken token, byte key)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                for (int i = 0; i <= 1; i++)
                {
                    token.ThrowIfCancellationRequested();
                    int cooldownDuration = 0;
                    await Task.Delay(1, token);
                    switch (key)
                    {
                        case KeyboardWrapper.VK_A:
                            cooldownDuration = int.Parse(txCoolA.Text);
                            break;

                        case KeyboardWrapper.VK_S:
                            cooldownDuration = int.Parse(txCoolS.Text);

                            break;

                        case KeyboardWrapper.VK_D:
                            cooldownDuration = int.Parse(txCoolD.Text);

                            break;

                        case KeyboardWrapper.VK_F:
                            cooldownDuration = int.Parse(txCoolF.Text);

                            break;

                        case KeyboardWrapper.VK_Q:
                            cooldownDuration = int.Parse(txCoolQ.Text);

                            break;

                        case KeyboardWrapper.VK_W:
                            cooldownDuration = int.Parse(txCoolW.Text);

                            break;

                        case KeyboardWrapper.VK_E:
                            cooldownDuration = int.Parse(txCoolE.Text);

                            break;

                        case KeyboardWrapper.VK_R:
                            cooldownDuration = int.Parse(txCoolR.Text);
                            break;
                    }

                    timer = new System.Timers.Timer(cooldownDuration);
                    switch (key)
                    {
                        case KeyboardWrapper.VK_A:
                            timer.Elapsed += (object source, ElapsedEventArgs e) => { _A = false; };;
                            break;

                        case KeyboardWrapper.VK_S:
                            timer.Elapsed += (object source, ElapsedEventArgs e) => { _S = false; };;

                            break;

                        case KeyboardWrapper.VK_D:
                            timer.Elapsed += (object source, ElapsedEventArgs e) => { _D = false; };;

                            break;

                        case KeyboardWrapper.VK_F:
                            timer.Elapsed += (object source, ElapsedEventArgs e) => { _F = false; };;

                            break;

                        case KeyboardWrapper.VK_Q:
                            timer.Elapsed += (object source, ElapsedEventArgs e) => { _Q = false; };;

                            break;

                        case KeyboardWrapper.VK_W:
                            timer.Elapsed += (object source, ElapsedEventArgs e) => { _W = false; };;

                            break;

                        case KeyboardWrapper.VK_E:
                            timer.Elapsed += (object source, ElapsedEventArgs e) => { _E = false; };;

                            break;

                        case KeyboardWrapper.VK_R:
                            timer.Elapsed += (object source, ElapsedEventArgs e) => { _R = false; };
                            break;
                    }

                    timer.AutoReset = false;
                    timer.Enabled = true;
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch
            {
            }
        }

        private void buttonSaveRotation_Click(object sender, EventArgs e)
        {
            if (comboBoxRotations.Text != "")
            {
                if (comboBoxRotations.Text != "main")
                {
                    rotation.chBoxGunlancer = (bool) chBoxGunlancer.Checked;
                    rotation.txtRevive = txtRevive.Text;
                    rotation.chBoxRevive = (bool) chBoxRevive.Checked;
                    rotation.txLeaveTimerFloor2 = txLeaveTimerFloor2.Text;
                    rotation.txLeaveTimerFloor3 = txLeaveTimerFloor3.Text;
                    rotation.txtRestart = txtRestart.Text;
                    rotation.txtPortalSearch = txtPortalSearch.Text;
                    rotation.instant = txtHeal30.Text;
                    rotation.potion = txtHeal70.Text;
                    rotation.txtHeal10 = txtHeal10.Text;
                    rotation.chboxinstant = (bool) checkBoxHeal30.Checked;
                    rotation.chboxheal = (bool) checkBoxHeal70.Checked;
                    rotation.chboxheal10 = (bool) checkBoxHeal10.Checked;
                    rotation.chBoxAutoRepair = (bool) chBoxAutoRepair.Checked;
                    rotation.autorepair = txtRepair.Text;

                    rotation.autologout = txtLOGOUT.Text;
                    rotation.chBoxautologout = chBoxLOGOUT.Checked;
                    rotation.chBoxAutoMovement = chBoxAutoMovement.Checked;
                    rotation.autorepair = txtRepair.Text;
                    rotation.chBoxShadowhunter = (bool) chBoxY.Checked;
                    rotation.chboxPaladin = (bool) chBoxPaladin.Checked;
                    rotation.chBoxBerserker = (bool) chBoxBerserker.Checked;
                    rotation.chBoxDeathblade = (bool) chBoxDeathblade.Checked;
                    rotation.chBoxDeathblade2 = (bool) chBoxDeathblade2.Checked;
                    rotation.chBoxSharpshooter = (bool) chBoxSharpshooter.Checked;
                    rotation.chBoxSoulfist = (bool) chBoxSoulfist.Checked;
                    rotation.chBoxSorcerer = (bool) chBoxSorcerer.Checked;
                    rotation.chBoxBard = (bool) chBoxBard.Checked;
                    rotation.chBoxGunlancer2 = (bool) chBoxGunlancer2.Checked;
                    rotation.chBoxChannelSwap = (bool) chBoxChannelSwap.Checked;
                    rotation.chBoxSaveAll = chBoxAutoMovement.Checked;
                    rotation.chBoxActivateF2 = chBoxActivateF2.Checked;
                    rotation.chBoxActivateF3 = chBoxActivateF3.Checked;
                    rotation.txtDungeon3search = txtDungeon3search.Text;
                    rotation.txtDungeon3 = txtDungeon3.Text;

                    rotation.txtDungeon2search = txtDungeon2search.Text;
                    rotation.txtDungeon2 = txtDungeon2.Text;
                    rotation.cQ = txCoolQ.Text;
                    rotation.cW = txCoolW.Text;
                    rotation.cE = txCoolE.Text;
                    rotation.cR = txCoolR.Text;
                    rotation.cA = txCoolA.Text;
                    rotation.cS = txCoolS.Text;
                    rotation.cD = txCoolD.Text;
                    rotation.cF = txCoolF.Text;
                    rotation.q = txQ.Text;
                    rotation.w = txW.Text;
                    rotation.e = txE.Text;
                    rotation.r = txR.Text;
                    rotation.a = txA.Text;
                    rotation.s = txS.Text;
                    rotation.d = txD.Text;
                    rotation.f = txF.Text;
                    rotation.txPQ = txPQ.Text;
                    rotation.txPW = txPW.Text;
                    rotation.txPE = txPE.Text;
                    rotation.txPR = txPR.Text;
                    rotation.txPA = txPA.Text;
                    rotation.txPS = txPS.Text;
                    rotation.txPD = txPD.Text;
                    rotation.txPF = txPF.Text;
                    rotation.chBoxDoubleQ = chBoxDoubleQ.Checked;
                    rotation.chBoxDoubleW = chBoxDoubleW.Checked;
                    rotation.chBoxDoubleE = chBoxDoubleE.Checked;
                    rotation.chBoxDoubleR = chBoxDoubleR.Checked;
                    rotation.chBoxDoubleA = chBoxDoubleA.Checked;
                    rotation.chBoxDoubleS = chBoxDoubleS.Checked;
                    rotation.chBoxDoubleD = chBoxDoubleD.Checked;
                    rotation.chBoxDoubleF = chBoxDoubleF.Checked;
                    rotation.comboBoxAutoAttack = comboBoxAutoAttack.SelectedIndex;
                    rotation.chBoxAwakening = chBoxAwakening.Checked;

                    rotation.Save(comboBoxRotations.Text);
                    MessageBox.Show("Rotation \"" + comboBoxRotations.Text + "\" saved");
                }
                else
                {
                    MessageBox.Show("Rotation can not be named \"main\"");
                }
            }
            else
            {
                MessageBox.Show("Please enter a name for your Rotation Config!");
            }
        }

        private void buttonLoadRotation_Click(object sender, EventArgs e)
        {
            rotation = Rotations.Load(comboBoxRotations.Text + ".ini");
            if (rotation != null)
            {
                txtRevive.Text = rotation.txtRevive;
                chBoxRevive.Checked = rotation.chBoxRevive;
                txtRestart.Text = rotation.txtRestart;
                chBoxGunlancer.Checked = rotation.chBoxGunlancer;
                txtPortalSearch.Text = rotation.txtPortalSearch;
                txtHeal30.Text = rotation.instant;
                txtHeal70.Text = rotation.potion;
                checkBoxHeal30.Checked = rotation.chboxinstant;
                checkBoxHeal70.Checked = rotation.chboxheal;
                checkBoxHeal10.Checked = rotation.chboxheal10;
                chBoxAutoRepair.Checked = rotation.chBoxAutoRepair;
                txtRepair.Text = rotation.autorepair;
                chBoxY.Checked = rotation.chBoxShadowhunter;
                chBoxPaladin.Checked = rotation.chboxPaladin;
                chBoxBerserker.Checked = rotation.chBoxBerserker;
                chBoxGunlancer2.Checked = rotation.chBoxGunlancer2;
                chBoxDeathblade.Checked = rotation.chBoxDeathblade;
                chBoxDeathblade2.Checked = rotation.chBoxDeathblade2;
                chBoxSharpshooter.Checked = rotation.chBoxSharpshooter;
                chBoxSoulfist.Checked = rotation.chBoxSoulfist;
                txtLOGOUT.Text = rotation.autologout;
                chBoxLOGOUT.Checked = rotation.chBoxautologout;
                txtHeal10.Text = rotation.txtHeal10;
                txLeaveTimerFloor2.Text = rotation.txLeaveTimerFloor2;
                txLeaveTimerFloor3.Text = rotation.txLeaveTimerFloor3;
                chBoxAutoMovement.Checked = rotation.chBoxAutoMovement;
                chBoxActivateF3.Checked = rotation.chBoxActivateF3;
                txtDungeon3search.Text = rotation.txtDungeon3search;
                txtDungeon3.Text = rotation.txtDungeon3;
                comboBoxAutoAttack.SelectedIndex = rotation.comboBoxAutoAttack;
                chBoxAwakening.Checked = rotation.chBoxAwakening;
                chBoxSorcerer.Checked = rotation.chBoxSorcerer;
                chBoxChannelSwap.Checked = rotation.chBoxChannelSwap;
                chBoxAutoMovement.Checked = rotation.chBoxSaveAll;
                chBoxActivateF2.Checked = rotation.chBoxActivateF2;
                txtDungeon2search.Text = rotation.txtDungeon2search;
                txtDungeon2.Text = rotation.txtDungeon2;
                txCoolQ.Text = rotation.cQ;
                txCoolW.Text = rotation.cW;
                txCoolE.Text = rotation.cE;
                txCoolR.Text = rotation.cR;
                txCoolA.Text = rotation.cA;
                txCoolS.Text = rotation.cS;
                txCoolD.Text = rotation.cD;
                txCoolF.Text = rotation.cF;
                txQ.Text = rotation.q;
                txW.Text = rotation.w;
                txE.Text = rotation.e;
                txR.Text = rotation.r;
                txA.Text = rotation.a;
                txS.Text = rotation.s;
                txD.Text = rotation.d;
                txF.Text = rotation.f;
                txPQ.Text = rotation.txPQ;
                txPW.Text = rotation.txPW;
                txPE.Text = rotation.txPE;
                txPR.Text = rotation.txPR;
                txPA.Text = rotation.txPA;
                txPS.Text = rotation.txPS;
                txPD.Text = rotation.txPD;
                txPF.Text = rotation.txPF;
                chBoxDoubleQ.Checked = rotation.chBoxDoubleQ;
                chBoxDoubleW.Checked = rotation.chBoxDoubleW;
                chBoxDoubleE.Checked = rotation.chBoxDoubleE;
                chBoxDoubleR.Checked = rotation.chBoxDoubleR;
                chBoxDoubleA.Checked = rotation.chBoxDoubleA;
                chBoxDoubleS.Checked = rotation.chBoxDoubleS;
                chBoxDoubleD.Checked = rotation.chBoxDoubleD;
                chBoxDoubleF.Checked = rotation.chBoxDoubleF;

                MessageBox.Show("Rotation \"" + comboBoxRotations.Text + "\" loaded");
            }
        }

        private void comboBoxRotations_MouseClick(object sender, MouseEventArgs e)
        {
            refreshRotationCombox();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void changeSkillSet(object sender, EventArgs e)
        {
            if (txPA.Text != "" && txPS.Text != "" && txPD.Text != "" && txPF.Text != "" && txPQ.Text != "" &&
                txPW.Text != "" && txPE.Text != "" && txPR.Text != "")
                SKILLS.skillset = new Dictionary<byte, int>()
                {
                    {KeyboardWrapper.VK_A, int.Parse(txPA.Text)},
                    {KeyboardWrapper.VK_S, int.Parse(txPS.Text)},
                    {KeyboardWrapper.VK_D, int.Parse(txPD.Text)},
                    {KeyboardWrapper.VK_F, int.Parse(txPF.Text)},
                    {KeyboardWrapper.VK_Q, int.Parse(txPQ.Text)},
                    {KeyboardWrapper.VK_W, int.Parse(txPW.Text)},
                    {KeyboardWrapper.VK_E, int.Parse(txPE.Text)},
                    {KeyboardWrapper.VK_R, int.Parse(txPR.Text)},
                }.ToList();
        }

        private void chBoxActivateF2_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxActivateF2.Checked)
            {
                txtDungeon2search.ReadOnly = false;
                txtDungeon2.ReadOnly = false;
                txLeaveTimerFloor2.ReadOnly = false;
            }
            else if (!chBoxActivateF2.Checked)
            {
                txtDungeon2search.ReadOnly = true;
                txtDungeon2.ReadOnly = true;
                txLeaveTimerFloor2.ReadOnly = true;
                chBoxActivateF3.Checked = false;
            }
        }

        private void chBoxActivateF3_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxActivateF3.Checked)
            {
                txtDungeon3search.ReadOnly = false;
                txtDungeon3.ReadOnly = false;
                txLeaveTimerFloor3.ReadOnly = false;
                chBoxActivateF2.Checked = true;
            }
            else if (!chBoxActivateF3.Checked)
            {
                txtDungeon3search.ReadOnly = true;
                txtDungeon3.ReadOnly = true;
                txLeaveTimerFloor3.ReadOnly = true;
            }
        }

        private void checkBoxHeal10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHeal10.Checked)
            {
                txtHeal10.ReadOnly = false;
            }
            else if (!checkBoxHeal10.Checked)
            {
                txtHeal10.ReadOnly = true;
            }
        }

        private void chBoxRevive_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxRevive.Checked)
            {
                txtRevive.ReadOnly = false;
            }
            else if (!chBoxRevive.Checked)
            {
                txtRevive.ReadOnly = true;
            }
        }

        private void lbStatus_TextChanged(object sender, EventArgs e)
        {
            formMinimized.labelMinimizedState.Text = lbStatus.Text;
        }

        private void textBoxTelegramAPI_TextChanged(object sender, EventArgs e)
        {
            conf.telegram = textBoxTelegramAPI.Text;
            conf.Save();
            
        }
        
        private void groupBox4_Enter(object sender, EventArgs e)
        {
        }

     

        private void labelSwap_Click_1(object sender, EventArgs e)
        {
            
            cts.Cancel();
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            if (Application.OpenForms.OfType<PixelAimbot.FishBot>().Count() == 1)
                Application.OpenForms.OfType<PixelAimbot.FishBot>().First().Close();

            FishBot Form = new FishBot();
            Form.Show();
            Application.OpenForms.OfType<PixelAimbot.ChaosBot>().First().Hide();
            Application.OpenForms.OfType<PixelAimbot.ChaosBot>().First().Close();
        }

        private void buttonTestTelegram_Click_1(object sender, EventArgs e)
        {
            
            var bot = new TelegramBotClient(textBoxTelegramAPI.Text);
            try
            {
                bot.TestApiAsync().Wait();
                telegramBotRunning = true;
                labelTelegramState.Text = "Status = Erfolgreich!";
                labelTelegramState.ForeColor = Color.Green;
                conf.telegram = textBoxTelegramAPI.Text;
                conf.Save();
                
            }
            catch (Exception ex)
            {
                labelTelegramState.Text = "Status = Fehler!";
                labelTelegramState.ForeColor = Color.Red;
            }
        }
        

        private void buttonConnectTelegram_Click(object sender, EventArgs e)
        {
            if (botIsRun)
            {

                botIsRun = false;
                labelTelegramState.Text = "Status = Getrennt";
                labelTelegramState.ForeColor = Color.White;
                buttonConnectTelegram.Text = "Verbinden";
            }
            else
            {
                TelegramTask = RunBotAsync(conf.telegram, telegramToken.Token);
                buttonConnectTelegram.Text = "Verbunden, jetzt Trennen?";
                buttonTestTelegram_Click_1(null, null);

            }
        }
    }
}