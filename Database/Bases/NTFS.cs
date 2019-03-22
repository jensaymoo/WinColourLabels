using System;
using System.IO;

namespace WinColourLabels.Database.Bases
{
    class NTFS : IDatabase
    {
        private string ntfs_stream_name = "WinColourLabels";

        public FileLabel GetLabel(string path)
        {
            NTFSStream stream = new NTFSStream(path, ntfs_stream_name);
            if (stream.Exists())
            {
                using (var filestream = stream.Open(FileAccess.Read, FileMode.Open, FileShare.Read))
                {
                    return (FileLabel)filestream.ReadByte();
                }
            }
            else return FileLabel.NOTHING;
        }

        public void SetLabel(string path, FileLabel label)
        {
            NTFSStream stream = new NTFSStream(path, ntfs_stream_name);
            try
            {
                if (label != FileLabel.NOTHING)
                {
                    using (var filestream = stream.Open(FileAccess.Write, FileMode.OpenOrCreate, FileShare.Write))
                    {
                        filestream.WriteByte((byte)label);
                    }
                }
                else stream.Delete();
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
