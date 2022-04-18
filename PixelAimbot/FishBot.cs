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
using Telegram.Bot;
using WindowsInput;
using WindowsInput.Native;

namespace PixelAimbot
{
    public partial class FishBot : Form
    {

        ///BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///
        ///                                                                                                                                               ///
        private bool _start = false;
        private bool _STOPP = false;
        private bool _stop = false;

        private bool _Floor1 = false;

        private bool _FloorFight = false;



        private bool _Potions = false;

        private bool _REPAIR = false;


        private bool _LOGOUT = false;


        //SKILL AND COOLDOWN//


        private System.Timers.Timer timer;


        public frmMinimized formMinimized = new frmMinimized();
        public static FishBot _FishBot;

        public Config conf = new Config();
        private bool telegramBotRunning = false;

        ///                                                                                                                                                 ///
        ///BOOLS ENDE////////////BOOLS ENDE////////////////BOOLS ENDE//////////////////BOOLS ENDE///////////////BOOLS ENDE/////////////////////BOOLS ENDE/////


        public string resourceFolder = "";

        private (int, int) PixelToAbsolute(double x, double y, Point screenResolution)
        {
            int newX = (int)(x / screenResolution.X * 65535);
            int newY = (int)(y / screenResolution.Y * 65535);
            return (newX, newY);
        }

        private static readonly Random random = new Random();
        public Rotations rotation = new Rotations();

        /////
        ///
        // 2. Import the RegisterHotKey Method
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

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

        public static InputSimulator inputSimulator = new InputSimulator();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public Layout_Keyboard currentLayout;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public static string ConfigPath { get; set; } = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();

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


            decimal normalized = (decimal)value * newResolution;
            decimal rescaledPosition = (decimal)normalized / oldResolution;

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
                catch { }
            }

            label15.Text = Config.version;
            int FirstHotkeyId = 1;
            int FirstHotKeyKey = (int)Keys.F9;
            // Register the "F9" hotkey
            Boolean F9Registered = RegisterHotKey(
                this.Handle, FirstHotkeyId, 0x0000, FirstHotKeyKey
            );

            // Repeat the same process but with F10
            int SecondHotkeyId = 2;
            int SecondHotKeyKey = (int)Keys.F10;
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
                cts.Cancel();
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
                            .AppendLine("Runtime: " + formMinimized.sw.Elapsed.Hours.ToString("D2") + ":" + formMinimized.sw.Elapsed.Minutes.ToString("D2") + ":" + formMinimized.sw.Elapsed.Seconds.ToString("D2"));

