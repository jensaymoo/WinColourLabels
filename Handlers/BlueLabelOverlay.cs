using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WinColourLabels.AbstractHandlers;
using Trinet.Core.IO.Ntfs;

namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("9a0bd999-40af-4ba9-b4e3-9303d474d4f3")]
    public class BlueLabelOverlay : AbstractIconOverlayHandler
    {
        protected override bool CanShowIconOverlay(string path)
        {
            if (FileSystem.AlternateDataStreamExists(path, "WinColourLabels"))
            {
                AlternateDataStreamInfo s = new AlternateDataStreamInfo(path,
                new SafeNativeMethods.Win32StreamInfo { StreamName = "WinColourLabels" });

                using (var stream = s.OpenRead())
                {
                    return (FileLabel)stream.ReadByte() == FileLabel.BLUE;
                }
            }
            else return false;
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
