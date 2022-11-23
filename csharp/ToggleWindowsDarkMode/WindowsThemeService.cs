using System.Runtime.InteropServices;
using DotNetWindowsRegistry;
using Microsoft.Win32;
using PInvoke;

namespace ToggleWindowsDarkMode;

public class WindowsThemeService
{
    private const string PersonalizeSubKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string SystemValueName = "SystemUsesLightTheme";
    private const string ApplicationsValueName = "AppsUseLightTheme";
    
    private readonly IRegistry _registry;

    public WindowsThemeService(IRegistry registry)
    {
        _registry = registry;
    }
    
    public Theme SystemTheme
    {
        get => GetCurrentTheme(SystemValueName);
        set => SetCurrentTheme(SystemValueName, value);
    }

    public Theme ApplicationsTheme
    {
        get => GetCurrentTheme(ApplicationsValueName);
        set => SetCurrentTheme(ApplicationsValueName, value);
    }

    public IntPtr BroadcastSettingSetChanged(string settingSetName)
    {
        var result = IntPtr.Zero;
        var lParam = Marshal.StringToHGlobalUni(settingSetName);

        User32.SendMessageTimeout(
            User32.HWND_BROADCAST,
            User32.WindowMessage.WM_SETTINGCHANGE,
            IntPtr.Zero,
            lParam,
            User32.SendMessageTimeoutFlags.SMTO_ABORTIFHUNG,
            5000,
            out result);
        
        Marshal.FreeHGlobal(lParam);

        return result;
    }
    
    public void ToggleLightDarkTheme()
    {
        var newTheme = SystemTheme == Theme.Light ? Theme.Dark : Theme.Light;
        SystemTheme = newTheme;
        ApplicationsTheme = newTheme;

        BroadcastSettingSetChanged("ImmersiveColorSet");
    }

    private Theme GetCurrentTheme(string themeValueName)
    {
        var subKey = OpenPersonalizeSubKey(false);
        var rawValue = subKey.GetValue(themeValueName);
        if (rawValue == null)
        {
            throw new InvalidOperationException(@$"Registry value not found: HKCU:\{PersonalizeSubKey}\{themeValueName}");
        }

        return Enum.Parse<Theme>(rawValue.ToString()!);
    }
    
    private void SetCurrentTheme(string themeValueName, Theme theme)
    {
        var subKey = OpenPersonalizeSubKey(true);
        subKey.SetValue(themeValueName, Convert.ToInt32(theme));
    }

    private IRegistryKey OpenPersonalizeSubKey(bool writable)
    {
        var currentUserKey = _registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
        if (currentUserKey == null)
        {
            throw new InvalidOperationException(@$"Registry key not found: HKCU");
        }

        var subKey = currentUserKey.OpenSubKey(PersonalizeSubKey, writable);
        if (subKey == null)
        {
            throw new InvalidOperationException(@$"Registry key not found: HKCU:\{PersonalizeSubKey}");
        }

        return subKey;
    }
}