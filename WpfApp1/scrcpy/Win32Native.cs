using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.scrcpy
{
    class Win32Native
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetParent")]
        public extern static IntPtr SetParent(IntPtr childPtr, IntPtr parentPtr);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint newLong);


        public const UInt32 WS_POPUP = 0x80000000;

        //assorted constants needed

        public static int GWL_STYLE = -16;

        public static int WS_CHILD = 0x40000000; //child window

        public static int WS_BORDER = 0x00800000; //window with border

        public static int WS_DLGFRAME = 0x00400000; //window with double border but no title

        public static int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar



        public const UInt32 WS_THICKFRAME = 0x40000;





        public const UInt32 WS_SIZEBOX = WS_THICKFRAME;
    }
}
