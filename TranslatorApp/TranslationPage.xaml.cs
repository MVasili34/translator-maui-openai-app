using MainLibrary;
using System.Text.RegularExpressions;
using ExceptionClasses;

namespace TranslatorApp;

public partial class TranslationPage : ContentPage
{
    int MainChosenLanguageFlag = 0;
    AIMakesRequest request;
    IEnumerable<Locale> locales;
    public TranslationPage()
	{
		InitializeComponent();
        BackgroundColor = new Color(192, 192, 255);
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        locales = await TextToSpeech.GetLocalesAsync();
    }
    public TranslationPage(string textToTranslate)
    {
        InitializeComponent();
        field1.Text = textToTranslate;
        BackgroundColor = new Color(192, 192, 255);
    }
    private void exchange_Clicked(object sender, EventArgs e)
    {
        if (MainChosenLanguageFlag == 1)
        {
            MainChosenLanguageFlag = 0;
            label2.Text = "English language field";
            label1.Text = "Поле для русского языка";
            field1.Placeholder = "Введите текст для перевода...";
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

    private async void translateBut_Clicked(object sender, EventArgs e)
    {
        request = new(Preferences.Get("OPEN_APIKEY", "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"),
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
           await DisplayAlert("Ошибка!", ex.Message, "Ок");
        }
    }
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
                            _ => throw new VoiceException("Ошибка озвучивания")
                        }))
                    });
#endif
#if ANDROID
                    await TextToSpeech.SpeakAsync(field1.Text, new SpeechOptions
                    {
                        Locale = locales.Single(l => l.Name ==( MainChosenLanguageFlag switch
                        {
                            0 => "Russian (Russia)",
                            1 => "English (United States)",
                            _ => throw new VoiceException("Ошибка озвучивания")
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
                            _ => throw new VoiceException("Ошибка озвучивания")
                        }))
                    });
#endif
#if ANDROID
                    await TextToSpeech.SpeakAsync(field2.Text, new SpeechOptions
                    {
                        Locale = locales.Single(l => l.Name ==( MainChosenLanguageFlag switch
                        {
                            0 => "English (United States)",
                            1 => "Russian (Russia)",
                            _ => throw new VoiceException("Ошибка озвучивания")
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