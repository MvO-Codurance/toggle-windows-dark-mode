using DotNetWindowsRegistry;
using FluentAssertions;
using Microsoft.Win32;

namespace ToggleWindowsDarkMode.Tests;

public class WindowsThemeServiceShould
{
    [Fact]
    public void Get_The_Current_System_Theme()
    {
        GetService().SystemTheme.Should().Be(Theme.Light);
    }
    
    [Fact]
    public void Set_The_System_Theme()
    {
        var sut = GetService();
            
        sut.SystemTheme = Theme.Dark;
        sut.SystemTheme.Should().Be(Theme.Dark);
        
        sut.SystemTheme = Theme.Light;
        sut.SystemTheme.Should().Be(Theme.Light);
    }
    
    [Fact]
    public void Get_The_Current_Applications_Theme()
    {
        GetService().ApplicationsTheme.Should().Be(Theme.Light);
    }
    
    [Fact]
    public void Set_The_Applications_Theme()
    {
        var sut = GetService();
            
        sut.ApplicationsTheme = Theme.Dark;
        sut.ApplicationsTheme.Should().Be(Theme.Dark);
        
        sut.ApplicationsTheme = Theme.Light;
        sut.ApplicationsTheme.Should().Be(Theme.Light);
    }

    private static WindowsThemeService GetService()
    {
        var registry = new InMemoryRegistry();
        
        registry.AddStructure(RegistryView.Default, string.Join(Environment.NewLine,
            @"HKEY_CURRENT_USER",
            @"  SOFTWARE",
            @"    Microsoft",
            @"      Windows",
            @"        CurrentVersion",
            @"          Themes",
            @"            Personalize",
            @"              SystemUsesLightTheme = 1")
        );
        
        registry.AddStructure(RegistryView.Default, string.Join(Environment.NewLine,
            @"HKEY_CURRENT_USER",
            @"  SOFTWARE",
            @"    Microsoft",
            @"      Windows",
            @"        CurrentVersion",
            @"          Themes",
            @"            Personalize",
            @"              AppsUseLightTheme = 1")
        );
        
        return new WindowsThemeService(registry);
    }
}