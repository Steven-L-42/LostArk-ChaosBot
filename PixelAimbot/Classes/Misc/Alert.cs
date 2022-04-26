using Emgu.CV.Stitching;

namespace PixelAimbot.Classes.Misc
{
    public class Alert
    {
        public static void Show(string msg, frmAlert.enmType type = frmAlert.enmType.Success)
        {
            frmAlert frm = new frmAlert();
            frm.TopMost = true;
            frm.showAlert(msg,type);
        }
    }
}