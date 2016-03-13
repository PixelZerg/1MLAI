using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib1MLAI
{
    public partial class BmpViewerControl : UserControl
    {
        public BmpViewerControl()
        {
            InitializeComponent();
        }

        private void BmpViewerControl_Load(object sender, EventArgs e)
        {
            foreach (PictureBoxSizeMode sm in Enum.GetValues(typeof(PictureBoxSizeMode)))
            {
                comboBox1.Items.Add(sm);
            }
        }
        public void LoadImage(Bitmap b)
        {
            try
            {
                pb.Image = b;
            }
            catch
            {
            }
        }
    }
}
