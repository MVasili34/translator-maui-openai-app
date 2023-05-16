using MainLibrary;

namespace TranslatorApp;


public partial class TextFromImage : ContentPage
{
    private string imagePath="";
	public TextFromImage()
	{
		InitializeComponent();
        BackgroundColor = new Color(192, 192, 255);
    }

    private async void imageButton_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        { 
          FileTypes=FilePickerFileType.Images,
          PickerTitle="Выберите изображение"
        });
        if (result is null) 
        {
            return;
        }
        var stream = await result.OpenReadAsync();
        pictureBox.Source = ImageSource.FromStream(()=> stream);
        imagePath = result.FullPath;
    }

    private async void engButton_Clicked(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(imagePath)) return;
        field1.Text = await TextFromImages.GetEnTextFromImage(imagePath);
    }
    private async void rusButton_Clicked(Object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty (imagePath)) return;
        field1.Text = await TextFromImages.GetRuTextFromImage(imagePath);
    }

    private async void redirect_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(field1.Text))
        await Navigation.PushAsync(new TranslationPage(field1.Text));
    }
}