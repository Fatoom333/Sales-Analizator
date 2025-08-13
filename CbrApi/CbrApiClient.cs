namespace CbrApi
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using ExceptionHandler;

    /// <summary>
    /// Клиент для получения курса валют с сайта Центрального Банка РФ
    /// </summary>
    public class CbrApiClient : ICbrApiClient, IDisposable
    {
        /// <summary>
        /// Экземпляр HttpClient для выполнения HTTP-запросов
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// Флаг, указывающий, был ли объект освобожден
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Базовый URL для получения ежедневных курсов валют
        /// </summary>
        private const string BaseUrl = "http://www.cbr.ru/scripts/XML_daily.asp";

        /// <summary>
        /// Инициализация экземпляра
        /// </summary>
        public CbrApiClient()
        {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// Получает курс валюты по указанному коду и дате
        /// </summary>
        /// <param name="currencyCode">Код валюты (например, "USD", "EUR")</param>
        /// <param name="date">Дата, для которой необходимо получить курс</param>
        /// <returns>Курс валюты в виде десятичного числа</returns>
        /// <exception cref="GetCurrencyRateException">
        /// Выбрасывается, если не найден курс для указанной валюты или произошла ошибка преобразования
        /// </exception>
        public async Task<decimal> GetCurrencyRateAsync(string currencyCode, DateTime date)
        {
            string dateParam = date.ToString("dd/MM/yyyy");
            string url = $"{BaseUrl}?date_req={dateParam}";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var charset = response.Content.Headers.ContentType?.CharSet;
                if (!string.IsNullOrEmpty(charset))
                {
                    try
                    {
                        Encoding.GetEncoding(charset);
                    }
                    catch (ArgumentException)
                    {
                        response.Content.Headers.ContentType.CharSet = "utf-8";
                    }
                }

                var xml = await response.Content.ReadAsStringAsync();
                XDocument doc = XDocument.Parse(xml);

                var rateString = doc.Descendants("Valute")
                    .FirstOrDefault(v => string.Equals(
                        v.Element("CharCode")?.Value,
                        currencyCode,
                        StringComparison.OrdinalIgnoreCase))
                    ?.Element("Value")?.Value;

                if (string.IsNullOrWhiteSpace(rateString))
                    throw new GetCurrencyRateException("Не найден курс для указанной валюты");

                if (!decimal.TryParse(rateString, out decimal rate))
                    throw new GetCurrencyRateException("Ошибка преобразования курса валюты");

                return rate;
            }
            catch (Exception e)
            {
                IExceptionHandler httpHandler = new HttpExceptionHandler();
                httpHandler.Handle(e);
                throw;
            }
        }

        /// <summary>
        /// Освобождает ресурсы, используемые объектом CbrApiClient
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                httpClient.Dispose();
                disposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}
