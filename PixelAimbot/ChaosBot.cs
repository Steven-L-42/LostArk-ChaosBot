using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelAimbot.Classes.OpenCV;
using System.Drawing.Imaging;

using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using System.IO;

namespace PixelAimbot
{
    public partial class ChaosBot : Form
    {
        public ChaosBot()
        {
            InitializeComponent();
            conf = Config.Load();
            // Combine the base folder with your specific folder....
            if (conf.username == "Mentalill" || conf.username == "ShiiikK" && Debugger.IsAttached)
            {
                if (Application.OpenForms["Debugging"] == null) {
                    new Debugging().Show();
                    btnHidden.Visible = true;
                }
            }
            Process[] processName = Process.GetProcessesByName("LostArk");
            if (processName.Length == 1)
            {
                handle = processName[0].MainWindowHandle;
                Rectangle rect;
                GetWindowRect(handle, out rect);
                System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.PrimaryScreen;

                if (screen.Bounds.Width > 2000 && screen.Bounds.Height > 1200 &&
                    screen.Bounds.Width > (rect.Right - rect.Left) && screen.Bounds.Height > (rect.Bottom - rect.Top))
                {
                    SetWindowPos(handle, HWND_BOTTOM, 0, 0, rect.Right - rect.Left, rect.Bottom - rect.Top, 0);
                    GetWindowRect(handle, out rect);

                    Task.Delay(5000);
                    isWindowed = true;
                    windowX = rect.X + 2;
                    windowY = rect.Y + 26;
                    windowWidth = 1920;
                    windowHeight = 1080;
                    screenWidth = 1920;
                    screenHeight = 1080;
                }
            }

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Recalc(0), Recalc(842, false));
            this.TopMost = true;
            
        
            
            this.FormBorderStyle = FormBorderStyle.None;
            RefreshRotationCombox();
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
            UnregisterHotKey(this.Handle, FirstHotkeyId);
            Boolean F9Registered = RegisterHotKey(this.Handle, FirstHotkeyId, 0x0000, FirstHotKeyKey);

            // Repeat the same process but with F10
            int SecondHotkeyId = 2;
            int SecondHotKeyKey = (int)Keys.F10;
            UnregisterHotKey(this.Handle, SecondHotkeyId);
            Boolean F10Registered = RegisterHotKey(this.Handle, SecondHotkeyId, 0x0000, SecondHotKeyKey);
            discordToken = new CancellationTokenSource();
            
            try
            {
                DiscordTask = DiscordBotAsync(conf.discorduser, discordToken.Token);
            }
            catch { }

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

        
        Point p1 = new Point(ChaosBot.Recalc(1326), ChaosBot.Recalc(220));
        Point p2 = new Point(0, 0);
        private async void btnPause_Click(object sender, EventArgs e)
        {
            if (_stop == true)
            {
                cts.Cancel();
                _formExists = 0;
                _RepairReset = true;
                _start = false;
                _stopp = true;
                _stop = false;
                _restart = false;
                _logout = false;


                _gunlancer = false;
                _shadowhunter = false;
                _berserker = false;
                _paladin = false;
                _deathblade = false;
                _sharpshooter = false;
                _bard = false;
                _sorcerer = false;
                _soulfist = false;

                _floor1 = false;
                _floor2 = false;

                _floorFight = false;
                _searchboss = false;

                _revive = false;
                _portaldetect = false;

                _ultimate = false;
                _doUltimateAttack = false;
                _potions = false;
                _firstSetupTransparency = true;



                _Q = true;
                _W = true;
                _E = true;
                _R = true;
                _A = true;
                _S = true;
                _D = true;
                _F = true;

                this.Show();
                FormMinimized.Hide();
                FormMinimized.sw.Reset();

                ChaosStop = DateTime.Now;
                ChaosTime = ChaosStop - ChaosStart;

                KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                await Task.Delay(humanizer.Next(10, 240) + 100);


                Rectangle Inventory = new Rectangle(ChaosBot.Recalc(1326), ChaosBot.Recalc(189, false), ChaosBot.Recalc(544), ChaosBot.Recalc(640, false));

                using (Bitmap bitmap = new Bitmap(543, 564))
                {
                    endinv = "EndInv.jpg";
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(p1, p2, Inventory.Size);
                    }

                    bitmap.Save(endinv, ImageFormat.Jpeg);

                }

                KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);


