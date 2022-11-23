using Microsoft.Win32;

namespace ToggleWindowsDarkMode;

public class WindowsThemeService
{
    private const string PersonalizeSubKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string SystemValueName = "SystemUsesLightTheme";
    private const string ApplicationsValueName = "AppsUseLightTheme";

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

    private static Theme ReadCurrentTheme(string themeValueName)
    {
        var subKey = Registry.CurrentUser.OpenSubKey(PersonalizeSubKey);
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

        var usesLightTheme = Convert.ToBoolean(rawValue);
        
        return usesLightTheme ? Theme.Light : Theme.Dark;
    }
}