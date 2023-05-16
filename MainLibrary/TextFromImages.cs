using Tesseract;
using System.IO;
using System.Drawing;
using System;

namespace MainLibrary
{
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
    public class TextFromImages : ImageText, IIMageText
    {
        TextFromImages() 
        { }
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