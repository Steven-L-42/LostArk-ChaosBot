using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private async Task DoLopangAction(CancellationToken token, int character)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                switch (character)
                {
                    case 1:
                        VirtualMouse.MoveTo(ChaosBot.Recalc(717), ChaosBot.Recalc(408, false), 10);
                        break;

                    case 2:
                        VirtualMouse.MoveTo(ChaosBot.Recalc(963), ChaosBot.Recalc(408, false), 10);
                        break;

                    case 3:
                        VirtualMouse.MoveTo(ChaosBot.Recalc(1225), ChaosBot.Recalc(410, false), 10);
                        break;

                    case 4:
                        VirtualMouse.MoveTo(ChaosBot.Recalc(714), ChaosBot.Recalc(526, false), 10);
                        break;

                    case 5:
                        VirtualMouse.MoveTo(ChaosBot.Recalc(975), ChaosBot.Recalc(529, false), 10);
                        break;

                    case 6:
                        VirtualMouse.MoveTo(ChaosBot.Recalc(1212), ChaosBot.Recalc(517, false), 10);
                        break;

                    case 7:
                        VirtualMouse.MoveTo(ChaosBot.Recalc(723), ChaosBot.Recalc(621, false), 10);
                        break;

                    case 8:
                        VirtualMouse.MoveTo(ChaosBot.Recalc(986), ChaosBot.Recalc(620, false), 10);
                        break;

                    case 9:
                        VirtualMouse.MoveTo(ChaosBot.Recalc(1204), ChaosBot.Recalc(620, false), 10);
                        break;
                }

                await Task.Delay(800, token);
                VirtualMouse.LeftClick();
                await Task.Delay(800, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1048), ChaosBot.Recalc(729, false), 10);
                await Task.Delay(800, token);
                VirtualMouse.LeftClick();
                VirtualMouse.MoveTo(ChaosBot.Recalc(903), ChaosBot.Recalc(603, false), 10);
                await Task.Delay(800, token);
                VirtualMouse.LeftClick();

                while (Pixel.PixelSearch(ChaosBot.Recalc(109), ChaosBot.Recalc(0, false),
                           ChaosBot.Recalc(455), ChaosBot.Recalc(149, false), 0x4AD731, 5).ToString() == "0")
                {
                    await Task.Delay(100, token);
                }

                await Task.Delay(1500, token);

                #region OpenQuestWindow

                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Select Quests..."));

                VirtualMouse.MoveTo(ChaosBot.Recalc(1686), ChaosBot.Recalc(1049, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(800, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1652), ChaosBot.Recalc(951, false), 10);
                VirtualMouse.LeftClick();

                await Task.Delay(800, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(818), ChaosBot.Recalc(243, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(800, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1101), ChaosBot.Recalc(240, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(800, token);
                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_BACK, 1000);

                KeyboardWrapper.PressKey(KeyboardWrapper.VK_L);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_O);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_P);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_A);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_N);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

                VirtualMouse.MoveTo(ChaosBot.Recalc(1294), ChaosBot.Recalc(580, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(600, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1291), ChaosBot.Recalc(658, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(600, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1294), ChaosBot.Recalc(734, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(600, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                await Task.Delay(600, token);

                #endregion OpenQuestWindow

                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Teleport Lopang..."));

                #region bifrost

                VirtualMouse.MoveTo(ChaosBot.Recalc(1680), ChaosBot.Recalc(1032, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(600, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1629), ChaosBot.Recalc(794, false), 10);
                VirtualMouse.LeftClick();

                #endregion bifrost

                #region portLopang

                await Task.Delay(600, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1473), ChaosBot.Recalc(513, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(600, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

                while (Pixel.PixelSearch(ChaosBot.Recalc(109), ChaosBot.Recalc(0, false),
                           ChaosBot.Recalc(455), ChaosBot.Recalc(149, false), 0x4AD731, 5).ToString() == "0")
                {
                    await Task.Delay(100, token);
                }
                await Task.Delay(1500, token);

                #endregion portLopang

                #region walkLopang

                await Task.Delay(5000, token);
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Walk around..."));

                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(1352, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1033), ChaosBot.Recalc(902, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1352, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1576), ChaosBot.Recalc(934, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1263, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1416), ChaosBot.Recalc(989, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1156, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1552), ChaosBot.Recalc(269, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(2358, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1552), ChaosBot.Recalc(269, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(2640, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1552), ChaosBot.Recalc(269, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(2657, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1172), ChaosBot.Recalc(409, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1484, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                VirtualMouse.MoveTo(ChaosBot.Recalc(433), ChaosBot.Recalc(797, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1519, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(484), ChaosBot.Recalc(770, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1534, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(505), ChaosBot.Recalc(770, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1941, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(534), ChaosBot.Recalc(772, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1833, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(597), ChaosBot.Recalc(797, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1941, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1215), ChaosBot.Recalc(870, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(2314, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1206), ChaosBot.Recalc(873, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(2100, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1217), ChaosBot.Recalc(786, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1326, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1291), ChaosBot.Recalc(362, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(2360, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1273), ChaosBot.Recalc(368, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(2026, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1378), ChaosBot.Recalc(648, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1726, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1272), ChaosBot.Recalc(556, false), 3);
                VirtualMouse.LeftClick();
                await Task.Delay(1726, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(1726, token);

                #endregion walkLopang

                #region bifrost

                VirtualMouse.MoveTo(ChaosBot.Recalc(1680), ChaosBot.Recalc(1032, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(800, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1629), ChaosBot.Recalc(794, false), 10);
                VirtualMouse.LeftClick();

                #endregion bifrost

                #region firstport

                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Quest 1..."));
                await Task.Delay(1800, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1475), ChaosBot.Recalc(596, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(600, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);
                while (Pixel.PixelSearch(ChaosBot.Recalc(109), ChaosBot.Recalc(0, false),
                           ChaosBot.Recalc(455), ChaosBot.Recalc(149, false), 0x4AD731, 5).ToString() == "0")
                {
                    await Task.Delay(100, token);
                }
                await Task.Delay(1500, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);

                #endregion firstport

                #region bifrost

                VirtualMouse.MoveTo(ChaosBot.Recalc(1680), ChaosBot.Recalc(1032, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(600, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1629), ChaosBot.Recalc(794, false), 10);
                VirtualMouse.LeftClick();

                #endregion bifrost

                #region secondport

                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Quest 2..."));

                VirtualMouse.MoveTo(ChaosBot.Recalc(1480), ChaosBot.Recalc(802, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(600, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);
                while (Pixel.PixelSearch(ChaosBot.Recalc(109), ChaosBot.Recalc(0, false),
                           ChaosBot.Recalc(455), ChaosBot.Recalc(149, false), 0x4AD731, 5).ToString() == "0")
                {
                    await Task.Delay(100, token);
                }
                await Task.Delay(500, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);

                #endregion secondport

                #region bifrost

                VirtualMouse.MoveTo(ChaosBot.Recalc(1680), ChaosBot.Recalc(1032, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(600, token);
                VirtualMouse.MoveTo(ChaosBot.Recalc(1629), ChaosBot.Recalc(794, false), 10);
                VirtualMouse.LeftClick();

                #endregion bifrost

                #region thirdport

                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Quest 3..."));

                VirtualMouse.MoveTo(ChaosBot.Recalc(1485), ChaosBot.Recalc(884, false), 10);
                VirtualMouse.LeftClick();
                await Task.Delay(600, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);
                while (Pixel.PixelSearch(ChaosBot.Recalc(109), ChaosBot.Recalc(0, false),
                           ChaosBot.Recalc(455), ChaosBot.Recalc(149, false), 0x4AD731, 5).ToString() == "0")
                {
                    await Task.Delay(100, token);
                }
                await Task.Delay(1500, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(800, token);

                #endregion thirdport
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
    }
}