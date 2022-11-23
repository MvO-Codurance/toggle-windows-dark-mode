using DotNetWindowsRegistry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToggleWindowsDarkMode;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => services
        .AddSingleton<IRegistry, WindowsRegistry>()
        .AddSingleton<WindowsThemeService>())
    .Build();
    
var service = host.Services.GetRequiredService<WindowsThemeService>();
service.ToggleLightDarkTheme();