using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private async Task RepairTask(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Auto-Repair starts ..."));
            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            // KLICK UNTEN RECHTS (RATGEBER)
            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);
            VirtualMouse.MoveTo(Recalc(1741), Recalc(1040, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
            await Task.Delay(500, token);

            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            // KLICK AUF BEGLEITER
            await Task.Delay(500, token);
            VirtualMouse.MoveTo(Recalc(1684), Recalc(823, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            // KLICK AUF AMBOSS
            await Task.Delay(500, token);
            VirtualMouse.MoveTo(Recalc(1291), Recalc(693, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            // KLICK AUF REPARIEREN
            await Task.Delay(500, token);
            VirtualMouse.MoveTo(Recalc(717), Recalc(745, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            await Task.Delay(500, token);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

            // 2x ESCAPE REPARATUR UND BEGLEITER FENSTER SCHLIEßEN
            await Task.Delay(2500, token);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);

            await Task.Delay(2500, token);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);


            _canrepair = false;
            var t3 = Task.Run(() => ThrowFishingRod(token));
            await Task.WhenAny(new[] {t3});
        }
    }
}