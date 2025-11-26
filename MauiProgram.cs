using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Memio.Services;
using Memio.ViewModel;
using Memio.View;

namespace Memio;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
        // Services
        builder.Services.AddSingleton<BoardService>();

        // Pages & VMs
        builder.Services.AddTransient<BoardViewModel>();
        builder.Services.AddTransient<BoardPage>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<CalendarViewModel>();
        builder.Services.AddTransient<CalendarPage>();
        builder.Services.AddTransient<TaskDetailPage>();
        builder.Services.AddTransient<SettingsPage>();

		return builder.Build();
	}
}