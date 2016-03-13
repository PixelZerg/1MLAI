using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1MLVisual
{
    public partial class ExportDialog : Form
    {
        public ExportDialog()
        {
            InitializeComponent();
        }
        public ExportDialog(string txt)
        {
            richTextBox1.Text = txt;
        }
    }
}
