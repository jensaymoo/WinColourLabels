using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

namespace WinColourLabels.AbstractHandlers
{
    [ComVisible(false)]
    public class AbstractIconOverlayHandler : SharpIconOverlayHandler
    {
        protected virtual bool CanShowIconOverlay(string path)
        {
            return false;
        }

        protected virtual Icon GetIconOverlay()
        {
            return null;
        }
        protected virtual int GetIconOverlayPriority()
        {
            return 100;
        }

        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes)
        {
            return CanShowIconOverlay(path);
        }

        protected override Icon GetOverlayIcon()
        {
            return GetIconOverlay();
        }

        protected override int GetPriority()
        {
            return GetIconOverlayPriority();
        }
    }
}
