using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace WinColourLabels.AbstractHandlers
{
    [ComVisible(false)]
    [COMServerAssociation(AssociationType.AllFilesAndFolders)]
    public abstract class AbstractContextMenuHandler : SharpContextMenu
    {
        protected string[] SelectedPaths
        {
            get { return SelectedItemPaths.ToArray(); }
        }

        protected abstract bool CanShowContextMenu();
        protected abstract ContextMenuStrip CreateContextMenu();

        protected override bool CanShowMenu() => CanShowContextMenu(); 
        protected override ContextMenuStrip CreateMenu() => CreateContextMenu();

    }
}
