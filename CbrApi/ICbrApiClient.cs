namespace CbrApi;

public interface ICbrApiClient
{
    /// <summary>
    /// Получает курс указанной валюты на заданную дату
    /// </summary>
    /// <param name="currencyCode">Код валюты</param>
    /// <param name="date">Дата, на которую запрашивается курс</param>
    /// <returns>Курс валюты или null, если данные не получены</returns>
    Task<decimal> GetCurrencyRateAsync(string currencyCode, DateTime date);
}