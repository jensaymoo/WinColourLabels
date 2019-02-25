using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WinColourLabels.AbstractHandlers;

namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("2b471e59-7514-48a6-9f03-359b540262ca")]
    public class GreenLabelOverlay : AbstractIconOverlayHandler
    {
        Database dbase = Database.GetInstance();

        protected override bool CanShowIconOverlay(string path)
        {
            FileLabel label = dbase.GetFileLabel(path);
            return label == FileLabel.GREEN;
        }

        protected override Icon GetIconOverlay()
        {
            return Properties.Resources.Green;
        }
        protected override int GetIconOverlayPriority()
        {
            return 0;
        }
    }
}
