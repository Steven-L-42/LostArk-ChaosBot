using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        public void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            healthPercent = HealthSlider.Value;
            
            labelheal.Text = "Heal at: " + ((ChaosBot.healthPercent - 631) * 100) / (853 - 631) + "% Life";
         
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
                        object health10 = au3.PixelSearch(Recalc(631), Recalc(962, false), Recalc(ChaosBot.healthPercent),
                            Recalc(968, false), 0x050405, 15);
                        if (health10.ToString() != "0")
                        {
                            au3.Send("{" + txtHeal10.Text + "}");
                            au3.Send("{" + txtHeal10.Text + "}");
                            au3.Send("{" + txtHeal10.Text + "}");
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Heal-Potion..."));
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