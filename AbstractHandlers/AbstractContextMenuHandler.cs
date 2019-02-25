using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace WinColourLabels.AbstractHandlers
{
    [ComVisible(false)]
    [COMServerAssociation(AssociationType.AllFilesAndFolders)]
    public abstract class AbstractContextMenuHandler : SharpContextMenu
    {
        protected IEnumerable<string> SelectedPaths
        {
            get { return SelectedItemPaths; }
        }

        protected virtual bool CanShowContextMenu()
        {
            return false;
        }

        protected virtual ContextMenuStrip CreateContextMenu()
        {
            return null;
        }

        protected override bool CanShowMenu()
        {
            return CanShowContextMenu();
        }

        protected override ContextMenuStrip CreateMenu()
        {
            return CreateContextMenu();
        }
    }
}
