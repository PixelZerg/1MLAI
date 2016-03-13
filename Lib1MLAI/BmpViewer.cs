using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib1MLAI
{
    public partial class BmpViewer : Form
    {
        private Bitmap bmp;
        public BmpViewer(Bitmap _bmp)
        {
            InitializeComponent();
            bmp = _bmp;
        }

        private void BmpViewer_Load(object sender, EventArgs e)
        {
            foreach (PictureBoxSizeMode sm in Enum.GetValues(typeof(PictureBoxSizeMode)))
            {
                comboBox1.Items.Add(sm);
            }
            LoadImage(bmp);
        }
        public void LoadImage(Bitmap b)
        {
            try
            {
                pb.Image = b;
                if (pb.Image.Size.Width > 200)
                {
                    this.Size = pb.Image.Size;
                }
                else
                {
                    if (pb.Image.Size.Width > 50)
                    {
                        this.Size = new Size(pb.Image.Size.Width * 4, pb.Image.Size.Height * 4);
                    }
                    else
                    {
                        this.Size = new Size(pb.Image.Size.Width * 16, pb.Image.Size.Height * 16);
                    }
                }
            }
            catch
            {
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pb.SizeMode = (PictureBoxSizeMode)comboBox1.SelectedItem;
        }
    }
}
