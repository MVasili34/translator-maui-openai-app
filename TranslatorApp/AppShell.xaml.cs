namespace TranslatorApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        FlyoutBackgroundColor = new Color(51, 51, 51);
		
#if ANDROID
	FlyoutBackgroundColor = new Color(255, 255, 255);
	itemNotToDisplay.IsVisible = false;
#endif
		
	}
}
