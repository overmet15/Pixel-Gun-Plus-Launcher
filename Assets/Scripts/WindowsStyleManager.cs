using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowStyleManager : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    private const int GWL_STYLE = -16;
    private const uint WS_BORDER = 0x00800000;
    private const uint WS_CAPTION = 0x00C00000;

    private void Start()
    {
        IntPtr hwnd = GetForegroundWindow();
        uint style = GetWindowLong(hwnd, GWL_STYLE);

        // Remove title bar and border
        style &= ~WS_BORDER;
        style &= ~WS_CAPTION;

        SetWindowLong(hwnd, GWL_STYLE, style);
    }
}
