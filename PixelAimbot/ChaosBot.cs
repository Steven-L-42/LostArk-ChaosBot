using System;
using System.Windows.Forms;
using AutoItX3Lib;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Generic;
using WindowsInput;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing.Imaging;
using System.Linq;

namespace PixelAimbot
    {
    public partial class ChaosBot : Form
    {


        /// OPENCV START  /// OPENCV START  /// OPENCV START  /// OPENCV START


        (int, int) PixelToAbsolute(double x, double y, Point screenResolution)
        {
            int newX = (int)(x / screenResolution.X * 65535);
            int newY = (int)(y / screenResolution.Y * 65535);
            return (newX, newY);
        }



        /// <summary>
        /// Class that detects the enemies given the enemy template, mask and threshold.
        /// Has one public function GetClosestEnemy, which returns the point of the closest enemy on the minimap.
        /// </summary>
        class EnemyDetector
        {
            private Image<Bgr, byte> _enemyTemplate;
            private Image<Bgr, byte> _enemyMask;
            private float _threshold;
            private readonly Point _myPosition = new Point(150, 128);

            public EnemyDetector(Image<Bgr, byte> enemyTemplate,
                Image<Bgr, byte> enemyMask, float threshold)
            {
                this._enemyMask = enemyMask;
                this._enemyTemplate = enemyTemplate;
                this._threshold = threshold;
            }

            private List<Point> DetectEnemies(Image<Bgr, byte> screenCapture)
            {
                List<Point> enemies = new List<Point>();
                screenCapture.ROI = new Rectangle(1593, 40, 296, 255);
                var minimap = screenCapture.Copy();
                var res = new Mat();
                double minVal = 0, maxVal = 0;
                Point minPoint = new Point();
                Point maxPoint = new Point();
                CvInvoke.MatchTemplate(minimap, this._enemyTemplate, res, TemplateMatchingType.SqdiffNormed, this._enemyMask);

                int h = this._enemyTemplate.Size.Height;
                int w = this._enemyTemplate.Size.Width;

                while (1 - minVal > this._threshold)
                {
                    CvInvoke.MinMaxLoc(res, ref minVal, ref maxVal, ref minPoint, ref maxPoint);
                    if (1 - minVal > this._threshold)
                    {
                        var lowerLeft = new Point(minPoint.X - w / 4, minPoint.Y - h / 4);
                        var upperLeft = new Point(minPoint.X - w / 4, minPoint.Y + h / 4);
                        var lowerRight = new Point(minPoint.X + w / 4, minPoint.Y - h / 4);
                        var upperRight = new Point(minPoint.X + w / 4, minPoint.Y + h / 4);
                        var points = new Point[]
                        {
                                lowerLeft,
                                lowerRight,
                                upperRight,
                                upperLeft
                        };
                        var vector = new VectorOfPoint(points);

                        CvInvoke.FillConvexPoly(res, vector, new MCvScalar(255));
                        enemies.Add(minPoint);
                    }
                }

                return enemies;
            }

            private double Distance(Point enemy)
            {
                return Math.Sqrt((Math.Pow(enemy.X - _myPosition.X, 2) + Math.Pow(enemy.Y - _myPosition.Y, 2)));
            }
            public Point? GetClosestEnemy(Image<Bgr, byte> screenCapture)
            {
                var enemies = DetectEnemies(screenCapture);
                var enemyAndPosition = enemies.Select(x => (x, Distance(x)));
                if (enemyAndPosition.Any())
                {
                    double minDist = Double.MaxValue;
                    Point closestEnemy = default;
                    foreach (var (enemy, distance) in enemyAndPosition)
                    {
                        if (distance < minDist)
                        {
                            minDist = distance;
                            closestEnemy = enemy;
                        }
                    }

                    return closestEnemy;
                }
                else
                {
                    return null;
                }
            }

        }
  
        /// OPENCV STOP /// OPENCV STOP /// OPENCV STOP /// OPENCV STOP /// OPENCV STOP /// OPENCV STOP


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
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is Starting..."));
                        break;
                    case 2:
                        btnPause_Click(null, null);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "STOPPED"));

                        break;
                }
            }

            base.WndProc(ref m);
        }


        AutoItX3 au3 = new AutoItX3();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                        int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        static readonly IntPtr HWND_TOP = new IntPtr(0);

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        const UInt32 SWP_NOSIZE = 0x0001;

        const UInt32 SWP_NOMOVE = 0x0002;

        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

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

        public ChaosBot()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            // 3. Register HotKeys

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
        CancellationTokenSource cts = new CancellationTokenSource();
        void btnPause_Click(object sender, EventArgs e)
        {
            _start = false;
            cts.Cancel();
            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "STOPPED!"));
        
        }
        bool _start = false;
        bool _REPAIR = false;
        bool _Shadowhunter = true;
        bool _Berserker = true;
        bool _Paladin = true;
        bool _LOGOUT = false;

        private System.Timers.Timer timer;

        async void btnStart_Click(object sender, EventArgs e)
        {

            if (chBoxSaveAll.Checked == true)
            {
                Properties.Settings.Default.dungeontimer = txtDungeon.Text;
                Properties.Settings.Default.left = txtLEFT.Text;
                Properties.Settings.Default.right = txtRIGHT.Text;
                Properties.Settings.Default.q = txtQ.Text;
                Properties.Settings.Default.w = txtW.Text;
                Properties.Settings.Default.e = txtE.Text;
                Properties.Settings.Default.r = txtR.Text;
                Properties.Settings.Default.a = txtA.Text;
                Properties.Settings.Default.s = txtS.Text;
                Properties.Settings.Default.d = txtD.Text;
                Properties.Settings.Default.f = txtF.Text;
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

                Properties.Settings.Default.dungeontimer = "70";
                Properties.Settings.Default.left = "LEFT";
                Properties.Settings.Default.right = "RIGHT";
                Properties.Settings.Default.q = "Q";
                Properties.Settings.Default.w = "W";
                Properties.Settings.Default.e = "E";
                Properties.Settings.Default.r = "R";
                Properties.Settings.Default.a = "A";
                Properties.Settings.Default.s = "S";
                Properties.Settings.Default.d = "D";
                Properties.Settings.Default.f = "F";
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
                Properties.Settings.Default.txtDungeon2 = "21";
                Properties.Settings.Default.txtDungeon2search = "9";
                Properties.Settings.Default.Save();

            }
            _start = true;
            // await Task.Run(new Action(STARTKLICK));
            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is starting..."));
            if (chBoxAutoRepair.Checked == true)
            {

                REPAIRTIMER();
            }
            else
            {
                _REPAIR = false;
            }
            if (chBoxLOGOUT.Checked == true)
            {

                LOGOUTTIMER();
            }
            else
            {
                _LOGOUT = false;
            }

            try
            {

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
        private void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            _LOGOUT = true;
        }

        async Task STARTKLICK(CancellationToken token)
        {


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
            Thread.Sleep(2000);
    
            var t2 = Task.Run(() => START(token));
            await Task.WhenAny(new[] { t2 });
        }

        async Task START(CancellationToken token)
        {

            for (int i = 0; i < 10; i++)
            {


                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    object walk = au3.PixelSearch(917, 334, 1477, 746, 0xD9DAD9);

                    if (walk.ToString() != "1")
                    {

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
                        au3.MouseClick("LEFT", (int)acceptCoord[0], (int)acceptCoord[1], 1, 10);
                        au3.MouseClick("LEFT", (int)acceptCoord[0], (int)acceptCoord[1], 1, 10);
                        au3.MouseClick("LEFT", (int)acceptCoord[0], (int)acceptCoord[1], 1, 10);
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
            Thread.Sleep(7000);


            var t3 = Task.Run(() => MOVE(token));
            await Task.WhenAny(new[] { t3 });
        }

        async Task MOVE(CancellationToken token)
        {
            


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
                        au3.Send("{" + txtBerserker.Text + "}");
                        au3.Send("{" + txtBerserker.Text + "}");
                        Thread.Sleep(300);
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
            var t4 = Task.Run(() => FIGHT(token));
            await Task.WhenAny(new[] { t4 });

        }

        async Task FIGHT(CancellationToken token)
        {
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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);



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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
          

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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    object ds = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (ds.ToString() != "1")
                    {
                        object[] dsCoord = (object[])ds;
                        au3.Send("{" + txtD.Text + "DOWN}");
                        au3.Send("{" + txtD.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
             

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
                    object a = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);
                    if (a.ToString() != "1")
                    {
                        object[] aCoord = (object[])a;
                        au3.Send("{" + txtA.Text + "DOWN}");
                        au3.Send("{" + txtA.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
               


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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    object s = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (s.ToString() != "1")
                    {
                        object[] sCoord = (object[])s;
                        au3.Send("{" + txtS.Text + "DOWN}");
                        au3.Send("{" + txtS.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
       


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
                    object f = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (f.ToString() != "1")
                    {
                        object[] fCoord = (object[])f;
                        au3.Send("{" + txtF.Text + "DOWN}");
                        au3.Send("{" + txtF.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
             


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
        
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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
         
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
                    object e = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (e.ToString() != "1")
                    {
                        object[] eCoord = (object[])e;
                        au3.Send("{" + txtE.Text + "DOWN}");
                        au3.Send("{" + txtE.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
         


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
                    object q = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (q.ToString() != "1")
                    {
                        object[] qCoord = (object[])q;
                        au3.Send("{" + txtQ.Text + "DOWN}");
                        au3.Send("{" + txtQ.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
         

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
                    object w = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (w.ToString() != "1")
                    {
                        object[] wCoord = (object[])w;
                        au3.Send("{" + txtW.Text + "DOWN}");
                        au3.Send("{" + txtW.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
            
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
                    object r = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (r.ToString() != "1")
                    {
                        object[] rCoord = (object[])r;
                        au3.Send("{" + txtR.Text + "DOWN}");
                        au3.Send("{" + txtR.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
      


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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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


        async Task SEARCHPORTAL(CancellationToken token)
        {

            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;

            for (int i = 0; i < 20; i++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Search Portal..."));
                    // Tunable variables
                    float threshold = 0.7f; // set this higher for fewer false positives and lower for fewer false negatives
                    var enemyTemplate =
                        new Image<Bgr, byte>("portal.png"); // icon of the enemy
                    var enemyMask =
                        new Image<Bgr, byte>("portalmask.png"); // make white what the important parts are, other parts should be black
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
                        inputSimulator.Mouse.LeftButtonClick();
                        au3.Send("{G}");
                        au3.Send("{G}");
                        au3.Send("{G}");
                        au3.Send("{G}");
                    }
                    else
                    {
                        au3.Send("{G}");
                        au3.Send("{G}");
                        au3.MouseClick("" + txtLEFT.Text + "", 1574, 526, 1);
                        Thread.Sleep(1000);
                        au3.Send("{G}");
                        au3.Send("{G}");
                        au3.Send("{G}");
                        au3.Send("{G}");
                        au3.MouseClick("" + txtLEFT.Text + "", 42, 538, 1);
                        Thread.Sleep(1000);
                        au3.Send("{G}");
                        au3.Send("{G}");
                        au3.Send("{G}");
                        au3.Send("{G}");
                    }

                    Random random = new Random();
                    var sleepTime = random.Next(350, 555);
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
            Thread.Sleep(4000);
            var t12 = Task.Run(() => SearchBoss(token));
            await Task.WhenAny(new[] { t12 });
        }

        async Task SearchBoss(CancellationToken token)
        {
            au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 1);
         

            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;

            for (int i = 0; i < int.Parse(txtDungeon2search.Text); i++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Search enemy..."));
                    // Tunable variables
                    float threshold = 0.7f; // set this higher for fewer false positives and lower for fewer false negatives
                    var enemyTemplate =
                        new Image<Bgr, byte>("enemy.png"); // icon of the enemy
                    var enemyMask =
                        new Image<Bgr, byte>("mask.png"); // make white what the important parts are, other parts should be black
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
                            inputSimulator.Mouse.LeftButtonClick();
                        }
                        else
                        {
                        au3.MouseClick("" + txtLEFT.Text + "", 1574, 526, 1);
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
            var t12 = Task.Run(() => FIGHT2(token));
            await Task.WhenAny(new[] { t12 });
        }
        
        async Task FIGHT2(CancellationToken token)
        {
            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;

            for (int i = 0; i < int.Parse(txtDungeon2.Text) / 3; i++)
            {
                try
                {

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                    object fight1 = au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (fight1.ToString() != "1")
                    {
                        object[] fight1Coord = (object[])fight1;
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);



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
                    object fight1 = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                    if (fight1.ToString() != "1")
                    {
                        object[] fight1Coord = (object[])fight1;
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
           


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                 

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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    object ds = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (ds.ToString() != "1")
                    {
                        object[] dsCoord = (object[])ds;
                        au3.Send("{" + txtD.Text + "DOWN}");
                        au3.Send("{" + txtD.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
               


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
                    object a = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);
                    if (a.ToString() != "1")
                    {
                        object[] aCoord = (object[])a;
                        au3.Send("{" + txtA.Text + "DOWN}");
                        au3.Send("{" + txtA.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                   


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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    object s = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                    if (s.ToString() != "1")
                    {
                        object[] sCoord = (object[])s;
                        au3.Send("{" + txtS.Text + "DOWN}");
                        au3.Send("{" + txtS.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object f = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (f.ToString() != "1")
                    {
                        object[] fCoord = (object[])f;
                        au3.Send("{" + txtF.Text + "DOWN}");
                        au3.Send("{" + txtF.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                       
                       
                       


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                       
                       
                       
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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                       
                       
                       
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
                    object e = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (e.ToString() != "1")
                    {
                        object[] eCoord = (object[])e;
                        au3.Send("{" + txtE.Text + "DOWN}");
                        au3.Send("{" + txtE.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object q = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (q.ToString() != "1")
                    {
                        object[] qCoord = (object[])q;
                        au3.Send("{" + txtQ.Text + "DOWN}");
                        au3.Send("{" + txtQ.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                       
                       
                       

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
                    object w = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (w.ToString() != "1")
                    {
                        object[] wCoord = (object[])w;
                        au3.Send("{" + txtW.Text + "DOWN}");
                        au3.Send("{" + txtW.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                       
                       
                       
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
                    object r = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (r.ToString() != "1")
                    {
                        object[] rCoord = (object[])r;
                        au3.Send("{" + txtR.Text + "DOWN}");
                        au3.Send("{" + txtR.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                       
                       
                       


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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
            var t12 = Task.Run(() => SearchBoss2(token));
            await Task.WhenAny(new[] { t12 });
        }

        async Task SearchBoss2(CancellationToken token)
        {
            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;

            for (int i = 0; i < int.Parse(txtDungeon2search.Text); i++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Search enemy..."));

                    // Tunable variables
                    float threshold = 0.7f; // set this higher for fewer false positives and lower for fewer false negatives
                    var enemyTemplate =
                        new Image<Bgr, byte>("enemy.png"); // icon of the enemy
                    var enemyMask =
                        new Image<Bgr, byte>("mask.png"); // make white what the important parts are, other parts should be black
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
                        inputSimulator.Mouse.LeftButtonClick();
                        
                    }
                    else
                    {
                        au3.MouseClick("" + txtLEFT.Text + "", 42, 538, 1);
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
            var t12 = Task.Run(() => FIGHT3(token));
            await Task.WhenAny(new[] { t12 });
        }
        async Task FIGHT3(CancellationToken token)
        {
            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;

            for (int i = 0; i < int.Parse(txtDungeon2.Text) / 3; i++)
            {
                try
                {
                 
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    object fight1 = au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (fight1.ToString() != "1")
                    {
                        object[] fight1Coord = (object[])fight1;
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);



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
                    object fight1 = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                    if (fight1.ToString() != "1")
                    {
                        object[] fight1Coord = (object[])fight1;
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                       
                       
                       


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                       
                       
                       

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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    object ds = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (ds.ToString() != "1")
                    {
                        object[] dsCoord = (object[])ds;
                        au3.Send("{" + txtD.Text + "DOWN}");
                        au3.Send("{" + txtD.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object a = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);
                    if (a.ToString() != "1")
                    {
                        object[] aCoord = (object[])a;
                        au3.Send("{" + txtA.Text + "DOWN}");
                        au3.Send("{" + txtA.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    object s = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (s.ToString() != "1")
                    {
                        object[] sCoord = (object[])s;
                        au3.Send("{" + txtS.Text + "DOWN}");
                        au3.Send("{" + txtS.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object f = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (f.ToString() != "1")
                    {
                        object[] fCoord = (object[])f;
                        au3.Send("{" + txtF.Text + "DOWN}");
                        au3.Send("{" + txtF.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                       
                       
                       


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                       
                       
                       
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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                       
                       
                       
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
                    object e = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (e.ToString() != "1")
                    {
                        object[] eCoord = (object[])e;
                        au3.Send("{" + txtE.Text + "DOWN}");
                        au3.Send("{" + txtE.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object q = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (q.ToString() != "1")
                    {
                        object[] qCoord = (object[])q;
                        au3.Send("{" + txtQ.Text + "DOWN}");
                        au3.Send("{" + txtQ.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                       
                       
                       

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
                    object w = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (w.ToString() != "1")
                    {
                        object[] wCoord = (object[])w;
                        au3.Send("{" + txtW.Text + "DOWN}");
                        au3.Send("{" + txtW.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                       
                       
                       
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
                    object r = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (r.ToString() != "1")
                    {
                        object[] rCoord = (object[])r;
                        au3.Send("{" + txtR.Text + "DOWN}");
                        au3.Send("{" + txtR.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                       
                       
                       


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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
            var t12 = Task.Run(() => SearchBoss3(token));
            await Task.WhenAny(new[] { t12 });
        }

        async Task SearchBoss3(CancellationToken token)
        {
            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;

            for (int i = 0; i < int.Parse(txtDungeon2search.Text); i++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Search enemy..."));
                    // Tunable variables
                    float threshold = 0.7f; // set this higher for fewer false positives and lower for fewer false negatives
                    var enemyTemplate =
                        new Image<Bgr, byte>("enemy.png"); // icon of the enemy
                    var enemyMask =
                        new Image<Bgr, byte>("mask.png"); // make white what the important parts are, other parts should be black
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
                        inputSimulator.Mouse.LeftButtonClick();

                    }
                    else
                    {
                        au3.MouseClick("" + txtLEFT.Text + "", 1574, 526, 1);
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
            var t12 = Task.Run(() => FIGHT4(token));
            await Task.WhenAny(new[] { t12 });
        }
        async Task FIGHT4(CancellationToken token)
        {
            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;

            for (int i = 0; i < int.Parse(txtDungeon2.Text) / 3; i++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                  
                    object fight1 = au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (fight1.ToString() != "1")
                    {
                        object[] fight1Coord = (object[])fight1;
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);



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
                    object fight1 = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                    if (fight1.ToString() != "1")
                    {
                        object[] fight1Coord = (object[])fight1;
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                       
                       
                       


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                       
                       
                       

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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    object ds = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (ds.ToString() != "1")
                    {
                        object[] dsCoord = (object[])ds;
                        au3.Send("{" + txtD.Text + "DOWN}");
                        au3.Send("{" + txtD.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object a = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);
                    if (a.ToString() != "1")
                    {
                        object[] aCoord = (object[])a;
                        au3.Send("{" + txtA.Text + "DOWN}");
                        au3.Send("{" + txtA.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    object s = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (s.ToString() != "1")
                    {
                        object[] sCoord = (object[])s;
                        au3.Send("{" + txtS.Text + "DOWN}");
                        au3.Send("{" + txtS.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object f = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (f.ToString() != "1")
                    {
                        object[] fCoord = (object[])f;
                        au3.Send("{" + txtF.Text + "DOWN}");
                        au3.Send("{" + txtF.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                       
                       
                       


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                       
                       
                       
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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                       
                       
                       
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
                    object e = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (e.ToString() != "1")
                    {
                        object[] eCoord = (object[])e;
                        au3.Send("{" + txtE.Text + "DOWN}");
                        au3.Send("{" + txtE.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object q = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (q.ToString() != "1")
                    {
                        object[] qCoord = (object[])q;
                        au3.Send("{" + txtQ.Text + "DOWN}");
                        au3.Send("{" + txtQ.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                       
                       
                       

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
                    object w = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (w.ToString() != "1")
                    {
                        object[] wCoord = (object[])w;
                        au3.Send("{" + txtW.Text + "DOWN}");
                        au3.Send("{" + txtW.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                       
                       
                       
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
                    object r = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (r.ToString() != "1")
                    {
                        object[] rCoord = (object[])r;
                        au3.Send("{" + txtR.Text + "DOWN}");
                        au3.Send("{" + txtR.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                       
                       
                       


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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
            var t12 = Task.Run(() => SearchBoss4(token));
            await Task.WhenAny(new[] { t12 });
        }
        async Task SearchBoss4(CancellationToken token)
        {
            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;

            for (int i = 0; i < int.Parse(txtDungeon2search.Text); i++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Search enemy..."));
                    // Tunable variables
                    float threshold = 0.7f; // set this higher for fewer false positives and lower for fewer false negatives
                    var enemyTemplate =
                        new Image<Bgr, byte>("enemy.png"); // icon of the enemy
                    var enemyMask =
                        new Image<Bgr, byte>("mask.png"); // make white what the important parts are, other parts should be black
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
                        inputSimulator.Mouse.LeftButtonClick();

                    }
                    else
                    {
                        au3.MouseClick("" + txtLEFT.Text + "", 42, 538, 1);
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
            var t12 = Task.Run(() => FIGHT5(token));
            await Task.WhenAny(new[] { t12 });
        }
        async Task FIGHT5(CancellationToken token)
        {
            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;

            for (int i = 0; i < int.Parse(txtDungeon2.Text) / 3; i++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                    object fight1 = au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (fight1.ToString() != "1")
                    {
                        object[] fight1Coord = (object[])fight1;
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);



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
                    object fight1 = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                    if (fight1.ToString() != "1")
                    {
                        object[] fight1Coord = (object[])fight1;
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                       
                       
                       


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                       
                       
                       

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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    object ds = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);
                    if (ds.ToString() != "1")
                    {
                        object[] dsCoord = (object[])ds;
                        au3.Send("{" + txtD.Text + "DOWN}");
                        au3.Send("{" + txtD.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object a = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);
                    if (a.ToString() != "1")
                    {
                        object[] aCoord = (object[])a;
                        au3.Send("{" + txtA.Text + "DOWN}");
                        au3.Send("{" + txtA.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    object s = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (s.ToString() != "1")
                    {
                        object[] sCoord = (object[])s;
                        au3.Send("{" + txtS.Text + "DOWN}");
                        au3.Send("{" + txtS.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object f = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (f.ToString() != "1")
                    {
                        object[] fCoord = (object[])f;
                        au3.Send("{" + txtF.Text + "DOWN}");
                        au3.Send("{" + txtF.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                       
                       
                       


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                       
                       
                       
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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                       
                       
                       
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
                    object e = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (e.ToString() != "1")
                    {
                        object[] eCoord = (object[])e;
                        au3.Send("{" + txtE.Text + "DOWN}");
                        au3.Send("{" + txtE.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object q = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (q.ToString() != "1")
                    {
                        object[] qCoord = (object[])q;
                        au3.Send("{" + txtQ.Text + "DOWN}");
                        au3.Send("{" + txtQ.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                       
                       
                       

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
                    object w = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (w.ToString() != "1")
                    {
                        object[] wCoord = (object[])w;
                        au3.Send("{" + txtW.Text + "DOWN}");
                        au3.Send("{" + txtW.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                       
                       
                       
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
                    object r = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (r.ToString() != "1")
                    {
                        object[] rCoord = (object[])r;
                        au3.Send("{" + txtR.Text + "DOWN}");
                        au3.Send("{" + txtR.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                       
                       
                       


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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
            var t12 = Task.Run(() => SearchBoss5(token));
            await Task.WhenAny(new[] { t12 });
        }


        async Task SearchBoss5(CancellationToken token)
        {
            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;

           
                for (int i = 0; i < int.Parse(txtDungeon2search.Text); i++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Search enemy..."));
                    // Tunable variables
                    float threshold = 0.7f; // set this higher for fewer false positives and lower for fewer false negatives
                    var enemyTemplate =
                        new Image<Bgr, byte>("boss.png"); // icon of the enemy
                    var enemyMask =
                        new Image<Bgr, byte>("bossmask.png"); // make white what the important parts are, other parts should be black
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
                        inputSimulator.Mouse.LeftButtonClick();

                    }
                    else
                    {
                        au3.MouseClick("" + txtLEFT.Text + "", 1574, 526, 1);
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
            var t12 = Task.Run(() => FIGHT6(token));
            await Task.WhenAny(new[] { t12 });
        }
        async Task FIGHT6(CancellationToken token)
        {
            _Shadowhunter = true;
            _Paladin = true;
            _Berserker = true;
           
            for (int i = 0; i < int.Parse(txtDungeon2.Text) / 3; i++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                    object fight1 = au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (fight1.ToString() != "1")
                    {
                        object[] fight1Coord = (object[])fight1;
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);



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
                    object fight1 = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10);

                    if (fight1.ToString() != "1")
                    {
                        object[] fight1Coord = (object[])fight1;
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight1Coord[0], (int)fight1Coord[1] + 70, 1, 1);
                       
                       
                       


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight2Coord[0], (int)fight2Coord[1] + 70, 1, 1);
                       
                       
                       

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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    object ds = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);


                    if (ds.ToString() != "1")
                    {
                        object[] dsCoord = (object[])ds;
                        au3.Send("{" + txtD.Text + "DOWN}");
                        au3.Send("{" + txtD.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)dsCoord[0], (int)dsCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object a = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);
                    if (a.ToString() != "1")
                    {
                        object[] aCoord = (object[])a;
                        au3.Send("{" + txtA.Text + "DOWN}");
                        au3.Send("{" + txtA.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)aCoord[0], (int)aCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    object s = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (s.ToString() != "1")
                    {
                        object[] sCoord = (object[])s;
                        au3.Send("{" + txtS.Text + "DOWN}");
                        au3.Send("{" + txtS.Text + "DOWN}");


                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)sCoord[0], (int)sCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object f = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (f.ToString() != "1")
                    {
                        object[] fCoord = (object[])f;
                        au3.Send("{" + txtF.Text + "DOWN}");
                        au3.Send("{" + txtF.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fCoord[0], (int)fCoord[1] + 70, 1, 1);
                       
                       
                       


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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight11Coord[0], (int)fight11Coord[1] + 70, 1, 1);
                       
                       
                       
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
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)fight22Coord[0], (int)fight22Coord[1] + 70, 1, 1);
                       
                       
                       
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
                    object e = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (e.ToString() != "1")
                    {
                        object[] eCoord = (object[])e;
                        au3.Send("{" + txtE.Text + "DOWN}");
                        au3.Send("{" + txtE.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)eCoord[0], (int)eCoord[1] + 70, 1, 1);
                       
                       
                       


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
                    object q = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);


                    if (q.ToString() != "1")
                    {
                        object[] qCoord = (object[])q;
                        au3.Send("{" + txtQ.Text + "DOWN}");
                        au3.Send("{" + txtQ.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)qCoord[0], (int)qCoord[1] + 70, 1, 1);
                       
                       
                       

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
                    object w = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (w.ToString() != "1")
                    {
                        object[] wCoord = (object[])w;
                        au3.Send("{" + txtW.Text + "DOWN}");
                        au3.Send("{" + txtW.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)wCoord[0], (int)wCoord[1] + 70, 1, 1);
                       
                       
                       
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
                    object r = au3.PixelSearch(750, 400, 1169, 697, 0xDD2C02, 10); au3.PixelSearch(650, 300, 1269, 797, 0xAD901C, 10);

                    if (r.ToString() != "1")
                    {
                        object[] rCoord = (object[])r;
                        au3.Send("{" + txtR.Text + "DOWN}");
                        au3.Send("{" + txtR.Text + "DOWN}");

                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                        au3.MouseClick("" + txtRIGHT.Text + "", (int)rCoord[0], (int)rCoord[1] + 70, 1, 1);
                       
                       
                       


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
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");
                            au3.Send("{" + txtY.Text + "}");

                            Thread.Sleep(500);
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
                            au3.Send("{" + txtPaladin.Text + "}");
                            au3.Send("{" + txtPaladin.Text + "}");
                            Thread.Sleep(500);
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
            var t12 = Task.Run(() => Exit1(token));
            await Task.WhenAny(new[] { t12 });
        }
 
        async Task Exit1(CancellationToken token)
            {
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
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
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
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
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
            var t6 = Task.Run(() => Exitaccept(token));
            await Task.WhenAny(new[] { t6 });
        }


        async Task Exitaccept(CancellationToken token)
        {
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
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
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
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
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
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
                        au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 10);
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
            if(_REPAIR == false && _LOGOUT == false)
            {
                await Task.Delay(2000);
                var t9 = Task.Run(() => RESTART(token));
                await Task.WhenAny(new[] { t9 });
            }

        }
        async Task LOGOUT(CancellationToken token)
        {


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
                    au3.MouseClick("LEFT", 1238, 728, 1, 10);
                    Thread.Sleep(2000);
                    au3.MouseClick("LEFT", 906, 575, 1, 10);
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

        async Task REPAIR(CancellationToken token)
        {


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

                    au3.MouseClick("LEFT", 1741, 1040, 1, 10);


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
                    au3.MouseClick("LEFT", 1684, 823, 1, 10);

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
                    au3.MouseClick("LEFT", 1256, 693, 1, 10);


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
                    au3.MouseClick("LEFT", 1085, 429, 1, 10);
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
            await Task.Delay(2000);
            var t10 = Task.Run(() => RESTART2(token));
            await Task.WhenAny(new[] { t10 });
        }
        async Task RESTART(CancellationToken token)
        {


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

            var t1 = Task.Run(() => START(token));
            await Task.WhenAny(new[] { t1 });
        }
        async Task RESTART2(CancellationToken token)
        {


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

            var t1 = Task.Run(() => START(token));
            await Task.WhenAny(new[] { t1 });
        }
        // ZUKUNFT //


        private void lbClose_Click(object sender, EventArgs e)
        {

            Application.Exit();
            Environment.Exit(0);

        }

        private void ChaosBot_Load(object sender, EventArgs e)
        {
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            txtDungeon.Text = Properties.Settings.Default.dungeontimer;
            txtLEFT.Text = Properties.Settings.Default.left;
            txtRIGHT.Text = Properties.Settings.Default.right;
            txtQ.Text = Properties.Settings.Default.q;
            txtW.Text = Properties.Settings.Default.w;
            txtE.Text = Properties.Settings.Default.e;
            txtR.Text = Properties.Settings.Default.r;
            txtA.Text = Properties.Settings.Default.a;
            txtS.Text = Properties.Settings.Default.s;
            txtD.Text = Properties.Settings.Default.d;
            txtF.Text = Properties.Settings.Default.f;
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
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
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

                Properties.Settings.Default.dungeontimer = "70";
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
                Properties.Settings.Default.txtDungeon2 = "21";
                Properties.Settings.Default.txtDungeon2search = "9";

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
            }
            catch { }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

            
                Properties.Settings.Default.left = "LEFT";
                Properties.Settings.Default.right = "RIGHT";
                Properties.Settings.Default.q = "Q";
                Properties.Settings.Default.w = "W";
                Properties.Settings.Default.e = "E";
                Properties.Settings.Default.r = "R";
                Properties.Settings.Default.a = "A";
                Properties.Settings.Default.s = "S";
                Properties.Settings.Default.d = "D";
                Properties.Settings.Default.f = "F";
               
                Properties.Settings.Default.Save();
              
                txtLEFT.Text = Properties.Settings.Default.left;
                txtRIGHT.Text = Properties.Settings.Default.right;
                txtQ.Text = Properties.Settings.Default.q;
                txtW.Text = Properties.Settings.Default.w;
                txtE.Text = Properties.Settings.Default.e;
                txtR.Text = Properties.Settings.Default.r;
                txtA.Text = Properties.Settings.Default.a;
                txtS.Text = Properties.Settings.Default.s;
                txtD.Text = Properties.Settings.Default.d;
                txtF.Text = Properties.Settings.Default.f;
               
            }
            catch { }
        }

        private void label36_Click(object sender, EventArgs e)
        {

        }
    }

}