                        await bot.SendTextMessageAsync(chatId, sb.ToString());
                    }
                    if (text.Contains("/unstuck"))
                    {
                        if (_stop)
                        {
                            cts.Cancel();

                        }
                        else
                        {
                            await bot.SendTextMessageAsync(chatId, "Bot isnt running!");
                        }

                    }
                    if (text.Contains("/inv"))
                    {
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                        await Task.Delay(100);
                        var picture = new PrintScreen();
                        var screen = picture.CaptureScreen();

                        Stream stream = ToStream(cropImage(screen, new Rectangle(FishBot.recalc(1322), PixelAimbot.FishBot.recalc(189, false), FishBot.recalc(544), FishBot.recalc(640, false))), ImageFormat.Png);
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

        private CancellationTokenSource cts = new CancellationTokenSource();

        private void btnPause_Click(object sender, EventArgs e)

        {
            if (_stop == true)
            {
                cts.Cancel();
                _start = false;
                _stop = false;
                _REPAIR = false;


                _LOGOUT = false;






                this.Show();
                formMinimized.Hide();
                formMinimized.sw.Reset();
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "STOPPED!"));
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {

            if (_start == false)
            {
                try
                {

                    formMinimized.StartPosition = FormStartPosition.Manual;
                    formMinimized.Location = new Point(0, recalc(28, false));
                    formMinimized.Size = new Size(recalc(594), recalc(28, false));
                    formMinimized.labelMinimizedState.Location = new Point(recalc(203), recalc(9, false));
                    formMinimized.labelRuntimer.Location = new Point(recalc(464), recalc(9, false));
                    formMinimized.labelTitle.Location = new Point(recalc(12), recalc(7, false));
                    formMinimized.timerRuntimer.Enabled = true;
                    formMinimized.sw.Reset();
                    formMinimized.sw.Start();
                    formMinimized.Show();
                    this.Hide();

                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is starting..."));
                    _start = true;
                    _stop = true;
                    cts = new CancellationTokenSource();
                    var token = cts.Token;

                    var t1 = Task.Run(() => START(token));

                    if (chBoxAutoRepair.Checked == true && _start == true)
                    {
                        var repair = Task.Run(() => REPAIRTIMER(token));


                    }
                    else
                    {
                        _REPAIR = false;
                    }
                    if (chBoxLOGOUT.Checked == true && _start == true)
                    {
                        var logout = Task.Run(() => LOGOUTTIMER(token));

                    }
                    else
                    {
                        _LOGOUT = false;
                    }
                    await Task.WhenAny(new[] { t1 });
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

        public async void REPAIRTIMER(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay((int.Parse(txtRepair.Text) * 1000) * 60, token);
                _REPAIR = true;
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
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
            catch { }
            // timer.Elapsed += OnTimedEvent2;
            //timer.AutoReset = false;
            //timer.Enabled = true;
            //cts.Cancel();

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


                var t3 = Task.Run(() => CheckGathering(token));
                await Task.WhenAny(new[] { t3 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }





        ///    FIGHT SEQUENCES    ///
        private async Task CheckGathering(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                var template =
                new Image<Bgr, byte>(resourceFolder + "/gathering.png");
                var mask =
                new Image<Bgr, byte>(resourceFolder + "/gathering.png");

                
                var Detector = new ScreenDetector(template, mask, 0.7f, ChaosBot.recalc(526), ChaosBot.recalc(953), ChaosBot.recalc(100), ChaosBot.recalc(107));
                var screenPrinter = new PrintScreen();
                var rawScreen = screenPrinter.CaptureScreen();
                Bitmap bitmapImage = new Bitmap(rawScreen);
                var screenCapture = bitmapImage.ToImage<Bgr, byte>();

                var item = Detector.GetBest(screenCapture, true);
                if(item.HasValue)
                {
                    // Found
                } else
                {
                    // Not Found
                }

            }
            catch { }
        }
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

                    _FloorFight = true;
                    // Fishbot start and cooldown checker for fishing blabla
                    //   var t12 = Task.Run(() => FLOORFIGHT(token));
                    //  var t14 = Task.Run(() => ULTIMATE(token));
                    //  var t16 = Task.Run(() => REVIVE(token));
                    //  var t18 = Task.Run(() => PORTALDETECT(token));
                    //  var t20 = Task.Run(() => POTIONS(token));
                    //  await Task.WhenAny(new[] { t12, t14, t16, t18, t20 });


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
            catch { }

        }




        ///    RUN AT SAME TIME    ///

        private async Task POTIONS(CancellationToken token)
        {

            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                while (_Potions == true && _FloorFight == true)
                {
                    try
                    {

                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        object health10 = au3.PixelSearch(recalc(633), recalc(962, false), recalc(651), recalc(969, false), 0x050405, 15);
                        object health30 = au3.PixelSearch(recalc(633), recalc(962, false), recalc(686), recalc(969, false), 0x050405, 15);
                        object health70 = au3.PixelSearch(recalc(633), recalc(962, false), recalc(820), recalc(970, false), 0x050405, 15);
                    }
                    catch (AggregateException)
                    {
                        Console.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Console.WriteLine("Bug");
                    }
                    catch { }
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
            catch { }
        }



        ///    BOT COMES TO THE END    ///


        private async Task LOGOUT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                for (int i = 0; i < 1; i++)
                {

                    await Task.Delay(20000, token);

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "LOGOUT Process starts..."));
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                    await Task.Delay(2000, token);
                    au3.MouseMove(recalc(1238), recalc(728, false), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    await Task.Delay(2000, token);
                    au3.MouseMove(recalc(906), recalc(575, false), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    await Task.Delay(1000, token);

                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "You are logged out!"));
                    _start = false;
                    cts.Cancel();



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
            catch { }
        }

        private async Task REPAIR(CancellationToken token)

        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);


                for (int i = 0; i < 1; i++)
                {

                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Auto-Repair starts in 20 seconds..."));
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    await Task.Delay(25000, token);



                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    au3.MouseMove(recalc(1741), recalc(1040, false), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                    await Task.Delay(2000, token);

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    await Task.Delay(1500, token);
                    au3.MouseMove(recalc(1684), recalc(823, false), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);




                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    await Task.Delay(1500, token);
                    au3.MouseMove(recalc(1256), recalc(693, false), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);




                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    await Task.Delay(1500, token);
                    au3.MouseMove(recalc(1085), recalc(429, false), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    await Task.Delay(1500, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                    await Task.Delay(1000, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);

                    _REPAIR = false;
                    _REPAIR = false;
                    var repair = Task.Run(() => REPAIRTIMER(token));

                }

                await Task.Delay(2000, token);
                //   var t10 = Task.Run(() => RESTART_AFTERREPAIR(token));
                //           await Task.WhenAny(new[] { t10 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }


        private void lbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        public void ChaosBot_Load(object sender, EventArgs e)
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
            txtRepair.Text = Properties.Settings.Default.autorepair;

            chBoxChannelSwap.Checked = Properties.Settings.Default.chBoxChannelSwap;
            chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxSaveAll;




        }

        private void ChaosBot_MouseDown(object sender, MouseEventArgs e)
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
            if (chBoxAutoRepair.Checked)
            {
                txtRepair.ReadOnly = false;
            }
            else
                if (!chBoxAutoRepair.Checked)
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
            else
               if (!chBoxLOGOUT.Checked)
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

        }
    }
}