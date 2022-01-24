using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace PresentationRemote.Core
{

    public class MouseMovement
    {
        private static int posX = 0, posY = 0;
        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        // Get Mouse Postion
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);
        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        // Mouse Movement
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);
        public static void SetMousePostion(int x, int y)
        {
            posX = x;
            posY = y;
            SetCursorPos(x, y);
        }


        // Mouse Events
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //
        //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mouse_event?redirectedfrom=MSDN
        //
        //This simulates a left mouse click
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a right mouse click
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;

        public static void LeftMouseClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN,posX, posY, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, posX, posY, 0, 0);
        }

        public static void RightMouseClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN,posX, posY, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, posX, posY, 0, 0);
        }


    }
}
