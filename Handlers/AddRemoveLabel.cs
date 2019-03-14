using SharpShell.Interop;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinColourLabels.AbstractHandlers;
using Trinet.Core.IO.Ntfs;
using System.IO;
using System.Threading.Tasks;

namespace WinColourLabels.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("3d855eb0-f92b-42a7-9ee8-c715d2c13942")]
    public sealed class AddRemoveLabelHandler : AbstractContextMenuHandler
    {
        private static string[] selecteditems;
        private static ContextMenuStrip menu;
        private static ToolStripMenuItem itemred;
        private static ToolStripMenuItem itemgreen;
        private static ToolStripMenuItem itemblue;
        private static ToolStripMenuItem itemorange;
        private static ToolStripMenuItem itempurple;
        private static ToolStripMenuItem itemdelete;


        static AddRemoveLabelHandler()
        {
            menu = new ContextMenuStrip();
            menu.Items.Add(new ToolStripSeparator());

            itemred = new ToolStripMenuItem
            {
                Image = Properties.Resources.Red16
            };
            itemred.Click += (sender, args) => AddLabelAsync(selecteditems, FileLabel.RED);
            menu.Items.Add(itemred);

            itemgreen = new ToolStripMenuItem
            {
                Image = Properties.Resources.Green16
            };
            itemgreen.Click += (sender, args) => AddLabelAsync(selecteditems, FileLabel.GREEN);
            menu.Items.Add(itemgreen);

            itemblue = new ToolStripMenuItem
            {
                Image = Properties.Resources.Blue16
            };
            itemblue.Click += (sender, args) => AddLabelAsync(selecteditems, FileLabel.BLUE);
            menu.Items.Add(itemblue);

            itemorange = new ToolStripMenuItem
            {
                Image = Properties.Resources.Orange16
            };
            itemorange.Click += (sender, args) => AddLabelAsync(selecteditems, FileLabel.ORANGE);
            menu.Items.Add(itemorange);

            itempurple = new ToolStripMenuItem
            {
                Image = Properties.Resources.Purple16
            };
            itempurple.Click += (sender, args) => AddLabelAsync(selecteditems, FileLabel.PURPLE);
            menu.Items.Add(itempurple);

            itemdelete = new ToolStripMenuItem();
            itemdelete.Click += (sender, args) => RemoveLabelAsync(selecteditems);
            menu.Items.Add(itemdelete);
        }

        protected override bool CanShowContextMenu()
        {
            return true;
        }
        protected override ContextMenuStrip CreateContextMenu()
        {
            selecteditems = SelectedPaths;

            itemred.Enabled = true;
            itemgreen.Enabled = true;
            itemblue.Enabled = true;
            itemorange.Enabled = true;
            itempurple.Enabled = true;
            itemdelete.Enabled = true;

            if (selecteditems.Length > 1)
            {
                itemred.Text = "Установить маркер \"Красный\" для всех";
                itemgreen.Text = "Установить маркер \"Зеленый\" для всех";
                itemblue.Text = "Установить маркер \"Синий\" для всех";
                itemorange.Text = "Установить маркер \"Оранжевый\" для всех";
                itempurple.Text = "Установить маркер \"Фиолетовый\" для всех";

                itemdelete.Text = "Отменить все маркеры...";
            }
            else
            {
                itemred.Text = "Установить маркер \"Красный\"";
                itemgreen.Text = "Установить маркер \"Зеленый\"";
                itemblue.Text = "Установить маркер \"Синий\"";
                itemorange.Text = "Установить маркер \"Оранжевый\"";
                itempurple.Text = "Установить маркер \"Фиолетовый\"";

                switch (GetLabel(selecteditems[0]))
                {
                    case FileLabel.RED:
                        itemred.Enabled = false;
                        itemdelete.Text = "Отменить маркер \"Красный\"";
                        break;
                    case FileLabel.GREEN:
                        itemgreen.Enabled = false;
                        itemdelete.Text = "Отменить маркер \"Зеленый\"";
                        break;
                    case FileLabel.BLUE:
                        itemblue.Enabled = false;
                        itemdelete.Text = "Отменить маркер \"Синий\"";
                        break;
                    case FileLabel.ORANGE:
                        itemorange.Enabled = false;
                        itemdelete.Text = "Отменить маркер \"Оранжевый\"";
                        break;
                    case FileLabel.PURPLE:
                        itempurple.Enabled = false;
                        itemdelete.Text = "Отменить маркер \"Фиолетовый\"";
                        break;
                    case FileLabel.NOTHING:
                        itemdelete.Enabled = false;
                        itemdelete.Text = "Отменить маркер...";
                        break;
                }
            }

            return menu;
        }
        private static FileLabel GetLabel(string item)
        {
            if (FileSystem.AlternateDataStreamExists(item, "WinColourLabels"))
            {
                AlternateDataStreamInfo s = new AlternateDataStreamInfo(item, "WinColourLabels", null, true);

                using (var stream = s.OpenRead())
                {
                    return (FileLabel)stream.ReadByte();
                }
            }
            else
                return FileLabel.NOTHING;
        }
        private static void AddLabel(string[] items, FileLabel label)
        {
            for (int i = 0; i < items.Length; i++)
            {
                AlternateDataStreamInfo s = new AlternateDataStreamInfo(items[i],
                    new SafeNativeMethods.Win32StreamInfo { StreamName = "WinColourLabels" });
                try
                {
                    using (var stream = s.OpenWrite())
                    {
                        stream.WriteByte((byte)label);
                    }
                }
                catch(UnauthorizedAccessException ex)
                {
                    DialogResult result = MessageBox.Show($"Доступ к {items[i]} запрещен.", "WinColourLabels", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Exclamation);
                    switch(result)
                    {
                        case DialogResult.Abort: return;
                        case DialogResult.Ignore: continue;
                        case DialogResult.Retry:
                            i--; continue;
                    }
                }

                Shell32.SHChangeNotify(0x00002000, 0x0005, Marshal.StringToHGlobalUni(items[i]), IntPtr.Zero);
            }
        }
        private static async void AddLabelAsync(string[] items, FileLabel label) => await Task.Run(() => AddLabel(items, label));
        private static void RemoveLabel(string[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                AlternateDataStreamInfo s = new AlternateDataStreamInfo(items[i],
                    new SafeNativeMethods.Win32StreamInfo { StreamName = "WinColourLabels" });
                try
                {
                    s.Delete();
                }
                catch (UnauthorizedAccessException ex)
                {
                    DialogResult result = MessageBox.Show($"Доступ к {items[i]} запрещен.", "WinColourLabels", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Exclamation);
                    switch (result)
                    {
                        case DialogResult.Abort: return;
                        case DialogResult.Ignore: continue;
                        case DialogResult.Retry:
                            i--; continue;
                    }
                }
                Shell32.SHChangeNotify(0x00002000, 0x0005, Marshal.StringToHGlobalUni(items[i]), IntPtr.Zero);
            }
        }
        private static async void RemoveLabelAsync(string[] items) => await Task.Run(() => RemoveLabel(items));
    }
}
