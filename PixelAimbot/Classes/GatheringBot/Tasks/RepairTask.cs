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
            
            // Öffne Ratgeber
            KeyboardWrapper.KeyDown(KeyboardWrapper.VK_ALT);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_P);
            await Task.Delay(50, token);
            KeyboardWrapper.KeyUp(KeyboardWrapper.VK_ALT);
            await Task.Delay(500, token);
            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            // KLICK AUF AMBOSS
            await Task.Delay(500, token);
            VirtualMouse.MoveTo(ChaosBot.Recalc(1291), ChaosBot.Recalc(693, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            // KLICK AUF REPARIEREN
            await Task.Delay(500, token);
            VirtualMouse.MoveTo(ChaosBot.Recalc(717), ChaosBot.Recalc(745, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
            token.ThrowIfCancellationRequested();
            await Task.Delay(1, token);

            await Task.Delay(500, token);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

            await Task.Delay(500, token);
            VirtualMouse.MoveTo(ChaosBot.Recalc(1785), ChaosBot.Recalc(1024, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
            
            await Task.Delay(500, token);
            VirtualMouse.MoveTo(ChaosBot.Recalc(1383), ChaosBot.Recalc(205, false), 5);
            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);


            
            var t3 = Task.Run(() => ThrowFishingRod(token));
            _canrepair = false;
            await Task.WhenAny(new[] {t3});
        }
    }
}