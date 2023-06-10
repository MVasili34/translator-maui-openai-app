using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary
{
    /// <summary>
    /// Интрефейс, абстрагирующий структуру логики работы переводчика
    /// </summary>
    interface AITranslate
    {
        string RequestTranslateToEn(string sRequest);
        string RequestTranslateToRu(string sRequest);
    }

    /// <summary>
    /// Класс, позволяющий переводить тексты с применением ИИ 
    /// </summary>
    public class AIMakesRequest : AITranslate
    {
        private readonly string OPENAI_API_KEY = "key here";
        private readonly string requestTemp = "0.7";
        private string sAnswer = "";

		/// <summary>
		/// Пустрой конструктор класса AIMakesRequest
		/// </summary>
		public AIMakesRequest() { }

		/// <summary>
		/// Перегруженный конструктор класса AIMakesRequest
		/// </summary>
        /// <param name="OPEN_AI_KEY"> 
        /// Ключ OpenAI для осуществления запросов к API
        /// </param>
        /// <param name="dTemp">
        /// Параметр настройки уровня качества перевода
        /// </param>
		public AIMakesRequest(string OPEN_AI_KEY, string dTemp)
        {
            this.OPENAI_API_KEY = OPEN_AI_KEY;
            this.requestTemp = dTemp.Replace(",", ".");
        }

        /// <summary>
        /// Публичный метод перевода русского текста на английский
        /// </summary>
        /// <param name="sRequest">
        /// В качестве входного параметра: текст на русском языке
        /// </param>
        /// <returns>
        /// В качестве выходного парамнетр: текст на английском языке
        /// </returns>
        public string RequestTranslateToEn(string sRequest)
        {
            try
            {
                sAnswer = SendRequest($"Translate this text to English: '{sRequest}'");
            }
            catch (WebException ex) when (ex.Response is HttpWebResponse result &&
                    result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                 return("Неверный ключ авторизации OpenAI!\nПеревод невозможен.");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return sAnswer.Trim();
        }

		/// <summary>
		/// Публичный метод перевода английского текста на русский
		/// </summary>
		/// <param name="sRequest">
		/// В качестве входного парамнетра: текст на английском языке
		/// </param>
		/// <returns> 
        /// В качестве выходного параметра: текст на русском языке </returns>
		public string RequestTranslateToRu(string sRequest)
        {
            try
            {
                sAnswer = SendRequest($"Переведи этот текст на Русский язык: '{sRequest}'");
            }
            catch (WebException ex) when (ex.Response is HttpWebResponse result &&
                    result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return("Неверный ключ авторизации OpenAI!\nПеревод невозможен.");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return sAnswer.Trim();
        }
        /// <summary>
        /// Метод отправки запроса OpenAI
        /// </summary>
        /// <param name="sQuestion">
        /// Промпт, осуществляющий логику перевода
        /// </param>
        /// <returns>
        /// Ответ на запрос
        /// </returns>
        private string SendRequest(string sQuestion)
        {
            //протоколы безопасности для HTTPS-соединения
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls12 |
                System.Net.SecurityProtocolType.Tls11 |
                System.Net.SecurityProtocolType.Tls;

            //URL адрес обработки запроса сервером OpenAI
            string apiEndpoint = "https://api.openai.com/v1/completions";
            WebRequest request = WebRequest.Create(apiEndpoint);
            request.Method = "POST";
            request.ContentType = "application/json";
            //задаём ключ авторизации для запроса на OpenAI
            request.Headers.Add("Authorization", "Bearer " + OPENAI_API_KEY);
            int iMaxTokens = 2048;

            string sUserId = "1";
            //модель запроса
            string sModel = "text-davinci-003";

            //строка запроса в формате JSON
            string data = "{";
            data += " \"model\":\"" + sModel + "\",";
            data += " \"prompt\": \"" + PadQuotes(sQuestion) + "\",";
            data += " \"max_tokens\": " + iMaxTokens + ",";
            data += " \"user\": \"" + sUserId + "\", ";
            data += " \"temperature\": " + requestTemp + ", ";
            data += " \"frequency_penalty\": 0.0" + ", ";
            data += " \"presence_penalty\": 0.0" + ", ";
            data += " \"stop\": [\"#\", \";\"]";
            data += "}";
            // Используя поток записи, отправляем данные на сервер
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                // Записываем данные из переменной data
                streamWriter.Write(data);
                // Сбрасываем буфер потока
                streamWriter.Flush();
                streamWriter.Close();
            }

            // Получаем ответ от сервера
            var response = request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            // Считываем все данные из потока чтения в строку sJson
            string sJson = streamReader.ReadToEnd();
            return Deserialization(sJson);
        }

        /// <summary>
        /// Метод десериализации ответа OpenAI
        /// </summary>
        /// <param name="sJson">
        /// Входной параметр: JSON строка ответа сервера OpenAI
        /// </param>
        /// <returns>
        /// Выходной параметр: Ответ на первоначальный запрос
        /// </returns>
        private string Deserialization(string sJson)
        {
            // Десериализуем JSON-ответ в объект JToken
            JToken response = JToken.Parse(sJson);
            string sResponse = response.SelectToken("choices[0].text").ToString();
            return sResponse;
        }

		/// <summary>
		/// Метод экранирования входной строки запроса
		/// </summary>
		/// <param name="s">
		/// Строка, попадающая в методы RequestTranslateToRu и RequestTranslateToEn
		/// </param>
		/// <returns>
        /// Строка, с экранированными символами
        /// </returns>
		private string PadQuotes(string s)
        {
            if (s.IndexOf("\\") != -1)
                s = s.Replace("\\", @"\\");

            if (s.IndexOf("\n\r") != -1)
                s = s.Replace("\n\r", @"\n");

            if (s.IndexOf("\r") != -1)
                s = s.Replace("\r", @"\r");

            if (s.IndexOf("\n") != -1)
                s = s.Replace("\n", @"\n");

            if (s.IndexOf("\t") != -1)
                s = s.Replace("\t", @"\t");

            if (s.IndexOf("\"") != -1)
                return s.Replace("\"", @"""");
            else
                return s;
        }
    }
}
