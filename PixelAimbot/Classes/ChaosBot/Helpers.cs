using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using IronOcr;
using Newtonsoft.Json.Linq;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        private void ChangeSkillSet(object sender, EventArgs e)
        {
            if (txPA.Text != "" && txPS.Text != "" && txPD.Text != "" && txPF.Text != "" && txPQ.Text != "" &&
                txPW.Text != "" && txPE.Text != "" && txPR.Text != "")
                _skills.skillset = new Dictionary<byte, int>
                {
                    {KeyboardWrapper.VK_A, int.Parse(txPA.Text)},
                    {KeyboardWrapper.VK_S, int.Parse(txPS.Text)},
                    {KeyboardWrapper.VK_D, int.Parse(txPD.Text)},
                    {KeyboardWrapper.VK_F, int.Parse(txPF.Text)},
                    {KeyboardWrapper.VK_Q, int.Parse(txPQ.Text)},
                    {KeyboardWrapper.VK_W, int.Parse(txPW.Text)},
                    {KeyboardWrapper.VK_E, int.Parse(txPE.Text)},
                    {KeyboardWrapper.VK_R, int.Parse(txPR.Text)}
                }.ToList();
        }

        private void CheckIsDigit(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        public static string ReadArea(Bitmap screenCapture, int x, int y, int width, int height, string whitelist = "")
        {
            tess.Configuration.EngineMode = TesseractEngineMode.TesseractAndLstm;
            tess.Language = OcrLanguage.EnglishBest;
            tess.MultiThreaded = true;
            tess.Configuration.ReadBarCodes = false;
            tess.Configuration.RenderSearchablePdfsAndHocr = false;
            tess.Configuration.PageSegmentationMode = TesseractPageSegmentationMode.Auto;
            if (whitelist != "")
            {
                tess.Configuration.WhiteListCharacters = whitelist;
            }


            string result = "";
            try
            {
                using (var input = new OcrInput())
                {
                    var contentArea = new Rectangle() {X = x, Y = y, Height = height, Width = width};
                    
                    input.AddImage(screenCapture, contentArea);
                    result = tess.Read(input).Text;
                    Debug.WriteLine(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }

            return null;
        }
       
        public  static Bitmap SetGrayscale(Bitmap img)
        {
    
            Bitmap temp = (Bitmap)img;
            Color c;
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    c = img.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);
    
                    img.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            return (Bitmap)img;
    
        }
        
        public static Bitmap RemoveNoise(Bitmap bmap)
        {
    
            for (var x = 0; x < bmap.Width; x++)
            {
                for (var y = 0; y < bmap.Height; y++)
                {
                    var pixel = bmap.GetPixel(x, y);
                    if (pixel.R < 162 && pixel.G < 162 && pixel.B < 162)
                        bmap.SetPixel(x, y, Color.Black);
                    else if (pixel.R > 162 && pixel.G > 162 && pixel.B > 162)
                        bmap.SetPixel(x, y, Color.White);
                }
            }
    
            return bmap;
        }
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int Between(int minimumValue, int maximumValue)
        {
            byte[] randomNumber = new byte[1];

            _generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            // We are using Math.Max, and substracting 0.00000000001, 
            // to ensure "multiplier" will always be between 0.0 and .99999999999
            // Otherwise, it's possible for it to be "1", which causes problems in our rounding.
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            // We need to add one to the range, to allow for the rounding done with Math.Floor
            int range = maximumValue - minimumValue + 1;

            double randomValueInRange = Math.Floor(multiplier * range);

            return (int) (minimumValue + randomValueInRange);
        }

        private string translateKey(int key)
        {
            string translate;
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

                case 0:
                    translate = "LEFT WALK";
                    break;

                case 1:
                    translate = "RIGHT WALK";
                    break;

                default:
                    translate = key.ToString();
                    break;
            }

            return translate;
        }

        private int CasttimeByKey(byte key)
        {
            var cooldownDuration = 500;
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

        public static (int, int) PixelToAbsolute(double x, double y, Point screenResolution)
        {
            int newX = (int) (x); // / screenResolution.X * 65535);
            int newY = (int) (y); // / screenResolution.Y * 65535);
            return (newX, newY);
        }

        public static (double, double) MinimapToDesktop(double x, double y, int additionalPercent = 20)
        {
            double calculatedPercentX = (x) / 394 * 100;
            double calculatedPercentY = (y) / 340 * 100;
            double multiplierX = 1;
            double multiplierY = 1;
            if (calculatedPercentX > 50)
            {
                calculatedPercentX += additionalPercent;
            }
            else if (calculatedPercentX < 50)
            {
                calculatedPercentX -= additionalPercent;
            }

            if (calculatedPercentY > 50)
            {
                calculatedPercentY += additionalPercent;
            }
            else if (calculatedPercentY < 50)
            {
                calculatedPercentY -= additionalPercent;
            }

            double resultX;
            double resultY;
            if (isWindowed)
            {
                resultX = 1920 / 100 * (calculatedPercentX * multiplierX);
                resultY = 1080 / 100 * (calculatedPercentY * multiplierY);
            }
            else
            {
                resultX = Screen.PrimaryScreen.Bounds.Width / 100 * (calculatedPercentX * multiplierX);
                resultY = Screen.PrimaryScreen.Bounds.Height / 100 * (calculatedPercentY * multiplierY);
            }
            
            
          //  resultX = ((resultX - (Screen.PrimaryScreen.Bounds.Width / 2)) * 0.5) +  
            return (resultX, resultY);
        }

        private byte UltimateKey(string text)
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
     
        public static Image<Bgr, Byte> byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Bitmap returnImage = (Bitmap) Image.FromStream(ms);

            return returnImage.ToImage<Bgr, byte>();
        }


        public static int Recalc(int value, bool horizontal = true, bool ignoreWindowed = false)
        {
            decimal oldResolution;
            decimal newResolution;
            int returnValue = value;
            if (isWindowed)
            {
                if (horizontal)
                {
                    if (ignoreWindowed)
                    {
                        return value;
                    }

                    return value + windowX;
                }
                else
                {
                    if (ignoreWindowed)
                    {
                        return value;
                    }

                    return value + windowY;
                }
            }
            else
            {
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

                if (oldResolution != newResolution)
                {
                    decimal normalized = (decimal) value * newResolution;
                    decimal rescaledPosition = (decimal) normalized / oldResolution;

                    returnValue = Decimal.ToInt32(rescaledPosition);
                }
            }

            return returnValue;
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

                    _timer = new System.Timers.Timer(cooldownDuration);
                    switch (key)
                    {
                        case KeyboardWrapper.VK_A:
                            _timer.Elapsed += (object source, ElapsedEventArgs e) => { _A = false; };

                            break;

                        case KeyboardWrapper.VK_S:
                            _timer.Elapsed += (object source, ElapsedEventArgs e) => { _S = false; };


                            break;

                        case KeyboardWrapper.VK_D:
                            _timer.Elapsed += (object source, ElapsedEventArgs e) => { _D = false; };


                            break;

                        case KeyboardWrapper.VK_F:
                            _timer.Elapsed += (object source, ElapsedEventArgs e) => { _F = false; };


                            break;

                        case KeyboardWrapper.VK_Q:
                            _timer.Elapsed += (object source, ElapsedEventArgs e) => { _Q = false; };
                            break;

                        case KeyboardWrapper.VK_W:
                            _timer.Elapsed += (object source, ElapsedEventArgs e) => { _W = false; };


                            break;

                        case KeyboardWrapper.VK_E:
                            _timer.Elapsed += (object source, ElapsedEventArgs e) => { _E = false; };


                            break;

                        case KeyboardWrapper.VK_R:
                            _timer.Elapsed += (object source, ElapsedEventArgs e) => { _R = false; };
                            break;
                    }

                    _timer.AutoReset = false;
                    _timer.Enabled = true;
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

        private void SetKeyCooldown(byte key)
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

        /// ///////////////////     /// ///////////////////

        // MAKE SCREENSHOT FROM ABILITYS
        // 
        public Image<Bgr, byte> skillQ;
        public Image<Bgr, byte> skillW;
        public Image<Bgr, byte> skillE;
        public Image<Bgr, byte> skillR;
        public Image<Bgr, byte> skillA;
        public Image<Bgr, byte> skillS;
        public Image<Bgr, byte> skillD;
        public Image<Bgr, byte> skillF;
       
        private void GetSkillQ()
        {
            try
            {
                AbilityScreen(Application.UserAppDataPath + "/SkillQ.jpg", ImageFormat.Jpeg, ChaosBot.Recalc(689), ChaosBot.Recalc(984, false));
                skillQ = new Image<Bgr, byte>(Application.UserAppDataPath + "/SkillQ.jpg");


                //var picture = new PrintScreen();
                //var screen = picture.CaptureScreen();

                //return CropImage(screen,
                //    new Rectangle(ChaosBot.Recalc(689), PixelAimbot.ChaosBot.Recalc(984, false),
                //        ChaosBot.Recalc(721), ChaosBot.Recalc(1008, false)));

            }
        catch (Exception ex)
            {
                Console.WriteLine("Skill Q Screenshot {0} {1}", ex.GetType().Name, ex.Message);
                //return null;
            }
        }
        private void GetSkillW()
        {
            try
            {

                AbilityScreen(Application.UserAppDataPath + "/SkillW.jpg", ImageFormat.Jpeg, ChaosBot.Recalc(733), ChaosBot.Recalc(984, false));
                skillW = new Image<Bgr, byte>(Application.UserAppDataPath + "/SkillW.jpg");
                //var picture = new PrintScreen();
                //var screen = picture.CaptureScreen();

                //return CropImage(screen,
                //   new Rectangle(ChaosBot.Recalc(733), PixelAimbot.ChaosBot.Recalc(985, false),
                //       ChaosBot.Recalc(768), ChaosBot.Recalc(1008, false)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Skill W Screenshot {0} {1}", ex.GetType().Name, ex.Message);
                //return null;
            }
        }
        private void GetSkillE()
        {
            try
            {
                AbilityScreen(Application.UserAppDataPath + "/SkillE.jpg", ImageFormat.Jpeg, ChaosBot.Recalc(781), ChaosBot.Recalc(984, false));
                skillE = new Image<Bgr, byte>(Application.UserAppDataPath + "/SkillE.jpg");

                //var picture = new PrintScreen();
                //var screen = picture.CaptureScreen();

                //return CropImage(screen,
                //    new Rectangle(ChaosBot.Recalc(781), PixelAimbot.ChaosBot.Recalc(985, false),
                //        ChaosBot.Recalc(815), ChaosBot.Recalc(1008, false)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Skill E Screenshot {0} {1}", ex.GetType().Name, ex.Message);
                //return null;
            }
        }
        private void GetSkillR()
        {
            try
            {
                AbilityScreen(Application.UserAppDataPath + "/SkillR.jpg", ImageFormat.Jpeg, ChaosBot.Recalc(827), ChaosBot.Recalc(984, false));
                skillR = new Image<Bgr, byte>(Application.UserAppDataPath + "/SkillR.jpg");

                //var picture = new PrintScreen();
                //var screen = picture.CaptureScreen();

                //return CropImage(screen,
                //    new Rectangle(ChaosBot.Recalc(827), PixelAimbot.ChaosBot.Recalc(985, false),
                //        ChaosBot.Recalc(860), ChaosBot.Recalc(1008, false)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Skill R Screenshot {0} {1}", ex.GetType().Name, ex.Message);
                //return null;
            }
        }
        private void GetSkillA()
        {
            try
            {
                AbilityScreen(Application.UserAppDataPath + "/SkillA.jpg", ImageFormat.Jpeg, ChaosBot.Recalc(709), ChaosBot.Recalc(1031, false));
                skillA = new Image<Bgr, byte>(Application.UserAppDataPath + "/SkillA.jpg");

                //var picture = new PrintScreen();
                //var screen = picture.CaptureScreen();

                //return CropImage(screen, 
                //     new Rectangle(ChaosBot.Recalc(709), PixelAimbot.ChaosBot.Recalc(1031, false),
                //         ChaosBot.Recalc(743), ChaosBot.Recalc(1055, false))); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Skill A Screenshot {0} {1}", ex.GetType().Name, ex.Message);
                //return null;
            }
        }
        private void GetSkillS()
        {
            try
            {
                AbilityScreen(Application.UserAppDataPath + "/SkillS.jpg", ImageFormat.Jpeg, ChaosBot.Recalc(757), ChaosBot.Recalc(1031, false));
                skillS = new Image<Bgr, byte>(Application.UserAppDataPath + "/SkillS.jpg");

                //var picture = new PrintScreen();
                //var screen = picture.CaptureScreen();

                //return CropImage(screen,
                //    new Rectangle(ChaosBot.Recalc(757), PixelAimbot.ChaosBot.Recalc(1031, false),
                //        ChaosBot.Recalc(790), ChaosBot.Recalc(1055, false)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Skill S Screenshot {0} {1}", ex.GetType().Name, ex.Message);
                //return null;
            }
        }
        private void GetSkillD()
        {
            try
            {
                AbilityScreen(Application.UserAppDataPath + "/SkillD.jpg", ImageFormat.Jpeg, ChaosBot.Recalc(805), ChaosBot.Recalc(1031, false));
                skillD = new Image<Bgr, byte>(Application.UserAppDataPath + "/SkillD.jpg");

                //var picture = new PrintScreen();
                //var screen = picture.CaptureScreen();

                //return CropImage(screen,
                //   new Rectangle(ChaosBot.Recalc(805), PixelAimbot.ChaosBot.Recalc(1031, false),
                //       ChaosBot.Recalc(837), ChaosBot.Recalc(1055, false)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Skill D Screenshot {0} {1}", ex.GetType().Name, ex.Message);
                //return null;
            }
        }
        private void GetSkillF()
        {
            try
            {
                AbilityScreen(Application.UserAppDataPath + "/SkillF.jpg", ImageFormat.Jpeg, ChaosBot.Recalc(850), ChaosBot.Recalc(1031, false));
                skillF = new Image<Bgr, byte>(Application.UserAppDataPath + "/SkillF.jpg");

                //var picture = new PrintScreen();
                //var screen = picture.CaptureScreen();

                //return CropImage(screen,
                //   new Rectangle(ChaosBot.Recalc(850), PixelAimbot.ChaosBot.Recalc(1031, false),
                //       ChaosBot.Recalc(882), ChaosBot.Recalc(1055, false)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Skill F Screenshot {0} {1}", ex.GetType().Name, ex.Message);
                //return null;
            }
        }

        // COMPARE SCREENSHOTS WITH OPENCV
        //
        public async Task SkillQ(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var template = skillQ;
              
                var detector = new ScreenDetector(template, null, 0.9f, ChaosBot.Recalc(689),
                    ChaosBot.Recalc(984, false), ChaosBot.Recalc(721, true, true), ChaosBot.Recalc(1008, false, true));

               detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
               var screenPrinter = new PrintScreen();

                while (_Q && !token.IsCancellationRequested)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                        {
                            var item = detector.GetBest(screenCapture, false);
                            if (item.HasValue)
                            {
                                _Q = false;
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
                    catch (Exception ex)
                    {
                        ExceptionHandler.SendException(ex);
                        int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                        Debug.WriteLine("[" + line + "]" + ex.Message);
                    }
                    await Task.Delay(100);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
        public async Task SkillW(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                var template = skillW;
                var detector = new ScreenDetector(template, null, 0.9f, ChaosBot.Recalc(733),
                    ChaosBot.Recalc(984, false), ChaosBot.Recalc(768, true, true), ChaosBot.Recalc(1008, false, true));

                detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
                var screenPrinter = new PrintScreen();
                while (_W && !token.IsCancellationRequested)
                {

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                        {
                            var item = detector.GetBest(screenCapture, false);
                            if (item.HasValue)
                            {
                                _W = false;
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
                    catch (Exception ex)
                    {
                        ExceptionHandler.SendException(ex);
                        int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                        Debug.WriteLine("[" + line + "]" + ex.Message);
                    }
                    await Task.Delay(100);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }

        }
        public async Task SkillE(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                var template = skillE;
                var detector = new ScreenDetector(template, null, 0.9f, ChaosBot.Recalc(781),
                    ChaosBot.Recalc(984, false), ChaosBot.Recalc(815, true, true), ChaosBot.Recalc(1008, false, true));

                detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
                var screenPrinter = new PrintScreen();
                while (_E && !token.IsCancellationRequested)
                {

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                        {
                            var item = detector.GetBest(screenCapture, false);
                            if (item.HasValue)
                            {
                                _E = false;
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
                    catch (Exception ex)
                    {
                        ExceptionHandler.SendException(ex);
                        int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                        Debug.WriteLine("[" + line + "]" + ex.Message);
                    }
                    await Task.Delay(100);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }

        }
        public async Task SkillR(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                var template = skillR;
                var detector = new ScreenDetector(template, null, 0.9f, ChaosBot.Recalc(827),
                    ChaosBot.Recalc(984, false), ChaosBot.Recalc(860, true, true), ChaosBot.Recalc(1008, false, true));

                detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
                var screenPrinter = new PrintScreen();
                while (_R && !token.IsCancellationRequested)
                {

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                        {
                            var item = detector.GetBest(screenCapture, false);
                            if (item.HasValue)
                            {
                                _R = false;
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
                    catch (Exception ex)
                    {
                        ExceptionHandler.SendException(ex);
                        int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                        Debug.WriteLine("[" + line + "]" + ex.Message);
                    }
                    await Task.Delay(100);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }

        }
        public async Task SkillA(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                var template = skillA;
                var detector = new ScreenDetector(template, null, 0.9f, ChaosBot.Recalc(709),
                    ChaosBot.Recalc(1031, false), ChaosBot.Recalc(743, true, true), ChaosBot.Recalc(1008, false, true));

                detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
                var screenPrinter = new PrintScreen();
                while (_A && !token.IsCancellationRequested)
                {

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                        {
                            var item = detector.GetBest(screenCapture, false);
                            if (item.HasValue)
                            {
                                _A = false;
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
                    catch (Exception ex)
                    {
                        ExceptionHandler.SendException(ex);
                        int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                        Debug.WriteLine("[" + line + "]" + ex.Message);
                    }
                    await Task.Delay(100);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }

        }
        public async Task SkillS(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                var template = skillS;
                var detector = new ScreenDetector(template, null, 0.9f, ChaosBot.Recalc(757),
                    ChaosBot.Recalc(1031, false), ChaosBot.Recalc(790, true, true), ChaosBot.Recalc(1055, false, true));
                
                detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
                var screenPrinter = new PrintScreen();
                while (_S && !token.IsCancellationRequested)
                {

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                        {
                            var item = detector.GetBest(screenCapture, false);
                            if (item.HasValue)
                            {
                                _S = false;
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
                    catch (Exception ex)
                    {
                        ExceptionHandler.SendException(ex);
                        int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                        Debug.WriteLine("[" + line + "]" + ex.Message);
                    }
                    await Task.Delay(100);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }

        }
        public async Task SkillD(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                var template = skillD;
                var detector = new ScreenDetector(template, null, 0.9f, ChaosBot.Recalc(805),
                    ChaosBot.Recalc(1031, false), ChaosBot.Recalc(837, true, true), ChaosBot.Recalc(1055, false, true));

                detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
                var screenPrinter = new PrintScreen();
                while (_D && !token.IsCancellationRequested)
                {

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                        {
                            var item = detector.GetBest(screenCapture, false);
                            if (item.HasValue)
                            {
                                _D = false;
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
                    catch (Exception ex)
                    {
                        ExceptionHandler.SendException(ex);
                        int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                        Debug.WriteLine("[" + line + "]" + ex.Message);
                    }
                    await Task.Delay(100);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }

        }
        public async Task SkillF(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                var template = skillF;
                var detector = new ScreenDetector(template, null, 0.9f, ChaosBot.Recalc(850),
                    ChaosBot.Recalc(1031, false), ChaosBot.Recalc(882, true, true), ChaosBot.Recalc(1055, false, true));

                detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
                var screenPrinter = new PrintScreen();
                while (_F && !token.IsCancellationRequested)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                        {
                            var item = detector.GetBest(screenCapture, false);
                            if (item.HasValue)
                            {
                                _F = false;
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
                    catch (Exception ex)
                    {
                        ExceptionHandler.SendException(ex);
                        int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                        Debug.WriteLine("[" + line + "]" + ex.Message);
                    }
                    await Task.Delay(100);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }

        }

        private void SetKeyCooldownGray(byte key)
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;
            switch (key)
            {
                case KeyboardWrapper.VK_A:
                    _A = true;
                    var t1 = Task.Run(() => SkillA(token));
                    break;

                case KeyboardWrapper.VK_S:
                    _S = true;
                    var t2 = Task.Run(() => SkillS(token));
                    break;

                case KeyboardWrapper.VK_D:
                    _D = true;
                    var t3 = Task.Run(() => SkillD(token));
                    break;

                case KeyboardWrapper.VK_F:
                    _F = true;
                    var t4 = Task.Run(() => SkillF(token));
                    break;

                case KeyboardWrapper.VK_Q:
                    _Q = true;
                    var t5 = Task.Run(() => SkillQ(token));
                    break;

                case KeyboardWrapper.VK_W:
                    _W = true;
                    var t6 = Task.Run(() => SkillW(token));
                    break;

                case KeyboardWrapper.VK_E:
                    _E = true;
                    var t7 = Task.Run(() => SkillE(token));
                    break;

                case KeyboardWrapper.VK_R:
                    _R = true;
                    var t8 = Task.Run(() => SkillR(token));
                    break;
            }
        }
        private bool isKeyOnCooldownGray(byte key)
        {
            var returnBoolean = false;
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

        /// ///////////////////     /// ///////////////////

        private bool isKeyOnCooldown(byte key)
        {
            var returnBoolean = false;
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

        private Point calculateFromCenter(int x, int y)
        {
            var centerX = screenWidth / 2;
            var centerY = screenHeight / 2;
            int resultX;
            int resultY;

            resultX = centerX - Recalc(500) + x;
            resultY = centerY - Recalc(390, false) + y;

            return new Point(resultX, resultY);
        }

        private bool IsDoubleKey(byte key)
        {
            var checkboxState = false;
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

        private static Bitmap CropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private Stream ToStream(Image image, ImageFormat format)
        {
            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        private void RefreshRotationCombox()
        {
            comboBoxRotations.Items.Clear();

            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            var webclient = new WebClient();
            var config = Config.Load();
            webclient.CachePolicy = noCachePolicy;
            var values = new NameValueCollection
            {
                ["user"] = config.username,
            };
            webclient.Headers.Add("Content-Type","application/x-www-form-urlencoded");
            webclient.UploadValuesAsync(new Uri("https://admin.symbiotic.link/api/getRotations"), "POST", values);
            webclient.UploadValuesCompleted += (s, e) =>
            {
                foreach (var entries in JArray.Parse(Encoding.Default.GetString(e.Result)))
                {
                    comboBoxRotations.Items.Add(entries["name"]);
                }
            };

            var files = Directory.GetFiles(ConfigPath);
            foreach (var file in files)
                if (Path.GetFileNameWithoutExtension(file) != "main")
                    File.Delete(file);
                    
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

       
        public void AbilityScreen(string filename, ImageFormat imageFormat, int sourceX, int sourceY)
        {
            using (Bitmap printscreen = new Bitmap(36, 28))
            {
                using (Graphics graphics = Graphics.FromImage(printscreen as Image))
                {
                    graphics.CopyFromScreen(sourceX, sourceY, 0, 0, printscreen.Size);
                }
                printscreen.Save(filename, imageFormat);
            }
        }
    }
}