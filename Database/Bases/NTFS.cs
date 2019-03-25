using System;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace WinColourLabels.Database.Bases
{
    class NTFS : IDatabase
    {
        private string ntfs_stream_name = "WinColourLabels";

        public FileLabel GetLabel(string path)
        {
            NTFSDataStream stream = new NTFSDataStream(path, ntfs_stream_name);
            if (stream.Exists())
            {
                try
                {
                    using (var filestream = stream.Open(FileAccess.Read, FileMode.Open, FileShare.ReadWrite))
                    {
                        return (FileLabel)filestream.ReadByte();
                    }
                }
                catch (NTFSDataStreamException)
                { return FileLabel.NOTHING; }
            }
            else return FileLabel.NOTHING;
        }

        public void SetLabel(string path, FileLabel label)
        {
            NTFSDataStream stream = new NTFSDataStream(path, ntfs_stream_name);
            if (label != FileLabel.NOTHING)
            {
                try
                {
                    using (var filestream = stream.Open(FileAccess.Write, FileMode.OpenOrCreate, FileShare.Read))
                    {
                        filestream.WriteByte((byte)label);
                    }
                }
                catch (NTFSDataStreamException)
                { return; }
        }
            else if (stream.Exists()) stream.Delete();
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
