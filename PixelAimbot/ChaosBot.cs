using AutoItX3Lib;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using PixelAimbot.Classes;
using PixelAimbot.Classes.Misc;
using System;
using System.Collections.Generic;
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
        /// OPENCV START  /// OPENCV START  /// OPENCV START  /// OPENCV START

        public string resourceFolder = "";

        private (int, int) PixelToAbsolute(double x, double y, Point screenResolution)
        {
            int newX = (int)(x / screenResolution.X * 65535);
            int newY = (int)(y / screenResolution.Y * 65535);
            return (newX, newY);
        }

        private static readonly Random random = new Random();

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
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        public Layout_Keyboard currentLayout;

        public ChaosBot()
        {
            InitializeComponent();

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string applicationFolder = Path.Combine(folder, "cb_res");

            resourceFolder = applicationFolder;

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            this.Text = RandomString(15);
            // 3. Register HotKeys
            label15.Text = Properties.Settings.Default.version;
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
                _stop = false;
                _start = false;
                cts.Cancel();
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "STOPPED!"));
            }
        }

        private bool _start = false;
        private bool _stop = false;
        private bool _REPAIR = false;
        private bool _Shadowhunter = true;
        private bool _Berserker = true;
        private bool _Paladin = true;
        private bool _LOGOUT = false;

        //SKILL AND COOLDOWN//
        private bool _Q = true;

        private bool _W = true;
        private bool _E = true;
        private bool _R = true;
        private bool _A = true;
        private bool _S = true;
        private bool _D = true;
        private bool _F = true;

        private bool _Y = true;
        private bool _Z = true;

        private System.Timers.Timer timer;

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (chBoxSaveAll.Checked == true)
            {
                Properties.Settings.Default.dungeontimer = txtDungeon.Text;
                Properties.Settings.Default.left = txtLEFT.Text;
                Properties.Settings.Default.right = txtRIGHT.Text;
                Properties.Settings.Default.q = txQ.Text;
                Properties.Settings.Default.w = txW.Text;
                Properties.Settings.Default.e = txE.Text;
                Properties.Settings.Default.r = txR.Text;
                Properties.Settings.Default.a = txA.Text;
                Properties.Settings.Default.s = txS.Text;
                Properties.Settings.Default.d = txD.Text;
                Properties.Settings.Default.f = txF.Text;
                Properties.Settings.Default.cQ = txCoolQ.Text;
                Properties.Settings.Default.cW = txCoolW.Text;
                Properties.Settings.Default.cE = txCoolE.Text;
                Properties.Settings.Default.cR = txCoolR.Text;
                Properties.Settings.Default.cA = txCoolA.Text;
                Properties.Settings.Default.cS = txCoolS.Text;
                Properties.Settings.Default.cD = txCoolD.Text;
                Properties.Settings.Default.cF = txCoolF.Text;

                Properties.Settings.Default.instant = txtInstant.Text;
                Properties.Settings.Default.potion = txtHeal.Text;
                Properties.Settings.Default.chboxinstant = checkBoxInstant.Checked;
                Properties.Settings.Default.chboxheal = checkBoxHeal.Checked;
                Properties.Settings.Default.chBoxAutoRepair = chBoxAutoRepair.Checked;
                Properties.Settings.Default.autorepair = txtRepair.Text;
                Properties.Settings.Default.chBoxShadowhunter = chBoxY.Checked;
                Properties.Settings.Default.chBoxBerserker = chBoxBerserker.Checked;
                Properties.Settings.Default.chboxPaladin = chBoxPaladin.Checked;
                Properties.Settings.Default.RestartTimer = txtRestartTimer.Text;
                Properties.Settings.Default.chBoxSaveAll = chBoxSaveAll.Checked;
                Properties.Settings.Default.chBoxActivateF2 = chBoxActivateF2.Checked;
                Properties.Settings.Default.txtDungeon2 = txtDungeon2.Text;
                Properties.Settings.Default.txtDungeon2search = txtDungeon2search.Text;

                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.dungeontimer = "60";
                Properties.Settings.Default.left = "LEFT";
                Properties.Settings.Default.right = "RIGHT";
                Properties.Settings.Default.q = "500";
                Properties.Settings.Default.w = "500";
                Properties.Settings.Default.e = "500";
                Properties.Settings.Default.r = "500";
                Properties.Settings.Default.a = "500";
                Properties.Settings.Default.s = "500";
                Properties.Settings.Default.d = "500";
                Properties.Settings.Default.f = "500";
                Properties.Settings.Default.cQ = "500";
                Properties.Settings.Default.cW = "500";
                Properties.Settings.Default.cE = "500";
                Properties.Settings.Default.cR = "500";
                Properties.Settings.Default.cA = "500";
                Properties.Settings.Default.cS = "500";
                Properties.Settings.Default.cD = "500";
                Properties.Settings.Default.cF = "500";
                Properties.Settings.Default.instant = "";
                Properties.Settings.Default.potion = "";
                Properties.Settings.Default.chboxinstant = false;
                Properties.Settings.Default.chboxheal = false;
                Properties.Settings.Default.chBoxAutoRepair = false;
                Properties.Settings.Default.autorepair = "10";
                Properties.Settings.Default.chBoxShadowhunter = false;
                Properties.Settings.Default.chBoxBerserker = false;
                Properties.Settings.Default.chboxPaladin = false;
                Properties.Settings.Default.RestartTimer = "25";
                Properties.Settings.Default.chBoxSaveAll = false;
                Properties.Settings.Default.chBoxActivateF2 = false;
                Properties.Settings.Default.txtDungeon2 = "15";
                Properties.Settings.Default.txtDungeon2search = "9";
                Properties.Settings.Default.Save();
            }

            // await Task.Run(new Action(STARTKLICK));
            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is starting..."));
            if (chBoxAutoRepair.Checked == true && _start == false)
            {
                REPAIRTIMER();
            }
            else
            {
                _REPAIR = false;
            }
            if (chBoxLOGOUT.Checked == true && _start == false)
            {
                LOGOUTTIMER();
            }
            else
            {
                _LOGOUT = false;
            }
            if (_start == false)
                try
                {
                    _start = true;
                    _stop = true;
                    cts = new CancellationTokenSource();
                    var token = cts.Token;
                    var t1 = Task.Run(() => STARTKLICK(token));
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

        public void REPAIRTIMER()
        {
            timer = new System.Timers.Timer((int.Parse(txtRepair.Text) * 1000) * 60);

            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        public void LOGOUTTIMER()
        {
            timer = new System.Timers.Timer((int.Parse(txtLOGOUT.Text) * 1000) * 60);

            timer.Elapsed += OnTimedEvent2;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _REPAIR = true;
        }

        private int fightSequence = 1;
        private int searchbossSequence = 1;

        private void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            _LOGOUT = true;
        }

        private async Task STARTKLICK(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object move1 = au3.PixelSearch(0, 0, 1920, 1080, 0x2A3540, 100);

                        if (move1.ToString() != "1")
                        {
                            au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 1);
                            Thread.Sleep(500);
                            au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 1);
                        }
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
                }
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
            Thread.Sleep(2000);

            var t2 = Task.Run(() => START(token));
            await Task.WhenAny(new[] { t2 });
        }

        private async Task START(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object walk = au3.PixelSearch(917, 334, 1477, 746, 0xD9DAD9);

                        if (walk.ToString() != "1")
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object[] walkCoord = (object[])walk;
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            au3.Send("{G}");
                            Thread.Sleep(500);
                        }
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
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object walk2 = au3.PixelSearch(1406, 841, 1673, 875, 0x856413, 5);

                        if (walk2.ToString() != "1")
                        {
                            object[] walk2Coord = (object[])walk2;
                            au3.MouseClick("LEFT", 1467, 858, 2, 10);
                            au3.MouseClick("LEFT", 1467, 858, 2, 10);
                            au3.MouseClick("LEFT", 1467, 858, 2, 10);
                            au3.MouseClick("LEFT", 1467, 858, 2, 10);
                            Thread.Sleep(500);
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
                    /////////////// ACCEPT
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object accept = au3.PixelSearch(861, 590, 954, 616, 0x334454, 8);

                        if (accept.ToString() != "1")
                        {
                            object[] acceptCoord = (object[])accept;
                            au3.MouseClick("LEFT", (int)acceptCoord[0], (int)acceptCoord[1], 1, 5);
                            au3.MouseClick("LEFT", (int)acceptCoord[0], (int)acceptCoord[1], 1, 5);
                            au3.MouseClick("LEFT", (int)acceptCoord[0], (int)acceptCoord[1], 1, 5);
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
            Thread.Sleep(7000);

            var t3 = Task.Run(() => MOVE(token));
            await Task.WhenAny(new[] { t3 });
        }

        private async Task MOVE(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is Running..."));

                        object move1 = au3.PixelSearch(0, 0, 1920, 1080, 0x2A3540, 100);

                        if (move1.ToString() != "1")
                        {
                            au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 2);
                            au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 2);
                            Thread.Sleep(1000);
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
                        await Task.Delay(100, token);

                        object move1 = au3.PixelSearch(0, 0, 1920, 1080, 0x2A3540, 100);

                        if (move1.ToString() != "1")
                        {
                            au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 2);
                            au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 2);
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
                        await Task.Delay(100, token);

                        if (chBoxBerserker.Checked == true && _Berserker == true)
                        {
                            Layout_Keyboard.simulateHold(VirtualKeyCode.VK_Z, 50);

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
            var t4 = Task.Run(() => STARTFIGHT(token));
            await Task.WhenAny(new[] { t4 });
        }

        private async Task STARTFIGHT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                _Shadowhunter = true;
                _Paladin = true;
                _Berserker = true;

                for (int i = 0; i < int.Parse(txtDungeon.Text) / 3; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object fight1 = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                        if (fight1.ToString() != "1")
                        {
                            object[] fight1Coord = (object[])fight1;
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 80, 3, 5);
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
                        await Task.Delay(100, token);
                        object fight2 = au3.PixelSearch(750, 400, 1169, 697, 0x955921, 1);
                        if (fight2.ToString() != "1")
                        {
                            object[] fight2Coord = (object[])fight2;
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 80, 3, 5);
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

                    ///////////////SPELLS
                    ///
                    try
                    {
                        if (chBoxY.Checked == true && _Shadowhunter == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object d = au3.PixelSearch(948, 969, 968, 979, 0xBC08F0, 10);

                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[])d;

                                Layout_Keyboard.simulateHold(VirtualKeyCode.VK_Y, 50);
                                _Shadowhunter = false;
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

                    try
                    {
                        if (chBoxPaladin.Checked == true && _Paladin == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object d = au3.PixelSearch(892, 1027, 934, 1060, 0x75D6FF, 10);

                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[])d;
                                Layout_Keyboard.simulateHold(VirtualKeyCode.VK_Y, 50);
                                _Paladin = false;
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

                    try

                    {
                        if (_D == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object ds = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                            if (ds.ToString() != "1")
                            {
                                object[] dsCoord = (object[])ds;
                                Layout_Keyboard.simulateHold(VirtualKeyCode.VK_D, int.Parse(txD.Text) / 10);

                                _D = false;
                                D_Cooldown();
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

                    try
                    {
                        if (_A == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object a = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);
                            if (a.ToString() != "1")
                            {
                                object[] aCoord = (object[])a;

                                Layout_Keyboard.simulateHold(currentLayout.A, int.Parse(txA.Text) / 10);

                                _A = false;
                                A_Cooldown();
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
                    //////////POTION
                    ///
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object health = au3.PixelSearch(633, 962, 820, 970, 0x050405, 20);

                        if (health.ToString() != "1")
                        {
                            object[] healthCoord = (object[])health;
                            au3.Send("{" + txtHeal.Text + "}");
                            au3.Send("{" + txtHeal.Text + "}");
                            au3.Send("{" + txtHeal.Text + "}");
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
                        await Task.Delay(100, token);
                        object healthi = au3.PixelSearch(633, 962, 680, 970, 0x050405, 20);

                        if (healthi.ToString() != "1")
                        {
                            object[] healthiCoord = (object[])healthi;
                            au3.Send(txtInstant.Text);
                            au3.Send(txtInstant.Text);
                            au3.Send(txtInstant.Text);
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
                    /////////POTION ENDE
                    try
                    {
                        if (_S == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object s = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                            if (s.ToString() != "1")
                            {
                                object[] sCoord = (object[])s;

                                Layout_Keyboard.simulateHold(currentLayout.S, int.Parse(txS.Text) / 10);

                                _S = false;
                                S_Cooldown();
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

                    try
                    {
                        if (_F == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object f = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                            if (f.ToString() != "1")
                            {
                                object[] fCoord = (object[])f;

                                Layout_Keyboard.simulateHold(currentLayout.F, int.Parse(txF.Text) / 10);

                                _F = false;
                                F_Cooldown();
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

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object fight11 = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                        if (fight11.ToString() != "1")
                        {
                            object[] fight11Coord = (object[])fight11;
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 80, 3, 5);
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
                        await Task.Delay(100, token);
                        object fight22 = au3.PixelSearch(750, 400, 1169, 697, 0x955921, 1);
                        if (fight22.ToString() != "1")
                        {
                            object[] fight22Coord = (object[])fight22;
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 80, 3, 5);
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
                        if (_E == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object e = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                            if (e.ToString() != "1")
                            {
                                object[] eCoord = (object[])e;

                                Layout_Keyboard.simulateHold(currentLayout.E, int.Parse(txE.Text) / 10);

                                _E = false;
                                E_Cooldown();
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

                    try
                    {
                        if (_Q == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object q = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                            if (q.ToString() != "1")
                            {
                                object[] qCoord = (object[])q;

                                Layout_Keyboard.simulateHold(currentLayout.Q, int.Parse(txQ.Text) / 10);

                                _Q = false;
                                Q_Cooldown();
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

                    try
                    {
                        if (_W == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object w = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                            if (w.ToString() != "1")
                            {
                                object[] wCoord = (object[])w;

                                Layout_Keyboard.simulateHold(currentLayout.W, int.Parse(txW.Text) / 10);

                                _W = false;
                                W_Cooldown();
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

                    try
                    {
                        if (_R == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object r = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                            if (r.ToString() != "1")
                            {
                                object[] rCoord = (object[])r;

                                Layout_Keyboard.simulateHold(currentLayout.R, int.Parse(txR.Text) / 10);

                                _R = false;
                                R_Cooldown();
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

                    try
                    {
                        if (chBoxY.Checked == true && _Shadowhunter == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object d = au3.PixelSearch(950, 1048, 972, 1056, 0xB30A78, 10);

                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[])d;
                                Layout_Keyboard.simulateHold(VirtualKeyCode.VK_Y, 50);

                                _Shadowhunter = false;
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

                    try
                    {
                        if (chBoxPaladin.Checked == true && _Paladin == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object d = au3.PixelSearch(892, 1027, 934, 1060, 0x75D6FF, 10);

                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[])d;
                                Layout_Keyboard.simulateHold(VirtualKeyCode.VK_Y, 50);

                                _Paladin = false;
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

                    //////////POTION
                    ///
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object health = au3.PixelSearch(633, 962, 820, 970, 0x050405, 20);

                        if (health.ToString() != "1")
                        {
                            object[] healthCoord = (object[])health;
                            au3.Send("{" + txtHeal.Text + "}");
                            au3.Send("{" + txtHeal.Text + "}");
                            au3.Send("{" + txtHeal.Text + "}");
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
                        await Task.Delay(100, token);
                        object healthi = au3.PixelSearch(633, 962, 680, 970, 0x050405, 20);

                        if (healthi.ToString() != "1")
                        {
                            object[] healthiCoord = (object[])healthi;
                            au3.Send(txtInstant.Text);
                            au3.Send(txtInstant.Text);
                            au3.Send(txtInstant.Text);
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

            if (chBoxActivateF2.Checked)
            {
                var t7 = Task.Run(() => SEARCHPORTAL(token));
                await Task.WhenAny(new[] { t7 });
            }
            else
            if (!chBoxActivateF2.Checked)
            {
                var t12 = Task.Run(() => Exit1(token));
                await Task.WhenAny(new[] { t12 });
            }
        }

        private async Task SEARCHPORTAL(CancellationToken token)
        {
 
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
               
                _Shadowhunter = true;
                _Paladin = true;
                _Berserker = true;
                for (int i = 0; i <= 10; i++)
                {
                    try
                    {
                        au3.Send("{G}");
                        au3.Send("{G}");

                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Search Portal..."));
                        // Tunable variables
                        float threshold = 0.7f; // set this higher for fewer false positives and lower for fewer false negatives
                        var enemyTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/portalenter1.png"); // icon of the enemy
                        var enemyMask =
                            new Image<Bgr, byte>(resourceFolder + "/portalentermask1.png"); // make white what the important parts are, other parts should be black
                                                                                            //var screenCapture = new Image<Bgr, byte>("D:/Projects/bot-enemy-detection/EnemyDetection/screen.png");
                        Point myPosition = new Point(150, 128);
                        Point screenResolution = new Point(1920, 1080);

                        // Main program loop
                        var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                        var screenPrinter = new PrintScreen();

                        screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                        var screenCapture = new Image<Bgr, byte>("screen.png");
                        var enemy = enemyDetector.GetClosestEnemy(screenCapture);
                        if (enemy.HasValue)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            CvInvoke.Rectangle(screenCapture,
                                new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                new MCvScalar(255));
                            var inputSimulator = new InputSimulator();
                            double x1 = 963f / myPosition.X;
                            double y1 = 551f / myPosition.Y;
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            var x2 = x1 * enemy.Value.X;
                            var y2 = y1 * enemy.Value.Y;
                            if (x2 <= 963)
                                x2 = x2 * 0.68f;
                            else
                                x2 = x2 * 1.38f;
                            if (y2 <= 551)
                                y2 = y2 * 0.68;
                            else
                                y2 = y2 * 1.38;
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                            inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);

                            var sim = new InputSimulator();

                            Layout_Keyboard.simulateHold(VirtualKeyCode.VK_G, 50);
                            au3.Send("{G}");
                            if (txtLEFT.Text == "LEFT")
                            {
                                inputSimulator.Mouse.LeftButtonClick();
                            }
                            else
                            {
                                inputSimulator.Mouse.RightButtonClick();
                            }
                            for (int t = 0; t < 100; t++)
                            {
                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_G);
                                Thread.Sleep(10);
                            }

                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_G);
                            au3.Send("{G}");
                            if (txtLEFT.Text == "LEFT")
                            {
                                inputSimulator.Mouse.LeftButtonClick();
                            }
                            else
                            {
                                inputSimulator.Mouse.RightButtonClick();
                            }
                            for (int t = 0; t < 100; t++)
                            {
                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_G);
                                Thread.Sleep(10);
                            }

                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_G);
                            au3.Send("{G}");
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
                    await Task.Delay(100, token);
                    Random random = new Random();
                    var sleepTime = random.Next(300, 500);
                    Thread.Sleep(sleepTime);
                    au3.Send("{G}");
                    au3.Send("{G}");
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

            var t12 = Task.Run(() => SearchBoss(token));
            await Task.WhenAny(new[] { t12 });
        }

        private async Task SearchBoss(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                _Shadowhunter = true;
                _Paladin = true;
                _Berserker = true;

                for (int i = 0; i < int.Parse(txtDungeon2search.Text); i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        if (searchbossSequence == 1)
                        {
                            au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 2);
                            au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 2);
                            searchbossSequence++;
                        }
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Search enemy..."));

                        float threshold = 0.8f;
                        var enemyTemplate =
                        new Image<Bgr, byte>(resourceFolder + "/enemy.png");
                        var enemyMask =
                        new Image<Bgr, byte>(resourceFolder + "/mask.png");
                        var BossTemplate =
                        new Image<Bgr, byte>(resourceFolder + "/boss.png");
                        var BossMask =
                        new Image<Bgr, byte>(resourceFolder + "/bossmask.png");
                        var mobTemplate =
                        new Image<Bgr, byte>(resourceFolder + "/mob.png");
                        var mobMask =
                        new Image<Bgr, byte>(resourceFolder + "/mobmask.png");
                        var portalTemplate =
                        new Image<Bgr, byte>(resourceFolder + "/portalenter1.png");
                        var portalMask =
                        new Image<Bgr, byte>(resourceFolder + "/portalentermask1.png");

                        Point myPosition = new Point(150, 128);
                        Point screenResolution = new Point(1920, 1080);

                        var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                        var BossDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                        var mobDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                        var portalDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                        var screenPrinter = new PrintScreen();

                        screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                        var screenCapture = new Image<Bgr, byte>("screen.png");
                        var enemy = enemyDetector.GetClosestEnemy(screenCapture);
                        var Boss = BossDetector.GetClosestEnemy(screenCapture);
                        var mob = mobDetector.GetClosestEnemy(screenCapture);
                        var portal = portalDetector.GetClosestEnemy(screenCapture);

                        if (portal.HasValue)
                        {
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Portal found..."));
                            var t16 = Task.Run(() => PortalFloor2(token));
                            await Task.WhenAny(new[] { t16 });
                        }
                        else
                        {
                            if (Boss.HasValue)
                            {
                                CvInvoke.Rectangle(screenCapture,
                                    new Rectangle(new Point(Boss.Value.X, Boss.Value.Y), BossTemplate.Size),
                                    new MCvScalar(255));
                                var inputSimulator = new InputSimulator();
                                double x1 = 963f / myPosition.X;
                                double y1 = 551f / myPosition.Y;

                                var x2 = x1 * Boss.Value.X;
                                var y2 = y1 * Boss.Value.Y;
                                if (x2 <= 963)
                                    x2 = x2 * 0.9f;
                                else
                                    x2 = x2 * 1.1f;
                                if (y2 <= 551)
                                    y2 = y2 * 0.9;
                                else
                                    y2 = y2 * 1.1;
                                var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                if (txtLEFT.Text == "LEFT")
                                {
                                    inputSimulator.Mouse.LeftButtonClick();
                                }
                                else
                                {
                                    inputSimulator.Mouse.RightButtonClick();
                                }
                            }
                            else
                            {
                                if (enemy.HasValue)
                                {
                                    CvInvoke.Rectangle(screenCapture,
                                        new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                        new MCvScalar(255));
                                    var inputSimulator = new InputSimulator();
                                    double x1 = 963f / myPosition.X;
                                    double y1 = 551f / myPosition.Y;

                                    var x2 = x1 * enemy.Value.X;
                                    var y2 = y1 * enemy.Value.Y;
                                    if (x2 <= 963)
                                        x2 = x2 * 0.9f;
                                    else
                                        x2 = x2 * 1.1f;
                                    if (y2 <= 551)
                                        y2 = y2 * 0.9;
                                    else
                                        y2 = y2 * 1.1;
                                    var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                    inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                    if (txtLEFT.Text == "LEFT")
                                    {
                                        inputSimulator.Mouse.LeftButtonClick();
                                    }
                                    else
                                    {
                                        inputSimulator.Mouse.RightButtonClick();
                                    }
                                }
                                else
                                {
                                    if (mob.HasValue)
                                    {
                                        CvInvoke.Rectangle(screenCapture,
                                            new Rectangle(new Point(mob.Value.X, mob.Value.Y), mobTemplate.Size),
                                            new MCvScalar(255));
                                        var inputSimulator = new InputSimulator();
                                        double x1 = 963f / myPosition.X;
                                        double y1 = 551f / myPosition.Y;

                                        var x2 = x1 * mob.Value.X;
                                        var y2 = y1 * mob.Value.Y;
                                        if (x2 <= 963)
                                            x2 = x2 * 0.9f;
                                        else
                                            x2 = x2 * 1.1f;
                                        if (y2 <= 551)
                                            y2 = y2 * 0.9;
                                        else
                                            y2 = y2 * 1.1;
                                        var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                        inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                        if (txtLEFT.Text == "LEFT")
                                        {
                                            inputSimulator.Mouse.LeftButtonClick();
                                        }
                                        else
                                        {
                                            inputSimulator.Mouse.RightButtonClick();
                                        }
                                    }
                                }
                            }
                        }

                        Random random = new Random();
                        var sleepTime = random.Next(150, 255);
                        Thread.Sleep(sleepTime);
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
            var t12 = Task.Run(() => FIGHT(token));
            await Task.WhenAny(new[] { t12 });
        }

        private async Task FIGHT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                _Shadowhunter = true;
                _Paladin = true;
                _Berserker = true;

                for (int i = 0; i < (int.Parse(txtDungeon2.Text) / 3) / 2; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);

                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                        object fight1 = au3.PixelSearch(320, 180, 1523, 911, 0xAD901C, 3);

                        if (fight1.ToString() != "1")
                        {
                            object[] fight1Coord = (object[])fight1;
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 80, 3, 5);
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
                        await Task.Delay(100, token);
                        object fight1 = au3.PixelSearch(650, 300, 1269, 797, 0xDD2C02, 10);

                        if (fight1.ToString() != "1")
                        {
                            object[] fight1Coord = (object[])fight1;
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 80, 3, 5);
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
                        await Task.Delay(100, token);
                        object fight2 = au3.PixelSearch(650, 300, 1269, 797, 0x955921, 1);
                        if (fight2.ToString() != "1")
                        {
                            object[] fight2Coord = (object[])fight2;
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 80, 3, 5);
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

                    ///////////////SPELLS
                    ///
                    try
                    {
                        if (chBoxY.Checked == true && _Shadowhunter == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object d = au3.PixelSearch(948, 969, 968, 979, 0xBC08F0, 10);

                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[])d;
                                Layout_Keyboard.simulateHold(VirtualKeyCode.VK_Y, 50);

                                _Shadowhunter = false;
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

                    try
                    {
                        if (chBoxPaladin.Checked == true && _Paladin == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object d = au3.PixelSearch(892, 1027, 934, 1060, 0x75D6FF, 10);

                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[])d;
                                Layout_Keyboard.simulateHold(VirtualKeyCode.VK_Y, 50);

                                _Paladin = false;
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

                    try
                    {
                        if (_D == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object ds = au3.PixelSearch(650, 300, 1269, 797, 0xDD2C02, 10);

                            if (ds.ToString() != "1")
                            {
                                object[] dsCoord = (object[])ds;

                                Layout_Keyboard.simulateHold(currentLayout.D, int.Parse(txD.Text) / 10);

                                _D = false;
                                D_Cooldown();
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
                    try
                    {
                        if (_D == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object ds = au3.PixelSearch(320, 180, 1523, 911, 0xAD901C, 3);

                            if (ds.ToString() != "1")
                            {
                                object[] dsCoord = (object[])ds;

                                Layout_Keyboard.simulateHold(currentLayout.D, int.Parse(txD.Text) / 10);

                                _D = false;
                                D_Cooldown();
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
                    try
                    {
                        if (_A == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object a = au3.PixelSearch(650, 300, 1269, 797, 0xDD2C02, 10);
                            if (a.ToString() != "1")
                            {
                                object[] aCoord = (object[])a;

                                Layout_Keyboard.simulateHold(currentLayout.A, int.Parse(txA.Text) / 10);

                                _A = false;
                                A_Cooldown();
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
                    try
                    {
                        if (_A == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object a = au3.PixelSearch(320, 180, 1523, 911, 0xAD901C, 3);
                            if (a.ToString() != "1")
                            {
                                object[] aCoord = (object[])a;

                                Layout_Keyboard.simulateHold(currentLayout.A, int.Parse(txA.Text) / 10);

                                _A = false;
                                A_Cooldown();
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
                    //////////POTION
                    ///
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object health = au3.PixelSearch(633, 962, 820, 970, 0x050405, 20);

                        if (health.ToString() != "1")
                        {
                            object[] healthCoord = (object[])health;
                            au3.Send("{" + txtHeal.Text + "}");
                            au3.Send("{" + txtHeal.Text + "}");
                            au3.Send("{" + txtHeal.Text + "}");
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
                        await Task.Delay(100, token);
                        object healthi = au3.PixelSearch(633, 962, 680, 970, 0x050405, 20);

                        if (healthi.ToString() != "1")
                        {
                            object[] healthiCoord = (object[])healthi;
                            au3.Send(txtInstant.Text);
                            au3.Send(txtInstant.Text);
                            au3.Send(txtInstant.Text);
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
                    /////////POTION ENDE
                    try
                    {
                        if (_S == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object s = au3.PixelSearch(650, 300, 1269, 797, 0xDD2C02, 10);

                            if (s.ToString() != "1")
                            {
                                object[] sCoord = (object[])s;

                                Layout_Keyboard.simulateHold(currentLayout.S, int.Parse(txS.Text) / 10);

                                _S = false;
                                S_Cooldown();
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
                    try
                    {
                        if (_S == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object s = au3.PixelSearch(320, 180, 1523, 911, 0xAD901C, 3);

                            if (s.ToString() != "1")
                            {
                                object[] sCoord = (object[])s;

                                Layout_Keyboard.simulateHold(currentLayout.S, int.Parse(txS.Text) / 10);

                                _S = false;
                                S_Cooldown();
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

                    try
                    {
                        if (_F == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object f = au3.PixelSearch(650, 300, 1269, 797, 0xDD2C02, 10);

                            if (f.ToString() != "1")
                            {
                                object[] fCoord = (object[])f;

                                Layout_Keyboard.simulateHold(currentLayout.F, int.Parse(txF.Text) / 10);

                                _F = false;
                                F_Cooldown();
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
                    try
                    {
                        if (_F == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object f = au3.PixelSearch(320, 180, 1523, 911, 0xAD901C, 3);

                            if (f.ToString() != "1")
                            {
                                object[] fCoord = (object[])f;

                                Layout_Keyboard.simulateHold(currentLayout.F, int.Parse(txF.Text) / 10);

                                _F = false;
                                F_Cooldown();
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

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object fight11 = au3.PixelSearch(650, 300, 1269, 797, 0xDD2C02, 10);

                        if (fight11.ToString() != "1")
                        {
                            object[] fight11Coord = (object[])fight11;
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 80, 3, 5);
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
                        await Task.Delay(100, token);
                        object fight22 = au3.PixelSearch(650, 300, 1269, 797, 0x955921, 1);
                        if (fight22.ToString() != "1")
                        {
                            object[] fight22Coord = (object[])fight22;
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 80, 3, 5);
                            au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 80, 3, 5);
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
                        if (_E == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object e = au3.PixelSearch(650, 300, 1269, 797, 0xDD2C02, 10);

                            if (e.ToString() != "1")
                            {
                                object[] eCoord = (object[])e;

                                Layout_Keyboard.simulateHold(currentLayout.E, int.Parse(txE.Text) / 10);

                                _E = false;
                                E_Cooldown();
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
                    try
                    {
                        if (_E == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object e = au3.PixelSearch(320, 180, 1523, 911, 0xAD901C, 3);

                            if (e.ToString() != "1")
                            {
                                object[] eCoord = (object[])e;

                                Layout_Keyboard.simulateHold(currentLayout.E, int.Parse(txE.Text) / 10);

                                _E = false;
                                E_Cooldown();
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

                    try
                    {
                        if (_Q == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object q = au3.PixelSearch(650, 300, 1269, 797, 0xDD2C02, 10);
                            if (q.ToString() != "1")
                            {
                                object[] qCoord = (object[])q;

                                Layout_Keyboard.simulateHold(currentLayout.Q, int.Parse(txQ.Text) / 10);

                                _Q = false;
                                Q_Cooldown();
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
                    try
                    {
                        if (_Q == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object q = au3.PixelSearch(320, 180, 1523, 911, 0xAD901C, 3);

                            if (q.ToString() != "1")
                            {
                                object[] qCoord = (object[])q;

                                Layout_Keyboard.simulateHold(currentLayout.Q, int.Parse(txQ.Text) / 10);


                                _Q = false;
                                Q_Cooldown();
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

                    try
                    {
                        if (_W == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object w = au3.PixelSearch(650, 300, 1269, 797, 0xDD2C02, 10);

                            if (w.ToString() != "1")
                            {
                                object[] wCoord = (object[])w;

                                Layout_Keyboard.simulateHold(currentLayout.W, int.Parse(txW.Text) / 10);

                                _W = false;
                                W_Cooldown();
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
                    try
                    {
                        if (_W == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object w = au3.PixelSearch(320, 180, 1523, 911, 0xAD901C, 3);

                            if (w.ToString() != "1")
                            {
                                object[] wCoord = (object[])w;

                                Layout_Keyboard.simulateHold(currentLayout.W, int.Parse(txW.Text) / 10);

                                _W = false;
                                W_Cooldown();
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

                    try
                    {
                        if (_R == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object r = au3.PixelSearch(650, 300, 1269, 797, 0xDD2C02, 10);

                            if (r.ToString() != "1")
                            {
                                object[] rCoord = (object[])r;

                                Layout_Keyboard.simulateHold(currentLayout.R, int.Parse(txR.Text) / 10);

                                _R = false;
                                R_Cooldown();
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
                    try
                    {
                        if (_R == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object r = au3.PixelSearch(320, 180, 1523, 911, 0xAD901C, 3);

                            if (r.ToString() != "1")
                            {
                                object[] rCoord = (object[])r;

                                Layout_Keyboard.simulateHold(currentLayout.R, int.Parse(txR.Text) / 10);

                               
                                _R = false;
                                R_Cooldown();
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

                    try
                    {
                        if (chBoxY.Checked == true && _Shadowhunter == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object d = au3.PixelSearch(948, 969, 968, 979, 0xBC08F0, 10);

                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[])d;
                                Layout_Keyboard.simulateHold(VirtualKeyCode.VK_Y, 50);

                                _Shadowhunter = false;
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

                    try
                    {
                        if (chBoxPaladin.Checked == true && _Paladin == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            object d = au3.PixelSearch(892, 1027, 934, 1060, 0x75D6FF, 10);

                            if (d.ToString() != "1")
                            {
                                object[] dCoord = (object[])d;
                                Layout_Keyboard.simulateHold(VirtualKeyCode.VK_Y, 50);

                                _Paladin = false;
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

                    //////////POTION
                    ///
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object health = au3.PixelSearch(633, 962, 820, 970, 0x050405, 20);

                        if (health.ToString() != "1")
                        {
                            object[] healthCoord = (object[])health;
                            au3.Send("{" + txtHeal.Text + "}");
                            au3.Send("{" + txtHeal.Text + "}");
                            au3.Send("{" + txtHeal.Text + "}");
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
                        await Task.Delay(100, token);
                        object healthi = au3.PixelSearch(633, 962, 680, 970, 0x050405, 20);

                        if (healthi.ToString() != "1")
                        {
                            object[] healthiCoord = (object[])healthi;
                            au3.Send(txtInstant.Text);
                            au3.Send(txtInstant.Text);
                            au3.Send(txtInstant.Text);
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
            fightSequence++;
            if(fightSequence == 6)
            {
                var t12 = Task.Run(() => Exit1(token));
                await Task.WhenAny(new[] { t12 });
            } else
            {
                var t13 = Task.Run(() => SearchBoss(token));
                await Task.WhenAny(new[] { t13 });
            }
            
        }

        private async Task PortalFloor2(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                _Shadowhunter = true;
                _Paladin = true;
                _Berserker = true;

                for (int i = 0; i <= 15; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Walk to Portal..."));

                        float threshold = 0.7f;

                        var portalTemplate =
                        new Image<Bgr, byte>(resourceFolder + "/portalenter1.png");
                        var portalMask =
                        new Image<Bgr, byte>(resourceFolder + "/portalentermask1.png");
                        Point myPosition = new Point(150, 128);
                        Point screenResolution = new Point(1920, 1080);

                        var portalDetector = new EnemyDetector(portalTemplate, portalMask, threshold);
                        var screenPrinter = new PrintScreen();

                        screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                        var screenCapture = new Image<Bgr, byte>("screen.png");
                        var portal = portalDetector.GetClosestEnemy(screenCapture);

                        if (portal.HasValue)
                        {
                            CvInvoke.Rectangle(screenCapture,
                                new Rectangle(new Point(portal.Value.X, portal.Value.Y), portalTemplate.Size),
                                new MCvScalar(255));
                            var inputSimulator = new InputSimulator();
                            double x1 = 963f / myPosition.X;
                            double y1 = 551f / myPosition.Y;

                            var x2 = x1 * portal.Value.X;
                            var y2 = y1 * portal.Value.Y;
                            if (x2 <= 963)
                                x2 = x2 * 0.9f;
                            else
                                x2 = x2 * 1.1f;
                            if (y2 <= 551)
                                y2 = y2 * 0.9;
                            else
                                y2 = y2 * 1.1;
                            var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                            inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                            if (txtLEFT.Text == "LEFT")
                            {
                                inputSimulator.Mouse.LeftButtonClick();
                            }
                            else
                            {
                                inputSimulator.Mouse.RightButtonClick();
                            }
                        }
                        else
                        {
                        }

                        Random random = new Random();
                        var sleepTime = random.Next(150, 255);
                        Thread.Sleep(sleepTime);
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
            var t12 = Task.Run(() => FIGHT(token));
            await Task.WhenAny(new[] { t12 });
        }

        private async Task Exit1(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                _Shadowhunter = true;
                _Paladin = true;
                _Berserker = true;
                for (int i = 0; i < 1; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object walk = au3.PixelSearch(77, 270, 190, 298, 0x29343F, 5);

                        if (walk.ToString() != "1")
                        {
                            object[] walkCoord = (object[])walk;
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
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
                        await Task.Delay(100, token);
                        object walk = au3.PixelSearch(77, 270, 190, 298, 0x29343F, 5);

                        if (walk.ToString() != "1")
                        {
                            object[] walkCoord = (object[])walk;
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
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
            var t6 = Task.Run(() => Exitaccept(token));
            await Task.WhenAny(new[] { t6 });
        }

        private async Task Exitaccept(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                for (int i = 0; i < 1; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object walk = au3.PixelSearch(560, 260, 1382, 817, 0x21BD08, 1);

                        if (walk.ToString() != "1")
                        {
                            object[] walkCoord = (object[])walk;
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                        }
                    }
                    catch { }
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        object walk = au3.PixelSearch(560, 260, 1382, 817, 0x21BD08, 1);

                        if (walk.ToString() != "1")
                        {
                            object[] walkCoord = (object[])walk;
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
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
                        await Task.Delay(100, token);
                        object walk = au3.PixelSearch(560, 260, 1382, 817, 0x21BD08, 1);

                        if (walk.ToString() != "1")
                        {
                            object[] walkCoord = (object[])walk;
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                            au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
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

            Thread.Sleep(2000);
            if (_REPAIR == true)
            {
                Thread.Sleep(2000);
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
                await Task.Delay(2000);
                var t9 = Task.Run(() => RESTART(token));
                await Task.WhenAny(new[] { t9 });
            }
        }

        private async Task LOGOUT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                for (int i = 0; i < 1; i++)
                {
                    try
                    {
                        Thread.Sleep(20000);
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "LOGOUT Process starts..."));
                        au3.Send("{ESCAPE}");
                        Thread.Sleep(2000);
                        au3.MouseClick("LEFT", 1238, 728, 1, 5);
                        Thread.Sleep(2000);
                        au3.MouseClick("LEFT", 906, 575, 1, 5);
                        Thread.Sleep(1000);
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
                await Task.Delay(100, token);

                for (int i = 0; i < 1; i++)
                {
                    try
                    {
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Auto-Repair starts in 20 seconds..."));
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        await Task.Delay(25000);
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
                        await Task.Delay(100, token);

                        au3.MouseClick("LEFT", 1741, 1040, 1, 5);
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
                    await Task.Delay(2000);
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);

                        await Task.Delay(1500);
                        au3.MouseClick("LEFT", 1684, 823, 1, 5);
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
                        await Task.Delay(100, token);

                        await Task.Delay(1500);
                        au3.MouseClick("LEFT", 1256, 693, 1, 5);
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
                        await Task.Delay(100, token);
                        await Task.Delay(1500);
                        au3.MouseClick("LEFT", 1085, 429, 1, 5);
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        await Task.Delay(1500);
                        au3.Send("{ESCAPE}");
                        await Task.Delay(1000);
                        au3.Send("{ESCAPE}");

                        _REPAIR = false;
                        _REPAIR = false;
                        REPAIRTIMER();
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
            await Task.Delay(2000);
            var t10 = Task.Run(() => RESTART2(token));
            await Task.WhenAny(new[] { t10 });
        }

        private async Task RESTART(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                for (int i = 0; i < 1; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot Paused: Resume in " + int.Parse(txtRestartTimer.Text) + " seconds."));
                        Thread.Sleep(int.Parse(txtRestartTimer.Text) * 1000);
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

            Thread.Sleep(2000);
            var t1 = Task.Run(() => START(token));
            await Task.WhenAny(new[] { t1 });
        }

        private async Task RESTART2(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                for (int i = 0; i < 1; i++)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Auto-Repair done!"));
                        Thread.Sleep(4000);
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

        // ZUKUNFT //

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
                Q = VirtualKeyCode.VK_Q,
                W = VirtualKeyCode.VK_W,
                E = VirtualKeyCode.VK_E,
                R = VirtualKeyCode.VK_R,
                A = VirtualKeyCode.VK_A,
                S = VirtualKeyCode.VK_S,
                D = VirtualKeyCode.VK_D,
                F = VirtualKeyCode.VK_F,
            };
            LAYOUT.Add(QWERTZ);

            Layout_Keyboard QWERTY = new Layout_Keyboard
            {
                LAYOUTS = "QWERTY",
                Q = VirtualKeyCode.VK_Q,
                W = VirtualKeyCode.VK_W,
                E = VirtualKeyCode.VK_E,
                R = VirtualKeyCode.VK_R,
                A = VirtualKeyCode.VK_A,
                S = VirtualKeyCode.VK_S,
                D = VirtualKeyCode.VK_D,
                F = VirtualKeyCode.VK_F,
            };
            LAYOUT.Add(QWERTY);

            Layout_Keyboard AZERTY = new Layout_Keyboard
            {
                LAYOUTS = "AZERTY",
                Q = VirtualKeyCode.VK_A,
                W = VirtualKeyCode.VK_Z,
                E = VirtualKeyCode.VK_E,
                R = VirtualKeyCode.VK_R,
                A = VirtualKeyCode.VK_Q,
                S = VirtualKeyCode.VK_S,
                D = VirtualKeyCode.VK_D,
                F = VirtualKeyCode.VK_F
            };
            LAYOUT.Add(AZERTY);

            comboBox1.DataSource = LAYOUT;
            comboBox1.DisplayMember = "LAYOUTS";
            currentLayout = comboBox1.SelectedItem as Layout_Keyboard;
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            txtDungeon.Text = Properties.Settings.Default.dungeontimer;
            txtLEFT.Text = Properties.Settings.Default.left;
            txtRIGHT.Text = Properties.Settings.Default.right;
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
            txtInstant.Text = Properties.Settings.Default.instant;
            txtHeal.Text = Properties.Settings.Default.potion;
            checkBoxInstant.Checked = Properties.Settings.Default.chboxinstant;
            checkBoxHeal.Checked = Properties.Settings.Default.chboxheal;
            chBoxAutoRepair.Checked = Properties.Settings.Default.chBoxAutoRepair;
            txtRepair.Text = Properties.Settings.Default.autorepair;
            chBoxY.Checked = Properties.Settings.Default.chBoxShadowhunter;
            chBoxPaladin.Checked = Properties.Settings.Default.chboxPaladin;
            chBoxBerserker.Checked = Properties.Settings.Default.chBoxBerserker;
            txtRestartTimer.Text = Properties.Settings.Default.RestartTimer;
            chBoxSaveAll.Checked = Properties.Settings.Default.chBoxSaveAll;
            chBoxActivateF2.Checked = Properties.Settings.Default.chBoxActivateF2;
            txtDungeon2search.Text = Properties.Settings.Default.txtDungeon2search;
            txtDungeon2.Text = Properties.Settings.Default.txtDungeon2;
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
            if (checkBoxInstant.Checked)
            {
                txtInstant.ReadOnly = false;
            }
            else
            if (!checkBoxInstant.Checked)
            {
                txtInstant.ReadOnly = true;
                txtInstant.Text = "";
            }
        }

        private void checkBoxHeal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHeal.Checked)
            {
                txtHeal.ReadOnly = false;
            }
            else
            if (!checkBoxHeal.Checked)
            {
                txtHeal.ReadOnly = true;
                txtHeal.Text = "";
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
                Properties.Settings.Default.dungeontimer = "60";
                Properties.Settings.Default.instant = "";
                Properties.Settings.Default.potion = "";
                Properties.Settings.Default.chboxinstant = false;
                Properties.Settings.Default.chboxheal = false;
                Properties.Settings.Default.chBoxAutoRepair = false;
                Properties.Settings.Default.autorepair = "10";
                Properties.Settings.Default.chBoxShadowhunter = false;
                Properties.Settings.Default.chBoxBerserker = false;
                Properties.Settings.Default.chboxPaladin = false;
                Properties.Settings.Default.RestartTimer = "25";
                Properties.Settings.Default.chBoxSaveAll = false;
                Properties.Settings.Default.chBoxActivateF2 = false;
                Properties.Settings.Default.txtDungeon2 = "15";
                Properties.Settings.Default.txtDungeon2search = "9";
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

                Properties.Settings.Default.Save();

                txtDungeon.Text = Properties.Settings.Default.dungeontimer;
                txtInstant.Text = Properties.Settings.Default.instant;
                txtHeal.Text = Properties.Settings.Default.potion;
                checkBoxInstant.Checked = Properties.Settings.Default.chboxinstant;
                checkBoxHeal.Checked = Properties.Settings.Default.chboxheal;
                chBoxAutoRepair.Checked = Properties.Settings.Default.chBoxAutoRepair;
                txtRepair.Text = Properties.Settings.Default.autorepair;
                chBoxY.Checked = Properties.Settings.Default.chBoxShadowhunter;
                chBoxPaladin.Checked = Properties.Settings.Default.chboxPaladin;
                chBoxBerserker.Checked = Properties.Settings.Default.chBoxBerserker;
                txtRestartTimer.Text = Properties.Settings.Default.RestartTimer;
                chBoxSaveAll.Checked = Properties.Settings.Default.chBoxSaveAll;
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

                txQ.Text = Properties.Settings.Default.cQ;
                txW.Text = Properties.Settings.Default.cW;
                txE.Text = Properties.Settings.Default.cE;
                txR.Text = Properties.Settings.Default.cR;
                txA.Text = Properties.Settings.Default.cA;
                txS.Text = Properties.Settings.Default.cS;
                txD.Text = Properties.Settings.Default.cD;
                txF.Text = Properties.Settings.Default.cF;
            }
            catch { }
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            frmGuide Form = new frmGuide();
            Form.Show();
        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Layout_Keyboard currentLayout = comboBox1.SelectedItem as Layout_Keyboard;
            lbQ.Text = currentLayout.Q.ToString().Replace("VK_", "");
            lbW.Text = currentLayout.W.ToString().Replace("VK_", "");
            lbE.Text = currentLayout.E.ToString().Replace("VK_", "");
            lbR.Text = currentLayout.R.ToString().Replace("VK_", "");
            lbA.Text = currentLayout.A.ToString().Replace("VK_", "");
            lbS.Text = currentLayout.S.ToString().Replace("VK_", "");
            lbD.Text = currentLayout.D.ToString().Replace("VK_", "");
            lbF.Text = currentLayout.F.ToString().Replace("VK_", "");

            lb2Q.Text = currentLayout.Q.ToString().Replace("VK_", "");
            lb2W.Text = currentLayout.W.ToString().Replace("VK_", "");
            lb2E.Text = currentLayout.E.ToString().Replace("VK_", "");
            lb2R.Text = currentLayout.R.ToString().Replace("VK_", "");
            lb2A.Text = currentLayout.A.ToString().Replace("VK_", "");
            lb2S.Text = currentLayout.S.ToString().Replace("VK_", "");
            lb2D.Text = currentLayout.D.ToString().Replace("VK_", "");
            lb2F.Text = currentLayout.F.ToString().Replace("VK_", "");
        }

        public void Q_Cooldown()
        {
            timer = new System.Timers.Timer(int.Parse(txCoolQ.Text) * 2);

            timer.Elapsed += Q_CooldownEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void Q_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _Q = true;
        }

        public void W_Cooldown()
        {
            timer = new System.Timers.Timer(int.Parse(txCoolW.Text) * 2);

            timer.Elapsed += W_CooldownEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void W_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _W = true;
        }

        public void E_Cooldown()
        {
            timer = new System.Timers.Timer(int.Parse(txCoolE.Text) * 2);

            timer.Elapsed += E_CooldownEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void E_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _E = true;
        }

        public void R_Cooldown()
        {
            timer = new System.Timers.Timer(int.Parse(txCoolR.Text) * 2);

            timer.Elapsed += R_CooldownEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void R_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _R = true;
        }

        public void A_Cooldown()
        {
            timer = new System.Timers.Timer(int.Parse(txCoolA.Text) * 2);

            timer.Elapsed += A_CooldownEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void A_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _A = true;
        }

        public void S_Cooldown()
        {
            timer = new System.Timers.Timer(int.Parse(txCoolS.Text) * 2);

            timer.Elapsed += S_CooldownEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void S_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _S = true;
        }

        public void D_Cooldown()
        {
            timer = new System.Timers.Timer(int.Parse(txCoolD.Text) * 2);

            timer.Elapsed += D_CooldownEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void D_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _D = true;
        }

        public void F_Cooldown()
        {
            timer = new System.Timers.Timer(int.Parse(txCoolF.Text) * 2);

            timer.Elapsed += F_CooldownEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void F_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _F = true;
        }
    }
}