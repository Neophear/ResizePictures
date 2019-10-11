using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResizePictures
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnChangeFolder_Click(object sender, EventArgs e)
        {
            if(fbdTargetFolder.ShowDialog() == DialogResult.OK)
                txtTargetFolder.Text = fbdTargetFolder.SelectedPath;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string resizePath = txtTargetFolder.Text + "\\Resize\\";
            Directory.CreateDirectory(resizePath);
            List<string> filenames = new List<string>(Directory.GetFiles(txtTargetFolder.Text));
            List<string> newFilenames = new List<string>();

            for (int i = 1; i <= filenames.Count; i++)
                newFilenames.Add(String.Format("image{0:000}.jpg", i));

            Random rnd = new Random();

            foreach (string filename in Directory.GetFiles(txtTargetFolder.Text))
            {
                Bitmap original = new Bitmap(filename);
                int filenameIndex = rnd.Next(0, newFilenames.Count);

                if (original.Height > nudHeight.Value || original.Width > nudWidth.Value)
                {
                    Size oldSize = original.Size;
                    Size newSize = original.Size;

                    if (oldSize.Width > nudWidth.Value)
                    {
                        newSize.Width = Convert.ToInt32(nudWidth.Value);
                        double scaling = (double)nudWidth.Value / oldSize.Width;
                        newSize.Height = Convert.ToInt32(oldSize.Height * scaling);
                    }

                    if (oldSize.Height > nudHeight.Value)
                    {
                        newSize.Height = Convert.ToInt32(nudHeight.Value);
                        double scaling = (double)nudHeight.Value / oldSize.Height;
                        newSize.Width = Convert.ToInt32(oldSize.Width * scaling);
                    }

                    Bitmap newImage = new Bitmap(newSize.Width, newSize.Height);
                    using (Graphics gr = Graphics.FromImage(newImage))
                    {
                        gr.SmoothingMode = SmoothingMode.HighQuality;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        gr.DrawImage(original, new Rectangle(0, 0, newSize.Width, newSize.Height));
                    }

                    

                    newImage.Save(resizePath + newFilenames[filenameIndex], System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                    File.Copy(filename, resizePath + newFilenames[filenameIndex]);

                newFilenames.RemoveAt(filenameIndex);
            }
        }
    }
}