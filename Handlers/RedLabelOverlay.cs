using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WinColourLabels.AbstractHandlers;
using Trinet.Core.IO.Ntfs;

namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("1ea95473-bc83-4fbf-8c12-33b804365cc5")]
    public class RedLabelOverlay : AbstractIconOverlayHandler
    {
        protected override bool CanShowIconOverlay(string path)
        {
            if (FileSystem.AlternateDataStreamExists(path, "WinColourLabels"))
            {
                AlternateDataStreamInfo s = new AlternateDataStreamInfo(path,
                new SafeNativeMethods.Win32StreamInfo { StreamName = "WinColourLabels" });

                using (var stream = s.OpenRead())
                {
                    return (FileLabel)stream.ReadByte() == FileLabel.RED;
                }
            }
            else return false;
        }

        protected override Icon GetIconOverlay()
        {
            return Properties.Resources.Red;
        }
        protected override int GetIconOverlayPriority()
        {
            return 0;
        }
    }
}
