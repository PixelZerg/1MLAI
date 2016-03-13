using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Lib1MLAI
{
    public class User32
    {
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWNORMAL = 1;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32")]
        private static extern bool EnumChildWindows(IntPtr intptr_0, EnumWindowProc enumWindowProc_0, IntPtr intptr_1);
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int FindWindow(string strClassName, string strWindowName);
        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, int hwndChildAfter, string strClassName, string strWindowName);
        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> list = new List<IntPtr>();
            GCHandle handle = GCHandle.Alloc(list);
            try
            {
                EnumWindowProc proc = new EnumWindowProc(User32.smethod_0);
                EnumChildWindows(parent, proc, GCHandle.ToIntPtr(handle));
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
            return list;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr intptr_0, StringBuilder stringBuilder_0, int int_0);
        public static RECT GetClientRect(IntPtr hWnd)
        {
            RECT rect;
            GetClientRect_1(hWnd, out rect);
            return rect;
        }

        [DllImport("user32.dll", EntryPoint = "GetClientRect", SetLastError = true)]
        private static extern bool GetClientRect_1(IntPtr intptr_0, out RECT rect_0);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        public static RECT GetWindowRect(IntPtr hWnd)
        {
            RECT rect;
            GetWindowRect_1(hWnd, out rect);
            return rect;
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowRect", SetLastError = true)]
        private static extern IntPtr GetWindowRect_1(IntPtr intptr_0, out RECT rect_0);
        [DllImport("User32.dll")]
        public static extern void GetWindowThreadProcessId(IntPtr hwnd, out int processId);
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        public static void SendMessage(IntPtr hwnd, int msg, int wParam, uint lParam)
        {
            SendMessage(hwnd, msg, wParam, (int)lParam);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetWindowPos(IntPtr intptr_0, IntPtr intptr_1, int int_0, int int_1, int int_2, int int_3, SetWindowPosFlags setWindowPosFlags_0);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        private static bool smethod_0(IntPtr intptr_0, IntPtr intptr_1)
        {
            List<IntPtr> target = GCHandle.FromIntPtr(intptr_1).Target as List<IntPtr>;
            if (target == null)
            {
               // throw new InvalidCastException(Class8.smethod_0(0x59e431eb));
            }
            target.Add(intptr_0);
            return true;
        }

        internal static string smethod_1(IntPtr intptr_0)
        {
            StringBuilder builder = new StringBuilder(0x100);
            if (GetClassName(intptr_0, builder, builder.Capacity) != 0)
            {
                return builder.ToString();
            }
            return null;
        }

        [DllImport("user32.dll")]
        internal static extern bool UpdateWindow(IntPtr intptr_0);

        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);
    }
}
