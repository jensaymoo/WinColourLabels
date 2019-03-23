using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace WinColourLabels.Database.Bases
{
    class NTFSStream
    {
        private string fullpath;
        public string FullPath { get { return fullpath; } }

        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                if (File.Exists(value))
                    UpdatePath();
                else if (Directory.Exists(value))
                    UpdatePath();
                else
                    throw new FileNotFoundException();

                void UpdatePath()
                {
                    path = value;
                    UpdateFullPath();
                }
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

        public NTFSStream(string Path, string StreamName)
        {
            this.Path = Path;
            this.StreamName = StreamName;
        }

        public FileStream Open(FileAccess access, FileMode mode, FileShare share)
        {
            if (mode == FileMode.Append) mode = FileMode.OpenOrCreate;
            new FileIOPermission(CalculateAccess(mode, access), path).Demand();

            SafeFileHandle handle = CreateFile(fullpath, access, share, IntPtr.Zero, mode, 0, IntPtr.Zero);
            if (handle.IsInvalid) throw new IOException(string.Empty, new Win32Exception());

            return new FileStream(handle, access);
        }
        public bool Delete()
        {
            if (Exists())
            {
                new FileIOPermission(FileIOPermissionAccess.Write, path).Demand();

                return DeleteFile(fullpath);
            }
            else
                throw new FileNotFoundException();
        }

        public bool Exists() => -1 != GetFileAttributes(fullpath);
        public static bool Exists(string Path, string StreamName) => -1 != GetFileAttributes(BuildStreamPath(Path, StreamName));
        
        private void UpdateFullPath() => fullpath = BuildStreamPath(path, streamname);

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
