using Microsoft.Extensions.Logging;
using Temapp.Services;

namespace Temapp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Register SupabaseService as singleton so it can be injected into pages
		builder.Services.AddSingleton<SupabaseService>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();

		// Make the built IServiceProvider available to pages via App.Services
		App.Services = app.Services;

		// Initialize Supabase client in background (fire-and-forget)
		try
		{
			var supa = app.Services.GetRequiredService<SupabaseService>();
			_ = Task.Run(async () => await supa.InicializarAsync());
		}
		catch
		{
			// If initialization fails here, it's non-fatal for startup — pages will still be able to call InicializarAsync.
		}

		return app;
	}
}
