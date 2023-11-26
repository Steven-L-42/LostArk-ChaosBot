using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class ChaosBot
    {

        private async Task SEARCHBOSS(CancellationToken token)
        {
            try
            {
                if (_searchboss == true && _floorFight == false && _stopp == false)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    if (_redStage >= 1)
                    {
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: search enemy..."));
                    }
                    else
                    if (_floorint2 == 1)
                    {
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: search enemy..."));
                    }
                   
                    if (_searchSequence == 1 || _redStage >= 1)
                    {
                        await Task.Delay(_humanizer.Next(10, 240) + 1500, token);
                        VirtualMouse.MoveTo(Recalc(960), Recalc(529, false), 10);
                        KeyboardWrapper.PressKey(_currentMouseButton);
                        if (chBoxCooldownDetection.Checked && chBoxY.Checked)
                        {
                            GetSkillQ();
                            GetSkillW();
                            GetSkillE();
                            GetSkillR();
                            GetSkillA();
                            GetSkillS();
                            GetSkillD();
                            GetSkillF();
                        }
                        _searchSequence = 2;
                    }

                    if (cmbGunlancer.SelectedIndex == 1 && _gunlancer == false && _searchboss == true)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);

                        KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);
                        _gunlancer = true;

                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Deactivate: Gunlancer Ultimate"));
                    }
                    
                    for (int i = 0; i < int.Parse(txtDungeon2search.Text); i++)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(_humanizer.Next(10, 240) + 100, token);
                        float threshold = 0.705f;

                        var enemyTemplate = ImageEnemy;
                        var enemyMask = ImageMask;
                        var BossTemplate = ImageBoss1;
                        var BossMask = ImageBossmask1;
                        var mobTemplate = ImageMob1;
                        var mobMask = ImageMobmask1;
                        var portalTemplate = ImagePortalenter1;
                        var portalMask = ImagePortalentermask1;

                        Point myPosition = new Point(Recalc(148), Recalc(127, false));
                        Point screenResolution = new Point(ScreenWidth, ScreenHeight);
                        var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                        var BossDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                        var mobDetector = new EnemyDetector(mobTemplate, mobMask, threshold);
                        var portalDetector = new EnemyDetector(portalTemplate, portalMask, threshold);

                        using (var screenCapture = _globalScreenPrinter.CaptureScreenImage())
                        {

                            var enemy = enemyDetector.GetClosestEnemy(screenCapture, false);
                            var Boss = BossDetector.GetClosestEnemy(screenCapture, false);
                            var mob = mobDetector.GetClosestEnemy(screenCapture, false);
                            var portal = portalDetector.GetClosestEnemy(screenCapture, false);

                            if (Boss.HasValue && _searchboss == true)
                            {
                                CvInvoke.Rectangle(screenCapture,
                                    new Rectangle(new Point(Boss.Value.X, Boss.Value.Y), BossTemplate.Size),
                                    new MCvScalar(255));
                                double distance_x = (ScreenWidth - Recalc(296)) / 2;
                                double distance_y = (ScreenHeight - Recalc(255, false)) / 2;

                                var boss_position = ((Boss.Value.X + distance_x), (Boss.Value.Y + distance_y));
                                double multiplier = 1;
                                var boss_position_on_minimap = ((Boss.Value.X), (Boss.Value.Y));
                                var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                                var dist = Math.Sqrt(
                                    Math.Pow((my_position_on_minimap.Item1 - boss_position_on_minimap.Item1), 2) +
                                    Math.Pow((my_position_on_minimap.Item2 - boss_position_on_minimap.Item2), 2));

                                if (dist < 180 && _searchboss == true)
                                {
                                    multiplier = 1.3;
                                }

                                double posx;
                                double posy;
                                if (boss_position.Item1 < (ScreenWidth / 2) && _searchboss == true)
                                {
                                    posx = boss_position.Item1 * (2 - multiplier);
                                }
                                else
                                {
                                    posx = boss_position.Item1 * multiplier;
                                }

                                if (boss_position.Item2 < (ScreenHeight / 2) && _searchboss == true)
                                {
                                    posy = boss_position.Item2 * (2 - multiplier);
                                }
                                else
                                {
                                    posy = boss_position.Item2 * multiplier;
                                }

                                var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);


                                if (_redStage >= 1)
                                {
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: Big-Boss found!"));
                                }
                                else
                                   if (_floorint2 == 1 && _searchboss == true)
                                {
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Big-Boss found!"));
                                }

                                VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2, 10);

                                KeyboardWrapper.AlternateHoldKey(_currentMouseButton, 1000);
                            }
                            else
                            {
                                if (enemy.HasValue && _searchboss == true)
                                {
                                    CvInvoke.Rectangle(screenCapture,
                                        new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                        new MCvScalar(255));
                                    double distance_x = (ScreenWidth - Recalc(296)) / 2;
                                    double distance_y = (ScreenHeight - Recalc(255, false)) / 2;

                                    var enemy_position = ((enemy.Value.X + distance_x), (enemy.Value.Y + distance_y));
                                    double multiplier = 1;
                                    var enemy_position_on_minimap = ((enemy.Value.X), (enemy.Value.Y));
                                    var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                                    var dist = Math.Sqrt(
                                        Math.Pow((my_position_on_minimap.Item1 - enemy_position_on_minimap.Item1), 2) +
                                        Math.Pow((my_position_on_minimap.Item2 - enemy_position_on_minimap.Item2), 2));

                                    if (dist < 180 && _searchboss == true)
                                    {
                                        multiplier = 1.3;
                                    }

                                    double posx;
                                    double posy;
                                    if (enemy_position.Item1 < (ScreenWidth / 2) && _searchboss == true)
                                    {
                                        posx = enemy_position.Item1 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posx = enemy_position.Item1 * multiplier;
                                    }

                                    if (enemy_position.Item2 < (ScreenHeight / 2) && _searchboss == true)
                                    {
                                        posy = enemy_position.Item2 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posy = enemy_position.Item2 * multiplier;
                                    }

                                    var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);

                                    if (_redStage >= 1)
                                    {
                                        lbStatus.Invoke((MethodInvoker)(() =>
                                            lbStatus.Text = "Floor 3: Mid-Boss found!"));
                                    }
                                    else
                                       if (_floorint2 == 1 && _searchboss == true)
                                    {
                                        lbStatus.Invoke((MethodInvoker)(() =>
                                            lbStatus.Text = "Floor 2: Mid-Boss found!"));
                                    }

                                    VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2, 10);

                                    KeyboardWrapper.AlternateHoldKey(_currentMouseButton, 1000);
                                }
                                else
                                {
                                    if (mob.HasValue && _searchboss == true)
                                    {
                                        CvInvoke.Rectangle(screenCapture,
                                            new Rectangle(new Point(mob.Value.X, mob.Value.Y), mobTemplate.Size),
                                            new MCvScalar(255));
                                        double distance_x = (ScreenWidth - Recalc(296)) / 2;
                                        double distance_y = (ScreenHeight - Recalc(255, false)) / 2;

                                        var mob_position = ((mob.Value.X + distance_x), (mob.Value.Y + distance_y));
                                        double multiplier = 1;
                                        var mob_position_on_minimap = ((mob.Value.X), (mob.Value.Y));
                                        var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                                        var dist = Math.Sqrt(
                                            Math.Pow((my_position_on_minimap.Item1 - mob_position_on_minimap.Item1),
                                                2) +
                                            Math.Pow((my_position_on_minimap.Item2 - mob_position_on_minimap.Item2),
                                                2));

                                        if (dist < 180 && _searchboss == true)
                                        {
                                            multiplier = 1.3;
                                        }

                                        double posx;
                                        double posy;
                                        if (mob_position.Item1 < (ScreenWidth / 2) && _searchboss == true)
                                        {
                                            posx = mob_position.Item1 * (2 - multiplier);
                                        }
                                        else
                                        {
                                            posx = mob_position.Item1 * multiplier;
                                        }

                                        if (mob_position.Item2 < (ScreenHeight / 2) && _searchboss == true)
                                        {
                                            posy = mob_position.Item2 * (2 - multiplier);
                                        }
                                        else
                                        {
                                            posy = mob_position.Item2 * multiplier;
                                        }

                                        var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                        if (_redStage >= 1)
                                        {
                                            lbStatus.Invoke(
                                              (MethodInvoker)(() => lbStatus.Text = "Floor 3: Mob found!"));
                                        }
                                        else
                                        if (_floorint2 == 1 && _searchboss == true)
                                        {
                                            lbStatus.Invoke(
                                                (MethodInvoker)(() => lbStatus.Text = "Floor 2: Mob found!"));
                                        }

                                        VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2, 10);

                                        KeyboardWrapper.AlternateHoldKey(_currentMouseButton, 1000);
                                    }
                                    else
                                        if (_redStage >= 1)
                                    {
                                        _canSearchEnemys = false;
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(1, token);
                                        VirtualMouse.MoveTo(Recalc(960), Recalc(240, false), 10);
                                        KeyboardWrapper.AlternateHoldKey(_currentMouseButton, 500);
                                        VirtualMouse.MoveTo(Recalc(960), Recalc(566, false), 10);
                                        KeyboardWrapper.PressKey(_currentMouseButton);
                                        _canSearchEnemys = true;
                                    }

                                }
                            }

                        }

                        Random random = new Random();
                        var sleepTime = random.Next(100, 150);
                        await Task.Delay(sleepTime);
                    }


                    if (_redStage >= 1)
                    {
                        _floor3 = true;
                        _floor1 = false;
                        _floor2 = false;
                        starten = true;
                        _bossKillDetection = true;
                        CtsBoss = new CancellationTokenSource();
                        _bossKill = CtsBoss.Token;
                        var t18 = Task.Run(() => BossKillDetection(_bossKill));
                        var t19 = Task.Run(() => Floor3BossTimer(_bossKill));
                   
                    }
                    else
                    if (_leave == false)
                    {
                        _leave = true;
                        _floor1 = false;
                        _floor2 = true;
                        starten = true;
                        _bossKillDetection = true;
                        CtsBoss = new CancellationTokenSource();
                        _bossKill = CtsBoss.Token;
                        var t17= Task.Run(() => Leavetimerfloor2(token));
                        var t18 = Task.Run(() => BossKillDetection(_bossKill));

                    }



                    _potions = true;
                    _revive = true;
                    _ultimate = true;
                    _floorFight = true;

                    _searchboss = false;

                    _gunlancer = true;
                    _shadowhunter = true;
                    _berserker = true;
     
                    _sharpshooter = true;
                    _bard = true;
                    _sorcerer = true;
                    _soulfist = true;
                    _ultimate = true;
                    _floorFight = true;

                    var t14 = Task.Run(() => UltimateAttack(token));
                    var t12 = Task.Run(() => Floortime(token));
                    await Task.WhenAny(new[] { t12, t14 });
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



    }
}