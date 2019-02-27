using SharpShell.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using WinColourLabels.AbstractHandlers;
using WinColourLabels.Database;


namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("3d855eb0-f92b-42a7-9ee8-c715d2c13942")]
    public sealed class AddRemoveLabelHandler : AbstractContextMenuHandler
    {
        private static ContextMenuStrip menu;
        private static ToolStripMenuItem itemred;
        private static ToolStripMenuItem itemgreen;
        private static ToolStripMenuItem itemblue;
        private static ToolStripMenuItem itemorange;
        private static ToolStripMenuItem itempurple;

        private static EventHandler handlerred;
        private static EventHandler handlergreen;
        private static EventHandler handlerblue;
        private static EventHandler handlerorange;
        private static EventHandler handlerpurple;

        static AddRemoveLabelHandler()
        {
            menu = new ContextMenuStrip();
            menu.Items.Add(new ToolStripButton());

            itemred = new ToolStripMenuItem
            {
                Image = Properties.Resources.Red16
            };
            menu.Items.Add(itemred);

            itemgreen = new ToolStripMenuItem
            {
                Image = Properties.Resources.Green16
            };
            menu.Items.Add(itemgreen);

            itemblue = new ToolStripMenuItem
            {
                Image = Properties.Resources.Blue16
            };
            menu.Items.Add(itemblue);

            itemorange = new ToolStripMenuItem
            {
                Image = Properties.Resources.Orange16
            };
            menu.Items.Add(itemorange);

            itempurple = new ToolStripMenuItem
            {
                Image = Properties.Resources.Purple16
            };
            menu.Items.Add(itempurple);
        }

        protected override bool CanShowContextMenu()
        {
            return true;
        }
        protected override ContextMenuStrip CreateContextMenu()
        {
            bool multipleselecteditems = SelectedPaths.Length > 1 ? true : false;

            string addlabel, deletelabel;

            if (multipleselecteditems)
            {
                addlabel = "Добавить метки";
                deletelabel = "Удалить метки";
            }
            else
            {
                addlabel = "Добавить метку";
                deletelabel = "Удалить метку";
            }

            itemred.Text = addlabel + " \"Красный\"";
            itemred.Click -= handlerred;
            handlerred = (sender, args) => AddLabel(FileLabel.RED);
            itemred.Click += handlerred;

            itemgreen.Text = addlabel + " \"Зеленый\"";
            itemgreen.Click -= handlergreen;
            handlergreen = (sender, args) => AddLabel(FileLabel.GREEN);
            itemgreen.Click += handlergreen;

            itemblue.Text = addlabel + " \"Синий\"";
            itemblue.Click -= handlerblue;
            handlerblue = (sender, args) => AddLabel(FileLabel.BLUE);
            itemblue.Click += handlerblue;

            itemorange.Text = addlabel + " \"Оранжевый\"";
            itemorange.Click -= handlerorange;
            handlerorange = (sender, args) => AddLabel(FileLabel.ORANGE);
            itemorange.Click += handlerorange;

            itempurple.Text = addlabel + " \"Фиолетовый\"";
            itempurple.Click -= handlerpurple;
            handlerpurple = (sender, args) => AddLabel(FileLabel.PURPLE);
            itempurple.Click += handlerpurple;

            for (int i = 0; i < SelectedPaths.Length; i++)
            {
                FileLabel label = DatabaseFacade.GetFileLabel(SelectedPaths[i]);
                switch (label)
                {
                    case FileLabel.NOTHING:
                        continue;
                    case FileLabel.RED:
                        itemred.Text = deletelabel + " \"Красный\"";
                        itemred.Click -= handlerred;
                        handlerred = (sender, args) => RemoveLabel(FileLabel.RED);
                        itemred.Click += handlerred;
                        continue;
                    case FileLabel.GREEN:
                        itemgreen.Text = deletelabel + " \"Зеленый\"";
                        itemgreen.Click -= handlergreen;
                        handlergreen = (sender, args) => RemoveLabel(FileLabel.GREEN);
                        itemgreen.Click += handlergreen;
                        continue;
                    case FileLabel.BLUE:
                        itemblue.Text = deletelabel + " \"Синий\"";
                        itemblue.Click -= handlerblue;
                        handlerblue = (sender, args) => RemoveLabel(FileLabel.BLUE);
                        itemblue.Click += handlerblue;
                        continue;
                    case FileLabel.ORANGE:
                        itemorange.Text = deletelabel + " \"Оранжевый\"";
                        itemorange.Click -= handlerorange;
                        handlerorange = (sender, args) => RemoveLabel(FileLabel.ORANGE);
                        itemorange.Click += handlerorange;
                        continue;
                    case FileLabel.PURPLE:
                        itempurple.Text = deletelabel + " \"Фиолетовый\"";
                        itempurple.Click -= handlerpurple;
                        handlerpurple = (sender, args) => RemoveLabel(FileLabel.PURPLE);
                        itempurple.Click += handlerpurple;
                        continue;
                }
            }
            return menu;
        }
        private void AddLabel(FileLabel label)
        {
            for (int i = 0; i < SelectedPaths.Length; i++)
            {
                DatabaseFacade.SetFileLabel(SelectedPaths[i], label);
                Shell32.SHChangeNotify(0x00002000, 0x0005, Marshal.StringToHGlobalUni(SelectedPaths[i]), IntPtr.Zero);
            }
            DatabaseFacade.SaveBaseAsync();
        }
        private void RemoveLabel(FileLabel label)
        {
            for(int i =0; i < SelectedPaths.Length; i++)
            {
                if (DatabaseFacade.GetFileLabel(SelectedPaths[i]) == label)
                {
                    DatabaseFacade.SetFileLabel(SelectedPaths[i], FileLabel.NOTHING);
                    Shell32.SHChangeNotify(0x00002000, 0x0005, Marshal.StringToHGlobalUni(SelectedPaths[i]), IntPtr.Zero);
                }
            }
            DatabaseFacade.SaveBaseAsync();
        }
    }
}
