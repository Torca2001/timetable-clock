using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SplashScreen;
using Newtonsoft.Json;

namespace SchoolManager
{
    public partial class Themefrm : Form
    {
        public Themefrm()
        {
            InitializeComponent();
        }

        public void Loadthemes()
        {
            try
            {
                string[] Files = Directory.GetFiles(Program.SETTINGS_DIRECTORY + "/Themes", "Theme.Json",
                    SearchOption.AllDirectories);
                listView1.Items.Clear();
                foreach (Themedata Dtheme in Program.Themes.Values)
                {
                    if (Dtheme!= Program.Themedata)
                    Dtheme.IDisposable();
                }
                Program.Themes.Clear();
                ListViewItem themeItem = new ListViewItem(Program.Defaulttheme.Name);
                themeItem.SubItems.Add(Program.Defaulttheme.Version);
                themeItem.SubItems.Add(Program.Defaulttheme.Author);
                listView1.Items.Add(themeItem);
                foreach (string file in Files)
                {
                    try
                    {
                        Themedata theme = JsonConvert.DeserializeObject<Themedata>(File.ReadAllText(file),
                            new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate });
                        if (Program.Themes.ContainsKey(theme.Name))
                            continue;
                        string directory = file.Substring(0, file.LastIndexOf("\\"));
                        theme.Preview = File.Exists(directory + "/Preview.png")
                            ? Image.FromFile(directory + "/Preview.png")
                            : null;
                        theme.Clockimage = File.Exists(directory + "/Clock.png")
                            ? Image.FromFile(directory + "/Clock.png")
                            : Program.Defaulttheme.Clockimage;
                        theme.Timetableimage = File.Exists(directory + "/Timetable.png")
                            ? ResizeImage(Image.FromFile(directory + "/Timetable.png"), 1180, 590)
                            : Program.Defaulttheme.Timetableimage;
                        Program.Themes.Add(theme.Name, theme);
                        ListViewItem themeItem2 = new ListViewItem(theme.Name);
                        themeItem2.SubItems.Add(theme.Version);
                        themeItem2.SubItems.Add(theme.Author);
                        listView1.Items.Add(themeItem2);
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(exp);
                    }
                }
            }
            catch
            {
                //do nothing
            }
        }

        private void Themefrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = (e.CloseReason == CloseReason.UserClosing);  // this cancels the close event.
        }

        private void Folderbutton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Program.SETTINGS_DIRECTORY+"/Themes");
        }

        private void Refreshbutton_Click(object sender, EventArgs e)
        {
            Loadthemes();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            if (listView1.SelectedItems[0].Text == "Default")
            {
                Previewbox.BackgroundImage = null;
                ItemName.Text = "Title: Default";
                ItemAuthor.Text = "Author: WilliamC";
                ItemVer.Text = "Version: 1";
                return;
            }
            if (Program.Themes.ContainsKey(listView1.SelectedItems[0].Text))
            {
                Previewbox.BackgroundImage = Program.Themes[listView1.SelectedItems[0].Text].Preview;
                ItemName.Text = "Title: "+Program.Themes[listView1.SelectedItems[0].Text].Name;
                ItemAuthor.Text = "Author: "+Program.Themes[listView1.SelectedItems[0].Text].Author;
                ItemVer.Text = "Version: "+Program.Themes[listView1.SelectedItems[0].Text].Version;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            if (listView1.SelectedItems[0].Text == "Default")
            {
                Program.Themedata = null;
                Program.SettingsData.Theme = "Default";
                return;
            }

            if (Program.Themes.ContainsKey(listView1.SelectedItems[0].Text))
            {
                Program.Themedata = Program.Themes[listView1.SelectedItems[0].Text];
                Program.SettingsData.Theme = listView1.SelectedItems[0].Text;
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            if (listView1.SelectedItems[0].Text == "Default")
                return;

            DialogResult result =
                MessageBox.Show("Are you sure you want to delete "+ listView1.SelectedItems[0].Text+"?", "Delete item", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes)
                return;
            if (Program.Themes.ContainsKey(listView1.SelectedItems[0].Text))
            {
                Program.Themes[listView1.SelectedItems[0].Text].IDisposable();
                Program.Themes.Remove(listView1.SelectedItems[0].Text);
                listView1.SelectedItems[0].Remove();
            }
        }
    }
}
