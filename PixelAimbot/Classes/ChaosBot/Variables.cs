using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using Timer = System.Timers.Timer;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        public static bool IsWindowed;
        private static int _windowX;
        private static int _windowY;
        private static int _windowWidth;
        private static int _windowHeight;

        public static int ChaosAllRounds;
        public static int ChaosAllStucks;
        public static int ChaosRedStages;
        public static int ChaosGameCrashed;

        public static int HealthPercent = 70;

        private static readonly Random Random = new Random();

        public static int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
        public static int ScreenHeight = Screen.PrimaryScreen.Bounds.Height;

        public static CancellationTokenSource Cts = new CancellationTokenSource();
        public static CancellationTokenSource CtsSkills = new CancellationTokenSource();
        public static CancellationTokenSource CtsBoss = new CancellationTokenSource();

        private readonly Priorized_Skills _skills = new Priorized_Skills();

        private readonly CancellationTokenSource _discordToken = new CancellationTokenSource();

        private readonly PrintScreen _globalScreenPrinter = new PrintScreen();
        private readonly Random _humanizer = new Random();


        private string steampath = @"C:\Program Files(x86)\Steam\steam.exe";

        private bool _a;
        private bool _bard;
        private bool _berserker;
        private bool _botIsRun = true;
        private bool _canSearchEnemys = true;
        private bool _d;
        private bool _deathblade;
        private bool _destroyer;
        private int _destroyerCounter;
        private bool _discordBotIsRun = true;
        private bool _doUltimateAttack;
        private bool _e;
        private bool _f;
        private int _fightSequence;
        private int _fightSequence2;
        private bool _firstSetupTransparency = true;

        private bool _floor1;
        private int _floor1Detectiontimer = 0;
        private bool _floor2;
        private bool _floor3;
        private bool _floorFight;
        private int _floorint2 = 1;
        private int _formExists;
        private bool _glavier;
        private int _globalLeavetimerfloor2 = 0;
        private bool _gunlancer;
        private int _leavetimer;
        private int _leavetimer1;
        private int _leavetimer2;
        private int _leavetimerfloor1 = 0;
        private int _leavetimerfloor2 = 0;
        private bool _logout;
        private DateTime _Logout;
        private bool _paladin;
        private bool _portaldetect;

        private bool _portalIsDetected;
        private bool _portalIsNotDetected;
        private bool _potions;


        //SKILL AND COOLDOWN//
        private bool _q;
        private bool _r;

        private bool _repair;
        private bool _repairReset = true;
        private DateTime _repairTimer;
        private bool _restart;
        private int _restartInt = 0;

        private bool _revive;
        private bool _s;
        private bool _searchboss;
        private int _searchSequence;
        private bool _shadowhunter;
        private bool _sharpshooter;
        private bool _sorcerer;
        private bool _soulfist;
        private bool _start;
        private bool _stop;
        private bool _stopp;
        private int _swap;


        private Timer _timer;
        private bool _ultimate;
        private bool _w;
        private int _walktopUTurn = 1;

        private string _comboattack = "";
        public Config Conf = new Config();
        private byte _currentHealKey;
        private byte _currentMouseButton;
        public Task DiscordTask;

        public frmMinimized FormMinimized = new frmMinimized();

        public Image<Bgr, byte> ImageBoss1 = ByteArrayToImage(Images.boss1);

        public Image<Bgr, byte> ImageBossHp = ByteArrayToImage(Images.bosshp);
        public Image<Bgr, byte> ImageBossHPmask = ByteArrayToImage(Images.bosshpmask);
        public Image<Bgr, byte> ImageBossmask1 = ByteArrayToImage(Images.bossmask1);

        public Image<Bgr, byte> ImageDeath = ByteArrayToImage(Images.death);
        public Image<Bgr, byte> ImageDeathEn = ByteArrayToImage(Images.deathEN);
        public Image<Bgr, byte> ImageEnemy = ByteArrayToImage(Images.enemy);
        public Image<Bgr, byte> ImageMask = ByteArrayToImage(Images.mask);
        public Image<Bgr, byte> ImageMob1 = ByteArrayToImage(Images.mob1);
        public Image<Bgr, byte> ImageMobmask1 = ByteArrayToImage(Images.mobmask1);
        public Image<Bgr, byte> ImagePortalenter1 = ByteArrayToImage(Images.portalenter1);
        public Image<Bgr, byte> ImagePortalentermask1 = ByteArrayToImage(Images.portalentermask1);
        public Image<Bgr, byte> ImageQuestmarker = ByteArrayToImage(Images.questmarker);
        public Image<Bgr, byte> ImageRedHp = ByteArrayToImage(Images.red_hp);
        public Image<Bgr, byte> ImageReviveNew = ByteArrayToImage(Images.revive_new);
        public Image<Bgr, byte> GameRestartSMG = ByteArrayToImage(Images.game_restart_smg);

        public Image<Bgr, byte> ImageRevive1 = ByteArrayToImage(Images.revive1);
        public Image<Bgr, byte> ImageReviveEnglish = ByteArrayToImage(Images.reviveEnglish);
        private bool _leave;
        private int _redStage;
        public Rotations rotation = new Rotations();


        public static string ConfigPath { get; set; } = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();
    }
}