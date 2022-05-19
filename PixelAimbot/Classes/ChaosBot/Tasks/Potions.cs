using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Microsoft.IO;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        public void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            healthPercent = HealthSlider1.Value;
            double distanceFromMin = (HealthSlider1.Value - HealthSlider1.Minimum);
            double sliderRange = (HealthSlider1.Maximum - HealthSlider1.Minimum);
            double sliderPercent = 100 * (distanceFromMin / sliderRange);
            labelheal.Text = "Heal at: " + Convert.ToInt32(sliderPercent) + "% Life";

        }
        private async Task Potions(CancellationToken token)
        {

            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                while (_potions && _floorFight && _stopp == false)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        object health10 = Pixel.PixelSearch(Recalc(ChaosBot.healthPercent - 10), Recalc(962, false), Recalc(ChaosBot.healthPercent),
                            Recalc(968, false), 0x050405, 15); 
                        if (health10.ToString() != "0")
                        {
                            KeyboardWrapper.PressKey(currentHealKey);
                            KeyboardWrapper.PressKey(currentHealKey);
                            KeyboardWrapper.PressKey(currentHealKey);
                            KeyboardWrapper.PressKey(currentHealKey);
                            KeyboardWrapper.PressKey(currentHealKey);


                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion..."));
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

                    Random random = new Random();
                    var sleepTime = random.Next(500, 570);
                    await Task.Delay(sleepTime);
                   

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
    }
}