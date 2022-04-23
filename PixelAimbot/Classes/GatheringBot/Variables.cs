using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private bool _start;

        private bool _stop;
        private bool _repair;
        private bool _canrepair;
        private bool _logout;
        private bool _fishing = false;
        private bool _buff;
        private bool _canFish = true;
        private bool _restart;
        private int _swap;
        private int _x, _y, _width, _height;
        private int _rodCounter;
        private bool _telegramBotRunning;

        PrintScreen _screenPrinter = new PrintScreen();

        private Image _rawScreen;
        private Bitmap _bitmapImage;
        Image<Bgr, byte> _screenCapture;

        public frmMinimized FormMinimized = new frmMinimized();
        public static GatheringBot _GatheringBot;

        private Config _conf = new Config();


        static int _screenWidth = Screen.PrimaryScreen.Bounds.Width;
        static int _screenHeight = Screen.PrimaryScreen.Bounds.Height;
        public Task TelegramTask;
        private CancellationTokenSource _telegramToken = new CancellationTokenSource();
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private bool _botIsRun = true;

        private string _resourceFolder = "";

        private static readonly Random random = new Random();
    }
}