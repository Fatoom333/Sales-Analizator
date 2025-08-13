namespace ExceptionHandler;

using GetUserInput;

/// <summary>
/// Обработчик исключений для HTTP-запросов
/// </summary>
public class HttpExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Обрабатывает исключения, связанные с HTTP-запросами, и выводит соответствующее сообщение
    /// </summary>
    /// <param name="ex">Исключение, возникшее при выполнении HTTP-запроса</param>
    /// <returns>true, если выполнение может быть продолжено</returns>
    public bool Handle(Exception ex)
    {
        string message;

        switch (ex)
        {
            case HttpRequestException:
                message = "При отправке HTTP-запроса произошла ошибка";
                break;

            case TaskCanceledException taskCanceledException
                when taskCanceledException.CancellationToken.IsCancellationRequested:
                message = "HTTP-запрос был отменён";
                break;

            case TaskCanceledException:
                message = "Время ожидания HTTP-запроса истекло";
                break;

            case OperationCanceledException:
                message = "Операция, связанная с HTTP-запросом, была отменена";
                break;

            case InvalidOperationException:
                message = "HTTP-запрос некорректен";
                break;

            case ArgumentException:
                message = "Для HTTP-запроса передан недопустимый аргумент";
                break;

            case NotSupportedException:
                message = "HTTP-запрос содержит неподдерживаемую операцию";
                break;

            case IOException:
                message = "При обработке HTTP-запроса произошла ошибка ввода-вывода";
                break;

            case GetCurrencyRateException:
                message = ex.Message;
                break;

            default:
                message = "При выполнении HTTP-запроса произошла непредвиденная ошибка";
                break;
        }

        Console.WriteLine(message);
        return true;
    }
}