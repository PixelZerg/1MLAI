using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lib1MLAI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public int Width =>
            (this.right - this.left);
        public int Height =>
            (this.bottom - this.top);
        public Rectangle ToRectangle() =>
            Rectangle.FromLTRB(this.left, this.top, this.right, this.bottom);
    }
}
