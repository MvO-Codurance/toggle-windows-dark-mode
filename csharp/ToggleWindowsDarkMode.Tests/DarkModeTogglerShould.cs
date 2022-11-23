using FluentAssertions;

namespace ToggleWindowsDarkMode.Tests;

public class DarkModeTogglerShould
{
    // NOTE: the unit tests assume the system is set to Light theme
    
    [Fact]
    public void Read_The_Current_System_Theme()
    {
        DarkModeToggler.GetSystemTheme().Should().Be(Theme.Light);
    }
    
    [Fact]
    public void Read_The_Current_Applications_Theme()
    {
        DarkModeToggler.GetApplicationsTheme().Should().Be(Theme.Light);
    }
    
}