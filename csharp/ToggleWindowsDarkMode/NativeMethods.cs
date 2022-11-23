using System.Runtime.InteropServices;

namespace ToggleWindowsDarkMode;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

public static class NativeMethods
{
    public static readonly IntPtr HWND_BROADCAST = (IntPtr)0xffff;
    public static readonly uint WM_SETTINGCHANGE = 0x1a;
    
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageTimeout(
        IntPtr hWnd, 
        uint Msg, 
        UIntPtr wParam, 
        string lParam, 
        SendMessageTimeoutFlags fuFlags, 
        uint uTimeout, 
        out UIntPtr lpdwResult);
    
    [Flags]
    public enum SendMessageTimeoutFlags : uint
    {
        SMTO_NORMAL             = 0x0,
        SMTO_BLOCK              = 0x1,
        SMTO_ABORTIFHUNG        = 0x2,
        SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
        SMTO_ERRORONEXIT        = 0x20
    }
}