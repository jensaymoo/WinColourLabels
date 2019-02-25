using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WinColourLabels.AbstractHandlers;

namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("9a0bd999-40af-4ba9-b4e3-9303d474d4f3")]
    public class BlueLabelOverlay : AbstractIconOverlayHandler
    {
        Database dbase = Database.GetInstance();

        protected override bool CanShowIconOverlay(string path)
        {
            FileLabel label = dbase.GetFileLabel(path);
            return label == FileLabel.BLUE;
        }

        protected override Icon GetIconOverlay()
        {
            return Properties.Resources.Blue;
        }
        protected override int GetIconOverlayPriority()
        {
            return 0;
        }
    }
}
