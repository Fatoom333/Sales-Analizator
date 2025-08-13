namespace ExceptionHandler;

/// <summary>
/// Исключение, возникающее при ошибке получения курса валюты
/// </summary>
public class GetCurrencyRateException : Exception
{
    /// <summary>
    /// Инициализирует новое исключение GetCurrencyRateException с заданным сообщением
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    public GetCurrencyRateException(string message) : base(message)
    {
    }
}