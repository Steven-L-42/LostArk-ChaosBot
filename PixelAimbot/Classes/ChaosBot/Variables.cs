using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        private bool _floor3;
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
        private bool _sharpshooter;
        private bool _bard;
        private bool _sorcerer;
        private bool _soulfist;
        private bool _doUltimateAttack;
        private bool _logout;

        //SKILL AND COOLDOWN//
        private bool _Q;
        private bool _W;
        private bool _E;
        private bool _R;
        private bool _A;
        private bool _S;
        private bool _D;
        private bool _F;

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

        public static int healthPercent = 70;

        public frmMinimized FormMinimized = new frmMinimized();
        public Config conf = new Config();
        private bool _telegramBotRunning;
        Random humanizer = new Random();

        public Task TelegramTask;
        public Task DiscordTask;
        public static IronTesseract tess = new IronTesseract();
        
        public string resourceFolder = "";
        private string comboattack = "";
        private Priorized_Skills _skills = new Priorized_Skills();
        
        private static readonly Random random = new Random();
        public Rotations rotation = new Rotations();
        
        public static string ConfigPath { get; set; } = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();

        private static int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        private static int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        private Layout_Keyboard _currentLayout;
        
        private CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationTokenSource telegramToken = new CancellationTokenSource();
        private CancellationTokenSource discordToken = new CancellationTokenSource();
    }
}