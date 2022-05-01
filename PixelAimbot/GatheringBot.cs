using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PixelAimbot
{
    public partial class GatheringBot : Form
    {
    
        public GatheringBot()
        {
            InitializeComponent();
            _conf = Config.Load();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Recalc(0), Recalc(842, false));
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string applicationFolder = Path.Combine(folder, "cb_res");
            _GatheringBot = this;
            _resourceFolder = applicationFolder;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = RandomString(15);
            _telegramToken = new CancellationTokenSource();
            _discordToken = new CancellationTokenSource();
            if (_conf.telegram != "" && !_telegramBotRunning)
            {
                textBoxTelegramAPI.Text = _conf.telegram;
                try
                {
                    buttonTestTelegram_Click_1(null, null);
                    TelegramTask = TelegramBotAsync(_conf.telegram, _telegramToken.Token);
                }
                catch
                {
                    // ignored
                }
            }
            try
            {
                conf = Config.Load();
                DiscordTask = DiscordBotAsync(conf.discorduser, _discordToken.Token);
            } catch {}
            
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



      

        private void btnPause_Click(object sender, EventArgs e)

        {
            if (_stop == true)
            {
                _cts.Cancel();
                _minigameFound = false;
                _checkEnergy = true;
                _start = false;
                _stop = false;
                _canrepair = false;
                FormMinimized.Hide();
                FormMinimized.sw.Reset();
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
                    FormMinimized.StartPosition = FormStartPosition.Manual;
                    FormMinimized.updateLabel("Gatheringbot");
                    FormMinimized.Location = new Point(0, Recalc(28, false));
                    FormMinimized.timerRuntimer.Enabled = true;
                    FormMinimized.sw.Reset();
                    FormMinimized.sw.Start();
                    FormMinimized.Show();
                    FormMinimized.Location = new Point(0, Recalc(28, false));
                    FormMinimized.Size = new Size(594, 28);

                    this.Hide();
                    _checkEnergy = true;
                    _minigameFound = false;
                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Bot is starting..."));
                    _start = true;
                    _stop = true;
                    _cts = new CancellationTokenSource();
                    var token = _cts.Token;

                    var t1 = Task.Run(() => Start(token));
                    if (chBoxAutoBuff.Checked == true)
                    {
                        _buff = true;
                    }
                    else
                    {
                        _buff = false;
                    }

                    if (chBoxLOGOUT.Checked == true && _start == true)
                    {
                        var logout = Task.Run(() => LOGOUTTIMER(token));
                    }
                    else
                    {
                        _logout = false;
                    }

                    await Task.WhenAny(new[] {t1});
                }
                catch(Exception ex)
                {
                    int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                    Debug.WriteLine("[" + line + "]" + ex.Message);
                }
            }
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


        private void btnInstructions_Click(object sender, EventArgs e)
        {
            frmGuide Form = new frmGuide();
            Form.Show();
        }


        private void lbStatus_TextChanged(object sender, EventArgs e)
        {
            FormMinimized.labelMinimizedState.Text = lbStatus.Text;
        }

        

        private void buttonSelectArea_Click(object sender, EventArgs e)
        {
            this.Hide();
            Alert.Show("Submit with Enter or Doubleclick\r\nto selected area", frmAlert.enmType.Info);
            SelectArea form1 = new SelectArea(false);
            form1.InstanceRef = this;
            form1.Show();
        }


        private void labelSwap_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
            _botIsRun = false;
            _discordBotIsRun = false;
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            
            if (Application.OpenForms.OfType<PixelAimbot.ChaosBot>().Count() == 1)
                Application.OpenForms.OfType<PixelAimbot.ChaosBot>().First().Close();

            ChaosBot Form = new ChaosBot();
            Form.Show();
            Application.OpenForms.OfType<PixelAimbot.GatheringBot>().First().Hide();
            Application.OpenForms.OfType<PixelAimbot.GatheringBot>().First().Close();
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

            Thread.Sleep(500);
            var template = new Image<Bgr, byte>(_resourceFolder + "/gathering.png");
            var mask = new Image<Bgr, byte>(_resourceFolder + "/gathering.png");


            var detector = new ScreenDetector(template, mask, 0.75f, ChaosBot.Recalc(550),
                ChaosBot.Recalc(997, false), ChaosBot.Recalc(56), ChaosBot.Recalc(54, false));
            using (_screenCapture = new Bitmap(_screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
            {
                var item = detector.GetBest(_screenCapture, false);
                if (item.HasValue)
                {
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_B);
                }
            }

            Thread.Sleep(1000);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_L);
            VirtualMouse.MoveTo(ChaosBot.Recalc(435), ChaosBot.Recalc(741, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
            VirtualMouse.MoveTo(ChaosBot.Recalc(666), ChaosBot.Recalc(489, false), 5);
            KeyboardWrapper.KeyDown(KeyboardWrapper.VK_LBUTTON);
            VirtualMouse.MoveTo(ChaosBot.Recalc(698), ChaosBot.Recalc(998, false), 5);
            KeyboardWrapper.KeyUp(KeyboardWrapper.VK_LBUTTON);
            VirtualMouse.MoveTo(ChaosBot.Recalc(666), ChaosBot.Recalc(588, false), 5);
            KeyboardWrapper.KeyDown(KeyboardWrapper.VK_LBUTTON);
            VirtualMouse.MoveTo(ChaosBot.Recalc(744), ChaosBot.Recalc(998, false), 5);
            KeyboardWrapper.KeyUp(KeyboardWrapper.VK_LBUTTON);
            // Lvl 30
            VirtualMouse.MoveTo(ChaosBot.Recalc(666), ChaosBot.Recalc(780, false), 5);
            KeyboardWrapper.KeyDown(KeyboardWrapper.VK_LBUTTON);
            VirtualMouse.MoveTo(ChaosBot.Recalc(799), ChaosBot.Recalc(998, false), 5);
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