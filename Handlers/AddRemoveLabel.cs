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


namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("3d855eb0-f92b-42a7-9ee8-c715d2c13942")]
    public sealed class AddRemoveLabelHandler : AbstractContextMenuHandler
    {
        Database dbase = Database.GetInstance();

        protected override bool CanShowContextMenu()
        {
            //foreach (var path in SelectedItemPaths)
            //{
            //    if (Path.GetExtension(path) == ".lnk") return false;
            //}

            return true;
        }
        protected override ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();

            var itemred = new ToolStripMenuItem
            {
                Text = "Добавить метку \"Красный\"",
                Image = Properties.Resources.Red16
            };
            itemred.Click += (sender, args) => AddLabel(FileLabel.RED);
            menu.Items.Add(itemred);

            var itemgreen = new ToolStripMenuItem
            {
                Text = "Добавить метку \"Зеленый\"",
                Image = Properties.Resources.Green16
            };
            itemgreen.Click += (sender, args) => AddLabel(FileLabel.GREEN);
            menu.Items.Add(itemgreen);

            var itemblue = new ToolStripMenuItem
            {
                Text = "Добавить метку \"Синий\"",
                Image = Properties.Resources.Blue16
            };
            itemblue.Click += (sender, args) => AddLabel(FileLabel.BLUE);
            menu.Items.Add(itemblue);

            var itemorange = new ToolStripMenuItem
            {
                Text = "Добавить метку \"Оранжевый\"",
                Image = Properties.Resources.Orange16
            };
            itemorange.Click += (sender, args) => AddLabel(FileLabel.ORANGE);
            menu.Items.Add(itemorange);

            var itempurple = new ToolStripMenuItem
            {
                Text = "Добавить метку \"Фиолетовый\"",
                Image = Properties.Resources.Purple16
            };
            itempurple.Click += (sender, args) => AddLabel(FileLabel.PURPLE);
            menu.Items.Add(itempurple);

            foreach (var path in SelectedPaths)
            {
                FileLabel label = dbase.GetFileLabel(path);
                switch (label)
                {
                    case FileLabel.NOTHING:
                        continue;
                    case FileLabel.RED:
                        itemred.Text = "Удалить метку \"Красный\"";
                        itemred.Click += (sender, args) => RemoveLabel(FileLabel.RED);
                        break;
                    case FileLabel.GREEN:
                        itemgreen.Text = "Удалить метку \"Зеленый\"";
                        itemgreen.Click += (sender, args) => RemoveLabel(FileLabel.GREEN);
                        break;
                    case FileLabel.BLUE:
                        itemblue.Text = "Удалить метку \"Синий\"";
                        itemblue.Click += (sender, args) => RemoveLabel(FileLabel.BLUE);
                        break;
                    case FileLabel.ORANGE:
                        itemorange.Text = "Удалить метку \"Оранжевый\"";
                        itemorange.Click += (sender, args) => RemoveLabel(FileLabel.ORANGE);
                        break;
                    case FileLabel.PURPLE:
                        itempurple.Text = "Удалить метку \"Фиолетовый\"";
                        itempurple.Click += (sender, args) => RemoveLabel(FileLabel.PURPLE);
                        break;
                }
            }
            return menu;
        }
        private void AddLabel(FileLabel tag)
        {
            foreach (var path in SelectedPaths)
            {
                dbase.SetFileLabel(path, tag);
                Shell32.SHChangeNotify(0x00002000, 0x0005, Marshal.StringToHGlobalUni(path), IntPtr.Zero);
            }
            dbase.SaveBaseAsync();
        }
        private void RemoveLabel(FileLabel label)
        {
            foreach (var path in SelectedPaths)
            {
                if (dbase.GetFileLabel(path) == label)
                {
                    dbase.SetFileLabel(path, FileLabel.NOTHING);
                    Shell32.SHChangeNotify(0x00002000, 0x0005, Marshal.StringToHGlobalUni(path), IntPtr.Zero);
                }
            }
            dbase.SaveBaseAsync();
        }
    }
}
