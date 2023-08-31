using MainLibrary;

namespace TranslatorApp;

/// <summary>
/// Класс представления перевода текста с изображений
/// </summary>
public partial class TextFromImage : ContentPage
{
    private string imagePath="";

	public TextFromImage()
	{
		InitializeComponent();
        BackgroundColor = new Color(51, 51, 51);
    }

    /// <summary>
	/// Обработчик нажатия кнопки выбора изображения.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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


    /// <summary>
	/// Обработчик нажатия кнопки распознавания текста на английский язык.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	private async void engButton_Clicked(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(imagePath)) return;
        field1.Text = await TextFromImages.GetEnTextFromImage(imagePath);
    }

    /// <summary>
    /// Обработчик нажатия кнопки распознавания текста на русский язык.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void rusButton_Clicked(Object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty (imagePath)) return;
        field1.Text = await TextFromImages.GetRuTextFromImage(imagePath);
    }

    /// <summary>
    /// Обработчик нажатия кнопки перехода в другую вкладку.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void redirect_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(field1.Text))
        await Navigation.PushAsync(new TranslationPage(field1.Text));
    }
}