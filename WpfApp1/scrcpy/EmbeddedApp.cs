using System;
using System.Runtime.InteropServices;

public class EmbeddedApp
{
    /// <summary>
    /// ��ȡ����ľ������
    /// </summary>
    /// <param name="lpClassName">��������</param>
    /// <param name="lpWindowName">���ڱ�����</param>
    /// <returns>���ؾ��</returns>
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


    public const int HWND_TOP = 0x0;
    public const int WM_COMMAND = 0x0112;
    public const int WM_QT_PAINT = 0xC2DC;
    public const int WM_PAINT = 0x000F;
    public const int WM_SIZE = 0x0005;
    public const int SWP_FRAMECHANGED = 0x0020;
}