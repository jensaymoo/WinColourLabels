using System;
using System.Runtime.InteropServices;
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
        private static string[] selected_items;

        private static ContextMenuStrip menu;

        private static ToolStripMenuItem red_menuitem;
        private static ToolStripMenuItem green_menuitem;
        private static ToolStripMenuItem blue_menuitem;
        private static ToolStripMenuItem orange_menuitem;
        private static ToolStripMenuItem purple_menuitem;
        private static ToolStripMenuItem remove_menuitem;

        private static string red_str_add = "Установить маркер \"Красный\"";
        private static string green_str_add = "Установить маркер \"Зеленый\"";
        private static string blue_str_add = "Установить маркер \"Синий\"";
        private static string orange_str_add = "Установить маркер \"Оранжевый\"";
        private static string purple_str_add = "Установить маркер \"Фиолетовый\"";

        private static string str_remove_all = "Отменить все маркеры...";
        private static string str_remove_empty = "Отменить маркер...";

        private static string red_str_remove = "Отменить маркер \"Красный\"";
        private static string green_str_remove = "Отменить маркер \"Зеленый\"";
        private static string blue_str_remove = "Отменить маркер \"Синий\"";
        private static string orange_str_remove = "Отменить маркер \"Оранжевый\"";
        private static string purple_str_remove = "Отменить маркер \"Фиолетовый\"";

        static AddRemoveLabelHandler()
        {
            menu = new ContextMenuStrip();
            menu.Items.Add(new ToolStripSeparator());

            red_menuitem = new ToolStripMenuItem
            {
                Text = red_str_add,
                Image = Properties.Resources.Red16
            };
            red_menuitem.Click += (sender, args) => DatabaseFacade.SetLabelAsync(selected_items, FileLabel.RED);
            menu.Items.Add(red_menuitem);

            green_menuitem = new ToolStripMenuItem
            {
                Text = green_str_add,
                Image = Properties.Resources.Green16
            };
            green_menuitem.Click += (sender, args) => DatabaseFacade.SetLabelAsync(selected_items, FileLabel.GREEN);
            menu.Items.Add(green_menuitem);

            blue_menuitem = new ToolStripMenuItem
            {
                Text = blue_str_add,
                Image = Properties.Resources.Blue16
            };
            blue_menuitem.Click += (sender, args) => DatabaseFacade.SetLabelAsync(selected_items, FileLabel.BLUE);
            menu.Items.Add(blue_menuitem);

            orange_menuitem = new ToolStripMenuItem
            {
                Text = orange_str_add,
                Image = Properties.Resources.Orange16
            };
            orange_menuitem.Click += (sender, args) => DatabaseFacade.SetLabelAsync(selected_items, FileLabel.ORANGE);
            menu.Items.Add(orange_menuitem);

            purple_menuitem = new ToolStripMenuItem
            {
                Text = purple_str_add,
                Image = Properties.Resources.Purple16
            };
            purple_menuitem.Click += (sender, args) => DatabaseFacade.SetLabelAsync(selected_items, FileLabel.PURPLE);
            menu.Items.Add(purple_menuitem);

            remove_menuitem = new ToolStripMenuItem();
            remove_menuitem.Click += (sender, args) => DatabaseFacade.SetLabelAsync(selected_items, FileLabel.NOTHING);
            menu.Items.Add(remove_menuitem);
        }

        protected override bool CanShowContextMenu() => true;

        protected override ContextMenuStrip CreateContextMenu()
        {
            selected_items = SelectedPaths;

            red_menuitem.Enabled = true;
            green_menuitem.Enabled = true;
            blue_menuitem.Enabled = true;
            orange_menuitem.Enabled = true;
            purple_menuitem.Enabled = true;
            remove_menuitem.Enabled = true;

            if (selected_items.Length > 1)
            {
                remove_menuitem.Text = str_remove_all;
            }
            else
            {
                switch (DatabaseFacade.GetLabel(selected_items[0]))
                {
                    case FileLabel.RED:
                        red_menuitem.Enabled = false;
                        remove_menuitem.Text = red_str_remove;
                        break;
                    case FileLabel.GREEN:
                        green_menuitem.Enabled = false;
                        remove_menuitem.Text = green_str_remove;
                        break;
                    case FileLabel.BLUE:
                        blue_menuitem.Enabled = false;
                        remove_menuitem.Text = blue_str_remove;
                        break;
                    case FileLabel.ORANGE:
                        orange_menuitem.Enabled = false;
                        remove_menuitem.Text = orange_str_remove;
                        break;
                    case FileLabel.PURPLE:
                        purple_menuitem.Enabled = false;
                        remove_menuitem.Text = purple_str_remove;
                        break;
                    case FileLabel.NOTHING:
                        remove_menuitem.Enabled = false;
                        remove_menuitem.Text = str_remove_empty;
                        break;
                }
            }

            return menu;
        }
    }
}
