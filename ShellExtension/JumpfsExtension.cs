using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using Core;

namespace ShellExtension
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".txt")]
    public class JumpfsExtension : SharpContextMenu
    {
        protected override bool CanShowMenu() => true;

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            //  Create a 'count lines' item
            var itemCountLines = new ToolStripMenuItem
            {
                Text = "Count Lines"
            };

            //  When we click, we'll call the 'CountLines' function
            itemCountLines.Click += (sender, args) => CountLines();

            //  Add the item to the context menu
            menu.Items.Add(itemCountLines);

            //  Return the menu
            return menu;
        }

        private void CountLines()
        {
            //  Builder for the output
            var builder = new StringBuilder();

            //  Go through each file
            foreach (var filePath in SelectedItemPaths)
            {
                //  Count the lines
                builder.AppendLine(string.Format("{0} - {1} Lines",
                    Path.GetFileName(filePath), File.ReadAllLines(filePath).Length));
            }

            var x = new CoreTest()
            //  Show the output

            MessageBox.Show(builder.ToString());
        }
    }
}
