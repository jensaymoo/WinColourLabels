using System;
using System.IO;

namespace WinColourLabels.Database.Bases
{
    public class NTFSDataStreamException : IOException
    {
        public string FullPath { get; }
        public NTFSDataStreamException(string message, string fullpathstream)
            : base(message)
        { FullPath = fullpathstream; }
    }
    public class NTFSDataStreamPathNotFoundException : NTFSDataStreamException
    {    
        public NTFSDataStreamPathNotFoundException(string fullpathstream)
            : base($"Path \"{fullpathstream}\" not found.", fullpathstream)
        { }
    }
    public class NTFSDataStreamPathIncorrectException : NTFSDataStreamException
    {
        public NTFSDataStreamPathIncorrectException(string fullpathstream)
            : base($"The path \"{fullpathstream}\" syntax is incorrect.", fullpathstream)
        { }
    }
    public class NTFSDataStreamAccessDeniedException : NTFSDataStreamException
    {
        public NTFSDataStreamAccessDeniedException(string fullpathstream)
            : base($"Access to the \"{fullpathstream}\" denied.", fullpathstream)
        { }
    }
    public class NTFSDataStreamSharingVoliationException : NTFSDataStreamException
    {
        public NTFSDataStreamSharingVoliationException(string fullpathstream)
            : base($"Cannot access the \"{fullpathstream}\" because it is being used by another process.", fullpathstream)
        { }
    }
    public class NTFSDataStreamLockVoliationException : NTFSDataStreamException
    {
        public NTFSDataStreamLockVoliationException(string fullpathstream)
            : base($"Cannot access the \"{fullpathstream}\" because another process has locked a portion of the file.", fullpathstream)
        { }
    }
    public class NTFSDataStreamAlreadyExistException : NTFSDataStreamException
    {
        public NTFSDataStreamAlreadyExistException(string fullpathstream)
            : base($"\"{fullpathstream}\" already exists.", fullpathstream)
        { }
    }

}