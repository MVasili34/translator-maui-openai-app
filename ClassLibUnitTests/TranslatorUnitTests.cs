using MainLibrary;

namespace ClassLibUnitTests
{
    public class TranslatorUnitTests
    {
        [Fact]
        public void TestGetEnglishTranslation()
        {
            //размещение
            string key = "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";
            string temp = "0.7";
            var request = new AIMakesRequest(key, temp);

            //действие
            string actual = request.RequestTranslateToEn("Яблоко.");

            //утверждене
            Assert.Equal("Apple.", actual);
        }
        [Fact]
        public void TestGetRussianTranslation()
        {
            //размещение
            string key = "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";
            string temp = "0.7";
            var request = new AIMakesRequest(key, temp);

            //действие
            string actual = request.RequestTranslateToRu("Rabbit.");

            //утверждене
            Assert.Equal("Кролик.", actual);
        }

        [Fact]
        public void CatchOpenAIKeyException()
        {
            //размещение
            string key = "Some kind of a key";
            string temp = "0.7";
            var request = new AIMakesRequest(key, temp);


            //утверждене
            Assert.Equal("Неверный ключ авторизации OpenAI!\nПеревод невозможен.", request.RequestTranslateToEn("SomeText"));
        }
    }
}