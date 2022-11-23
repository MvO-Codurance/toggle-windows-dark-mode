# Toggle between Windows 10/11 dark/light mode/theme

$regKeyPath = "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize"
$currentUseLightTheme = Get-ItemPropertyValue -Path $regKeyPath -Name SystemUsesLightTheme
$newUseLightTheme = (1 - $currentUseLightTheme)

Set-ItemProperty -Path $regKeyPath -Name SystemUsesLightTheme -Value $newUseLightTheme -Type Dword -Force
Set-ItemProperty -Path $regKeyPath -Name AppsUseLightTheme -Value $newUseLightTheme -Type Dword -Force

# Just changing the registry doesn't reliably change all windows/apps, possibly because they are only listening 
# for "RegNotifyChangeKeyValue". For example, the Windows taskbar and the system tray font are unreliable. 
# The "proper" way to detect system changes like this is to listen for a "WM_SETTINGCHANGE" message with lParam 
# of "ImmersiveColorSet". Therefore we broadcast this to all top-level windows via SendMessageTimeout().
Add-Type -TypeDefinition @"
    using System;
    using System.Runtime.InteropServices;

    public class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            IntPtr hWnd, 
            uint Msg, 
            UIntPtr wParam, 
            string lParam, 
            uint fuFlags, 
            uint uTimeout, 
            out UIntPtr lpdwResult);
    }
"@

$HWND_BROADCAST = [IntPtr] 0xffff
$WM_SETTINGCHANGE = 0x1a
$SMTO_ABORTIFHUNG = 0x2
$result = [UIntPtr]::Zero

[void] ([Nativemethods]::SendMessageTimeout(
        $HWND_BROADCAST, 
        $WM_SETTINGCHANGE, 
        [UIntPtr]::Zero, 
        'ImmersiveColorSet', 
        $SMTO_ABORTIFHUNG, 
        5000, 
        [ref] $result))