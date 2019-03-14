using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WinColourLabels.AbstractHandlers;
using Trinet.Core.IO.Ntfs;

namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("d5d205a8-b9e3-41c2-a2f8-dd79b49000a3")]
    public class PurpleLabelOverlay : AbstractIconOverlayHandler
    {
        protected override bool CanShowIconOverlay(string path)
        {
            if (FileSystem.AlternateDataStreamExists(path, "WinColourLabels"))
            {
                AlternateDataStreamInfo s = new AlternateDataStreamInfo(path,
                new SafeNativeMethods.Win32StreamInfo { StreamName = "WinColourLabels" });

                using (var stream = s.OpenRead())
                {
                    return (FileLabel)stream.ReadByte() == FileLabel.PURPLE;
                }
            }
            else return false;
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
