using Microsoft.Maui.Media;
using ExceptionClasses;
namespace TranslatorApp;


/// <summary>
/// Класс представления настроек приложения
/// </summary>
public partial class SettingPage : ContentPage
{
    IEnumerable<Locale> locales;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    public SettingPage()
	{
		InitializeComponent();
        BackgroundColor = new Color(192, 192, 255);

#if ANDROID
combobox2.IsEnabled = false;
combobox1.IsEnabled = false;
#endif
    }

    /// <summary>
    /// Асинхоронный метод для выгрузки настроек
    /// </summary>
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        openAIField.Text = Preferences.Get("OPEN_APIKEY", "key here");
        slider.Value = Preferences.Get("dTemp", 0.7);
#if WINDOWS
        locales = await TextToSpeech.GetLocalesAsync();
        foreach (Locale locale in locales) 
        {
            if (locale.Language == "ru-RU")
            combobox1.Items.Add(locale.Name);
            if (locale.Language == "en-US")
            combobox2.Items.Add(locale.Name);
        }
        if (combobox1.Items.Count == 0)
        {
            combobox1.Items.Add("NONE");
        }
        else
        {
             combobox1.SelectedItem =  Preferences.Get("comboValue1", "Microsoft Irina");
        }
        if (combobox2.Items.Count == 0)
        {
            combobox2.Items.Add("NONE");
        }
        else
        {
             combobox2.SelectedItem =  Preferences.Get("comboValue2", "Microsoft David");
        }
#endif
    }

	/// <summary>
	/// Обработчик нажатия кнопки сохранения.
	/// </summary>
	/// <param name="sender">Источник события.</param>
	/// <param name="e">Аргументы события.</param>
	private void save_Button(object sender, EventArgs e) 
    {
        try
        {
            if (String.IsNullOrWhiteSpace(openAIField.Text))
            {
                throw new EmptyFieldException("Пустое поле ввода");
            }
            else
            {
                Preferences.Set("OPEN_APIKEY",  openAIField.Text);
                Preferences.Set("dTemp", Math.Round(slider.Value, 1));
#if WINDOWS
                if (combobox1.SelectedItem.ToString() != "NONE")
                {
                    Preferences.Set("comboValue1", combobox1.SelectedItem.ToString());
                }
                if (combobox2.SelectedItem.ToString() != "NONE")
                {
                    Preferences.Set("comboValue2", combobox2.SelectedItem.ToString());
                }
#endif
                DisplayAlert("Успех!", "Настройки сохранены!", "OK");
            }
        }
        
        catch (EmptyFieldException)
        {
            DisplayAlert("Ошибка!", "Пустое поле ввода", "OK");
        } 
        catch (Exception ex)
        {
            DisplayAlert("Ошибка!", ex.Message, "OK");
        }
    }
    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
    }
}
