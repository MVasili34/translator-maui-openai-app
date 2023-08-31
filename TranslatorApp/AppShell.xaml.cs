namespace TranslatorApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        FlyoutBackgroundColor = new Color(33, 33, 33);
		

#if ANDROID
	FlyoutBackgroundColor = new Color(64, 68, 72);
        itemNotToDisplay.IsVisible = false;
#endif

    }
}
