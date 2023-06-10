using MainLibrary;
using System.Text.RegularExpressions;
using ExceptionClasses;

namespace TranslatorApp;

/// <summary>
/// Класс представления перевода текста
/// </summary>
public partial class TranslationPage : ContentPage
{
    int MainChosenLanguageFlag = 0;
    AIMakesRequest request;
    IEnumerable<Locale> locales;

	/// <summary>
	/// Констркуто класса TranslationPage
	/// </summary>
	public TranslationPage()
	{
		InitializeComponent();
        BackgroundColor = new Color(192, 192, 255);
    }

    /// <summary>
    /// Метод получения списка синтезаторово речи системы
    /// </summary>
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        locales = await TextToSpeech.GetLocalesAsync();
    }

	/// <summary>
	/// Перегруженный констркуто класса TranslationPage,
    /// получающий текст для перевода с другого представления
	/// </summary>
    /// <param name="textToTranslate"> 
    /// Текст для перевода с другого представления
    /// </param>
	public TranslationPage(string textToTranslate)
    {
        InitializeComponent();
        field1.Text = textToTranslate;
        BackgroundColor = new Color(192, 192, 255);
    }

	/// <summary>
	/// Обработчик нажатия кнопки смены режима работы.
	/// </summary>
	/// <param name="sender">Источник события.</param>
	/// <param name="e">Аргументы события.</param>
	private void exchange_Clicked(object sender, EventArgs e)
    {
        if (MainChosenLanguageFlag == 1)
        {
            MainChosenLanguageFlag = 0;
            label2.Text = "English language field";
            label1.Text = "Поле для русского языка";
            field1.Placeholder = "Введите текст...";
        }
        else
        {
            field1.Placeholder = "Enter text to translate...";
            MainChosenLanguageFlag = 1;
            label1.Text = "English language field";
            label2.Text = "Поле для русского языка";
        }
        field2.Text = String.Empty;
    }

	/// <summary>
	/// Обработчик нажатия кнопки выполения перевода.
	/// </summary>
	/// <param name="sender">Источник события.</param>
	/// <param name="e">Аргументы события.</param>
	private async void translateBut_Clicked(object sender, EventArgs e)
    {
        request = new(Preferences.Get("OPEN_APIKEY", "key here"),
             Preferences.Get("dTemp", 0.7).ToString());
        try
        {
            field2.Text = String.Empty;
            if (String.IsNullOrWhiteSpace(field1.Text) == false &&
                Regex.IsMatch(field1.Text, @"^[\p{IsBasicLatin}\p{IsCyrillic}\p{P}\p{Nd}\s]+$"))
            {
                string textToTranslate = field1.Text;
                field2.Text = await Task.Run(() => MainChosenLanguageFlag switch
                {
                    0 => request.RequestTranslateToEn(textToTranslate),
                    1 => request.RequestTranslateToRu(textToTranslate),
                    _ => ""
                });
            }
        }
        catch (Exception ex)
        {
           await DisplayAlert("Ошибка!", ex.Message, "OK");
        }
    }

	/// <summary>
	/// Обработчик нажатия кнопки синтезатора речи поля ввода.
	/// </summary>
	/// <param name="sender">Источник события.</param>
	/// <param name="e">Аргументы события.</param>
	private async void speaker1_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(field1.Text) == false)
            {
#if WINDOWS
                    await TextToSpeech.SpeakAsync(field1.Text, new SpeechOptions
                    {
                        Locale = locales.Single(l => l.Name ==( MainChosenLanguageFlag switch
                        {
                            0 => Preferences.Get("comboValue1", "Microsoft Irina"),
                            1 => Preferences.Get("comboValue2", "Microsoft David"),
                            _ => throw new VoiceException("Ошибка синтезатора")
                        }))
                    });
#endif
#if ANDROID
                    await TextToSpeech.SpeakAsync(field1.Text, new SpeechOptions
                    {
                        Locale = locales.Single(l => l.Name ==( MainChosenLanguageFlag switch
                        {
                            0 => "Русский (Россия)",
                            1 => "Английский (Великобритания)",
                            _ => throw new VoiceException("Ошибка синтезатора")
                        }))
                    });
#endif
			}
		}
        catch (VoiceException ex)
        {
            await DisplayAlert("Ошибка!", ex.Message, "OK");
        }
        catch (Exception ex)
        {
           await DisplayAlert("Ошибка!", ex.Message, "OK");
        }
    }

	/// <summary>
	/// Обработчик нажатия кнопки синтезатора речи поля вывода.
	/// </summary>
	/// <param name="sender">Источник события.</param>
	/// <param name="e">Аргументы события.</param>
	private async void speaker2_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(field2.Text) == false)
            {
#if WINDOWS
                    await TextToSpeech.SpeakAsync(field2.Text, new SpeechOptions
                    {
                        Locale = locales.Single(l => l.Name ==( MainChosenLanguageFlag switch
                        {
                            0 => Preferences.Get("comboValue2", "Microsoft David"),
                            1 => Preferences.Get("comboValue1", "Microsoft Irina"),
                            _ => throw new VoiceException("Ошибка синтезатора")
                        }))
                    });
#endif
#if ANDROID
                    await TextToSpeech.SpeakAsync(field2.Text, new SpeechOptions
                    {
                        Locale = locales.Single(l => l.Name ==( MainChosenLanguageFlag switch
                        {
                            0 => "Английский (Великобритания)",
                            1 => "Русский (Россия)",
                            _ => throw new VoiceException("Ошибка синтезатора")
                        }))
                    });
#endif
			}
		}
        catch (VoiceException ex)
        {
            await DisplayAlert("Ошибка!", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка!", ex.Message, "OK");
        }
    }

	/// <summary>
	/// Обработчик события подсчёта кол-ва символов в поле ввода.
	/// </summary>
	/// <param name="sender">Источник события.</param>
	/// <param name="e">Аргументы события.</param>
	private async void OnEditorTextChanged1(object sender, EventArgs e)
    {
        if (field1.Text.Length > 300)
        {
            await DisplayAlert("Предупреждение!", "Ограничение на 300 символов!", "OK");
            field1.Text = field1.Text.Substring(0, 300);
        }
        label3.Text = field1.Text.Length.ToString() + "/300";
    }
}
