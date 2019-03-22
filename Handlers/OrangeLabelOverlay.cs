using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WinColourLabels.AbstractHandlers;
using WinColourLabels.Database;

namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("75292a8d-e35d-4eb8-9bb5-5b066a8a327b")]
    public class OrangeLabelOverlay : AbstractIconOverlayHandler
    {
        protected override bool CanShowIconOverlay(string path)
        {
            if (DatabaseFacade.GetLabel(path) == FileLabel.ORANGE)
                return true;
            else return false;
        }

        protected override Icon GetIconOverlay()
        {
            return Properties.Resources.Orange;
        }
        protected override int GetIconOverlayPriority()
        {
            return 0;
        }
    }
}
