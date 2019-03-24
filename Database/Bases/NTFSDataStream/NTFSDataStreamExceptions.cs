using System;
using System.IO;

namespace WinColourLabels.Database.Bases
{
    public class NTFSDataStreamPathNotFoundException : IOException
    {
        public string FullPath { get; }

        public NTFSDataStreamPathNotFoundException(string path)
            : base($"Path \"{path}\" not found.")
        { FullPath = path; }
    }
    public class NTFSDataStreamNotFoundException : IOException
    {
        public string FullStreamPath { get; }

        public NTFSDataStreamNotFoundException(string fullpathstream)
            : base($"NTFS data stream \"{fullpathstream}\" not found.")
        { FullStreamPath = fullpathstream; }
    }
    public class NTFSDataStreamAlreadyExistException : IOException
    {
        public string FullStreamPath { get; }

        public NTFSDataStreamAlreadyExistException(string fullpathstream)
            : base($"NTFS data stream \"{fullpathstream}\" already exists.")
        { FullStreamPath = fullpathstream; }
    }
    public class NTFSDataStreamNotOpenedException : IOException
    {
        public FileMode Mode { get; }
        public FileAccess Access { get; }
        public FileShare Share { get; }

        public string FullStreamPath { get; }

        public NTFSDataStreamNotOpenedException(string fullpathstream, FileMode mode, FileAccess access, 
            FileShare share, Exception inner) 
            : base($"NTFS data stream \"{fullpathstream}\" can not be opened.", inner)
        {
            FullStreamPath = fullpathstream;

            Mode = mode;
            Access = access;
            Share = share;
        }
    }
}