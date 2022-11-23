using FluentAssertions;

namespace ToggleWindowsDarkMode.Tests;

public class WindowsThemeServiceShould
{
    // NOTE: these unit tests assume the system is set to Light theme
    
    [Fact]
    public void Get_The_Current_System_Theme()
    {
        new WindowsThemeService().SystemTheme.Should().Be(Theme.Light);
    }
    
    // [Fact]
    // public void Set_The_System_Theme()
    // {
    //     var sut = new WindowsThemeService();
    //         
    //     sut.SystemTheme = Theme.Dark;
    //     sut.SystemTheme.Should().Be(Theme.Dark);
    //     
    //     sut.SystemTheme = Theme.Light;
    //     sut.SystemTheme.Should().Be(Theme.Light);
    // }
    
    [Fact]
    public void Get_The_Current_Applications_Theme()
    {
        new WindowsThemeService().ApplicationsTheme.Should().Be(Theme.Light);
    }
    
}