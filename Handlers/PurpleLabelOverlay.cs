using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WinColourLabels.AbstractHandlers;

namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("d5d205a8-b9e3-41c2-a2f8-dd79b49000a3")]
    public class PurpleLabelOverlay : AbstractIconOverlayHandler
    {
        Database dbase = Database.GetInstance();

        protected override bool CanShowIconOverlay(string path)
        {
            FileLabel label = dbase.GetFileLabel(path);
            return label == FileLabel.PURPLE;
        }

        protected override Icon GetIconOverlay()
        {
            return Properties.Resources.Purple;
        }
        protected override int GetIconOverlayPriority()
        {
            return 0;
        }
    }
}
