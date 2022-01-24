using System;
using System.Runtime.InteropServices;

namespace PresentationRemote.Core
{

    public class MouseMovement
    {
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

      

    }
}