                Bitmap start = new Bitmap(startinv);
                StartInvColor = start;
                StartInvGray = new Bitmap(start.Width, start.Height);

                Bitmap end = new Bitmap(endinv);
                EndInvColor = end;
                EndInvGray = new Bitmap(end.Width, end.Height);

                for (int i = 0; i < start.Width; i++)
                {
                    for (int x = 0; x < start.Height; x++)
                    {
                        Color oc = start.GetPixel(i, x);
                        int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                        Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                        StartInvGray.SetPixel(i, x, nc);
                    }
                }

                for (int j = 0; j < end.Width; j++)
                {
                    for (int y = 0; y < end.Height; y++)
                    {
                        Color oc = end.GetPixel(j, y);
                        int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                        Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                        EndInvGray.SetPixel(j, y, nc);
                    }
                }

                ChaosRunTimed form = new ChaosRunTimed();
                form.Show();


                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "STOPPED!"));
                await Task.Delay(humanizer.Next(10, 240) + 1000);
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "READY!"));
            }
        }
        public static TimeSpan ChaosTime = new TimeSpan();
        public static DateTime ChaosStart;
        public static DateTime ChaosStop;

        public static Bitmap StartInvColor;
        public static string startinv;

        public static Bitmap EndInvColor;
        public static string endinv;

        public static Bitmap StartInvGray;
        public static Bitmap EndInvGray;

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_start == false)
            {
                try
                {
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "READY!"));
                    _start = true;
                    _stop = true;
                    cts = new CancellationTokenSource();
                    var token = cts.Token;
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    Process[] processName = Process.GetProcessesByName("LostArk");
                    _formExists++;
                    if (_formExists == 1)
                    {
                        FormMinimized.StartPosition = FormStartPosition.Manual;
                        FormMinimized.Location = new Point(Recalc(0), Recalc(28, false));
                        FormMinimized.timerRuntimer.Enabled = true;
                        FormMinimized.sw.Reset();
                        FormMinimized.sw.Start();
                        FormMinimized.Show();
                        FormMinimized.Size = new Size(594, 28);
                        this.Hide();
                        ChaosAllRounds = 0;
                        ChaosAllStucks = 0;
                        ChaosPerfectRounds = 0;
                        ChaosGameCrashed = 0;

                        ChaosStart = DateTime.Now;

                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        processName = Process.GetProcessesByName("LostArk");
                        if (processName.Length == 1)
                        {
                            handle = processName[0].MainWindowHandle;
                            SetForegroundWindow(handle);
                        }

                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                        await Task.Delay(humanizer.Next(10, 240) + 1000);


                        Rectangle Inventory = new Rectangle(ChaosBot.Recalc(1322), ChaosBot.Recalc(189, false), ChaosBot.Recalc(544), ChaosBot.Recalc(640, false));

                        using (Bitmap bitmap = new Bitmap(543,564))
                        {
                            startinv = "StartInv.jpg";
                            using (Graphics g = Graphics.FromImage(bitmap))
                            {
                                g.CopyFromScreen(p1, p2, Inventory.Size);
                            }

                            bitmap.Save(startinv, ImageFormat.Jpeg);
                        }

                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                        await Task.Delay(humanizer.Next(10, 240) + 500);
                    }
                    if (processName.Length == 0 && chBoxCrashDetection.Checked)
                    {
                        ChaosGameCrashed++;
                        _stop = true;
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_F10);
                        await Task.Delay(5000);
                        DiscordSendMessage("Game Crashed - Bot Stopped!");
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "GAME CRASHED - BOT STOPPED!"));

                    }
                    var t1 = Task.Run(() => Start(token));
                    if (chBoxAutoRepair.Checked && _RepairReset == true)
                    {

                        _RepairReset = false;
                        _repairTimer = DateTime.Now.AddMinutes(Convert.ToDouble(txtRepair.Text));
                    }
                   
                    if (chBoxLOGOUT.Checked)
                    {
                        
                        var dateNow = DateTime.Now;
                        if(cmbHOUR.SelectedIndex < dateNow.Hour)
                        {
                            _Logout = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day+1, cmbHOUR.SelectedIndex, cmbMINUTE.SelectedIndex, 00);

                        }
                        else
                        {
                            _Logout = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, cmbHOUR.SelectedIndex, cmbMINUTE.SelectedIndex, 00);

                        }

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

        private void comboBoxMouse_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBoxMouse.SelectedIndex == 0)
            {
                currentMouseButton = KeyboardWrapper.VK_LBUTTON;
            }
            else
            {
                currentMouseButton = KeyboardWrapper.VK_RBUTTON;
            }            
        }

        private void cmbHealKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHealKey.SelectedIndex == 0)
            {
                currentHealKey = KeyboardWrapper.VK_F1;
            }
            else if (cmbHealKey.SelectedIndex == 1)
            {
                currentHealKey = KeyboardWrapper.VK_1;
            }
            else if (cmbHealKey.SelectedIndex == 2)
            {
                currentHealKey = KeyboardWrapper.VK_2;
            }
            else if (cmbHealKey.SelectedIndex == 3)
            {
                currentHealKey = KeyboardWrapper.VK_3;
            }
            else if (cmbHealKey.SelectedIndex == 4)
            {
                currentHealKey = KeyboardWrapper.VK_4;
            }
            else if (cmbHealKey.SelectedIndex == 5)
            {
                currentHealKey = KeyboardWrapper.VK_5;
            }
            else if (cmbHealKey.SelectedIndex == 6)
            {
                currentHealKey = KeyboardWrapper.VK_6;
            }
            else if (cmbHealKey.SelectedIndex == 7)
            {
                currentHealKey = KeyboardWrapper.VK_7;
            }
            else if (cmbHealKey.SelectedIndex == 8)
            {
                currentHealKey = KeyboardWrapper.VK_8;
            }
            else if (cmbHealKey.SelectedIndex == 9)
            {
                currentHealKey = KeyboardWrapper.VK_9;
            }
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
            comboBoxMouse.SelectedIndex = 0;
            
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);

            chBoxUnstuckF1.Checked = Properties.Settings.Default.chBoxUnstuckF1;
            txtRestart.Text = Properties.Settings.Default.txtRestart;
            chBoxCrashDetection.Checked = Properties.Settings.Default.chBoxCrashDetection;
            checkBoxDiscordNotifications.Checked = Properties.Settings.Default.checkBoxDiscordNotifications;
            HealthSlider1.Value = Properties.Settings.Default.HealthSlider1;
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
            
            txLeaveTimerFloor2.Text = Properties.Settings.Default.txLeaveTimerFloor2;
            textBoxAutoAttack.Text = Properties.Settings.Default.textBoxAutoAttack;
            chBoxAwakening.Checked = Properties.Settings.Default.chBoxAwakening;
            txtDeath.Text = Properties.Settings.Default.txtDeath;
            chBoxRevive.Checked = Properties.Settings.Default.chBoxRevive;
            comboBox1.SelectedIndex = Properties.Settings.Default.comboBox1;
            cmbHOUR.SelectedIndex = Properties.Settings.Default.cmbHOUR;
            cmbMINUTE.SelectedIndex = Properties.Settings.Default.cmbMINUTE;
            chBoxLeavetimer.Checked = Properties.Settings.Default.chBoxLeavetimer;
            radioEnglish.Checked = Properties.Settings.Default.radioEnglish;
            radioGerman.Checked = Properties.Settings.Default.radioGerman;
            cmbHealKey.SelectedIndex = Properties.Settings.Default.cmbHealKey;

            healthPercent = HealthSlider1.Value;
            double distanceFromMin = (HealthSlider1.Value - HealthSlider1.Minimum);
            double sliderRange = (HealthSlider1.Maximum - HealthSlider1.Minimum);
            double sliderPercent = 100 * (distanceFromMin / sliderRange);
            labelheal.Text = "Heal at: " + Convert.ToInt32(sliderPercent) + "% Life";

          
        }

        private void lbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void ChaosBot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
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
                
            }
        }

        private void chBoxLOGOUT_CheckedChanged(object sender, EventArgs e)
        {
            
           if (!chBoxLOGOUT.Checked)
            {
                _logout = false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.chBoxCrashDetection = true;
                Properties.Settings.Default.checkBoxDiscordNotifications = true;
                Properties.Settings.Default.HealthSlider1 = 801;
                Properties.Settings.Default.chBoxGunlancer = false;
                Properties.Settings.Default.chBoxRevive = false;
                Properties.Settings.Default.txtDeath = "70";
                Properties.Settings.Default.txLeaveTimerFloor2 = "165";
                Properties.Settings.Default.txLeaveTimerFloor3 = "180";

                Properties.Settings.Default.chBoxUnstuckF1 = true;
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
                Properties.Settings.Default.textBoxAutoAttack = "1500";
                Properties.Settings.Default.chBoxAwakening = false;
                Properties.Settings.Default.txtHeal10 = "F1";
                Properties.Settings.Default.comboBox1 = 0;
                Properties.Settings.Default.cmbHOUR = DateTime.Now.Hour;
                Properties.Settings.Default.cmbMINUTE = DateTime.Now.Minute;
                Properties.Settings.Default.chBoxLeavetimer = false;
                Properties.Settings.Default.radioGerman = false;
                Properties.Settings.Default.radioEnglish = true;
                Properties.Settings.Default.cmbHealKey = 0;


                Properties.Settings.Default.Save();
                chBoxCrashDetection.Checked = Properties.Settings.Default.chBoxCrashDetection;
                checkBoxDiscordNotifications.Checked = Properties.Settings.Default.checkBoxDiscordNotifications;
                HealthSlider1.Value = Properties.Settings.Default.HealthSlider1;
                chBoxGunlancer.Checked = Properties.Settings.Default.chBoxGunlancer;
                txtRestart.Text = Properties.Settings.Default.txtRestart;
                chBoxRevive.Checked = Properties.Settings.Default.chBoxRevive;
                txtDeath.Text = Properties.Settings.Default.txtDeath;
                chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxAutoMovement;
                txLeaveTimerFloor2.Text = Properties.Settings.Default.txLeaveTimerFloor2;
                chBoxUnstuckF1.Checked = Properties.Settings.Default.chBoxUnstuckF1;
                chBoxLOGOUT.Checked = Properties.Settings.Default.chBoxLOGOUT;

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
                cmbHOUR.Text = Properties.Settings.Default.txtLOGOUT;
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
               
                chBoxBard.Checked = Properties.Settings.Default.chBoxBard;
                chBoxGunlancer2.Checked = Properties.Settings.Default.chBoxGunlancer2;
                textBoxAutoAttack.Text = Properties.Settings.Default.textBoxAutoAttack;

                chBoxAwakening.Checked = Properties.Settings.Default.chBoxAwakening;
                comboBox1.SelectedIndex = Properties.Settings.Default.comboBox1;
                cmbHOUR.SelectedIndex = Properties.Settings.Default.cmbHOUR;
                cmbMINUTE.SelectedIndex = Properties.Settings.Default.cmbMINUTE;
                chBoxLeavetimer.Checked = Properties.Settings.Default.chBoxLeavetimer;
                radioEnglish.Checked = Properties.Settings.Default.radioEnglish;
                radioGerman.Checked = Properties.Settings.Default.radioGerman;
                cmbHealKey.SelectedIndex = Properties.Settings.Default.cmbHealKey;

            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
 
        private void btnInstructions_Click(object sender, EventArgs e)
        {

            frmGuide Fm1 = new frmGuide();

            if (!_GuideLoaded)
            {
               
                Fm1.Show();
                _GuideLoaded = true;
            }
            
        }


       
        private void buttonSaveRotation_Click(object sender, EventArgs e)
        {
            if (comboBoxRotations.Text != "")
            {
                if (comboBoxRotations.Text != "main")
                {
                    rotation.chBoxCrashDetection = chBoxCrashDetection.Checked;
                    rotation.checkBoxDiscordNotifications = checkBoxDiscordNotifications.Checked;
                    rotation.HealthSlider1 = HealthSlider1.Value;
                    rotation.chBoxGunlancer = (bool)chBoxGunlancer.Checked;
                    rotation.txtDeath = txtDeath.Text;
                    rotation.chBoxRevive = (bool)chBoxRevive.Checked;
                    rotation.txLeaveTimerFloor2 = txLeaveTimerFloor2.Text;
              
                    rotation.txtRestart = txtRestart.Text;
                    rotation.chBoxUnstuckF1 = chBoxUnstuckF1.Checked;

                    rotation.chBoxAutoRepair = (bool)chBoxAutoRepair.Checked;
                    rotation.autorepair = txtRepair.Text;

                    rotation.autologout = cmbHOUR.Text;
                    rotation.chBoxautologout = chBoxLOGOUT.Checked;
                    rotation.chBoxAutoMovement = chBoxAutoMovement.Checked;
                    rotation.autorepair = txtRepair.Text;
                    rotation.chBoxShadowhunter = (bool)chBoxY.Checked;
                    rotation.chboxPaladin = (bool)chBoxPaladin.Checked;
                    rotation.chBoxBerserker = (bool)chBoxBerserker.Checked;
                    rotation.chBoxDeathblade = (bool)chBoxDeathblade.Checked;
                    rotation.chBoxDeathblade2 = (bool)chBoxDeathblade2.Checked;
                    rotation.chBoxSharpshooter = (bool)chBoxSharpshooter.Checked;
                    rotation.chBoxSoulfist = (bool)chBoxSoulfist.Checked;
                    rotation.chBoxSorcerer = (bool)chBoxSorcerer.Checked;
                    rotation.chBoxBard = (bool)chBoxBard.Checked;
                    rotation.chBoxGunlancer2 = (bool)chBoxGunlancer2.Checked;
                    rotation.chBoxChannelSwap = (bool)chBoxChannelSwap.Checked;
                    rotation.chBoxSaveAll = chBoxAutoMovement.Checked;
                    rotation.chBoxActivateF2 = chBoxActivateF2.Checked;
                

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
                    rotation.textBoxAutoAttack = textBoxAutoAttack.Text;
                    rotation.chBoxAwakening = chBoxAwakening.Checked;
                    rotation.mouseButton = comboBoxMouse.SelectedIndex;
                    rotation.comboBox1 = comboBox1.SelectedIndex;
                    rotation.cmbMINUTE = cmbMINUTE.SelectedIndex;
                    rotation.cmbHOUR = cmbHOUR.SelectedIndex;
                    rotation.chBoxLeavetimer = chBoxLeavetimer.Checked;
                    rotation.radioGerman = radioGerman.Checked;
                    rotation.radioEnglish = radioEnglish.Checked;
                    rotation.cmbHealKey = cmbHealKey.SelectedIndex;

                    rotation.Save(comboBoxRotations.Text);
                    Alert.Show("Rotation \"" + comboBoxRotations.Text + "\" saved");
                }
                else
                {
                    Alert.Show("Rotation can not be named \"main\"", frmAlert.enmType.Error);
                }
            }
            else
            {
                Alert.Show("Please enter a name for your Rotation Config!", frmAlert.enmType.Error);
            }
        }

        private void buttonLoadRotation_Click(object sender, EventArgs e)
        {
            rotation = Rotations.Load(comboBoxRotations.Text + ".ini");
            if (rotation != null)
            {
                /*
                if (rotation.HealthSlider1 > 100)
                {
                    rotation.HealthSlider1 = 100;
                }
                */
                chBoxCrashDetection.Checked = rotation.chBoxCrashDetection;
                checkBoxDiscordNotifications.Checked = rotation.checkBoxDiscordNotifications;
                HealthSlider1.Value = rotation.HealthSlider1;
                txtDeath.Text = rotation.txtDeath;
                chBoxRevive.Checked = rotation.chBoxRevive;
                txtRestart.Text = rotation.txtRestart;
                chBoxGunlancer.Checked = rotation.chBoxGunlancer;
                chBoxUnstuckF1.Checked = rotation.chBoxUnstuckF1;

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
                cmbHOUR.Text = rotation.autologout;
                chBoxLOGOUT.Checked = rotation.chBoxautologout;
                txLeaveTimerFloor2.Text = rotation.txLeaveTimerFloor2;
          
                chBoxAutoMovement.Checked = rotation.chBoxAutoMovement;
               
                textBoxAutoAttack.Text = rotation.textBoxAutoAttack;
                chBoxAwakening.Checked = rotation.chBoxAwakening;
                chBoxSorcerer.Checked = rotation.chBoxSorcerer;
                chBoxChannelSwap.Checked = rotation.chBoxChannelSwap;
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
                comboBoxMouse.SelectedIndex = rotation.mouseButton;
                comboBox1.SelectedIndex = rotation.comboBox1;
                cmbHOUR.SelectedIndex = rotation.cmbHOUR;
                cmbMINUTE.SelectedIndex = rotation.cmbMINUTE;
                chBoxLeavetimer.Checked = rotation.chBoxLeavetimer;
                radioEnglish.Checked = rotation.radioEnglish;
                radioGerman.Checked = rotation.radioGerman;
                cmbHealKey.SelectedIndex = rotation.cmbHealKey;

                if (comboBoxMouse.SelectedIndex == 0)
                {
                    currentMouseButton = KeyboardWrapper.VK_LBUTTON;
                }
                else
                {
                    currentMouseButton = KeyboardWrapper.VK_RBUTTON;
                }        
                Alert.Show("Rotation \"" + comboBoxRotations.Text + "\" loaded");
            }
        }

        private void comboBoxRotations_MouseClick(object sender, MouseEventArgs e)
        {
            RefreshRotationCombox();
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
               
            }
        }

        

        private void chBoxRevive_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxRevive.Checked)
            {
                txtDeath.ReadOnly = false;
            }
            else if (!chBoxRevive.Checked)
            {
                txtDeath.ReadOnly = true;
            }
        }

        private void lbStatus_TextChanged(object sender, EventArgs e)
        {
            FormMinimized.labelMinimizedState.Text = lbStatus.Text;
        }


        private void labelSwap_Click_1(object sender, EventArgs e)
        {
            cts.Cancel();
            //_botIsRun = false;
            _discordBotIsRun = false;
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            if (Application.OpenForms.OfType<PixelAimbot.GatheringBot>().Count() == 1)
                Application.OpenForms.OfType<PixelAimbot.GatheringBot>().First().Close();

            GatheringBot Form = new GatheringBot();
            Form.Show();
            Application.OpenForms.OfType<PixelAimbot.ChaosBot>().First().Hide();
            Application.OpenForms.OfType<PixelAimbot.ChaosBot>().First().Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label18.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        int test = 0;
        private async void button1_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;

            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);
            Process[] processName = Process.GetProcessesByName("LostArk");
            processName = Process.GetProcessesByName("LostArk");
            if (processName.Length == 1)
            {
                handle = processName[0].MainWindowHandle;
                SetForegroundWindow(handle);
            }
            test++;
            if(test == 2)
            {
                test = 0;
                cts.Cancel();
            }
            else
            {
                teststart = true;
                awakening = true;
                var leave = Task.Run(() => TEST(token));

            }


        }
        public bool teststart;
        //public bool awakening;
        private async Task TEST(CancellationToken token)
        {

            // 'HIER WURDE DIE BOSS DETECTION GETESTET'
            //
            //

            
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                while (teststart)
                {

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(humanizer.Next(10, 240) + 100, token);
                    float threshold = 0.8f;

                    var BossTemplate = Image_bossHP;
                    var BossMask = Image_bossHPmask;

                    Point myPosition = new Point(Recalc(148), Recalc(127, false));
                    Point screenResolution = new Point(screenWidth, screenHeight);

                    var BossDetector = new BossDetector(BossTemplate, BossMask, threshold);
                    var screenPrinter = new PrintScreen();

                    var rawScreen = screenPrinter.CaptureScreen();
                    Bitmap bitmapImage = new Bitmap(rawScreen);
                    using (var screenCapture = bitmapImage.ToImage<Bgr, byte>())
                    {
                        var Boss = BossDetector.GetClosestEnemy(screenCapture, false);

                        if (Boss.HasValue)
                        {
                            lbStatus.Invoke(
             (MethodInvoker)(() => lbStatus.Text = "BOSS FIGHT!"));

                            while (true)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                object Awakening = Pixel.PixelSearch(Recalc(1161), Recalc(66, false), Recalc(1187),
                                    Recalc(83, false), 0x9C1B16, 50);
                                if (Awakening.ToString() == "0")
                                {

                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "AWAKENING..."));
                                    KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_V, 2000);
                                    await Task.Delay(500, token);
                                    KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_V, 2000);
                                    await Task.Delay(500, token);
                                    KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_V, 2000);
                                    await Task.Delay(500, token);
                                    KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_V, 2000);
                                    awakening = false;
                                    gefunden = true;



                                }
                                Random random2 = new Random();
                                var sleepTime2 = random2.Next(100, 150);
                                Thread.Sleep(sleepTime2);
                            }
                            gefunden = true;

                        }
                        else if (!Boss.HasValue && gefunden == true)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            await Task.Delay(humanizer.Next(10, 240) + 3000, token);

                            lbStatus.Invoke(
                            (MethodInvoker)(() => lbStatus.Text = "Floor2 Complete..."));
                            starten = false;
                            gefunden = false;
                            _stopp = true;
                            _portalIsDetected = false;

                            _portalIsNotDetected = false;
                            _floorFight = false;
                            _searchboss = false;
                            _revive = false;
                            _ultimate = false;
                            _portaldetect = false;
                            _potions = false;
                            _floor1 = false;
                            _floor2 = false;

                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            var leave = Task.Run(() => Leavedungeon(token));
                            await Task.WhenAny(leave);
                        }

                    }

                    Random random = new Random();
                    var sleepTime = random.Next(100, 150);
                    Thread.Sleep(sleepTime);
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
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

       

        private void chBoxLeavetimer_MouseClick(object sender, MouseEventArgs e)
        {
            if (chBoxLeavetimer.Checked)
            {

                DialogResult dialogResult = MessageBox.Show("By activating the checkbox, you can\n" +
                                "determine when the bot should leave the dungeon\n" +
                                "However, this means that the bot may not be able\n" +
                                "to defeat the boss in time!\n\n" +
                                "If you want to activate the Auto Leave after\n" +
                                "Defeat the Boss, then 'deactivate' the checkbox!\n\n" +
                                "Do you really want to 'Activate' the Manual Leavetimer?\n" +
                                "Then press Yes, otherwise press No.",
                                "You activated manual Dungeon Leave!", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    chBoxLeavetimer.Checked = true;
                }
                else if (dialogResult == DialogResult.No)
                {
                    chBoxLeavetimer.Checked = false;
                }
            }
            else if (!chBoxLeavetimer.Checked)
            {

                DialogResult dialogResult = MessageBox.Show("If you have difficulties with this, please activate\n" +
                                "the LEAVETIMER checkbox until we have provided an update.\n\n" +
                                "Do you really want to 'Deactivate' the Manual Leavetimer?\n" +
                                "Then press Yes, otherwise press No.",
                                "You activated auto Dungeon Leave!", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    chBoxLeavetimer.Checked = false;
                }
                else if (dialogResult == DialogResult.No)
                {
                    chBoxLeavetimer.Checked = true;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            frmStatistic Form = new frmStatistic();
            Form.Show();
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxRotations_MouseEnter(object sender, EventArgs e)
        {
            RefreshRotationCombox();
        }
    }
}