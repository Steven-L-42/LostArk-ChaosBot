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
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace PixelAimbot
{
    public partial class ChaosBot : Form
    {
       
        ///BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///
        ///                                                                                                                                               ///
        private bool _start = false;

        private bool _stop = false;

        private bool _Floor1 = false;
        private bool _Floor2 = false;
        private bool _Floor3 = false;
        private bool _Floor1Fight = false;
        private bool _Floor2Fight = false;
        private bool _Floor3Fight = false;

        private bool _REPAIR = false;
        private bool _Shadowhunter = false;
        private bool _Berserker = false;
        private bool _Paladin = false;
        private bool _Deathblade = false;
        private bool _Sharpshooter = false;
        private bool _Bard = false;
        private bool _Sorcerer = false;
        private bool _Soulfist = false;
        private bool _SkillFight2 = false;
        private bool _MakeMove = false;

        private bool _LOGOUT = false;
        private bool Search = false;

        //SKILL AND COOLDOWN//
        private bool _Q = true;

        private bool _W = true;
        private bool _E = true;
        private bool _R = true;
        private bool _A = true;
        private bool _S = true;
        private bool _D = true;
        private bool _F = true;

        private System.Timers.Timer timer;
        private int fightSequence = 0;
        private int fightSequence2 = 0;
        private int searchSequence = 0;
        private int searchSequence2 = 0;
        private int CompleteIteration = 1;
        private int fightOnSecondAbility = 1;
        private int walktopUTurn = 1;
        private int walktopUTurn2 = 1;
        private int Floor2 = 1;
        private int Floor3 = 1;

        ///                                                                                                                                                 ///
        ///BOOLS ENDE////////////BOOLS ENDE////////////////BOOLS ENDE//////////////////BOOLS ENDE///////////////BOOLS ENDE/////////////////////BOOLS ENDE/////

        /// OPENCV START  /// OPENCV START  /// OPENCV START  /// OPENCV START

        public string resourceFolder = "";
        Priorized_Skills SKILLS = new Priorized_Skills();

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

        public ChaosBot()
        {
            InitializeComponent();

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

        private void btnPause_Click(object sender, EventArgs e)

        {
            if (_stop == true)
            {
                cts.Cancel();
                _start = false;
                _stop = false;
                _REPAIR = false;
                _Shadowhunter = false;
                _Berserker = false;
                _Paladin = false;
                _Bard = false;

                _Deathblade = false;
                _Sharpshooter = false;
                _Sorcerer = false;
                _Soulfist = false;


                _LOGOUT = false;



                _Q = true;
                _W = true;
                _E = true;
                _R = true;
                _A = true;
                _S = true;
                _D = true;
                _F = true;






                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "STOPPED!"));
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {

            if (_start == false)
                try
                {
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
            return cooldownDuration / 10;
        }

        public async void LOGOUTTIMER(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay((int.Parse(txtLOGOUT.Text) * 1000) * 60, token);
                _LOGOUT = true;
                cts.Cancel();
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

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _REPAIR = true;
        }

        private void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            _LOGOUT = true;
        }

     

        private async Task START(CancellationToken token)
        {
            try
            {
                _Berserker = true;
                CompleteIteration = 1;
                Floor2 = 1;
                Floor3 = 1;
                searchSequence = 0;
                searchSequence2 = 0;
                fightSequence = 0;
                fightSequence2 = 0;
                walktopUTurn = 1;

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
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

                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);



                            await Task.Delay(1000, token);

                        }
                        catch (AggregateException)
                        {
                            MessageBox.Show("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            MessageBox.Show("Bug");
                        }
                        catch { }
                        ////////////////////////////////HIER FOLGT ENTER 2
                        try
                        {
                            au3.MouseMove(recalc(1467), recalc(858, false), 10);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                          

                            await Task.Delay(1000, token);
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
                        /////////////// ACCEPT
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object walk = au3.PixelSearch(recalc(560), recalc(260, false), recalc(1382), recalc(817, false), 0x21BD08, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseMove(recalc(903), recalc(605, false), 5);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
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
                await Task.Delay(7000, token);

                var t3 = Task.Run(() => MOVE(token));
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

        private async Task MOVE(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Set Transparency and Scale..."));
                    au3.MouseMove(recalc(1900), recalc(50, false), 1);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    au3.MouseMove(recalc(1871), recalc(260, false), 1);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    au3.MouseMove(recalc(1902), recalc(87, false), 1);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    au3.MouseMove(recalc(1871), recalc(260, false), 1);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    object minimizeChat = au3.PixelSearch(recalc(1896), recalc(385, false), recalc(1909), recalc(392, false), 0xFFF1C6, 100);
                    if (minimizeChat.ToString() == "0")
                    {
                        au3.MouseMove(recalc(1901), recalc(389, false), 1); 
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot moves to start the Dungeon..."));
                            au3.MouseMove(recalc(960), recalc(529, false), 1);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                            await Task.Delay(1000, token);

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
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            au3.MouseMove(recalc(960), recalc(529, false), 2);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                            

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
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            if (chBoxBerserker.Checked == true && _Berserker == true)
                            {
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_Y);
                                _Berserker = false;
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
                _Floor1 = true;
                var t12 = Task.Run(() => FLOORTIME(token));
                await Task.WhenAny(new[] { t12 });
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

        public (int, int) searchImageAndClick(string templateImage, string templateMask, string foundText, float threshold = 0.7f, double softMultiplier = 1, double hardMultiplier = 1.2)
        {
            // Tunable variables
            var enemyTemplate =
                new Image<Bgr, byte>(resourceFolder + templateImage); // icon of the enemy
            var enemyMask =
                new Image<Bgr, byte>(resourceFolder + templateMask); // make white what the important parts are, other parts should be black
                                                                     //var screenCapture = new Image<Bgr, byte>("D:/Projects/bot-enemy-detection/EnemyDetection/screen.png");
            Point myPosition = new Point(recalc(148), recalc(127, false));
            Point screenResolution = new Point(screenWidth, screenHeight);

            // Main program loop
            var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
            var screenPrinter = new PrintScreen();


            screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
            var screenCapture = new Image<Bgr, byte>("screen.png");
            var enemy = enemyDetector.GetClosestEnemy(screenCapture);
            if (enemy.HasValue)
            {
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = foundText));
                CvInvoke.Rectangle(screenCapture,
                    new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                    new MCvScalar(255));

                double distance_x = (screenWidth - recalc(296)) / 2;
                double distance_y = (screenHeight - recalc(255, false)) / 2;

                var friend_position = ((enemy.Value.X + distance_x), (enemy.Value.Y + distance_y));
                double multiplier = softMultiplier;
                var friend_position_on_minimap = ((enemy.Value.X), (enemy.Value.Y));
                var my_position_on_minimap = ((recalc(296) / 2), (recalc(255, false) / 2));
                var dist = Math.Sqrt(Math.Pow((my_position_on_minimap.Item1 - friend_position_on_minimap.Item1), 2) + Math.Pow((my_position_on_minimap.Item2 - friend_position_on_minimap.Item2), 2));

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

            return (0, 0);


        }
        private async Task SEARCHPORTAL(CancellationToken token)
        {
            try
            {

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);


                    _Shadowhunter = true;
                    _Paladin = true;
                    _Berserker = true;
                    for (int i = 0; i <= int.Parse(txtPortalSearch.Text) * 1.1; i++)
                    {
                        try
                        {

                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Search Portal..."));

                            var absolutePositions = searchImageAndClick("/portalenter1.png", "/portalentermask1.png", "Floor 1: Portal found...");

                            inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);

                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 1: Enter Portal..."));

                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                            if (txtLEFT.Text == "LEFT")
                            {
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                            }
                            else
                            {
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RBUTTON);
                            }
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

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


                        Random random = new Random();
                        var sleepTime = random.Next(50, 100);
                        await Task.Delay(sleepTime, token);
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
                searchSequence = 1;
                walktopUTurn = 0;
                _Floor2 = true;
                var t12 = Task.Run(() => SEARCHBOSS(token));
                await Task.WhenAny(new[] { t12 });
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

        private async Task SEARCHBOSS(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: search enemy..."));
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    _Shadowhunter = true;
                    _Paladin = true;
                    _Berserker = true;
                    if (searchSequence == 1)
                    {
                        au3.MouseMove(recalc(960), recalc(529, false), 1);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        
                        searchSequence++;
                    }

                    for (int i = 0; i < int.Parse(txtDungeon2search.Text) * 2.1; i++)
                    {

                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            float shardthreshold = 1f;
                            float threshold = 0.69f;
                            var shardTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/shard.png");
                            var shardMask =
                            new Image<Bgr, byte>(resourceFolder + "/shardmask.png");
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
                            var shardDetector = new EnemyDetector(shardTemplate, shardMask, shardthreshold);
                            var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                            var BossDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                            var mobDetector = new EnemyDetector(mobTemplate, mobMask, threshold);
                            var portalDetector = new EnemyDetector(portalTemplate, portalMask, threshold);
                            var screenPrinter = new PrintScreen();

                            screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                            var screenCapture = new Image<Bgr, byte>("screen.png");
                            var shard = shardDetector.GetClosestEnemy(screenCapture);
                            var enemy = enemyDetector.GetClosestEnemy(screenCapture);
                            var Boss = BossDetector.GetClosestEnemy(screenCapture);
                            var mob = mobDetector.GetClosestEnemy(screenCapture);
                            var portal = portalDetector.GetClosestEnemy(screenCapture);

                            if (shard.HasValue)
                            {
                                CvInvoke.Rectangle(screenCapture,
                                    new Rectangle(new Point(shard.Value.X, shard.Value.Y), shardTemplate.Size),
                                    new MCvScalar(255));

                                double distance_x = (screenWidth - recalc(296)) / 2;
                                double distance_y = (screenHeight - recalc(255, false)) / 2;

                                var shard_position = ((shard.Value.X + distance_x), (shard.Value.Y + distance_y));
                                double multiplier = 1;
                                var shard_position_on_minimap = ((shard.Value.X), (shard.Value.Y));
                                var my_position_on_minimap = ((recalc(296) / 2), (recalc(255, false) / 2));
                                var dist = Math.Sqrt(Math.Pow((my_position_on_minimap.Item1 - shard_position_on_minimap.Item1), 2) + Math.Pow((my_position_on_minimap.Item2 - shard_position_on_minimap.Item2), 2));

                                if (dist < 180)
                                {
                                    multiplier = 1.2;
                                }

                                double posx;
                                double posy;
                                if (shard_position.Item1 < (screenWidth / 2))
                                {
                                    posx = shard_position.Item1 * (2 - multiplier);
                                }
                                else
                                {
                                    posx = shard_position.Item1 * multiplier;
                                }
                                if (shard_position.Item2 < (screenHeight / 2))
                                {
                                    posy = shard_position.Item2 * (2 - multiplier);
                                }
                                else
                                {
                                    posy = shard_position.Item2 * multiplier;
                                }


                                var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: Shard found!"));
                                inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                if (txtLEFT.Text == "LEFT")
                                {
                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                }
                                else
                                {
                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_RBUTTON);
                                }
                            }
                            else
                            {
                                if (Boss.HasValue)
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
                                    var dist = Math.Sqrt(Math.Pow((my_position_on_minimap.Item1 - boss_position_on_minimap.Item1), 2) + Math.Pow((my_position_on_minimap.Item2 - boss_position_on_minimap.Item2), 2));

                                    if (dist < 180)
                                    {
                                        multiplier = 1.2;
                                    }

                                    double posx;
                                    double posy;
                                    if (boss_position.Item1 < (screenWidth / 2))
                                    {
                                        posx = boss_position.Item1 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posx = boss_position.Item1 * multiplier;
                                    }
                                    if (boss_position.Item2 < (screenHeight / 2))
                                    {
                                        posy = boss_position.Item2 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posy = boss_position.Item2 * multiplier;
                                    }


                                    var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Big-Boss found!"));
                                    inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                    if (txtLEFT.Text == "LEFT")
                                    {
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                    }
                                    else
                                    {
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_RBUTTON);
                                    }
                                }
                                else
                                {
                                    if (enemy.HasValue)
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
                                        var dist = Math.Sqrt(Math.Pow((my_position_on_minimap.Item1 - enemy_position_on_minimap.Item1), 2) + Math.Pow((my_position_on_minimap.Item2 - enemy_position_on_minimap.Item2), 2));

                                        if (dist < 180)
                                        {
                                            multiplier = 1.2;
                                        }

                                        double posx;
                                        double posy;
                                        if (enemy_position.Item1 < (screenWidth / 2))
                                        {
                                            posx = enemy_position.Item1 * (2 - multiplier);
                                        }
                                        else
                                        {
                                            posx = enemy_position.Item1 * multiplier;
                                        }
                                        if (enemy_position.Item2 < (screenHeight / 2))
                                        {
                                            posy = enemy_position.Item2 * (2 - multiplier);
                                        }
                                        else
                                        {
                                            posy = enemy_position.Item2 * multiplier;
                                        }


                                        var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Mid-Boss found!"));
                                        inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                        if (txtLEFT.Text == "LEFT")
                                        {
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                        }
                                        else
                                        {
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_RBUTTON);
                                        }
                                    }
                                    else
                                    {
                                        if (mob.HasValue)
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
                                            var dist = Math.Sqrt(Math.Pow((my_position_on_minimap.Item1 - mob_position_on_minimap.Item1), 2) + Math.Pow((my_position_on_minimap.Item2 - mob_position_on_minimap.Item2), 2));

                                            if (dist < 180)
                                            {
                                                multiplier = 1.2;
                                            }

                                            double posx;
                                            double posy;
                                            if (mob_position.Item1 < (screenWidth / 2))
                                            {
                                                posx = mob_position.Item1 * (2 - multiplier);
                                            }
                                            else
                                            {
                                                posx = mob_position.Item1 * multiplier;
                                            }
                                            if (mob_position.Item2 < (screenHeight / 2))
                                            {
                                                posy = mob_position.Item2 * (2 - multiplier);
                                            }
                                            else
                                            {
                                                posy = mob_position.Item2 * multiplier;
                                            }


                                            var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Mob found!"));

                                            inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                            if (txtLEFT.Text == "LEFT")
                                            {
                                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                            }
                                            else
                                            {
                                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RBUTTON);
                                            }
                                        }
                                    }
                                }
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

                    if (Floor2 == 1)
                    { _Floor2 = true; }

                    var t12 = Task.Run(() => FLOORTIME(token));
                    await Task.WhenAny(new[] { t12 });
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

        private async Task FLOORTIME(CancellationToken token)
        {

            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                if (_Floor1 == true)
                {

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        walktopUTurn = 0;

                        Search = false;
                        _Shadowhunter = true;
                        _Berserker = true;
                        _Paladin = true;
                        _Deathblade = true;
                        _Sharpshooter = true;
                        _Sorcerer = true;
                        _Soulfist = true;

                        _Floor1Fight = true;
                        var t12 = Task.Run(() => FLOORFIGHT(token));
                        await Task.WhenAny(new[] { t12 });
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
                if (_Floor2 == true)
                {

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        _SkillFight2 = true;
                        fightOnSecondAbility = 1;
                        walktopUTurn2 = 0;
                        fightSequence++;

                        Search = false;
                        _Shadowhunter = true;
                        _Berserker = true;
                        _Paladin = true;
                        _Deathblade = true;
                        _Sharpshooter = true;
                        _Bard = true;
                        _Sorcerer = true;
                        _Soulfist = true;

                        _Floor2Fight = true;
                        var t14 = Task.Run(() => FLOORFIGHT(token));
                        await Task.Delay(int.Parse(txtDungeon2.Text) * 1000);

                        _Floor2Fight = false;

                        if (fightSequence == int.Parse(txtDungeon2Iteration.Text) && chBoxActivateF2.Checked == true && chBoxActivateF3.Checked == false)
                        {
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "ChaosDungeon Floor 2 Complete!"));

                            Search = true;
                            var t12 = Task.Run(() => LEAVEDUNGEON(token));
                            await Task.WhenAny(new[] { t12 });
                        }
                        else

                        if (fightSequence >= int.Parse(txtDungeon2Iteration.Text) - 1 && chBoxActivateF3.Checked == true)
                        {

                            Search = true;
                            var t13 = Task.Run(() => FLOOR2PORTAL(token));
                            await Task.WhenAny(new[] { t13 });
                        }
                        else
                        if (fightSequence < int.Parse(txtDungeon2Iteration.Text))
                        {

                            Search = true;
                            var t13 = Task.Run(() => SEARCHBOSS(token));
                            await Task.WhenAny(new[] { t13 });
                        }
                        await Task.WhenAny(new[] { t14 });
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
                if (_Floor3 == true)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        CompleteIteration = 1;
                        fightSequence2++;
                        _Shadowhunter = true;
                        _Berserker = true;
                        _Paladin = true;
                        _Deathblade = true;
                        _Sharpshooter = true;
                        _Bard = true;
                        _Sorcerer = true;
                        _Soulfist = true;

                        _Floor3Fight = true;
                        var t14 = Task.Run(() => FLOORFIGHT(token));
                        await Task.Delay(int.Parse(txtDungeon3.Text) * 1000, token);

                        _Floor3Fight = false;

                        if (fightSequence2 == int.Parse(txtDungeon3Iteration.Text))
                        {

                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Leaved ChaosDungeon - not completed!"));
                            var t12 = Task.Run(() => LEAVEDUNGEON(token));
                            await Task.WhenAny(new[] { t12 });
                        }
                        else
                        if (fightSequence2 < int.Parse(txtDungeon3Iteration.Text))
                        {
                            searchSequence2 = 1;
                            var t13 = Task.Run(() => SEARCHBOSS2(token));
                            await Task.WhenAny(new[] { t13 });
                        }
                        await Task.WhenAny(new[] { t14 });

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

        private async Task FLOORFIGHT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                try
                {
                    while (_Floor1Fight == true && Search == false)
                    {

                        foreach (KeyValuePair<byte, int> skill in SKILLS.skillset.OrderBy(x => x.Value))
                        {
                            try
                            {

                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                object fight = au3.PixelSearch(recalc(600), recalc(250, false), recalc(1319), recalc(843, false), 0xDD2C02, 10);
                                if (fight.ToString() != "1" && Search == false)
                                {
                                    object[] fightCoord = (object[])fight;
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 100);
                                    KeyboardWrapper.AlternateHoldKey(skill.Key, casttimeByKey(skill.Key));

                                    if (chBoxDoubleQ.Checked || chBoxDoubleW.Checked || chBoxDoubleE.Checked || chBoxDoubleR.Checked || chBoxDoubleA.Checked || chBoxDoubleS.Checked || chBoxDoubleD.Checked || chBoxDoubleF.Checked)
                                    {
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Key Pressed twice!"));
                                        KeyboardWrapper.PressKey(skill.Key);
                                    }
                                    setKeyCooldown(skill.Key); // Set Cooldown
                                    var td = Task.Run(() => SkillCooldown(token, skill.Key));
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 100);
                                    fightOnSecondAbility++;
                                    if (isKeyOnCooldown(skill.Key) == false && Search == false)
                                    {
                                        try
                                        {
                                            token.ThrowIfCancellationRequested();
                                            await Task.Delay(1, token);
                                            walktopUTurn++;
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);

                                            fightOnSecondAbility = 1;
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

                                if (walktopUTurn == 3 && chBoxAutoMovement.Checked && Search == false)
                                {

                                    try
                                    {
                                       
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        au3.MouseMove(recalc(960), recalc(240, false), 10);
                                        KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 5000);
                                        au3.MouseMove(recalc(960), recalc(566, false), 10);
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                                        walktopUTurn++;
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
                                if (walktopUTurn == 8 && chBoxAutoMovement.Checked && Search == false)
                                {

                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        au3.MouseMove(recalc(523), recalc(810, false), 10);
                                        KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 9000);
                                        au3.MouseMove(recalc(1007), recalc(494, false), 10);
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                        await Task.Delay(1, token);

                                        walktopUTurn++;
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
                                if (walktopUTurn == 13 && chBoxAutoMovement.Checked && Search == false)
                                {

                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);

                                        au3.MouseMove(recalc(1578), recalc(524, false), 10);
                                        KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 10000);
                                        au3.MouseMove(recalc(905), recalc(531, false), 10);
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                                        walktopUTurn++;
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
                                if (walktopUTurn == 18 && chBoxAutoMovement.Checked && Search == false)
                                {

                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);

                                            au3.MouseMove(recalc(523), recalc(810, false), 10);
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 7000);
                                            au3.MouseMove(recalc(960), recalc(500, false), 10);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                            await Task.Delay(1, token);
                                        
                                        walktopUTurn++;
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
                                if (walktopUTurn == 23 && chBoxAutoMovement.Checked && Search == false)
                                {

                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);

                                        au3.MouseMove(recalc(960), recalc(70, false), 10);
                                        KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_LBUTTON, 6000);
                                        au3.MouseMove(recalc(960), recalc(566, false), 10);
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                        await Task.Delay(1, token);


                                        walktopUTurn++;
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
                                if (walktopUTurn == 23 && chBoxAutoMovement.Checked && Search == false)
                                {
                                    walktopUTurn = 1;
                                    await Task.Delay(1, token);
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
                            ///REVIVE
                            ///
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                float thresh = 0.9f;
                                var ReviveDeutschTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/revive1.png");
                                var ReviveDeutschMask =
                                new Image<Bgr, byte>(resourceFolder + "/revivemask1.png");

                                var ReviveEnglishTemplate =
                               new Image<Bgr, byte>(resourceFolder + "/reviveEnglish.png");
                                var ReviveEnglishMask =
                                new Image<Bgr, byte>(resourceFolder + "/reviveEnglishmask.png");

                                Point screenResolution = new Point(screenWidth, screenHeight);
                                var ReviveDeutschDetector = new EnterDetectors(ReviveDeutschTemplate, ReviveDeutschMask, thresh);
                                var ReviveEnglishDetector = new EnterDetectors(ReviveEnglishTemplate, ReviveEnglishMask, thresh);
                                var screenPrinter = new PrintScreen();
                                screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                                var screenCapture = new Image<Bgr, byte>("screen.png");
                                var ReviveDeutsch = ReviveDeutschDetector.GetClosestEnter(screenCapture);
                                var ReviveEnglish = ReviveEnglishDetector.GetClosestEnter(screenCapture);
                                if (ReviveDeutsch.HasValue || ReviveEnglish.HasValue)
                                {
                                    _SkillFight2 = false;
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "REVIVE!"));
                                    au3.MouseMove(recalc(1374), recalc(467, false), 10);
                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                    _SkillFight2 = true;
                                }
                                Random random = new Random();
                                var sleepTime = random.Next(150, 255);
                                await Task.Delay(sleepTime, token);
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
                            /// 
                            /// REVIVE ENDE
                            ///Portal Erkennung Start
                            ///
                            try
                            {

                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                object fight = au3.PixelSearch(recalc(114), recalc(208, false), recalc(168), recalc(220, false), 0xDBC7AC, 5);
                                if (fight.ToString() != "1" && Search == false)
                                {
                                    _SkillFight2 = false;
                                    object[] fightCoord = (object[])fight;
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 1: Portal detected!"));

                                    if (chBoxActivateF2.Checked)
                                    {
                                        _Floor1Fight = false;
                                        Search = true;
                                        _Floor1 = false;
                                        var t7 = Task.Run(() => SEARCHPORTAL(token));
                                        await Task.WhenAny(new[] { t7 });
                                    }
                                    else
                                    if (!chBoxActivateF2.Checked)
                                    {
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "ChaosDungeon Floor 1 Complete!"));
                                        _Floor1Fight = false;
                                        Search = true;
                                        _Floor1 = false;
                                        var leave = Task.Run(() => LEAVEDUNGEON(token));
                                        await Task.WhenAny(new[] { leave });
                                    }

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
                            /// 
                            /// Portal Erkennung ENDE
                            ///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);


                                if (chBoxPaladin.Checked == true && _Paladin == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(892), recalc(1027, false), recalc(934), recalc(1060, false), 0x75D6FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);
                                            _Paladin = false;
                                       
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Paladin Ultimate"));
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
                                else
                               if (chBoxDeathblade.Checked == true && _Deathblade == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(986), recalc(1029, false), recalc(1017), recalc(1035, false), 0xDAE7F3, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);
                                            _Deathblade = false;

                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_Y);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Deathblade Ultimate"));
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
                                else
                               if (chBoxSharpshooter.Checked == true && _Sharpshooter == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(1006), recalc(1049, false), recalc(1019), recalc(1068, false), 0x09B4EB, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);

                                            _Sharpshooter = false;

                                            var Sharpshooter = Task.Run(() => SharpshooterSecondPress(token));

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sharpshooter Ultimate"));
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
                                else
                               if (chBoxSorcerer.Checked == true && _Sorcerer == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(1006), recalc(1038, false), recalc(1010), recalc(1042, false), 0x8993FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);

                                            _Sorcerer = false;

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sorcerer Ultimate"));
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
                                else
                               if (chBoxSoulfist.Checked == true && _Soulfist == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);

                                        KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);

                                        _Soulfist = false;

                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Soulfist Ultimate"));

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
                                //////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    object health = au3.PixelSearch(recalc(633), recalc(962, false), recalc(651), recalc(969, false), 0x050405, 15);
                                    if (health.ToString() != "1" && checkBoxHeal10.Checked)
                                    {
                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 10%"));
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
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    object health = au3.PixelSearch(recalc(633), recalc(962, false), recalc(820), recalc(970, false), 0x050405, 15);

                                    if (health.ToString() != "1" && checkBoxHeal70.Checked)
                                    {


                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 70%"));
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
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    object healthi = au3.PixelSearch(recalc(633), recalc(962, false), recalc(686), recalc(969, false), 0x050405, 15);

                                    if (healthi.ToString() != "1" && checkBoxHeal30.Checked)
                                    {


                                        object[] healthiCoord = (object[])healthi;
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 30%"));
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
                    while (_Floor2Fight == true && Search == false)
                    {

                        foreach (KeyValuePair<byte, int> skill in SKILLS.skillset.OrderBy(x => x.Value))
                        {
                            try
                            {

                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                object fight = au3.PixelSearch(recalc(600), recalc(250, false), recalc(1319), recalc(843, false), 0xDD2C02, 10);
                                if (fight.ToString() != "1" && Search == false && _SkillFight2 == true)
                                {
                                    object[] fightCoord = (object[])fight;
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 100);
                                    KeyboardWrapper.AlternateHoldKey(skill.Key, casttimeByKey(skill.Key));

                                    if (chBoxDoubleQ.Checked || chBoxDoubleW.Checked || chBoxDoubleE.Checked || chBoxDoubleR.Checked || chBoxDoubleA.Checked || chBoxDoubleS.Checked || chBoxDoubleD.Checked || chBoxDoubleF.Checked)
                                    {
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Key Pressed twice!"));
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_Q);
                                    }
                                    setKeyCooldown(skill.Key); // Set Cooldown
                                    var td = Task.Run(() => SkillCooldown(token, skill.Key));
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 100);
                                    // fightOnSecondAbility++;
                                    if (isKeyOnCooldown(skill.Key) == false && Search == false)
                                    {
                                        try
                                        {
                                            token.ThrowIfCancellationRequested();
                                            await Task.Delay(1, token);

                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);

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

                            ///REVIVE
                            ///
                           
                                    
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                float thresh = 0.9f;
                                var ReviveDeutschTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/revive1.png");
                                var ReviveDeutschMask =
                                new Image<Bgr, byte>(resourceFolder + "/revivemask1.png");

                                var ReviveEnglishTemplate =
                               new Image<Bgr, byte>(resourceFolder + "/reviveEnglish.png");
                                var ReviveEnglishMask =
                                new Image<Bgr, byte>(resourceFolder + "/reviveEnglishmask.png");

                                Point screenResolution = new Point(screenWidth, screenHeight);
                                var ReviveDeutschDetector = new EnterDetectors(ReviveDeutschTemplate, ReviveDeutschMask, thresh);
                                var ReviveEnglishDetector = new EnterDetectors(ReviveEnglishTemplate, ReviveEnglishMask, thresh);
                                var screenPrinter = new PrintScreen();
                                screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                                var screenCapture = new Image<Bgr, byte>("screen.png");
                                var ReviveDeutsch = ReviveDeutschDetector.GetClosestEnter(screenCapture);
                                var ReviveEnglish = ReviveEnglishDetector.GetClosestEnter(screenCapture);
                                if (ReviveDeutsch.HasValue || ReviveEnglish.HasValue)
                                {
                                    _SkillFight2 = false;
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "REVIVE!"));
                                    au3.MouseMove(recalc(1374), recalc(467, false), 10);
                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                    _SkillFight2 = true;
                                }
                                Random random = new Random();
                                var sleepTime = random.Next(150, 255);
                                await Task.Delay(sleepTime, token);
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
                            /// 
                            /// REVIVE ENDE
                            ///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                if (chBoxBard.Checked == true && _Bard == true)
                                {
                                    try
                                    {

                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);

                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_Y);

                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Bard try to heal..."));


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
                                else
                                if (chBoxY.Checked == true && _Shadowhunter == true)
                                {
                                    try
                                    {

                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);

                                        object d = au3.PixelSearch(recalc(948), recalc(969, false), recalc(968), recalc(979, false), 0xBC08F0, 5);

                                        if (d.ToString() != "1")
                                        {

                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);
                                            _Shadowhunter = false;
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Shadowhunter Ultimate"));

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
                                else
                                if (chBoxPaladin.Checked == true && _Paladin == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(892), recalc(1027, false), recalc(934), recalc(1060, false), 0x75D6FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);
                                            _Paladin = false;

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Paladin Ultimate"));
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
                                else
                               if (chBoxDeathblade.Checked == true && _Deathblade == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(986), recalc(1029, false), recalc(1017), recalc(1035, false), 0xDAE7F3, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);
                                            _Deathblade = false;

                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_Y);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Deathblade Ultimate"));
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
                                else
                               if (chBoxSharpshooter.Checked == true && _Sharpshooter == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(1006), recalc(1049, false), recalc(1019), recalc(1068, false), 0x09B4EB, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);

                                            _Sharpshooter = false;

                                            var Sharpshooter = Task.Run(() => SharpshooterSecondPress(token));

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sharpshooter Ultimate"));
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
                                else
                               if (chBoxSorcerer.Checked == true && _Sorcerer == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(1006), recalc(1038, false), recalc(1010), recalc(1042, false), 0x8993FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);

                                            _Sorcerer = false;

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sorcerer Ultimate"));
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
                                else
                               if (chBoxSoulfist.Checked == true && _Soulfist == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);

                                        KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);

                                        _Soulfist = false;

                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Soulfist Ultimate"));

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
                                //////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    object health = au3.PixelSearch(recalc(633), recalc(962, false), recalc(651), recalc(969, false), 0x050405, 15);
                                    if (health.ToString() != "1" && checkBoxHeal10.Checked)
                                    {
                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 10%"));
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
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    object health = au3.PixelSearch(recalc(633), recalc(962, false), recalc(820), recalc(970, false), 0x050405, 15);

                                    if (health.ToString() != "1" && checkBoxHeal70.Checked)
                                    {


                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 70%"));
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
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    object healthi = au3.PixelSearch(recalc(633), recalc(962, false), recalc(686), recalc(969, false), 0x050405, 15);

                                    if (healthi.ToString() != "1" && checkBoxHeal30.Checked)
                                    {


                                        object[] healthiCoord = (object[])healthi;
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 30%"));
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
                    while (_Floor3Fight == true && Search == false)
                    {
                        foreach (KeyValuePair<byte, int> skill in SKILLS.skillset.OrderBy(x => x.Value))
                        {
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                object shardHit = au3.PixelSearch(recalc(600), recalc(250, false), recalc(1319), recalc(843, false), 0x630E17, 10);
                                object fight = au3.PixelSearch(recalc(600), recalc(250, false), recalc(1319), recalc(843, false), 0xDD2C02, 10);
                                if (fight.ToString() != "1" && shardHit.ToString() != "1" && Search == false)
                                {
                                    object[] shardHitCoord = (object[])shardHit;
                                    object[] fightCoord = (object[])fight;
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                                    au3.MouseMove((int)shardHitCoord[0], (int)shardHitCoord[1] + 100);
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 80);
                                    KeyboardWrapper.AlternateHoldKey(skill.Key, casttimeByKey(skill.Key));

                                    if (chBoxDoubleQ.Checked || chBoxDoubleW.Checked || chBoxDoubleE.Checked || chBoxDoubleR.Checked || chBoxDoubleA.Checked || chBoxDoubleS.Checked || chBoxDoubleD.Checked || chBoxDoubleF.Checked)
                                    {
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Key Pressed twice!"));
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_Q);
                                    }
                                    setKeyCooldown(skill.Key); // Set Cooldown
                                    var td = Task.Run(() => SkillCooldown(token, skill.Key));
                                    au3.MouseMove((int)shardHitCoord[0], (int)shardHitCoord[1] + 100);
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 80);
                                    fightOnSecondAbility++;
                                    if (isKeyOnCooldown(skill.Key) == false && Search == false)
                                    {
                                        try
                                        {
                                            token.ThrowIfCancellationRequested();
                                            await Task.Delay(1, token);
                                            fightOnSecondAbility++;
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_C);


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
                            ///REVIVE
                            ///
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                float thresh = 0.9f;
                                var ReviveDeutschTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/revive1.png");
                                var ReviveDeutschMask =
                                new Image<Bgr, byte>(resourceFolder + "/revivemask1.png");

                                var ReviveEnglishTemplate =
                               new Image<Bgr, byte>(resourceFolder + "/reviveEnglish.png");
                                var ReviveEnglishMask =
                                new Image<Bgr, byte>(resourceFolder + "/reviveEnglishmask.png");

                                Point screenResolution = new Point(screenWidth, screenHeight);
                                var ReviveDeutschDetector = new EnterDetectors(ReviveDeutschTemplate, ReviveDeutschMask, thresh);
                                var ReviveEnglishDetector = new EnterDetectors(ReviveEnglishTemplate, ReviveEnglishMask, thresh);
                                var screenPrinter = new PrintScreen();
                                screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                                var screenCapture = new Image<Bgr, byte>("screen.png");
                                var ReviveDeutsch = ReviveDeutschDetector.GetClosestEnter(screenCapture);
                                var ReviveEnglish = ReviveEnglishDetector.GetClosestEnter(screenCapture);
                                if (ReviveDeutsch.HasValue || ReviveEnglish.HasValue)
                                {
                                    _SkillFight2 = false;
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "REVIVE!"));
                                    au3.MouseMove(recalc(1374), recalc(467, false), 10);
                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                    _SkillFight2 = true;
                                }
                                Random random = new Random();
                                var sleepTime = random.Next(150, 255);
                                await Task.Delay(sleepTime, token);
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
                            /// 
                            /// REVIVE ENDE
                            ///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                if (chBoxBard.Checked == true && _Bard == true)
                                {
                                    try
                                    {

                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);

                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_Y);
                                        await Task.Delay(1, token);

                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Bard try to heal..."));


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
                                else
                                if (chBoxY.Checked == true && _Shadowhunter == true)
                                {
                                    try
                                    {

                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);

                                        object d = au3.PixelSearch(recalc(948), recalc(969, false), recalc(968), recalc(979, false), 0xBC08F0, 5);

                                        if (d.ToString() != "1")
                                        {

                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);
                                            _Shadowhunter = false;
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Shadowhunter Ultimate"));

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
                                else
                                if (chBoxPaladin.Checked == true && _Paladin == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(892), recalc(1027, false), recalc(934), recalc(1060, false), 0x75D6FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);
                                            _Paladin = false;

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Paladin Ultimate"));
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
                                else
                               if (chBoxDeathblade.Checked == true && _Deathblade == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(986), recalc(1029, false), recalc(1017), recalc(1035, false), 0xDAE7F3, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);
                                            _Deathblade = false;

                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_Y);
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Deathblade Ultimate"));
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
                                else
                               if (chBoxSharpshooter.Checked == true && _Sharpshooter == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(1006), recalc(1049, false), recalc(1019), recalc(1068, false), 0x09B4EB, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);

                                            _Sharpshooter = false;

                                            var Sharpshooter = Task.Run(() => SharpshooterSecondPress(token));

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sharpshooter Ultimate"));
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
                                else
                               if (chBoxSorcerer.Checked == true && _Sorcerer == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        object d = au3.PixelSearch(recalc(1006), recalc(1038, false), recalc(1010), recalc(1042, false), 0x8993FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);

                                            _Sorcerer = false;

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sorcerer Ultimate"));
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
                                else
                               if (chBoxSoulfist.Checked == true && _Soulfist == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);

                                        KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_Y, 500);

                                        _Soulfist = false;

                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Soulfist Ultimate"));

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
                                //////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    object health = au3.PixelSearch(recalc(633), recalc(962, false), recalc(651), recalc(969, false), 0x050405, 15);
                                    if (health.ToString() != "1" && checkBoxHeal10.Checked)
                                    {
                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 10%"));
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
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    object health = au3.PixelSearch(recalc(633), recalc(962, false), recalc(820), recalc(970, false), 0x050405, 15);

                                    if (health.ToString() != "1" && checkBoxHeal70.Checked)
                                    {


                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 70%"));
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
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    object healthi = au3.PixelSearch(recalc(633), recalc(962, false), recalc(686), recalc(969, false), 0x050405, 15);

                                    if (healthi.ToString() != "1" && checkBoxHeal30.Checked)
                                    {


                                        object[] healthiCoord = (object[])healthi;
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 30%"));
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

        private void setKeyCooldown(byte key)
        {
            switch (key)
            {
                case KeyboardWrapper.VK_A:
                    _A = false;
                    break;

                case KeyboardWrapper.VK_S:
                    _S = false;
                    break;

                case KeyboardWrapper.VK_D:
                    _D = false;
                    break;

                case KeyboardWrapper.VK_F:
                    _F = false;
                    break;

                case KeyboardWrapper.VK_Q:
                    _Q = false;
                    break;

                case KeyboardWrapper.VK_W:
                    _W = false;
                    break;

                case KeyboardWrapper.VK_E:
                    _E = false;
                    break;

                case KeyboardWrapper.VK_R:
                    _R = false;
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

        private async Task SEARCHBOSS2(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: search enemy..."));
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    _Shadowhunter = true;
                    _Paladin = true;
                    _Berserker = true;
                    if (searchSequence2 == 1)
                    {
                        au3.MouseMove(recalc(960), recalc(529, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        searchSequence2++;
                    }

                    for (int i = 0; i < int.Parse(txtDungeon3search.Text); i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            float threshold = 0.7f;
                            var shardTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/shard.png");
                            var shardMask =
                            new Image<Bgr, byte>(resourceFolder + "/shardmask.png");
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

                            Point myPosition = new Point(recalc(150), recalc(128, false));
                            Point screenResolution = new Point(screenWidth, screenHeight);
                            var shardDetector = new EnemyDetector(shardTemplate, shardMask, threshold);
                            var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                            var BossDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                            var mobDetector = new EnemyDetector(mobTemplate, mobMask, threshold);

                            var screenPrinter = new PrintScreen();

                            screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                            var screenCapture = new Image<Bgr, byte>("screen.png");
                            var shard = shardDetector.GetClosestEnemy(screenCapture);
                            var enemy = enemyDetector.GetClosestEnemy(screenCapture);
                            var Boss = BossDetector.GetClosestEnemy(screenCapture);
                            var mob = mobDetector.GetClosestEnemy(screenCapture);

                            if (CompleteIteration == 1)
                            {

                                au3.MouseMove(recalc(960), recalc(529, false), 10);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                CompleteIteration++;
                            }
                            else
                            {
                                if (shard.HasValue)
                                {
                                    CvInvoke.Rectangle(screenCapture,
                                        new Rectangle(new Point(shard.Value.X, shard.Value.Y), shardTemplate.Size),
                                        new MCvScalar(255));
                                    double distance_x = (screenWidth - recalc(296)) / 2;
                                    double distance_y = (screenHeight - recalc(255, false)) / 2;

                                    var shard_position = ((shard.Value.X + distance_x), (shard.Value.Y + distance_y));
                                    double multiplier = 1;
                                    var shard_position_on_minimap = ((shard.Value.X), (shard.Value.Y));
                                    var my_position_on_minimap = ((recalc(296) / 2), (recalc(255, false) / 2));
                                    var dist = Math.Sqrt(Math.Pow((my_position_on_minimap.Item1 - shard_position_on_minimap.Item1), 2) + Math.Pow((my_position_on_minimap.Item2 - shard_position_on_minimap.Item2), 2));

                                    if (dist < 180)
                                    {
                                        multiplier = 1.2;
                                    }

                                    double posx;
                                    double posy;
                                    if (shard_position.Item1 < (screenWidth / 2))
                                    {
                                        posx = shard_position.Item1 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posx = shard_position.Item1 * multiplier;
                                    }
                                    if (shard_position.Item2 < (screenHeight / 2))
                                    {
                                        posy = shard_position.Item2 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posy = shard_position.Item2 * multiplier;
                                    }


                                    var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: Shard found!"));
                                    inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                    if (txtLEFT.Text == "LEFT")
                                    {
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                    }
                                    else
                                    {
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_RBUTTON);
                                    }
                                }
                                else
                                {
                                    if (enemy.HasValue)
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
                                        var dist = Math.Sqrt(Math.Pow((my_position_on_minimap.Item1 - enemy_position_on_minimap.Item1), 2) + Math.Pow((my_position_on_minimap.Item2 - enemy_position_on_minimap.Item2), 2));

                                        if (dist < 180)
                                        {
                                            multiplier = 1.2;
                                        }

                                        double posx;
                                        double posy;
                                        if (enemy_position.Item1 < (screenWidth / 2))
                                        {
                                            posx = enemy_position.Item1 * (2 - multiplier);
                                        }
                                        else
                                        {
                                            posx = enemy_position.Item1 * multiplier;
                                        }
                                        if (enemy_position.Item2 < (screenHeight / 2))
                                        {
                                            posy = enemy_position.Item2 * (2 - multiplier);
                                        }
                                        else
                                        {
                                            posy = enemy_position.Item2 * multiplier;
                                        }


                                        var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: Mid-Boss found!"));
                                        inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                        if (txtLEFT.Text == "LEFT")
                                        {
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                        }
                                        else
                                        {
                                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_RBUTTON);
                                        }
                                    }
                                    else
                                    {
                                        if (mob.HasValue)
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
                                            var dist = Math.Sqrt(Math.Pow((my_position_on_minimap.Item1 - mob_position_on_minimap.Item1), 2) + Math.Pow((my_position_on_minimap.Item2 - mob_position_on_minimap.Item2), 2));

                                            if (dist < 180)
                                            {
                                                multiplier = 1.2;
                                            }

                                            double posx;
                                            double posy;
                                            if (mob_position.Item1 < (screenWidth / 2))
                                            {
                                                posx = mob_position.Item1 * (2 - multiplier);
                                            }
                                            else
                                            {
                                                posx = mob_position.Item1 * multiplier;
                                            }
                                            if (mob_position.Item2 < (screenHeight / 2))
                                            {
                                                posy = mob_position.Item2 * (2 - multiplier);
                                            }
                                            else
                                            {
                                                posy = mob_position.Item2 * multiplier;
                                            }


                                            var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: Mob found!"));

                                            inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                            if (txtLEFT.Text == "LEFT")
                                            {
                                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                            }
                                            else
                                            {
                                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RBUTTON);
                                            }
                                        }

                                    }

                                }
                            }


                            Random random = new Random();
                            var sleepTime = random.Next(150, 255);
                            await Task.Delay(sleepTime, token);
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

                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object walk = au3.PixelSearch(recalc(560), recalc(260, false), recalc(1382), recalc(817, false), 0x21BD08, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseMove(recalc(903), recalc(605, false), 5);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    object complete = au3.PixelSearch(recalc(31), recalc(97, false), recalc(81), recalc(108, false), 0x8A412C, 5);
                                    if (complete.ToString() != "1")
                                    {
                                        object[] completeCoord = (object[])complete;
                                        au3.MouseMove(recalc(191), recalc(285, false), 5);
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                   

                                        await Task.Delay(1000, token);
                                    }
                                    await Task.Delay(2000, token);
                                    if (_REPAIR == true)
                                    {
                                        await Task.Delay(2000, token);
                                        var t7 = Task.Run(() => REPAIR(token));
                                        await Task.WhenAny(new[] { t7 });
                                    }
                                    else
                                    if (_LOGOUT == true)
                                    {
                                        var t11 = Task.Run(() => LOGOUT(token));
                                        await Task.WhenAny(new[] { t11 });
                                    }
                                    else
                                    if (_REPAIR == false && _LOGOUT == false)
                                    {
                                        await Task.Delay(2000, token);
                                        var t9 = Task.Run(() => RESTART(token));
                                        await Task.WhenAny(new[] { t9 });
                                    }
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
                if (Floor3 == 1)
                { _Floor3 = true; }

                var t12 = Task.Run(() => FLOORTIME(token));
                await Task.WhenAny(new[] { t12 });
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

        private async Task FLOOR2PORTAL(CancellationToken token)
        {
            try
            {

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);


                    _Shadowhunter = true;
                    _Paladin = true;
                    _Berserker = true;
                    for (int i = 0; i <= 20; i++)
                    {
                        try
                        {
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);


                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Search Portal..."));
                            // Tunable variables
                            float threshold = 0.7f; // set this higher for fewer false positives and lower for fewer false negatives
                            var enemyTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/portalenter1.png"); // icon of the enemy
                            var enemyMask =
                                new Image<Bgr, byte>(resourceFolder + "/portalentermask1.png"); // make white what the important parts are, other parts should be black
                                                                                                //var screenCapture = new Image<Bgr, byte>("D:/Projects/bot-enemy-detection/EnemyDetection/screen.png");
                            Point myPosition = new Point(recalc(150), recalc(128, false));
                            Point screenResolution = new Point(screenWidth, screenHeight);

                            // Main program loop
                            var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                            var screenPrinter = new PrintScreen();

                            screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                            var screenCapture = new Image<Bgr, byte>("screen.png");
                            var enemy = enemyDetector.GetClosestEnemy(screenCapture);
                            if (enemy.HasValue)
                            {
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Portal found..."));
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                CvInvoke.Rectangle(screenCapture,
                                    new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                    new MCvScalar(255));

                                double distance_x = (screenWidth - recalc(296)) / 2;
                                double distance_y = (screenHeight - recalc(255, false)) / 2;

                                var enemy_position = ((enemy.Value.X + distance_x), (enemy.Value.Y + distance_y));
                                double multiplier = 1;
                                var enemy_position_on_minimap = ((enemy.Value.X), (enemy.Value.Y));
                                var my_position_on_minimap = ((recalc(296) / 2), (recalc(255, false) / 2));
                                var dist = Math.Sqrt(Math.Pow((my_position_on_minimap.Item1 - enemy_position_on_minimap.Item1), 2) + Math.Pow((my_position_on_minimap.Item2 - enemy_position_on_minimap.Item2), 2));

                                if (dist < 180)
                                {
                                    multiplier = 1.2;
                                }

                                double posx;
                                double posy;
                                if (enemy_position.Item1 < (screenWidth / 2))
                                {
                                    posx = enemy_position.Item1 * (2 - multiplier);
                                }
                                else
                                {
                                    posx = enemy_position.Item1 * multiplier;
                                }
                                if (enemy_position.Item2 < (screenHeight / 2))
                                {
                                    posy = enemy_position.Item2 * (2 - multiplier);
                                }
                                else
                                {
                                    posy = enemy_position.Item2 * multiplier;
                                }


                                var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Enter Portal..."));

                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                                if (txtLEFT.Text == "LEFT")
                                {
                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                }
                                else
                                {
                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_RBUTTON);
                                }
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);



                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                                if (txtLEFT.Text == "LEFT")
                                {
                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                }
                                else
                                {
                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_RBUTTON);
                                }

                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                            }
                            else
                            {
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

                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        Random random = new Random();
                        var sleepTime = random.Next(300, 500);
                        await Task.Delay(sleepTime, token);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

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
                searchSequence2 = 1;
                var t12 = Task.Run(() => SEARCHBOSS2(token));
                await Task.WhenAny(new[] { t12 });
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

        private async Task LEAVEDUNGEON(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    _Bard = false;
                    _Shadowhunter = false;
                    _Berserker = false;
                    _Paladin = false;
                    _Deathblade = false;
                    _Sharpshooter = false;
                    _Bard = false;
                    _Sorcerer = false;
                    _Soulfist = false;



                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object walk = au3.PixelSearch(recalc(77), recalc(270, false), recalc(190), recalc(298, false), 0x29343F, 5);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseMove((int)walkCoord[0], (int)walkCoord[1], 5);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
         
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
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object walk = au3.PixelSearch(recalc(77), recalc(270, false), recalc(190), recalc(298, false), 0x29343F, 5);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseMove((int)walkCoord[0], (int)walkCoord[1], 5);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                             
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
                var t6 = Task.Run(() => LEAVEACCEPT(token));
                await Task.WhenAny(new[] { t6 });
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

        private async Task LEAVEDUNGEONCOMPLETE(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    _Shadowhunter = true;
                    _Paladin = true;
                    _Berserker = true;
                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object walk = au3.PixelSearch(recalc(141), recalc(274, false), recalc(245), recalc(294, false), 0x29343F, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseMove((int)walkCoord[0], (int)walkCoord[1], 5);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
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
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object walk = au3.PixelSearch(recalc(141), recalc(274, false), recalc(245), recalc(294, false), 0x29343F, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseMove((int)walkCoord[0], (int)walkCoord[1], 5);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
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
                var t6 = Task.Run(() => LEAVEACCEPT(token));
                await Task.WhenAny(new[] { t6 });
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

        private async Task LEAVEACCEPT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object walk = au3.PixelSearch(recalc(560), recalc(260, false), recalc(1382), recalc(817, false), 0x21BD08, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseMove(recalc(903), recalc(605, false), 5);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                            }
                        }
                        catch { }
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object walk = au3.PixelSearch(recalc(560), recalc(260, false), recalc(1382), recalc(817, false), 0x21BD08, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseMove(recalc(903), recalc(605, false), 5);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
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
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object walk = au3.PixelSearch(recalc(560), recalc(260, false), recalc(1382), recalc(817, false), 0x21BD08, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseMove(recalc(903), recalc(605, false), 5);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
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

                await Task.Delay(2000, token);
                if (_REPAIR == true)
                {
                    await Task.Delay(2000, token);
                    var t7 = Task.Run(() => REPAIR(token));
                    await Task.WhenAny(new[] { t7 });
                }
                else
                if (_LOGOUT == true)
                {
                    var t11 = Task.Run(() => LOGOUT(token));
                    await Task.WhenAny(new[] { t11 });
                }
                else
                if (_REPAIR == false && _LOGOUT == false)
                {
                    await Task.Delay(2000, token);
                    var t9 = Task.Run(() => RESTART(token));
                    await Task.WhenAny(new[] { t9 });
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

        private async Task LOGOUT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                for (int i = 0; i < 1; i++)
                {
                    try
                    {
                        await Task.Delay(20000, token);

                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "LOGOUT Process starts..."));
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                        await Task.Delay(2000, token);
                        au3.MouseMove(recalc(1238), recalc(728, false),5);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(2000, token);
                        au3.MouseMove(recalc(906), recalc(575, false), 5);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(1000, token);

                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "You are loged out!"));
                        _start = false;
                        cts.Cancel();


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

        private async Task REPAIR(CancellationToken token)

        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Auto-Repair starts in 20 seconds..."));
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            await Task.Delay(25000, token);
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

                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            au3.MouseMove(recalc(1741), recalc(1040, false), 5);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
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
                        await Task.Delay(2000, token);
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            await Task.Delay(1500, token);
                            au3.MouseMove(recalc(1684), recalc(823, false), 5);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                       
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

                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            await Task.Delay(1500, token);
                            au3.MouseMove(recalc(1256), recalc(693, false), 5);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    
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

                        try
                        {
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
                await Task.Delay(2000, token);
                var t10 = Task.Run(() => RESTART_AFTERREPAIR(token));
                await Task.WhenAny(new[] { t10 });
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

        private async Task RESTART(CancellationToken token)
        {
            try
            {
                Boolean restart = false;
                while (!restart)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);


                    float threshold = 0.9f;
                    var handTemplate =
                    new Image<Bgr, byte>(resourceFolder + "/hand.png");
                    var handMask =
                    new Image<Bgr, byte>(resourceFolder + "/handmask.png");

                    var handDetector = new ScreenDetector(handTemplate, handMask, threshold, ChaosBot.recalc(640), ChaosBot.recalc(132, false), ChaosBot.recalc(660), ChaosBot.recalc(609, false));

                    var screenPrinter = new PrintScreen();

                    screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                    var screenCapture = new Image<Bgr, byte>("screen.png");
                    var hand = handDetector.GetClosestItem(screenCapture);

                    if (!hand.HasValue)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(50, token);
                    }
                    else
                    {
                        restart = true;
                    }
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


            await Task.Delay(2000, token);
            var t1 = Task.Run(() => START(token));
            await Task.WhenAny(new[] { t1 });

        }

        private async Task RESTART_AFTERREPAIR(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Auto-Repair done!"));
                            await Task.Delay(4000, token);
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
                var t1 = Task.Run(() => START(token));
                await Task.WhenAny(new[] { t1 });
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
                F = KeyboardWrapper.VK_F
            };
            LAYOUT.Add(AZERTY);

            comboBox1.DataSource = LAYOUT;
            comboBox1.DisplayMember = "LAYOUTS";
            currentLayout = comboBox1.SelectedItem as Layout_Keyboard;
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);

            txtPortalSearch.Text = Properties.Settings.Default.txtPortalSearch;
            txtLEFT.Text = Properties.Settings.Default.left;

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
            chBoxBerserker.Checked = Properties.Settings.Default.chBoxBerserker;
            txtRestartTimer.Text = Properties.Settings.Default.RestartTimer;
            chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxSaveAll;
            chBoxActivateF2.Checked = Properties.Settings.Default.chBoxActivateF2;
            txtDungeon2search.Text = Properties.Settings.Default.txtDungeon2search;
            txtDungeon2.Text = Properties.Settings.Default.txtDungeon2;
            txtDungeon3search.Text = Properties.Settings.Default.txtDungeon3search;
            txtDungeon3.Text = Properties.Settings.Default.txtDungeon3;
            chBoxActivateF3.Checked = Properties.Settings.Default.chBoxActivateF3;
            txtDungeon3Iteration.Text = Properties.Settings.Default.txtDungeon3Iteration;
            txtDungeon2Iteration.Text = Properties.Settings.Default.txtDungeon2Iteration;

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
            else
            if (!checkBoxHeal30.Checked)
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
            else
            if (!checkBoxHeal70.Checked)
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.txtDungeon3Iteration = "12";
                Properties.Settings.Default.txtDungeon2Iteration = "9";

                Properties.Settings.Default.txtPortalSearch = "12";
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
                Properties.Settings.Default.chboxPaladin = false;
                Properties.Settings.Default.RestartTimer = "25";
                Properties.Settings.Default.chBoxSaveAll = false;
                Properties.Settings.Default.chBoxActivateF2 = false;
                Properties.Settings.Default.txtDungeon2 = "15";
                Properties.Settings.Default.txtDungeon2search = "7";
                Properties.Settings.Default.txtDungeon3 = "20";
                Properties.Settings.Default.txtDungeon3search = "10";
                Properties.Settings.Default.chBoxActivateF3 = false;
                Properties.Settings.Default.chBoxAutoMovement = false;


                Properties.Settings.Default.chBoxSharpshooter = false;
                Properties.Settings.Default.chBoxSorcerer = false;
                Properties.Settings.Default.chBoxDeathblade = false;

                Properties.Settings.Default.RQ = "1";
                Properties.Settings.Default.RW = "2";
                Properties.Settings.Default.RE = "3";
                Properties.Settings.Default.RR = "4";
                Properties.Settings.Default.RA = "5";
                Properties.Settings.Default.RS = "6";
                Properties.Settings.Default.RD = "7";
                Properties.Settings.Default.RF = "8";

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

                Properties.Settings.Default.Save();

                chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxAutoMovement;
                txtDungeon3Iteration.Text = Properties.Settings.Default.txtDungeon3Iteration;
                txtDungeon2Iteration.Text = Properties.Settings.Default.txtDungeon2Iteration;
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
                chBoxSorcerer.Checked = Properties.Settings.Default.chBoxSorcerer;
                chBoxSharpshooter.Checked = Properties.Settings.Default.chBoxSharpshooter;
                chBoxSoulfist.Checked = Properties.Settings.Default.chBoxSoulfist;
                txtRestartTimer.Text = Properties.Settings.Default.RestartTimer;
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


                txPQ.Text = Properties.Settings.Default.RQ;
                txPW.Text = Properties.Settings.Default.RW;
                txPE.Text = Properties.Settings.Default.RE;
                txPR.Text = Properties.Settings.Default.RR;
                txPA.Text = Properties.Settings.Default.RA;
                txPS.Text = Properties.Settings.Default.RS;
                txPD.Text = Properties.Settings.Default.RD;
                txPF.Text = Properties.Settings.Default.RF;
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
            }
            catch { }
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            frmGuide Form = new frmGuide();
            Form.Show();
        }
        private string translateKey(int key)
        {
            string translate = "";
            switch(key)
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

        }

        public async void SharpshooterSecondPress(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(3000, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_Y);
               

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
                            timer.Elapsed += A_CooldownEvent;
                            break;

                        case KeyboardWrapper.VK_S:
                            timer.Elapsed += S_CooldownEvent;

                            break;

                        case KeyboardWrapper.VK_D:
                            timer.Elapsed += D_CooldownEvent;

                            break;

                        case KeyboardWrapper.VK_F:
                            timer.Elapsed += F_CooldownEvent;

                            break;

                        case KeyboardWrapper.VK_Q:
                            timer.Elapsed += Q_CooldownEvent;

                            break;

                        case KeyboardWrapper.VK_W:
                            timer.Elapsed += W_CooldownEvent;

                            break;

                        case KeyboardWrapper.VK_E:
                            timer.Elapsed += E_CooldownEvent;

                            break;

                        case KeyboardWrapper.VK_R:
                            timer.Elapsed += R_CooldownEvent;
                            break;
                    }
                    timer.AutoReset = false;
                    timer.Enabled = true;
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

        private void Q_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _Q = true;
        }

        private void W_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _W = true;
        }

        private void E_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _E = true;
        }

        private void R_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _R = true;
        }

        private void A_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _A = true;
        }

        private void S_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _S = true;
        }

        private void D_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _D = true;
        }

        private void F_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _F = true;
        }

        private void buttonSaveRotation_Click(object sender, EventArgs e)
        {


            if (comboBoxRotations.Text != "")
            {
                if (comboBoxRotations.Text != "main")
                {
                    rotation.txtDungeon2Iteration = txtDungeon2Iteration.Text;
                    rotation.txtDungeon3Iteration = txtDungeon3Iteration.Text;

                    rotation.txtPortalSearch = txtPortalSearch.Text;
                    rotation.instant = txtHeal30.Text;
                    rotation.potion = txtHeal70.Text;
                    rotation.txtHeal10 = txtHeal10.Text;
                    rotation.chboxinstant = (bool)checkBoxHeal30.Checked;
                    rotation.chboxheal = (bool)checkBoxHeal70.Checked;
                    rotation.chboxheal10 = (bool)checkBoxHeal10.Checked;
                    rotation.chBoxAutoRepair = (bool)chBoxAutoRepair.Checked;
                    rotation.autorepair = txtRepair.Text;

                    rotation.autologout = txtLOGOUT.Text;
                    rotation.chBoxautologout = chBoxLOGOUT.Checked;
                    rotation.chBoxAutoMovement = chBoxAutoMovement.Checked;
                    rotation.autorepair = txtRepair.Text;
                    rotation.chBoxShadowhunter = (bool)chBoxY.Checked;
                    rotation.chboxPaladin = (bool)chBoxPaladin.Checked;
                    rotation.chBoxBerserker = (bool)chBoxBerserker.Checked;
                    rotation.chBoxDeathblade = (bool)chBoxDeathblade.Checked;
                    rotation.chBoxSharpshooter = (bool)chBoxSharpshooter.Checked;
                    rotation.chBoxSoulfist = (bool)chBoxSoulfist.Checked;
                    rotation.chBoxSorcerer = (bool)chBoxSorcerer.Checked;
                    rotation.chBoxBard = (bool)chBoxBard.Checked;
                    rotation.RestartTimer = txtRestartTimer.Text;
                    rotation.chBoxSaveAll = chBoxAutoMovement.Checked;
                    rotation.chBoxActivateF2 = chBoxActivateF2.Checked;
                    rotation.chBoxActivateF3 = chBoxActivateF3.Checked;
                    rotation.txtDungeon3search = txtDungeon3search.Text;
                    rotation.txtDungeon3 = txtDungeon3.Text;
                    rotation.txtLEFT = txtLEFT.Text;

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
                    rotation.pQ = txPQ.Text;
                    rotation.pW = txPW.Text;
                    rotation.pE = txPE.Text;
                    rotation.pR = txPR.Text;
                    rotation.pA = txPA.Text;
                    rotation.pS = txPS.Text;
                    rotation.pD = txPD.Text;
                    rotation.pF = txPF.Text;
                    rotation.chBoxDoubleQ = chBoxDoubleQ.Checked;
                    rotation.chBoxDoubleW = chBoxDoubleW.Checked;
                    rotation.chBoxDoubleE = chBoxDoubleE.Checked;
                    rotation.chBoxDoubleR = chBoxDoubleR.Checked;
                    rotation.chBoxDoubleA = chBoxDoubleA.Checked;
                    rotation.chBoxDoubleS = chBoxDoubleS.Checked;
                    rotation.chBoxDoubleD = chBoxDoubleD.Checked;
                    rotation.chBoxDoubleF = chBoxDoubleF.Checked;


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
                txtLEFT.Text = rotation.left;
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
                chBoxBard.Checked = rotation.chBoxBard;
                chBoxDeathblade.Checked = rotation.chBoxDeathblade;
                chBoxSharpshooter.Checked = rotation.chBoxSharpshooter;
                chBoxSoulfist.Checked = rotation.chBoxSoulfist;
                txtLOGOUT.Text = rotation.autologout;
                chBoxLOGOUT.Checked = rotation.chBoxautologout;
                txtHeal10.Text = rotation.txtHeal10;
                txtDungeon2Iteration.Text = rotation.txtDungeon2Iteration;
                txtDungeon3Iteration.Text = rotation.txtDungeon3Iteration;
                chBoxAutoMovement.Checked = rotation.chBoxAutoMovement;
                chBoxActivateF3.Checked = rotation.chBoxActivateF3;
                txtDungeon3search.Text = rotation.txtDungeon3search;
                txtDungeon3.Text = rotation.txtDungeon3;

                chBoxSorcerer.Checked = rotation.chBoxSorcerer;
                txtRestartTimer.Text = rotation.RestartTimer;
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
                txPQ.Text = rotation.pQ;
                txPW.Text = rotation.pW;
                txPE.Text = rotation.pE;
                txPR.Text = rotation.pR;
                txPA.Text = rotation.pA;
                txPS.Text = rotation.pS;
                txPD.Text = rotation.pD;
                txPF.Text = rotation.pF;
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
            if (txPA.Text != "" && txPS.Text != "" && txPD.Text != "" && txPF.Text != "" && txPQ.Text != "" && txPW.Text != "" && txPE.Text != "" && txPR.Text != "")
                SKILLS.skillset = new Dictionary<byte, int>()
            {
                { KeyboardWrapper.VK_A, int.Parse(txPA.Text)},
                { KeyboardWrapper.VK_S, int.Parse(txPS.Text)},
                { KeyboardWrapper.VK_D, int.Parse(txPD.Text)},
                { KeyboardWrapper.VK_F, int.Parse(txPF.Text)},
                { KeyboardWrapper.VK_Q, int.Parse(txPQ.Text)},
                { KeyboardWrapper.VK_W, int.Parse(txPW.Text)},
                { KeyboardWrapper.VK_E, int.Parse(txPE.Text)},
                { KeyboardWrapper.VK_R, int.Parse(txPR.Text)},
            }.ToList();
        }

        private void chBoxActivateF2_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxActivateF2.Checked)
            {
                txtDungeon2search.ReadOnly = false;
                txtDungeon2.ReadOnly = false;
                txtDungeon2Iteration.ReadOnly = false;

            }
            else
               if (!chBoxActivateF2.Checked)
            {
                txtDungeon2search.ReadOnly = true;
                txtDungeon2.ReadOnly = true;
                txtDungeon2Iteration.ReadOnly = true;

            }
        }

        private void chBoxActivateF3_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxActivateF3.Checked)
            {
                txtDungeon3search.ReadOnly = false;
                txtDungeon3.ReadOnly = false;
                txtDungeon3Iteration.ReadOnly = false;
            }
            else
              if (!chBoxActivateF3.Checked)
            {
                txtDungeon3search.ReadOnly = true;
                txtDungeon3.ReadOnly = true;
                txtDungeon3Iteration.ReadOnly = true;

            }
        }


        private void button1_Click_2(object sender, EventArgs e)
        {
            try
            {



                object fight = au3.PixelSearch(recalc(114), recalc(208, false), recalc(168), recalc(220, false), 0xDBC7AC, 5);
                if (fight.ToString() != "1" && Search == false)
                {
                    object[] fightCoord = (object[])fight;
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Portal detected"));

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

      
    }
}