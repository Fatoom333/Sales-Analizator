namespace ExceptionHandler;

using System.Security;
using GetUserInput;
using System.Text.Json;

/// <summary>
/// Обработчик исключений для операций с файлами
/// </summary>
public class FileExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Обрабатывает исключения, связанные с файловыми операциями, и выводит соответствующее сообщение
    /// </summary>
    /// <param name="ex">Исключение, возникшее при работе с файлом</param>
    /// <returns>false, поскольку в случае ошибки программа не перезапускается автоматически</returns>
    public bool Handle(Exception ex)
    {
        string message;

        switch (ex)
        {
            case FileNotFoundException:
                message = "Файл не найден";
                break;
            case DirectoryNotFoundException:
                message = "Указанная директория не найдена";
                break;
            case PathTooLongException:
                message = "Путь к файлу слишком длинный";
                break;
            case UnauthorizedAccessException:
                message = "Нет прав для доступа к файлу";
                break;
            case IOException:
                message = "Ошибка ввода-вывода";
                break;
            case NotSupportedException:
                message = "Формат пути к файлу не поддерживается";
                break;
            case SecurityException:
                message = "Ограничения безопасности запрещают доступ к файлу";
                break;
            case ArgumentException:
                message = "Путь файла содержит недопустимые символы";
                break;
            case InvalidOperationException:
                message = "Недопустимая операция при работе с файлом";
                break;
            case JsonException:
                message = "Ошибка при чтении JSON файла";
                break;
            case NullReferenceException:
                message = "Данных нет или они повреждены";
                break;
            default:
                message = "Произошла неизвестная ошибка при работе с файлом";
                break;
        }

        Console.WriteLine(message);
        return false;
    }
}