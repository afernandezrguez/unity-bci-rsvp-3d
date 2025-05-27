using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

public class BringToFront : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int SW_RESTORE = 9;

    public void BringUnityToFront()
    {
        Process currentProcess = Process.GetCurrentProcess();
        IntPtr hWnd = currentProcess.MainWindowHandle;

        // Restaurar si está minimizada
        ShowWindow(hWnd, SW_RESTORE);
        // Llevar al frente
        SetForegroundWindow(hWnd);
    }
}
