using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1MLdbg
{
    class Program
    {
        public static bool running = true;
        static void Main(string[] args)
        {
            Lib1MLAI.Bluestacks.Start();
            Lib1MLAI.BmpViewer bmpviewer = new Lib1MLAI.BmpViewer(new System.Drawing.Bitmap(1,1));
            new System.Threading.Thread(() =>
            {
                int no = 0;
                while (running)
                {
                    Bitmap b = Lib1MLAI.Bluestacks.MainLoop();
                    bmpviewer.LoadImage(b);
                    if (no % 2 == 0)
                    {
                        Lib1MLAI.Bluestacks.actionqueue.Add(true);
                    }
                    else
                    {
                        Lib1MLAI.Bluestacks.actionqueue.Add(false);
                    }
                    no++;
                }
            }).Start();
            bmpviewer.ShowDialog();
        }
    }
}
