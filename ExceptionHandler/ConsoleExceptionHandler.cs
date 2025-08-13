namespace ExceptionHandler;

using System.Text.Json;
using GetUserInput;

/// <summary>
/// Общий обработчик исключений, выводящий сообщение об ошибке в консоль и запрашивающий перезапуск программы
/// </summary>
public class ConsoleExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Обрабатывает исключение, выводит соответствующее сообщение и запрашивает у пользователя решение о перезапуске программы
    /// </summary>
    /// <param name="ex">Исключение, которое необходимо обработать</param>
    /// <returns>true если пользователь хочет перезапустить программу, иначе false</returns>
    public bool Handle(Exception ex)
    {
        string message;

        switch (ex)
        {
            case ArgumentNullException:
                message = "Недопустимо использовать пустое значение";
                break;
            case FormatException:
                message = "Некорректный формат ввода";
                break;
            case OverflowException:
                message = "Введенное значение превышает допустимые пределы";
                break;
            case InvalidOperationException:
                message = "Некорректная операция при выполнении";
                break;
            case ArgumentOutOfRangeException:
                message = "Значение выходит за пределы допустимого диапазона";
                break;
            case ArgumentException:
                message = "Передан некорректный аргумент";
                break;
            case NullReferenceException
                nullReferenceException:
                var defaultMessageEnglish = "Object reference not set to an instance of an object.";
                message = string.Equals(nullReferenceException.Message, defaultMessageEnglish,
                    StringComparison.InvariantCulture)
                    ? "Вначале надо ввести данные"
                    : nullReferenceException.Message;
                break;
            default:
                Type s = ex.GetType();
                if (ex is AggregateException || ex is JsonException)
                {
                    message = string.Empty;
                }
                else
                {
                    message = "Произошла неизвестная ошибка";
                }

                break;
        }

        if (!string.IsNullOrEmpty(message)) Console.WriteLine(message);
        return GetUserInput.GetUserRestartDecision();
    }
}