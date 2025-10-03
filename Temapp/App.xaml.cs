namespace Temapp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}

// Service locator helper: App.Services will be set from MauiProgram so pages can resolve DI services when created by XAML
public partial class App
{
	public static IServiceProvider? Services { get; set; }
}