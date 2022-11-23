using DotNetWindowsRegistry;
using Microsoft.Win32;

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
        get => ReadCurrentTheme(SystemValueName);
        set { }
    }

    public Theme ApplicationsTheme
    {
        get => ReadCurrentTheme(ApplicationsValueName);
        set { }
    }

    private Theme ReadCurrentTheme(string themeValueName)
    {
        IRegistryKey? currentUserKey = _registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
        if (currentUserKey == null)
        {
            throw new InvalidOperationException(@$"Registry key not found: HKCU");
        }
        
        var subKey = currentUserKey.OpenSubKey(PersonalizeSubKey);
        if (subKey == null)
        {
            throw new InvalidOperationException(@$"Registry key not found: HKCU:\{PersonalizeSubKey}");
        }

        var rawValue = subKey.GetValue(themeValueName);
        if (rawValue == null)
        {
            throw new InvalidOperationException(
                @$"Registry value not found: HKCU:\{PersonalizeSubKey}\{themeValueName}");
        }

        var usesLightTheme = Convert.ToBoolean(Convert.ToInt32(rawValue));
        
        return usesLightTheme ? Theme.Light : Theme.Dark;
    }
}