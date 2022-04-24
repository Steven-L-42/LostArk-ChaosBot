﻿using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelAimbot
{
    public partial class ChaosBot : Form
    {
       

       
        public ChaosBot()
        {
            InitializeComponent();
            conf = Config.Load();
          
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Recalc(0), Recalc(842, false));
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string applicationFolder = Path.Combine(folder, "cb_res");

            resourceFolder = applicationFolder;
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
            if (conf.telegram != "" && !_telegramBotRunning)
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


        private async void btnPause_Click(object sender, EventArgs e)
        {
            if (_stop == true)
            {
                cts.Cancel();
                _formExists = 0;
                _RepairReset = true;
                _start = false;
                _stopp = false;
                _stop = false;
                _restart = false;
                _logout = false;
                _repair = false;

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
                _floor3 = false;
                _floorFight = false;
                _searchboss = false;

                _revive = false;
                _portaldetect = false;
                _ultimate = false;
                _potions = false;

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
                    _formExists++;
                    if (_formExists == 1)
                    {
                        FormMinimized.StartPosition = FormStartPosition.Manual;
                        FormMinimized.Location = new Point(0, Recalc(28, false));
                        FormMinimized.timerRuntimer.Enabled = true;
                        FormMinimized.sw.Reset();
                        FormMinimized.sw.Start();
                        FormMinimized.Show();
                        FormMinimized.Size = new Size(594, 28);
                        this.Hide();
                    }


                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "READY!"));
                    _start = true;
                    _stop = true;
                    cts = new CancellationTokenSource();
                    var token = cts.Token;

                    var t1 = Task.Run(() => Start(token));

                    if (chBoxAutoRepair.Checked == true && _RepairReset == true)
                    {
                        _RepairReset = false;
                        var repair = Task.Run(() => Repairtimer());
                    }
                    else
                    {
                        _repair = false;
                    }

                    if (chBoxLOGOUT.Checked == true)
                    {
                        var logout = Task.Run(() => Logouttimer());
                    }
                    else
                    {
                        _logout = false;
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
            _currentLayout = comboBox1.SelectedItem as Layout_Keyboard;
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);

            chBoxUnstuckF1.Checked = Properties.Settings.Default.chBoxUnstuckF1;
            txtRestart.Text = Properties.Settings.Default.txtRestart;

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
            textBoxAutoAttack.Text = Properties.Settings.Default.textBoxAutoAttack;
            chBoxAwakening.Checked = Properties.Settings.Default.chBoxAwakening;
            txtRevive.Text = Properties.Settings.Default.txtRevive;
            chBoxRevive.Checked = Properties.Settings.Default.chBoxRevive;
           
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


        private void chBoxAutoRepair_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAutoRepair.Checked)
            {
                txtRepair.ReadOnly = false;
            }
            else if (!chBoxAutoRepair.Checked)
            {
                txtRepair.ReadOnly = true;
                _repair = false;
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
                _logout = false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.chBoxGunlancer = false;
                Properties.Settings.Default.chBoxRevive = false;
                Properties.Settings.Default.txtRevive = "85";
                Properties.Settings.Default.txLeaveTimerFloor2 = "150";
                Properties.Settings.Default.txLeaveTimerFloor3 = "180";

                Properties.Settings.Default.chBoxUnstuckF1 = false;
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

                Properties.Settings.Default.Save();
                chBoxGunlancer.Checked = Properties.Settings.Default.chBoxGunlancer;
                txtRestart.Text = Properties.Settings.Default.txtRestart;
                chBoxRevive.Checked = Properties.Settings.Default.chBoxRevive;
                txtRevive.Text = Properties.Settings.Default.txtRevive;
                chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxAutoMovement;
                txLeaveTimerFloor3.Text = Properties.Settings.Default.txLeaveTimerFloor3;
                txLeaveTimerFloor2.Text = Properties.Settings.Default.txLeaveTimerFloor2;
                chBoxUnstuckF1.Checked = Properties.Settings.Default.chBoxUnstuckF1;
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
                textBoxAutoAttack.Text = Properties.Settings.Default.textBoxAutoAttack;

                chBoxAwakening.Checked = Properties.Settings.Default.chBoxAwakening;
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            frmGuide Form = new frmGuide();
            Form.Show();
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
                    rotation.chBoxUnstuckF1 = chBoxUnstuckF1.Checked;
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
                    rotation.textBoxAutoAttack = textBoxAutoAttack.Text;
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
                chBoxUnstuckF1.Checked = rotation.chBoxUnstuckF1;
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

                MessageBox.Show("Rotation \"" + comboBoxRotations.Text + "\" loaded");
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
            FormMinimized.labelMinimizedState.Text = lbStatus.Text;
        }

        
        private void labelSwap_Click_1(object sender, EventArgs e)
        {
            cts.Cancel();
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            if (Application.OpenForms.OfType<PixelAimbot.GatheringBot>().Count() == 1)
                Application.OpenForms.OfType<PixelAimbot.GatheringBot>().First().Close();

            GatheringBot Form = new GatheringBot();
            Form.Show();
            Application.OpenForms.OfType<PixelAimbot.ChaosBot>().First().Hide();
            Application.OpenForms.OfType<PixelAimbot.ChaosBot>().First().Close();
        }
        
    }
}