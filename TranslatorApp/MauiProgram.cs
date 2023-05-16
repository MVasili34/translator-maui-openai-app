namespace TranslatorApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Segoe-UI-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("Segoe-UI-Bold.ttf", "OpenSansSemibold");
			});

        return builder.Build();
	}
}
