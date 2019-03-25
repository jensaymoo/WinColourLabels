using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinColourLabels.Database.Bases
{
    enum ErrorCodes : int
    {
        ERROR_SUCCESS = 0,
        ERROR_FILE_NOT_FOUND = 2,
        ERROR_PATH_NOT_FOUND = 3,
        ERROR_ACCESS_DENIED = 5,
        ERROR_SHARING_VIOLATION = 32,
        ERROR_LOCK_VIOLATION = 33,
        ERROR_FILE_EXISTS = 80,
        ERROR_INVALID_PARAMETER = 87,
        ERROR_BUFFER_OVERFLOW = 111,
        ERROR_INVALID_NAME = 123,
        ERROR_ALREADY_EXISTS = 183,
        ERROR_FILENAME_EXCED_RANGE = 206,
    }

    static class NTFSDataStreamNative
    {
        public static void Throw(int error, string path)
        {
            switch (error)
            {
                case (int)ErrorCodes.ERROR_SUCCESS: return;
                case (int)ErrorCodes.ERROR_FILE_NOT_FOUND:
                case (int)ErrorCodes.ERROR_PATH_NOT_FOUND:
                    throw new NTFSDataStreamPathNotFoundException(path);
                case (int)ErrorCodes.ERROR_ACCESS_DENIED:
                    throw new NTFSDataStreamAccessDeniedException(path);
                case (int)ErrorCodes.ERROR_SHARING_VIOLATION:
                    throw new NTFSDataStreamSharingVoliationException(path);
                case (int)ErrorCodes.ERROR_LOCK_VIOLATION:
                    throw new NTFSDataStreamLockVoliationException(path);
                case (int)ErrorCodes.ERROR_FILE_EXISTS:
                case (int)ErrorCodes.ERROR_ALREADY_EXISTS:
                    throw new NTFSDataStreamAlreadyExistException(path);
                case (int)ErrorCodes.ERROR_BUFFER_OVERFLOW:
                case (int)ErrorCodes.ERROR_INVALID_NAME:
                case (int)ErrorCodes.ERROR_FILENAME_EXCED_RANGE:
                    throw new NTFSDataStreamPathIncorrectException(path);
                default:
                    {
                        Marshal.ThrowExceptionForHR(-2147024896 | error);
                        break;
                    }
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeleteFile(string name);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetFileAttributes(string fileName);

    }
}
