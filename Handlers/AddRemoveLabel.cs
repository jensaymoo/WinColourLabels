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
            EventHandler redaddhandler = (s, e) => AddLabel(FileLabel.RED);
            itemred.Click += redaddhandler ;
            menu.Items.Add(itemred);

            var itemgreen = new ToolStripMenuItem
            {
                Text = "Добавить метку \"Зеленый\"",
                Image = Properties.Resources.Green16
            };
            EventHandler greenaddhandler = (sender, args) => AddLabel(FileLabel.GREEN);
            itemgreen.Click += greenaddhandler;
            menu.Items.Add(itemgreen);

            var itemblue = new ToolStripMenuItem
            {
                Text = "Добавить метку \"Синий\"",
                Image = Properties.Resources.Blue16
            };
            EventHandler blueaddhandler = (sender, args) => AddLabel(FileLabel.BLUE);
            itemblue.Click += blueaddhandler;
            menu.Items.Add(itemblue);

            var itemorange = new ToolStripMenuItem
            {
                Text = "Добавить метку \"Оранжевый\"",
                Image = Properties.Resources.Orange16
            };
            EventHandler orangeaddhandler = (sender, args) => AddLabel(FileLabel.ORANGE);
            itemorange.Click += orangeaddhandler;
            menu.Items.Add(itemorange);

            var itempurple = new ToolStripMenuItem
            {
                Text = "Добавить метку \"Фиолетовый\"",
                Image = Properties.Resources.Purple16
            };
            EventHandler purpleaddhandler = (sender, args) => AddLabel(FileLabel.PURPLE);
            itempurple.Click += purpleaddhandler;
            menu.Items.Add(itempurple);

            for (int i = 0; i < SelectedPaths.Length; i++)
            {
                FileLabel label = dbase.GetFileLabel(SelectedPaths[i]);
                switch (label)
                {
                    case FileLabel.NOTHING:
                        continue;
                    case FileLabel.RED:
                        itemred.Text = "Удалить метку \"Красный\"";
                        itemred.Click -= redaddhandler;
                        itemred.Click += (sender, args) => RemoveLabel(FileLabel.RED);
                        break;
                    case FileLabel.GREEN:
                        itemgreen.Text = "Удалить метку \"Зеленый\"";
                        itemgreen.Click -= greenaddhandler;
                        itemgreen.Click += (sender, args) => RemoveLabel(FileLabel.GREEN);
                        break;
                    case FileLabel.BLUE:
                        itemblue.Text = "Удалить метку \"Синий\"";
                        itemblue.Click -= blueaddhandler;
                        itemblue.Click += (sender, args) => RemoveLabel(FileLabel.BLUE);
                        break;
                    case FileLabel.ORANGE:
                        itemorange.Text = "Удалить метку \"Оранжевый\"";
                        itemorange.Click -= orangeaddhandler;
                        itemorange.Click += (sender, args) => RemoveLabel(FileLabel.ORANGE);
                        break;
                    case FileLabel.PURPLE:
                        itempurple.Text = "Удалить метку \"Фиолетовый\"";
                        itempurple.Click -= purpleaddhandler;
                        itempurple.Click += (sender, args) => RemoveLabel(FileLabel.PURPLE);
                        break;
                }
            }
            return menu;
        }
        private void AddLabel(FileLabel label)
        {
            for (int i = 0; i < SelectedPaths.Length; i++)
            {
                dbase.SetFileLabel(SelectedPaths[i], label);
                Shell32.SHChangeNotify(0x00002000, 0x0005, Marshal.StringToHGlobalUni(SelectedPaths[i]), IntPtr.Zero);
            }
            dbase.SaveBaseAsync();
        }
        private void RemoveLabel(FileLabel label)
        {
            for(int i =0; i < SelectedPaths.Length; i++)
            {
                if (dbase.GetFileLabel(SelectedPaths[i]) == label)
                {
                    dbase.SetFileLabel(SelectedPaths[i], FileLabel.NOTHING);

                    Shell32.SHChangeNotify(0x00002000, 0x0005, Marshal.StringToHGlobalUni(SelectedPaths[i]), IntPtr.Zero);
                }
            }
            dbase.SaveBaseAsync();
        }
    }
}
