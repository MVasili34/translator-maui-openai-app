namespace TranslatorApp;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		BackgroundColor = new Color(192, 192, 255);
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new TranslationPage());
    }
}

