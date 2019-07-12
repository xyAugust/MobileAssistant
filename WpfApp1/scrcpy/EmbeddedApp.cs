using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class EmbeddedApp
{
    /// <summary>
    /// 获取窗体的句柄函数
    /// </summary>
    /// <param name="lpClassName">窗口类名</param>
    /// <param name="lpWindowName">窗口标题名</param>
    /// <returns>返回句柄</returns>
    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    public static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);

    [DllImport("user32.dll")]
    public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter,
                int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint newLong);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool ShowWindow(IntPtr hWnd, short State);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    public const int HWND_TOP = 0x0;
    public const int WM_COMMAND = 0x0112;
    public const int WM_QT_PAINT = 0xC2DC;
    public const int WM_PAINT = 0x000F;
    public const int WM_SIZE = 0x0005;
    public const int SWP_FRAMECHANGED = 0x0020;
    public const int GWL_STYLE = (-16);
    public const int WS_CAPTION = 0xC00000;
    public const int WS_CAPTION_2 = 0XC0000;
     
}