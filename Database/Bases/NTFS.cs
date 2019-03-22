using System;
using Trinet.Core.IO.Ntfs;

namespace WinColourLabels.Database.Bases
{
    class NTFS : IDatabase
    {
        private string ntfs_stream_name = "WinColourLabels";

        public FileLabel GetLabel(string path)
        {
            if (FileSystem.AlternateDataStreamExists(path, ntfs_stream_name))
            {
                AlternateDataStreamInfo s = new AlternateDataStreamInfo(path, ntfs_stream_name, null, true);
                using (var stream = s.OpenRead())
                {
                    return (FileLabel)stream.ReadByte();
                }
            }
            else return FileLabel.NOTHING;
        }

        public void SetLabel(string path, FileLabel label)
        {
            AlternateDataStreamInfo s = new AlternateDataStreamInfo(path, ntfs_stream_name, null, true);
            try
            {
                if (label != FileLabel.NOTHING)
                {
                    using (var stream = s.OpenWrite())
                    {
                        stream.WriteByte((byte)label);
                    }
                }
                else s.Delete();
            }
            catch (UnauthorizedAccessException)
            { }
        }

        public void SetLabel(string[] paths, FileLabel label)
        {
            foreach (string path in paths)
            {
                SetLabel(path, label);
            }
        }
    }
}
