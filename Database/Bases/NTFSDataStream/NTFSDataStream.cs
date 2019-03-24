using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace WinColourLabels.Database.Bases
{
    class NTFSDataStream
    {
        private string fullpathstream;
        public string FullPathStream { get { return fullpathstream; } }

        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                if (File.Exists(value) || Directory.Exists(value))
                {
                    path = value;
                    UpdateFullPath();
                }
                else
                    throw new NTFSDataStreamPathNotFoundException(value);
            }
        }

        private string streamname;
        public string StreamName
        {
            get { return streamname; }
            set
            {
                streamname = value;
                if (!string.IsNullOrEmpty(value)) UpdateFullPath();
            }
        }

        public NTFSDataStream(string Path, string StreamName)
        {
            this.Path = Path;
            this.StreamName = StreamName;
        }

        public FileStream Open(FileAccess access, FileMode mode, FileShare share)
        {
            if (mode == FileMode.Append) mode = FileMode.OpenOrCreate;

            bool exist = Exists();

            if (mode == FileMode.Open) if(!exist) throw new NTFSDataStreamNotFoundException(fullpathstream);
            if (mode == FileMode.Create) if (exist) throw new NTFSDataStreamAlreadyExistException(fullpathstream);
            
            new FileIOPermission(CalculateAccess(mode, access), path).Demand();

            SafeFileHandle handle = CreateFile(fullpathstream, access, share, IntPtr.Zero, mode, 0, IntPtr.Zero);
            if (handle.IsInvalid) throw new NTFSDataStreamNotOpenedException(fullpathstream, mode, access, share, new Win32Exception());

            return new FileStream(handle, access);
        }
        public bool Delete()
        {
            if (Exists())
            {
                new FileIOPermission(FileIOPermissionAccess.Write, path).Demand();

                return DeleteFile(fullpathstream);
            }
            else
                throw new NTFSDataStreamNotFoundException(fullpathstream);
        }

        public bool Exists() => -1 != GetFileAttributes(fullpathstream);
        public static bool Exists(string Path, string StreamName) => -1 != GetFileAttributes(BuildStreamPath(Path, StreamName));
        
        private void UpdateFullPath() => fullpathstream = BuildStreamPath(path, streamname);

        private static string BuildStreamPath(string Path, string StreamName)
        {
            if (string.IsNullOrEmpty(StreamName))
                return Path;
            else
                return Path + ':' + StreamName;
        }
        private static FileIOPermissionAccess CalculateAccess(FileMode mode, FileAccess access)
        {
            FileIOPermissionAccess permissions = FileIOPermissionAccess.NoAccess;
            switch (mode)
            {
                case FileMode.Append:
                    permissions = FileIOPermissionAccess.Append;
                    break;

                case FileMode.Create:
                case FileMode.CreateNew:
                case FileMode.OpenOrCreate:
                case FileMode.Truncate:
                    permissions = FileIOPermissionAccess.Write;
                    break;

                case FileMode.Open:
                    permissions = FileIOPermissionAccess.Read;
                    break;
            }
            switch (access)
            {
                case FileAccess.ReadWrite:
                    permissions |= FileIOPermissionAccess.Write;
                    permissions |= FileIOPermissionAccess.Read;
                    break;

                case FileAccess.Write:
                    permissions |= FileIOPermissionAccess.Write;
                    break;

                case FileAccess.Read:
                    permissions |= FileIOPermissionAccess.Read;
                    break;
            }

            return permissions;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool DeleteFile(string name);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetFileAttributes(string fileName);
    }
}
