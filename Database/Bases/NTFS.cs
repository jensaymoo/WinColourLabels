using System.IO;
using NTFSDataStreams;
using NTFSDataStreams.Exceptions;

namespace WinColourLabels.Database.Bases
{
    class NTFS : IDatabase
    {
        private string ntfs_stream_name = "WinColourLabels";

        public FileLabel GetLabel(string path)
        {
            if (NTFSDataStream.Exists(path, ntfs_stream_name))
            {
                try
                {
                    using (var stream = new NTFSDataStream(path, ntfs_stream_name,
                    FileAccess.Read, FileMode.Open, FileShare.ReadWrite))
                    {
                        return (FileLabel)stream.ReadByte();
                    }
                }
                catch (NTFSDataStreamException)
                {
                    return FileLabel.NOTHING;
                }
            }
            else return FileLabel.NOTHING;
        }

        public void SetLabel(string path, FileLabel label)
        {
            if (label != FileLabel.NOTHING)
            {
                try
                {
                    using (var stream = new NTFSDataStream(path, ntfs_stream_name,
                        FileAccess.Write, FileMode.OpenOrCreate, FileShare.Read))
                    {
                        stream.WriteByte((byte)label);
                    }
                }
                catch (NTFSDataStreamException)
                {
                    return;
                }
            }
            else if (NTFSDataStream.Exists(path, ntfs_stream_name))
            {
                try
                {
                    NTFSDataStream.Delete(path, ntfs_stream_name);
                }
                catch (NTFSDataStreamException)
                {
                    return;
                }
            }
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
