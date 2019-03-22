using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinColourLabels.Database
{
    interface IDatabase
    {
        FileLabel GetLabel(string path);
        void SetLabel(string path, FileLabel label);
        void SetLabel(string[] path, FileLabel label);
    }
}
