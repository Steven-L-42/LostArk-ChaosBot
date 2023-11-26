using PixelAimbot.Classes.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace PixelAimbot
{
    public partial class ChaosBot : Form
    {
        CancellationToken _token;
        CancellationToken _tokenSkills;
        CancellationToken _bossKill;

        public ChaosBot()
        {
            InitializeComponent();
            _token = Cts.Token;
            _tokenSkills = CtsSkills.Token;
            _bossKill = CtsBoss.Token;

            Conf = Config.Load();
            
            // Combine the base folder with your specific folder....
            if (Conf.username == "Mentalill" || Conf.username == "ShiiikK" && Debugger.IsAttached)
            {
                if (Application.OpenForms["Debugging"] == null)
                {
                    new Debugging().Show();
                }
            }

            Process[] processName = Process.GetProcessesByName("LostArk");
            if (processName.Length == 1)
            {
                handle = processName[0].MainWindowHandle;
                GetWindowRect(handle, out var rect);
                Screen screen = Screen.PrimaryScreen;

                if (screen.Bounds.Width > 2000 && screen.Bounds.Height > 1200 &&
                    screen.Bounds.Width > (rect.Right - rect.Left) && screen.Bounds.Height > (rect.Bottom - rect.Top))
                {
                    SetWindowPos(handle, HWND_BOTTOM, 0, 0, rect.Right - rect.Left, rect.Bottom - rect.Top, 0);
                    GetWindowRect(handle, out rect);

                    Task.Delay(5000);
                    IsWindowed = true;
                    _windowX = rect.X + 2;
                    _windowY = rect.Y + 26;
                    _windowWidth = 1920;
                    _windowHeight = 1080;
                    ScreenWidth = 1920;
                    ScreenHeight = 1080;
                }
            }

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Recalc(0), Recalc(842, false));
            //this.Location = new Point(Recalc(0), Recalc(842, false));
            this.TopMost = true;


            this.FormBorderStyle = FormBorderStyle.None;
            RefreshRotationCombox();
            
            this.Text = RandomString(15);
            // 3. Register HotKeys
            label15.Text = Config.version;
            // Set an unique id to your Hotkey, it will be used to
            // identify which hotkey was pressed in your code to execute something
            int firstHotkeyId = 1;
            // Set the Hotkey triggerer the F9 key
            // Expected an integer value for F9: 0x78, but you can convert the Keys.KEY to its int value
            // See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx
            int firstHotKeyKey = (int) Keys.F9;
            // Register the "F9" hotkey
            UnregisterHotKey(this.Handle, firstHotkeyId);
            Boolean f9Registered = RegisterHotKey(this.Handle, firstHotkeyId, 0x0000, firstHotKeyKey);

            // Repeat the same process but with F10
            int secondHotkeyId = 2;
            int secondHotKeyKey = (int) Keys.F10;
            UnregisterHotKey(this.Handle, secondHotkeyId);
            Boolean f10Registered = RegisterHotKey(this.Handle, secondHotkeyId, 0x0000, secondHotKeyKey);

            _discordToken = new CancellationTokenSource();


            try
            {
                DiscordTask = DiscordBotAsync(Conf.discorduser, _discordToken.Token);
            }
            catch
            {
                // ignored
            }

            SetupControls();
            // 4. Verify if both hotkeys were succesfully registered, if not, show message in the console
            if (!f9Registered)
            {
                btnStart_Click(null, null);
            }

            if (!f10Registered)
            {
                btnPause_Click(null, null);
            }
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
        private int _gameCrashed = 0;
        private async void GameCrashed()
        {
            _gameCrashed++;
            ChaosGameCrashed++;

            Cts.Cancel();
            CtsSkills.Cancel();
            CtsBoss.Cancel();

            _repairReset = true;
            _start = false;
            _stopp = true;
            _stop = false;
            _restart = false;
            _logout = false;
            starten = false;
            _leave = false;
            _floorint2 = 1;
            _gunlancer = false;
            _shadowhunter = false;
            _berserker = false;
            _paladin = false;
            _deathblade = false;
            _destroyer = false;

            _glavier = false;
            _sharpshooter = false;
            _bard = false;
            _sorcerer = false;
            _soulfist = false;

            _floor1 = false;
            _floor2 = false;
            _floor3 = false;

            _floorFight = false;
            _searchboss = false;

            _revive = false;
            _portaldetect = false;
            _portalIsDetected = true;
            _ultimate = false;
            _doUltimateAttack = false;
            _potions = false;
            _firstSetupTransparency = true;

            _q = true;
            _w = true;
            _e = true;
            _r = true;
            _a = true;
            _s = true;
            _d = true;
            _f = true;

          //  DiscordSendMessage("Game Crashed - Bot Stopped!");
            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "GAME CRASHED - BOT STOPPED!"));
            
            await Task.Delay(_humanizer.Next(10, 240) + 1000);
            for (int i = 0; i < 50; i++)
            {
                Cts.Cancel();
                CtsBoss.Cancel();
                CtsSkills.Cancel();
                await Task.Delay(100);
            }
            await Task.Run(() => Game_Restart());

        }

        private async void btnPause_Click(object sender, EventArgs e)
        {
            if (_stop)
            {
                
                Cts.Cancel();
                CtsSkills.Cancel();
                CtsBoss.Cancel();

                _gameCrashed = 0;
                _formExists = 0;
                _repairReset = true;
                _stop = false;
                _stopp = true;
               
                _restart = false;
                _logout = false;
                starten = false;
                _leave = false;
                _floorint2 = 1;
                _gunlancer = false;
                _shadowhunter = false;
                _berserker = false;
                _paladin = false;
                _deathblade = false;
                _destroyer = false;

                _glavier = false;
                _sharpshooter = false;
                _bard = false;
                _sorcerer = false;
                _soulfist = false;

                _floor1 = false;
                _floor2 = false;
                _floor3 = false;

                _floorFight = false;
                _searchboss = false;

                _revive = false;
                _portaldetect = false;
                _portalIsDetected = true;
                _ultimate = false;
                _doUltimateAttack = false;
                _potions = false;
                _firstSetupTransparency = true;

                _q = true;
                _w = true;
                _e = true;
                _r = true;
                _a = true;
                _s = true;
                _d = true;
                _f = true;
         

                this.Show();
                FormMinimized.Hide();
                FormMinimized.sw.Reset();


                if (chBoxCompare.Checked)
                {
                    ChaosStop = DateTime.Now;
                    ChaosTime = ChaosStop - ChaosStart;

                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                    await Task.Delay(_humanizer.Next(10, 240) + 1000, _token);

                    EndInventar = GetEndPic();

                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);


                    //Bitmap start = new Bitmap(startinv);
                    //StartInvColor = start;
                    //StartInvGray = new Bitmap(start.Width, start.Height);

                    //Bitmap end = new Bitmap(endinv);
                    //EndInvColor = end;
                    //EndInvGray = new Bitmap(end.Width, end.Height);

                    //for (int i = 0; i < start.Width; i++)
                    //{
                    //    for (int x = 0; x < start.Height; x++)
                    //    {
                    //        Color oc = start.GetPixel(i, x);
                    //        int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                    //        Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                    //        StartInvGray.SetPixel(i, x, nc);
                    //    }
                    //}

                    //for (int j = 0; j < end.Width; j++)
                    //{
                    //    for (int y = 0; y < end.Height; y++)
                    //    {
                    //        Color oc = end.GetPixel(j, y);
                    //        int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                    //        Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                    //        EndInvGray.SetPixel(j, y, nc);
                    //    }
                    //}

                    ChaosRunTimed form = new ChaosRunTimed();
                    form.Show();
                }

                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "STOPPED!"));
                
                for(int i = 0; i < 50; i++)
                {
                    Cts.Cancel();
                    CtsBoss.Cancel();
                    CtsSkills.Cancel();
                    await Task.Delay(100);
                }
            
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "READY!"));

                _start = false;
                

            }
        }

        private Bitmap GetStartPic()
        {
            return GetEndPic();
        }

        private Bitmap GetEndPic()
        {
            try
            {
                var picture = new PrintScreen();
                var screen = picture.CaptureScreen();

                return CropImage(screen,
                    new Rectangle(ChaosBot.Recalc(1326), PixelAimbot.ChaosBot.Recalc(229, false),
                        ChaosBot.Recalc(544), ChaosBot.Recalc(640, false)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("BITMAP Screenshot {0} {1}", ex.GetType().Name, ex.Message);
                return null;
            }
        }

        public static TimeSpan ChaosTime;
        public static DateTime ChaosStart;
        public static DateTime ChaosStop;

        public static Bitmap StartInventar;
        public static Bitmap EndInventar;

        public static Bitmap StartInvColor;
        public static string startinv;

        public static Bitmap EndInvColor;
        public static string endinv;

        public static Bitmap StartInvGray;
        public static Bitmap EndInvGray;

        public static Bitmap SaveForUser;
        public static string SaveFor;

        private async void btnStart_Click(object sender, EventArgs e)
        {
          
            if (_start == false)
            {
                try
                {
                    Process[] processName = Process.GetProcessesByName("LostArk");
                    if (processName.Length == 0 && chBoxCrashDetection.Checked)
                    {
                        GameCrashed();
                        return;
                    }

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

                        if (chBoxCompare.Checked)
                        {
                            ChaosAllRounds = 0;
                            ChaosAllStucks = 0;
                            ChaosRedStages = 0;
                            ChaosGameCrashed = 0;

                            ChaosStart = DateTime.Now;


                            processName = Process.GetProcessesByName("LostArk");
                            if (processName.Length == 1)
                            {
                                handle = processName[0].MainWindowHandle;
                                SetForegroundWindow(handle);
                            }

                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                            await Task.Delay(_humanizer.Next(10, 240) + 1000, _token);

                            StartInventar = GetStartPic();

                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_I);
                            await Task.Delay(_humanizer.Next(10, 240) + 500, _token);
                        }
                    }


                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "READY!"));
                    _start = true;
                    _stop = true;
                    Cts = new CancellationTokenSource();
                    var token = Cts.Token;
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    var t1 = Task.Run(() => Start(token), token);

                    token.ThrowIfCancellationRequested(); // DELETE IF RESTART DONT WORK // SHIIIKK

                    if (chBoxAutoRepair.Checked && _repairReset)
                    {
                        _repairReset = false;
                        _repairTimer = DateTime.Now.AddMinutes(Convert.ToDouble(txtRepair.Text));
                    }

                    if (chBoxLOGOUT.Checked)
                    {
                        var dateNow = DateTime.Now;
                        _Logout = cmbHOUR.SelectedIndex < dateNow.Hour
                            ? new DateTime(dateNow.Year, dateNow.Month, dateNow.Day + 1, cmbHOUR.SelectedIndex,
                                cmbMINUTE.SelectedIndex, 00)
                            : new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, cmbHOUR.SelectedIndex,
                                cmbMINUTE.SelectedIndex, 00);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is Layout_Keyboard currentLayout)
            {
                lbPQ.Text = lb2Q.Text = lbQ.Text = translateKey(currentLayout.Q);
                lbPW.Text = lb2W.Text = lbW.Text = translateKey(currentLayout.W);
                lbPE.Text = lb2E.Text = lbE.Text = translateKey(currentLayout.E);
                lbPR.Text = lb2R.Text = lbR.Text = translateKey(currentLayout.R);
                lbPA.Text = lb2A.Text = lbA.Text = translateKey(currentLayout.A);
                lbPS.Text = lb2S.Text = lbS.Text = translateKey(currentLayout.S);
                lbPD.Text = lb2D.Text = lbD.Text = translateKey(currentLayout.D);
                lbPF.Text = lb2F.Text = lbF.Text = translateKey(currentLayout.F);
                txBoxUltimateKey2.Text = txBoxUltimateKey.Text = translateKey(currentLayout.Y);
            }
        }

        private void comboBoxMouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentMouseButton = comboBoxMouse.SelectedIndex == 0
                ? KeyboardWrapper.VK_LBUTTON
                : KeyboardWrapper.VK_RBUTTON;
        }

        private void cmbHealKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHealKey.SelectedIndex == 0)
            {
                _currentHealKey = KeyboardWrapper.VK_F1;
            }
            else if (cmbHealKey.SelectedIndex == 1)
            {
                _currentHealKey = KeyboardWrapper.VK_1;
            }
            else if (cmbHealKey.SelectedIndex == 2)
            {
                _currentHealKey = KeyboardWrapper.VK_2;
            }
            else if (cmbHealKey.SelectedIndex == 3)
            {
                _currentHealKey = KeyboardWrapper.VK_3;
            }
            else if (cmbHealKey.SelectedIndex == 4)
            {
                _currentHealKey = KeyboardWrapper.VK_4;
            }
            else if (cmbHealKey.SelectedIndex == 5)
            {
                _currentHealKey = KeyboardWrapper.VK_5;
            }
            else if (cmbHealKey.SelectedIndex == 6)
            {
                _currentHealKey = KeyboardWrapper.VK_6;
            }
            else if (cmbHealKey.SelectedIndex == 7)
            {
                _currentHealKey = KeyboardWrapper.VK_7;
            }
            else if (cmbHealKey.SelectedIndex == 8)
            {
                _currentHealKey = KeyboardWrapper.VK_8;
            }
            else if (cmbHealKey.SelectedIndex == 9)
            {
                _currentHealKey = KeyboardWrapper.VK_9;
            }
        }

        private void ChaosBot_Load(object sender, EventArgs e)
        {
            tabControl2.ItemSize = new Size(75, 15);
            List<Layout_Keyboard> layout = new List<Layout_Keyboard>();
            if (layout == null) throw new ArgumentNullException(nameof(layout));
            Layout_Keyboard qwertz = new Layout_Keyboard
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
            layout.Add(qwertz);

            Layout_Keyboard qwerty = new Layout_Keyboard
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
            layout.Add(qwerty);

            Layout_Keyboard azerty = new Layout_Keyboard
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
                Y = KeyboardWrapper.VK_W,
            };
            layout.Add(azerty);
            comboBox1.DataSource = layout;
            comboBox1.DisplayMember = "LAYOUTS";
            comboBoxMouse.SelectedIndex = 0;

            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);

            SetupControls();


            HealthPercent = HealthSlider1.Value;
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

        private void chBoxNPCRepair_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxNPCRepair.Checked)
            {
                txtRepair.ReadOnly = false;
                chBoxAutoRepair.Checked = false;
                chBoxValtanAltQ.Checked = true;
            }
            else if (!chBoxAutoRepair.Checked && !chBoxNPCRepair.Checked)
            {
                chBoxValtanAltQ.Checked = rotation.chBoxValtanAltQ;
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
                SetupControls();
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

        private void SetupControls()
        {
            steampath = @"C:\Program Files (x86)\Steam\steam.exe";
            chBoxCrashDetection.Checked = true;
            checkBoxDiscordNotifications.Checked = true;
            HealthSlider1.Value = 801;
            cmbGunlancer.SelectedIndex = 0;
            cmbBard.SelectedIndex = 0;
            txtRestart.Text = "15";
            chBoxRevive.Checked = false;
            txtDeath.Text = "70";
            chBoxAutoMovement.Checked = true;
            txLeaveTimerFloor2.Text = "165";
            chBoxUnstuckF1.Checked = true;
            chBoxLOGOUT.Checked = false;

            chBoxAutoRepair.Checked = false;
            txtRepair.Text = "10";
            chBoxY.Checked = false;
            chBoxPaladin.Checked = false;
            chBoxBerserker.Checked = false;
            cmbDeathblade.SelectedIndex = 0;

            chBoxSorcerer.Checked = false;
            chBoxSharpshooter.Checked = false;
            chBoxSoulfist.Checked = false;
            chBoxChannelSwap.Checked = false;
            chBoxAutoMovement.Checked = false;
            chBoxActivateF2.Checked = false;
            txtDungeon2search.Text = "5";
            txtDungeon2.Text = "15";
            txCoolQ.Text = "500";
            txCoolW.Text = "500";
            txCoolE.Text = "500";
            txCoolR.Text = "500";
            txCoolA.Text = "500";
            txCoolS.Text = "500";
            txCoolD.Text = "500";
            txCoolF.Text = "500";
            cmbHOUR.Text = "";
            txQ.Text = "500";
            txW.Text = "500";
            txE.Text = "500";
            txR.Text = "500";
            txA.Text = "500";
            txS.Text = "500";
            txD.Text = "500";
            txF.Text = "500";

            txPQ.Text = "1";
            txPW.Text = "2";
            txPE.Text = "3";
            txPR.Text = "4";
            txPA.Text = "5";
            txPS.Text = "6";
            txPD.Text = "7";
            txPF.Text = "8";
            chBoxDoubleQ.Checked = false;
            chBoxDoubleW.Checked = false;
            chBoxDoubleE.Checked = false;
            chBoxDoubleR.Checked = false;
            chBoxDoubleA.Checked = false;
            chBoxDoubleS.Checked = false;
            chBoxDoubleD.Checked = false;
            chBoxDoubleF.Checked = false;

            
            textBoxAutoAttack.Text = "1500";

            chBoxAwakening.Checked = false;
            cmbHOUR.SelectedIndex = 0;
            cmbMINUTE.SelectedIndex = 0;
            chBoxLeavetimer.Checked = false;
            radioEnglish.Checked = true;
            radioGerman.Checked = false;
            cmbHealKey.SelectedIndex = 0;
            chBoxValtanAltQ.Checked = false;
            chBoxCompare.Checked = false;
            chBoxNPCRepair.Checked = false;
            chBoxGlavier.Checked = false;
            chBoxCooldownDetection.Checked = true;
            cmBoxEsoterik1.SelectedIndex = 0;
            cmBoxEsoterik2.SelectedIndex = 0;
            cmBoxEsoterik3.SelectedIndex = 0;
            cmBoxEsoterik4.SelectedIndex = 0;
            chBoxRedStage.Checked = false;
            cmbDestroyer.SelectedIndex = 0;
            txCharSelect.Text = "1";
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            frmGuide form = new frmGuide();
            form.Show();
        }


        private void buttonSaveRotation_Click(object sender, EventArgs e)
        {
            if (comboBoxRotations.Text != "")
            {
                if (comboBoxRotations.Text != "main")
                {
                    rotation.steampath = steampath;
                    rotation.chBoxCrashDetection = chBoxCrashDetection.Checked;
                    rotation.checkBoxDiscordNotifications = checkBoxDiscordNotifications.Checked;
                    rotation.HealthSlider1 = HealthSlider1.Value;
                    rotation.cmbGunlancer = cmbGunlancer.SelectedIndex;
                    rotation.cmbBard = cmbBard.SelectedIndex;
                    rotation.txtDeath = txtDeath.Text;
                    rotation.chBoxRevive = chBoxRevive.Checked;
                    rotation.txLeaveTimerFloor2 = txLeaveTimerFloor2.Text;

                    rotation.txtRestart = txtRestart.Text;
                    rotation.chBoxUnstuckF1 = chBoxUnstuckF1.Checked;

                    rotation.chBoxAutoRepair = chBoxAutoRepair.Checked;
                    rotation.autorepair = txtRepair.Text;

                    rotation.autologout = cmbHOUR.Text;
                    rotation.chBoxautologout = chBoxLOGOUT.Checked;
                    rotation.chBoxAutoMovement = chBoxAutoMovement.Checked;
                    rotation.autorepair = txtRepair.Text;
                    rotation.chBoxShadowhunter = chBoxY.Checked;
                    rotation.chboxPaladin = chBoxPaladin.Checked;
                    rotation.chBoxBerserker = chBoxBerserker.Checked;

                    rotation.cmbDeathblade = cmbDeathblade.SelectedIndex;

                    rotation.chBoxSharpshooter = chBoxSharpshooter.Checked;
                    rotation.chBoxSoulfist = chBoxSoulfist.Checked;
                    rotation.chBoxSorcerer = chBoxSorcerer.Checked;

                    rotation.chBoxChannelSwap = chBoxChannelSwap.Checked;
                    rotation.chBoxSaveAll = chBoxAutoMovement.Checked;
                    rotation.chBoxActivateF2 = chBoxActivateF2.Checked;


                    rotation.txtDungeon2Search = txtDungeon2search.Text;
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
                    rotation.txPq = txPQ.Text;
                    rotation.txPw = txPW.Text;
                    rotation.txPe = txPE.Text;
                    rotation.txPr = txPR.Text;
                    rotation.txPa = txPA.Text;
                    rotation.txPs = txPS.Text;
                    rotation.txPd = txPD.Text;
                    rotation.txPf = txPF.Text;
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
                    rotation.cmbMinute = cmbMINUTE.SelectedIndex;
                    rotation.cmbHour = cmbHOUR.SelectedIndex;
                    rotation.chBoxLeavetimer = chBoxLeavetimer.Checked;
                    rotation.radioGerman = radioGerman.Checked;
                    rotation.radioEnglish = radioEnglish.Checked;
                    rotation.cmbHealKey = cmbHealKey.SelectedIndex;
                    rotation.chBoxValtanAltQ = chBoxValtanAltQ.Checked;
                    rotation.chBoxCompare = chBoxCompare.Checked;
                    rotation.chBoxNpcRepair = chBoxNPCRepair.Checked;
                    rotation.chBoxGlavier = chBoxGlavier.Checked;
                    rotation.chBoxCooldownDetection = chBoxCooldownDetection.Checked;
                    rotation.cmBoxEsoterik1 = cmBoxEsoterik1.SelectedIndex;
                    rotation.cmBoxEsoterik2 = cmBoxEsoterik2.SelectedIndex;
                    rotation.cmBoxEsoterik3 = cmBoxEsoterik3.SelectedIndex;
                    rotation.cmBoxEsoterik4 = cmBoxEsoterik4.SelectedIndex;
                    rotation.chBoxRedStage = chBoxRedStage.Checked;
                    rotation.cmbDestroyer = cmbDestroyer.SelectedIndex;
                    rotation.cmbBard = cmbBard.SelectedIndex;
                    rotation.txCharSelect = txCharSelect.Text;


                    rotation.Save(comboBoxRotations.Text);
                    Alert.Show("Rotation \"" + comboBoxRotations.Text + "\" saved");
                }
                else
                {
                    Alert.Show("Rotation can not be named \"main\"", FrmAlert.EnmType.Error);
                }
            }
            else
            {
                Alert.Show("Rotation name cannot be clear!", FrmAlert.EnmType.Error);
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
                steampath = rotation.steampath;
                chBoxCrashDetection.Checked = rotation.chBoxCrashDetection;
                checkBoxDiscordNotifications.Checked = rotation.checkBoxDiscordNotifications;
                HealthSlider1.Value = rotation.HealthSlider1;
                txtDeath.Text = rotation.txtDeath;
                chBoxRevive.Checked = rotation.chBoxRevive;
                txtRestart.Text = rotation.txtRestart;
                cmbGunlancer.SelectedIndex = rotation.cmbGunlancer;
                cmbBard.SelectedIndex = rotation.cmbBard;
                chBoxUnstuckF1.Checked = rotation.chBoxUnstuckF1;

                chBoxAutoRepair.Checked = rotation.chBoxAutoRepair;
                txtRepair.Text = rotation.autorepair;
                chBoxY.Checked = rotation.chBoxShadowhunter;
                chBoxPaladin.Checked = rotation.chboxPaladin;
                chBoxBerserker.Checked = rotation.chBoxBerserker;
                cmbDeathblade.SelectedIndex = rotation.cmbDeathblade;
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
                txtDungeon2search.Text = rotation.txtDungeon2Search;
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
                txPQ.Text = rotation.txPq;
                txPW.Text = rotation.txPw;
                txPE.Text = rotation.txPe;
                txPR.Text = rotation.txPr;
                txPA.Text = rotation.txPa;
                txPS.Text = rotation.txPs;
                txPD.Text = rotation.txPd;
                txPF.Text = rotation.txPf;
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
                cmbHOUR.SelectedIndex = rotation.cmbHour;
                cmbMINUTE.SelectedIndex = rotation.cmbMinute;
                chBoxLeavetimer.Checked = rotation.chBoxLeavetimer;
                radioEnglish.Checked = rotation.radioEnglish;
                radioGerman.Checked = rotation.radioGerman;
                cmbHealKey.SelectedIndex = rotation.cmbHealKey;
                chBoxValtanAltQ.Checked = rotation.chBoxValtanAltQ;
                chBoxCompare.Checked = rotation.chBoxCompare;
                chBoxNPCRepair.Checked = rotation.chBoxNpcRepair;
                chBoxGlavier.Checked = rotation.chBoxGlavier;
                chBoxCooldownDetection.Checked = rotation.chBoxCooldownDetection;
                cmBoxEsoterik1.SelectedIndex = rotation.cmBoxEsoterik1;
                cmBoxEsoterik2.SelectedIndex = rotation.cmBoxEsoterik2;
                cmBoxEsoterik3.SelectedIndex = rotation.cmBoxEsoterik3;
                cmBoxEsoterik4.SelectedIndex = rotation.cmBoxEsoterik4;
                chBoxRedStage.Checked = rotation.chBoxRedStage;
                cmbDestroyer.SelectedIndex = rotation.cmbDestroyer;
                txCharSelect.Text = rotation.txCharSelect;
      

                _currentMouseButton = comboBoxMouse.SelectedIndex == 0
                    ? KeyboardWrapper.VK_LBUTTON
                    : KeyboardWrapper.VK_RBUTTON;
                Alert.Show("Rotation \"" + comboBoxRotations.Text + "\" loaded");
            }
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
          
                Cts.Cancel();
                CtsBoss.Cancel();
                CtsSkills.Cancel();
          
            //_botIsRun = false;
            _discordBotIsRun = false;
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            if (Application.OpenForms.OfType<GatheringBot>().Count() == 1)
                Application.OpenForms.OfType<GatheringBot>().First().Close();

            GatheringBot form = new GatheringBot();
            form.Show();
            Application.OpenForms.OfType<ChaosBot>().First().Hide();
            Application.OpenForms.OfType<ChaosBot>().First().Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label18.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void chBoxLeavetimer_MouseClick(object sender, MouseEventArgs e)
        {
            //if (chBoxLeavetimer.Checked)
            //{

            //    DialogResult dialogResult = MessageBox.Show("By activating the checkbox, you can\n" +
            //                    "determine when the bot should leave the dungeon\n" +
            //                    "However, this means that the bot may not be able\n" +
            //                    "to defeat the boss in time!\n\n" +
            //                    "If you want to activate the Auto Leave after\n" +
            //                    "Defeat the Boss, then 'deactivate' the checkbox!\n\n" +
            //                    "Do you really want to 'Activate' the Manual Leavetimer?\n" +
            //                    "Then press Yes, otherwise press No.",
            //                    "You activated manual Dungeon Leave!", MessageBoxButtons.YesNo);

            //    if (dialogResult == DialogResult.Yes)
            //    {
            //        chBoxLeavetimer.Checked = true;
            //    }
            //    else if (dialogResult == DialogResult.No)
            //    {
            //        chBoxLeavetimer.Checked = false;
            //    }
            //}
            //else if (!chBoxLeavetimer.Checked)
            //{

            //    DialogResult dialogResult = MessageBox.Show("If you have difficulties with this, please activate\n" +
            //                    "the LEAVETIMER checkbox until we have provided an update.\n\n" +
            //                    "Do you really want to 'Deactivate' the Manual Leavetimer?\n" +
            //                    "Then press Yes, otherwise press No.",
            //                    "You activated auto Dungeon Leave!", MessageBoxButtons.YesNo);

            //    if (dialogResult == DialogResult.Yes)
            //    {
            //        chBoxLeavetimer.Checked = false;
            //    }
            //    else if (dialogResult == DialogResult.No)
            //    {
            //        chBoxLeavetimer.Checked = true;
            //    }
            //}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            frmStatistic form = new frmStatistic();
            form.Show();
        }

        private void comboBoxRotations_MouseEnter(object sender, EventArgs e)
        {
            RefreshRotationCombox();
        }

        private void chBoxCooldownDetection_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxCooldownDetection.Checked)
            {
                groupBox8.Visible =
                    label13.Visible = false;

                lbAutoDetectHint.Visible =
                    cmBoxEsoterik4.Visible =
                        cmBoxEsoterik1.Visible =
                            cmBoxEsoterik2.Visible =
                                cmBoxEsoterik3.Visible =
                                    lbSpecialSkills.Visible =
                                        btnSpecialSkillsInfo.Visible = true;
                cmBoxEsoterik1.SelectedIndex = rotation.cmBoxEsoterik1;
                cmBoxEsoterik2.SelectedIndex = rotation.cmBoxEsoterik2;
                cmBoxEsoterik3.SelectedIndex = rotation.cmBoxEsoterik3;
                cmBoxEsoterik4.SelectedIndex = rotation.cmBoxEsoterik4;
            }
            else
            {
                groupBox8.Visible = label13.Visible = true;

                lbAutoDetectHint.Visible =
                    cmBoxEsoterik1.Visible =
                        cmBoxEsoterik2.Visible =
                            cmBoxEsoterik3.Visible =
                                cmBoxEsoterik4.Visible =
                                    lbSpecialSkills.Visible =
                                        btnSpecialSkillsInfo.Visible = false;

                cmBoxEsoterik1.SelectedIndex =
                    cmBoxEsoterik2.SelectedIndex =
                        cmBoxEsoterik3.SelectedIndex =
                            cmBoxEsoterik4.SelectedIndex = 0;
            }
        }

        private void btnSpecialSkillsInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Different classes have special abilities such as Wardancer esotericism.\n" +
                            "These abilities are grayscaled at the start of the dungeon and only\n" +
                            "become usable after a certain moment.\n\n" +
                            "If you use such abilities then select the appropriate\n" +
                            "key on which the ability sits.The bot does the rest on its own.\n\n" +
                            "You can deposit up to 4 such abilities.\n\n" +
                            "If you don't want to use any, then set all boxes to 'OFF'",
                "Special Abilitys like Esoterik etc.");
        }

        private void chBoxBossKillLeave_MouseClick(object sender, MouseEventArgs e)
        {
            chBoxBossKillLeave.Checked = true;
        }

        private void chBoxRedStage_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chBoxRedStage_MouseClick(object sender, MouseEventArgs e)
        {
            //chBoxRedStage.Checked = false;
        }

        private void chBoxAwakening2_CheckedChanged(object sender, EventArgs e)
        {
            chBoxAwakening.Checked = chBoxAwakening2.Checked;
        }

        private void chBoxAwakening_CheckedChanged(object sender, EventArgs e)
        {
            chBoxAwakening2.Checked = chBoxAwakening.Checked;
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Process.Start("https://user.symbiotic.link");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var t9 = Task.Run(() => Game_Restart());
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void txCharSelect_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                int x = Convert.ToInt16(txCharSelect.Text);
                if (x == 0 || x > 7)
                {
                    MessageBox.Show("You can only set Characters from 1 to 7!");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txInfo_MouseClick(object sender, MouseEventArgs e)
        {
            Label tb = (Label)sender;
            int visibleTime = 10000;  //in milliseconds
            ToolTip tt = new ToolTip();
            tt.Show("You can change ULTIMATE KEY\n" +
                    "under Classes -> Cast-Time Section.\n\n"+
                    
                    "Change your Keyboard Layout on\n"+
                    "this DropDown Menu.", tb, 0, 0, visibleTime);

        }

        private void btnSteamPath_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = @"C:\";
            dialog.IsFolderPicker = false;
            MessageBox.Show("Select Steam.exe\n\n"+ @"Example: C:\Program Files (x86)\Steam\steam.exe");
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                steampath = dialog.FileName;
                MessageBox.Show("Your Selection: "+dialog.FileName);
            }
        }

        private void comboBoxRotations_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private async void TESTBUTTON_Click(object sender, EventArgs e)
        //{
        //    Process[] processName;
        //    processName = Process.GetProcessesByName("LostArk");
        //    if (processName.Length == 1)
        //    {
        //        handle = processName[0].MainWindowHandle;
        //        SetForegroundWindow(handle);
        //    }

        //    Cts = new CancellationTokenSource();
        //    var token = Cts.Token;
        //    token.ThrowIfCancellationRequested();
        //    await Task.Delay(1, token);
        //    var t1 = Task.Run(() => Repair(token), token);

        //    token.ThrowIfCancellationRequested();
        //}
    }
}