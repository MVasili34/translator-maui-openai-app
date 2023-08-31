using Tesseract;
using System.IO;
using System.Drawing;
using System;

namespace MainLibrary
{
	/// <summary>
	/// Интрефейс, абстрагирующий структуру логики 
    /// работы распознавателя текста с изображений
	/// </summary>
	public interface IIMageText
    {
        static Task<String> GetRuTextFromImage(string FilePath)
        {
            return Task.Run(() => "");
        }

		static Task<String> GetEnTextFromImage(string FilePath)
        {
            return Task.Run(() => "");
        }
    }

	/// <summary>
	/// Абстрактный класс распознавателя 
    /// текста с изображений
	/// </summary>
	public abstract class ImageText 
    {
		public virtual Task<String> GetRuTextFromImage(string FilePath)
        {
            return Task.Run(() => "Русский текст");
        }

		public virtual Task<String> GetEnTextFromImage(string FilePath)
        {
            return Task.Run(() => "Английский текст");
        }
    }

    /// <summary>
    /// Класс распознавания текста с изображения
    /// </summary>
    public class TextFromImages : ImageText, IIMageText
    {
		TextFromImages() { }

		/// <summary>
		/// Асинхронный скрывающий метод получения русского текста с изображения
		/// </summary>
		/// <param name="FilePath"> Путь к файлу изображения </param>
		/// <returns> Строка с распознанным текстом в асинхронном поток </returns>
		new public static async Task<string> GetRuTextFromImage(string FilePath)
        {
            try
            {
                using (var engine = new TesseractEngine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"),
                    "rus", EngineMode.Default))
                {
                  
                        using (var page = engine.Process(Pix.LoadFromFile(FilePath)))
                        {
                            return await Task.Run(() => page.GetText());
                        }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

		/// <summary>
		/// Асинхронный скрывающий метод получения английского текста с изображения
		/// </summary>
		/// <param name="FilePath"> Путь к файлу изображения </param>
		/// <returns> Строка с распознанным текстом в асинхронном потоке </returns>
		new public static async Task<string> GetEnTextFromImage(string FilePath)
        {
            try
            {
                using (var engine = new TesseractEngine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"),
                    "eng", EngineMode.Default))
                {
                    using (var page = engine.Process(Pix.LoadFromFile(FilePath)))
                    {
                        return await Task.Run(() => page.GetText());
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}