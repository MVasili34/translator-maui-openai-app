namespace TranslatorApp;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		BackgroundColor = new Color(51, 51, 51);
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new TranslationPage());
    }
}

