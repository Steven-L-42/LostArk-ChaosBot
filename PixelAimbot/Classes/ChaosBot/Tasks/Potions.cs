using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelAimbot
{
    partial class ChaosBot
    {
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
                        object health10 = au3.PixelSearch(Recalc(633), Recalc(962, false), Recalc(651),
                            Recalc(969, false), 0x050405, 15);
                        if (health10.ToString() != "0" && checkBoxHeal10.Checked)
                        {
                            au3.Send("{" + txtHeal10.Text + "}");
                            au3.Send("{" + txtHeal10.Text + "}");
                            au3.Send("{" + txtHeal10.Text + "}");
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Potion at 10%"));
                        }
                        else

                            token.ThrowIfCancellationRequested();

                        await Task.Delay(1, token);
                        object health30 = au3.PixelSearch(Recalc(633), Recalc(962, false), Recalc(686),
                            Recalc(969, false), 0x050405, 15);
                        if (health30.ToString() != "0" && checkBoxHeal30.Checked)
                        {
                            au3.Send("{" + txtHeal30.Text + "}");
                            au3.Send("{" + txtHeal30.Text + "}");
                            au3.Send("{" + txtHeal30.Text + "}");
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Potion at 30%"));
                        }
                        else

                            token.ThrowIfCancellationRequested();

                        await Task.Delay(1, token);
                        object health70 = au3.PixelSearch(Recalc(633), Recalc(962, false), Recalc(820),
                            Recalc(969, false), 0x050405, 15);
                        if (health70.ToString() != "0" && checkBoxHeal70.Checked)
                        {
                            au3.Send("{" + txtHeal70.Text + "}");
                            au3.Send("{" + txtHeal70.Text + "}");
                            au3.Send("{" + txtHeal70.Text + "}");
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Potion at 70%"));
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