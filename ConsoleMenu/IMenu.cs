using Sales;

namespace ConsoleMenu;

/// <summary>
/// Интерфейс для работы с меню приложения
/// </summary>
public interface IMenu
{
    /// <summary>
    /// Заголовок меню
    /// </summary>
    string Title { get; }
    
    /// <summary>
    /// Запускает взаимодействие с пользователем через консольное меню
    /// </summary>
    SalesManager Start();
    
    /// <summary>
    /// Обрабатывает выбранный пункт меню
    /// </summary>
    /// <param name="choice">Строка, представляющая выбранный пункт меню</param>
    bool ProcessMenuChoice(string choice);
}
