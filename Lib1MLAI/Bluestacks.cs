using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib1MLAI
{
    public class Bluestacks
    {
        #region moo
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("user32.dll")]
        static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern short VkKeyScan(char char_0);
        #endregion


        public static AutoItX3Lib.AutoItX3Class au = new AutoItX3Lib.AutoItX3Class();
        private static string _blhandle = "";
        public static string blhandle {       
            get { return _blhandle; }
            set { _blhandle = value; iblhandle = new IntPtr(Convert.ToInt64(blhandle.Substring(2, blhandle.Length - 2), 16)); }
        }
        public static IntPtr iblhandle = IntPtr.Zero;
        private static int failno = 0;
        public static Bitmap[] adxs = new Bitmap[] { Properties.Resources.adx1 , Properties.Resources.adx2,Properties.Resources.adx3,Properties.Resources.adx5 };
        public static bool running = true;

        private static int adfindfails = 0;
        private static double adtolerance = 0;
        private static Stopwatch lastnogrey = new Stopwatch();

        private static Color igred = Color.FromArgb(245, 8, 73);
        public static bool ingame = false;

        public static bool udied = false;

        public static List<bool> actionqueue = new List<bool>();

        //   public static BmpViewer directf = null;

        /// <param name="b">true = press if not already pressing & false = release if not already released.</param>
        public static void Input(bool b)
        {
            actionqueue.Add(b);
        }

        public static void Start()
        {
            StartBS();
            blhandle = au.WinGetHandle("Bluestacks");
            Console.WriteLine(blhandle);
            int fails = 0;
            while (true)
            {
                if (iblhandle == IntPtr.Zero)
                {
                    StartBS();
                    Thread.Sleep(1000);
                    blhandle = au.WinGetHandle("Bluestacks");
                }
                else
                {
                    if(fails==0)
                    Console.WriteLine("Bluestacks started sucessfully with hwnd: "+blhandle);

                    Thread.Sleep(1000);
                    if (searchBitmap(Properties.Resources.brownlogo, PrintBLWindow(), 0.0).Width != 0)
                    {
                        Console.WriteLine("App launched sucessfully!");
                        au.Send("{SPACE}");
                        //while (searchBitmap(Properties.Resources.cornerdialog, PrintBLWindow(), 0.0).Width == 0)
                        //{
                        //    Console.Write(".");
                        //    System.Threading.Thread.Sleep(500);
                        //}
                        //Console.WriteLine("Instructions dialog!");

                        //while (searchBitmap(Properties.Resources.cornerdialog, PrintBLWindow(), 0.0).Width != 0)
                        //{
                        //    au.Send("{SPACE}");
                        //    System.Threading.Thread.Sleep(500);
                        //}
                        //Console.WriteLine("Instructions dialog removed");
                        Thread.Sleep(2000);
                        au.Send("{SPACE}");
                        ingame = true;
                     //   if (directf == null)
                     //   {
                          //  directf = new BmpViewer(PrintBLWindow());
                       //     new Thread(()=>
                          //  directf.ShowDialog()).Start();
                      //  }
                        break;
                    }
                    else if (fails > 20)
                    {
                        Console.WriteLine("App failed to launch within 20 seconds");
                        //iblhandle = IntPtr.Zero;
                        blhandle = "0x00000000";
                        fails = 0;
                    }
                    else
                    {
                        fails++;
                    }
                }
            }

           // new Thread(() => { while (running) { MainLoop(); } }).Start();
        }
        public static void StartBS()
        {
            foreach (var proc in Process.GetProcesses())
            {
                if (proc.MainWindowTitle == "Bluestacks App Player")
                {
                    proc.Kill();
                    ingame = false;
                    break;
                }
            }
            Process.Start(@"C:\Program Files (x86)\BlueStacks\HD-RunApp.exe", "-p com.smgstudio.onemoreline -a com.prime31.UnityPlayerActivity");
        }
        public static Bitmap MainLoop()
        {
            Bitmap b = PrintBLWindow();
       //     directf.LoadImage(b);
            int fails = 0;
            if (running && ingame)
            {
            //    if (PixelSearch(b, igred.ToArgb(), 0).X == -1)
                {
                 //   ingame = false;
                  //  Console.WriteLine("nig");
                  //  return b;
                }
                //ingame = true;

                if (au.WinActive("[HANDLE:" + blhandle + "]") != 0)
                {
                    int no = 0;
                    if (GreyBackCheck(b))
                    {
                        //if (searchBitmap(Properties.Resources.diewallside, b, 0.1).Width!=0)
                        //{
                        //    Console.WriteLine("Die wall side!!");
                        //    while (!RestLoop(b)) { AdLoop(b); }
                        //}

                        if (lastnogrey.ElapsedMilliseconds > 2000 && lastnogrey.ElapsedMilliseconds < 2100)
                        {
                            adfindfails = 0;
                            Console.WriteLine("Checking ads via schedule");
                            AdLoop(b);
                            lastnogrey.Stop();
                            lastnogrey.Reset();
                            lastnogrey.Start();
                        }
                        if (RestLoop(b))
                        {
                            //Console.WriteLine("u died");
                        }
                        //  au.Send(" ");

                        try
                        {
                            bool down = actionqueue.First();
                            if (down)
                            {
                                au.Send("{SPACE DOWN}");
                            }
                            else
                            {
                                au.Send("{SPACE UP}");
                            }
                            actionqueue.RemoveAt(0);
                        }
                        catch { }
                    }
                    else
                    {
                        udied = true;
                        Console.WriteLine("U died");

                        Console.WriteLine(lastnogrey.ElapsedMilliseconds);
                        if (lastnogrey.ElapsedMilliseconds > 1000)
                        {
                            adtolerance = 0;
                            Console.WriteLine("Reset adsearch tolerance to 0");
                        }
                        lastnogrey.Stop();
                        lastnogrey.Reset();
                        lastnogrey.Start();
                        IconLoop(b);
                        if (!AdLoop(b))
                        {
                            fails++;
                            if (fails > 100)
                            {
                                Console.WriteLine("100 fails. Restarting");
                                Start();
                            }
                        }
                    }
                    no++;
                }
                else
                {
                    Console.WriteLine("Bluestacks is not active. Halting bot for 5 seconds");
                    System.Threading.Thread.Sleep(5000);
                    Console.WriteLine("Resuming bot");
                    au.WinActivate("[HANDLE:" + blhandle + "]");
                }
            }
            else if (!ingame)
            {
                //while (PixelSearch(b, igred.ToArgb(), 0).X == -1)
                //{
                //    Console.WriteLine("waiting for ig...");
                //}
                ingame = true;
            }
            return b;
        }
        public static bool GreyBackCheck(Bitmap screen)
        {
            return searchBitmap(Properties.Resources.greyback, screen, 0.0).Width != 0;
        }
        public static bool RestLoop(Bitmap screen)
        {
            Rectangle r = searchBitmap(Properties.Resources.rest, screen, 0.0);
            if (r.Width != 0)
            {
                Console.WriteLine("Restarting!");

                RECT rect;
                GetWindowRect(new HandleRef(null, iblhandle), out rect);
                au.WinActivate("[HANDLE:" + blhandle + "]");
                au.MouseClick("LEFT", rect.Left + r.X + (r.Width / 2), rect.Top + r.Y + (r.Height / 2));
                au.WinSetState("[HANDLE:" + blhandle + "]", "", au.SW_SHOWNOACTIVATE);
                actionqueue.Clear();
                Console.WriteLine("Cleared actionqueue");
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool AdLoop(Bitmap screen)
        {
               System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
            Console.Write("Checking for ads...");
              s.Start();
            bool found = false;
            foreach (var adx in adxs)
            {
                Rectangle r = searchBitmap(adx, screen, adtolerance);
                if (r.Width != 0)
                {
                    adtolerance = 0.0;
                    Console.Write("Found Ad: " + adx.GetHashCode() + " at "+r+"! ");
                    RECT rect;
                  //  IntPtr hwnd = new IntPtr(Convert.ToInt64(blhandle.Substring(2, blhandle.Length - 2), 16));
                    GetWindowRect(new HandleRef(null, iblhandle), out rect);
                    //au.ControlClick("[HANDLE:" + blhandle + "]", "", "", "LEFT", 1, rect.Left + r.X + (r.Width / 2), rect.Top + r.Y + (r.Height / 2));
                    // ClickOnPointTool.ClickOnPoint(hwnd, new Point(rect.Left + r.X + (r.Width / 2), rect.Top + r.Y + (r.Height / 2)));
                    au.WinActivate("[HANDLE:" + blhandle + "]");
                    au.MouseClick("LEFT",rect.Left + r.X+(r.Width/2), rect.Top + r.Y+(r.Height/2));
                    au.WinSetState("[HANDLE:" + blhandle + "]", "", au.SW_SHOWNOACTIVATE);
                    found = true;
                    break;
                }
            }
            s.Stop();
            Console.WriteLine("done in " + s.ElapsedMilliseconds + "ms");
            if (!found)
            {
                adfindfails++;
                if (adfindfails > 5)
                {
                    adtolerance += 0.01;
                    Console.WriteLine("increased adsearch tolerance to: "+adtolerance);
                }
            }
            return found;
        }

        public static void IconLoop(Bitmap screen)
        {
            Rectangle r = searchBitmap(Properties.Resources.icon, screen, 0.0);
            if (r.Width != 0)
            {
                Console.WriteLine("Clicking on Icon!");
                RECT rect;
                GetWindowRect(new HandleRef(null, iblhandle), out rect);
                au.WinActivate("[HANDLE:" + blhandle + "]");
                au.MouseClick("LEFT", rect.Left + r.X + (r.Width / 2), rect.Top + r.Y + (r.Height / 2));
                au.WinSetState("[HANDLE:" + blhandle + "]", "", au.SW_SHOWNOACTIVATE);
            }
        }

        public static Bitmap CaptureBLWindow(bool hide=false)
        {
            if (hide) { au.WinActivate("[HANDLE:" + blhandle + "]");
                au.WinSetState("[HANDLE:" + blhandle + "]", "", au.SW_SHOW); }
           // System.Threading.Thread.Sleep(1000);
            RECT rect;
            IntPtr h = iblhandle;//new IntPtr(Convert.ToInt64(handle.Substring(2, handle.Length - 2),16));
            Console.WriteLine("Getting snapshot of handle:" + h);
            if (!GetWindowRect(new HandleRef(null, h), out rect))
            {
                MessageBox.Show("WINDOW GETTING ERROR!");
                return null;
            }
            else
            {
                Console.WriteLine("Left:{0};Top:{1};Right:{2};Bottom:{3}", rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
            Bitmap bmp = new Bitmap(rect.Right-rect.Left, rect.Bottom-rect.Top, PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(bmp);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            Console.WriteLine("Bounds: " + bounds);
            // gfx.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            gfx.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            
            if (hide) au.WinSetState("[HANDLE:" + blhandle + "]","",au.SW_HIDE);
            return bmp;
        }
        public static Bitmap PrintBLWindow()
        {
            IntPtr hwnd = iblhandle;//new IntPtr(Convert.ToInt64(handle.Substring(2, handle.Length - 2), 16));
         //   Console.WriteLine("Getting snapshot of handle:" + hwnd);

            RECT rc;
            GetWindowRect(new HandleRef(null,hwnd), out rc);

            Bitmap bmp = new Bitmap(rc.Right-rc.Left, rc.Bottom-rc.Top, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();
            bool succeeded = PrintWindow(hwnd, hdcBitmap, 0);
            gfxBmp.ReleaseHdc(hdcBitmap);
            if (!succeeded)
            {
                gfxBmp.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(Point.Empty, bmp.Size));
            }
            IntPtr hRgn = CreateRectRgn(0, 0, 0, 0);
            GetWindowRgn(hwnd, hRgn);
            Region region = Region.FromHrgn(hRgn);//err here once
            if (!region.IsEmpty(gfxBmp))
            {
                gfxBmp.ExcludeClip(region);
                gfxBmp.Clear(Color.Transparent);
            }
            gfxBmp.Dispose();
            return bmp;
        }

        public static Rectangle searchBitmap(Bitmap smallBmp, Bitmap bigBmp, double tolerance)
        {
            try
            {
                BitmapData smallData =
                  smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
                           System.Drawing.Imaging.ImageLockMode.ReadOnly,
                           System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                BitmapData bigData =
                  bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height),
                           System.Drawing.Imaging.ImageLockMode.ReadOnly,
                           System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                int smallStride = smallData.Stride;
                int bigStride = bigData.Stride;

                int bigWidth = bigBmp.Width;
                int bigHeight = bigBmp.Height - smallBmp.Height + 1;
                int smallWidth = smallBmp.Width * 3;
                int smallHeight = smallBmp.Height;

                Rectangle location = Rectangle.Empty;
                int margin = Convert.ToInt32(255.0 * tolerance);

                unsafe
                {
                    byte* pSmall = (byte*)(void*)smallData.Scan0;
                    byte* pBig = (byte*)(void*)bigData.Scan0;

                    int smallOffset = smallStride - smallBmp.Width * 3;
                    int bigOffset = bigStride - bigBmp.Width * 3;

                    bool matchFound = true;

                    for (int y = 0; y < bigHeight; y++)
                    {
                        for (int x = 0; x < bigWidth; x++)
                        {
                            byte* pBigBackup = pBig;
                            byte* pSmallBackup = pSmall;

                            //Look for the small picture.
                            for (int i = 0; i < smallHeight; i++)
                            {
                                int j = 0;
                                matchFound = true;
                                for (j = 0; j < smallWidth; j++)
                                {
                                    //With tolerance: pSmall value should be between margins.
                                    int inf = pBig[0] - margin;
                                    int sup = pBig[0] + margin;
                                    if (sup < pSmall[0] || inf > pSmall[0])
                                    {
                                        matchFound = false;
                                        break;
                                    }

                                    pBig++;
                                    pSmall++;
                                }

                                if (!matchFound) break;

                                //We restore the pointers.
                                pSmall = pSmallBackup;
                                pBig = pBigBackup;

                                //Next rows of the small and big pictures.
                                pSmall += smallStride * (1 + i);
                                pBig += bigStride * (1 + i);
                            }

                            //If match found, we return.
                            if (matchFound)
                            {
                                location.X = x;
                                location.Y = y;
                                location.Width = smallBmp.Width;
                                location.Height = smallBmp.Height;
                                break;
                            }
                            //If no match found, we restore the pointers and continue.
                            else
                            {
                                pBig = pBigBackup;
                                pSmall = pSmallBackup;
                                pBig += 3;
                            }
                        }

                        if (matchFound) break;

                        pBig += bigOffset;
                    }
                }

                try
                {
                    bigBmp.UnlockBits(bigData);
                }
                catch { }
                try
                {
                    smallBmp.UnlockBits(smallData);
                }
                catch { }
                return location;
            }
            catch { return Rectangle.Empty; }
            }

        #region ps
        public static Point PixelSearch(Rectangle rect, int PixelColor, int Shade_Variation)
        {
            Color Pixel_Color = Color.FromArgb(PixelColor);

            Point Pixel_Coords = new Point(-1, -1);
            Bitmap RegionIn_Bitmap = CaptureScreenRegion(rect);
            BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int[] Formatted_Color = new int[3] { Pixel_Color.B, Pixel_Color.G, Pixel_Color.R }; //bgr

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);

                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //blue
                        {
                            if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //green
                            {
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //red
                                {
                                    Pixel_Coords = new Point(x + rect.X, y + rect.Y);
                                    goto end;
                                }
                            }
                        }
                    }
                }
            }

            end:
            return Pixel_Coords;
        }
        public static Point PixelSearch(Bitmap b, int PixelColor, int Shade_Variation)
        {
            Color Pixel_Color = Color.FromArgb(PixelColor);

            Point Pixel_Coords = new Point(-1, -1);
            Bitmap RegionIn_Bitmap = b;
            BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int[] Formatted_Color = new int[3] { Pixel_Color.B, Pixel_Color.G, Pixel_Color.R }; //bgr

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);

                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //blue
                        {
                            if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //green
                            {
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //red
                                {
                                    Pixel_Coords = new Point(x// + rect.X
                                        , y
                                        //+ rect.Y
                                        );
                                    goto end;
                                }
                            }
                        }
                    }
                }
            }

            end:
            return Pixel_Coords;
        }

        private static Bitmap CaptureScreenRegion(Rectangle rect)
        {
            Bitmap BMP = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);
            Graphics GFX = System.Drawing.Graphics.FromImage(BMP);
            GFX.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            return BMP;
        }
        #endregion

        #region ext cb 
        internal static void User32PostMessage(IntPtr intptr_0, int int_0, int int_1, int int_2)
        {
            User32.PostMessage(intptr_0, int_0, int_1, int_2);
        }

        public static void Click(IntPtr intptr_0, Point point_0)
        {
            User32PostMessage(intptr_0, 0x201, 1, (point_0.Y << 0x10) | (point_0.X & 0xffff));
            Thread.Sleep(15);
        }

        public static Keys ConvertCharToVirtualKey(char ch)
        {
            short num = VkKeyScan(ch);
            Keys keys = ((Keys)num) & (Keys.OemClear | Keys.LButton);
            int num2 = num >> 8;
            if ((num2 & 1) != 0)
            {
                keys |= Keys.Shift;
            }
            if ((num2 & 2) != 0)
            {
                keys |= Keys.Control;
            }
            if ((num2 & 4) != 0)
            {
                keys |= Keys.Alt;
            }
            return keys;
        }

        public static void SendKey(IntPtr intptr_0, Keys keys_0)
        {
            User32PostMessage(intptr_0, 0x100, (int)keys_0, 0);
            User32PostMessage(intptr_0, 0x101, (int)keys_0, 0);
        }
        #endregion

        #region click on point
        public class ClickOnPointTool
        {

            [DllImport("user32.dll")]
            static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

            [DllImport("user32.dll")]
            internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

#pragma warning disable 649
            internal struct INPUT
            {
                public UInt32 Type;
                public MOUSEKEYBDHARDWAREINPUT Data;
            }

            [StructLayout(LayoutKind.Explicit)]
            internal struct MOUSEKEYBDHARDWAREINPUT
            {
                [FieldOffset(0)]
                public MOUSEINPUT Mouse;
            }

            internal struct MOUSEINPUT
            {
                public Int32 X;
                public Int32 Y;
                public UInt32 MouseData;
                public UInt32 Flags;
                public UInt32 Time;
                public IntPtr ExtraInfo;
            }

#pragma warning restore 649


            public static void ClickOnPoint(IntPtr wndHandle, Point clientPoint)
            {
                var oldPos = Cursor.Position;

                /// get screen coordinates
                ClientToScreen(wndHandle, ref clientPoint);

                /// set cursor on coords, and press mouse
                Cursor.Position = new Point(clientPoint.X, clientPoint.Y);

                var inputMouseDown = new INPUT();
                inputMouseDown.Type = 0; /// input type mouse
                inputMouseDown.Data.Mouse.Flags = 0x0002; /// left button down

                var inputMouseUp = new INPUT();
                inputMouseUp.Type = 0; /// input type mouse
                inputMouseUp.Data.Mouse.Flags = 0x0004; /// left button up

                var inputs = new INPUT[] { inputMouseDown, inputMouseUp };
                SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));

                /// return mouse 
                Cursor.Position = oldPos;
            }

        }
        #endregion
    }
}