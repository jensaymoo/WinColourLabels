using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace WinColourLabels.Database.Bases
{
    class NTFSStream
    {
        private string fullpath;
        public NTFSStream(string Path, string StreamName)
        {
            fullpath = Path + ":" + StreamName;
        }

        public FileStream Open(FileAccess access, FileMode mode, FileShare share)
        {
            if (mode == FileMode.Append) mode = FileMode.OpenOrCreate;

            SafeFileHandle handle = CreateFile(fullpath, access, share, IntPtr.Zero, mode, 0, IntPtr.Zero);
            if (handle.IsInvalid) throw new UnauthorizedAccessException("Не удалось открыть поток файла.", new Win32Exception());

            return new FileStream(handle, access);
        }
        public void Delete()
        {
            if (Exists())
                DeleteFile(fullpath);
        }

        public bool Exists()
        {
            return -1 != GetFileAttributes(fullpath);
        }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool DeleteFile(string name);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetFileAttributes(string fileName);
    }
}
