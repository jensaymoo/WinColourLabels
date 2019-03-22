using System.Drawing;
using System.Runtime.InteropServices;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

namespace WinColourLabels.AbstractHandlers
{
    [ComVisible(false)]
    public abstract class AbstractIconOverlayHandler : SharpIconOverlayHandler
    {
        protected abstract Icon GetIconOverlay();
        protected abstract bool CanShowIconOverlay(string path);
        protected abstract int GetIconOverlayPriority();

        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes) => CanShowIconOverlay(path);
        protected override Icon GetOverlayIcon() => GetIconOverlay();
        protected override int GetPriority() => GetIconOverlayPriority();
    }
}
