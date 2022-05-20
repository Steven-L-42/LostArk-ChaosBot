using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using IronOcr;
using PixelAimbot.Classes.Misc;
using Timer = System.Timers.Timer;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        private bool _start;
        private bool _RepairReset = true;
        private bool _botIsRun = true;
        private bool _discordBotIsRun = true;
        private bool _stopp;
        private bool _stop;
        private bool _restart;

        private bool _floor1;
        private bool _floor2;
        private bool _floorFight;
        private bool _searchboss;

        private bool _portalIsDetected;
        private bool _portalIsNotDetected;

        private bool _revive;
        private bool _portaldetect;

        private bool _ultimate;
        private bool _potions;

        private bool _repair;
        private bool _gunlancer;
        private bool _shadowhunter;
        private bool _berserker;
        private bool _paladin;
        private bool _deathblade;
        private bool _Glavier;
        private bool _sharpshooter;
        private bool _bard;
        private bool _sorcerer;
        private bool _soulfist;
        private bool _doUltimateAttack;
        private bool _logout;

        private bool _GuideLoaded;

        //SKILL AND COOLDOWN//
        private bool _Q;
        private bool _W;
        private bool _E;
        private bool _R;
        private bool _A;
        private bool _S;
        private bool _D;
        private bool _F;


        public static bool isWindowed = false;
        private static int windowX = 0;
        private static int windowY = 0;
        private static int windowWidth = 0;
        private static int windowHeight = 0;

        public static int ChaosAllRounds;
        public static int ChaosAllStucks;
        public static int ChaosPerfectRounds;
        public static int ChaosGameCrashed;


        private Timer _timer;
    
        private int _fightSequence;
        private int _fightSequence2;
        private int _searchSequence;
        private int _walktopUTurn = 1;
        private int _leavetimer;
        private int _leavetimer2;
        private int _leavetimer1;
        private int _floorint2 = 1;
        private int _floorint3 = 1;
        private int _swap;
        private int _formExists;
        private bool _canSearchEnemys = true;
        private bool _firstSetupTransparency = true;
        private DateTime _repairTimer;
        private DateTime _Logout;
        
        public static int healthPercent = 70;
    
        public frmMinimized FormMinimized = new frmMinimized();
        public Config conf = new Config();
        private bool _telegramBotRunning;
        Random humanizer = new Random();

        public Task TelegramTask;
        public Task DiscordTask;
        public static IronTesseract tess = new IronTesseract();
        
        private string comboattack = "";

        private Priorized_Skills _skills = new Priorized_Skills();
        
        private static readonly Random random = new Random();
        public Rotations rotation = new Rotations();
        
        public static string ConfigPath { get; set; } = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();

        public static int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        private static int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        private Layout_Keyboard _currentLayout;
        private byte currentMouseButton;
        private byte currentHealKey;

        public static CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationTokenSource telegramToken = new CancellationTokenSource();
        private CancellationTokenSource discordToken = new CancellationTokenSource();

        public Image<Bgr, Byte> Image_bossHP = byteArrayToImage(Images.bosshp);
        public Image<Bgr, Byte> Image_bossHPmask = byteArrayToImage(Images.bosshpmask);

        public Image<Bgr, Byte> Image_boss1 = byteArrayToImage(Images.boss1);
        public Image<Bgr, Byte> Image_bossmask1 = byteArrayToImage(Images.bossmask1);
        public Image<Bgr, Byte> Image_enemy = byteArrayToImage(Images.enemy);
        public Image<Bgr, Byte> Image_mask = byteArrayToImage(Images.mask);
        public Image<Bgr, Byte> Image_mob1 = byteArrayToImage(Images.mob1);
        public Image<Bgr, Byte> Image_mobmask1 = byteArrayToImage(Images.mobmask1);
        public Image<Bgr, Byte> Image_portalenter1 = byteArrayToImage(Images.portalenter1);
        public Image<Bgr, Byte> Image_portalentermask1 = byteArrayToImage(Images.portalentermask1);
        public Image<Bgr, Byte> Image_questmarker = byteArrayToImage(Images.questmarker);
        public Image<Bgr, Byte> Image_red_hp = byteArrayToImage(Images.red_hp);
        public Image<Bgr, Byte> Image_revive_new = byteArrayToImage(Images.revive_new);

        public Image<Bgr, Byte> Image_death = byteArrayToImage(Images.death);
        public Image<Bgr, Byte> Image_deathEN = byteArrayToImage(Images.deathEN);


        public Image<Bgr, Byte> Image_revive1 = byteArrayToImage(Images.revive1);
        public Image<Bgr, Byte> Image_reviveEnglish = byteArrayToImage(Images.reviveEnglish);


        
        public static Image<Bgr, Byte> Image_GuardianStone1 = byteArrayToImage(Images.GuardianStone1);
        public static Image<Bgr, Byte> Image_GuardianStone1Mask = byteArrayToImage(Images.GuardianStone1Mask);
        public static Image<Bgr, Byte> Image_DestructStone3 = byteArrayToImage(Images.DestructStone3);
        public static Image<Bgr, Byte> Image_DestructStone3Mask = byteArrayToImage(Images.DestructStone3Mask);

    }
}