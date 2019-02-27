using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinColourLabels.Database
{
    interface IDatabase
    {
        FileLabel GetFileLabel(string path);
        void SetFileLabel(string path, FileLabel label);

        void SaveBase();
        void SaveBaseAsync();
    }
}
