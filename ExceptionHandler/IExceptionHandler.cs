namespace ExceptionHandler;

/// <summary>
/// Интерфейс для обработки исключений
/// </summary>
public interface IExceptionHandler
{
    /// <summary>
    /// Обрабатывает исключение и возвращает true, если приложение должно перезапуститься, иначе false
    /// </summary>
    /// <param name="ex">Исключение для обработки</param>
    /// <returns>Флаг для перезапуска приложения</returns>
    bool Handle(Exception ex);
}