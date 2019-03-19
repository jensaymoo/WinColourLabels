using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WinColourLabels.AbstractHandlers;
using Trinet.Core.IO.Ntfs;

namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("2b471e59-7514-48a6-9f03-359b540262ca")]
    public class GreenLabelOverlay : AbstractIconOverlayHandler
    {
        protected override bool CanShowIconOverlay(string path)
        {
            if (FileSystem.AlternateDataStreamExists(path, AddRemoveLabelHandler.ntfs_stram_name))
            {
                AlternateDataStreamInfo s = new AlternateDataStreamInfo(path, AddRemoveLabelHandler.ntfs_stram_name, null, true);
                using (var stream = s.OpenRead())
                {
                    return (FileLabel)stream.ReadByte() == FileLabel.GREEN;
                }
            }
            else return false;
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
