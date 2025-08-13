namespace ConsoleMenu;

using Sales;
using GetUserInput;

/// <summary>
/// Меню транзакций для управления операциями продаж
/// </summary>
public class TransactionMenu : IMenu
{
    /// <summary>
    /// Менеджер продаж для управления транзакциями
    /// </summary>
    internal SalesManager salesManager { get; set; }

    /// <summary>
    /// Инициализация экземпляра TransactionMenu
    /// </summary>
    public TransactionMenu()
    {
        salesManager = null;
    }

    /// <summary>
    /// Заголовок меню транзакций
    /// </summary>
    public string Title => "Меню транзакций";

    /// <summary>
    /// Запуск меню транзакций и выполнение операций до выхода
    /// </summary>
    /// <returns>Менеджер продаж с выполненными транзакциями</returns>
    public SalesManager Start()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine($"===== {Title} =====");
            Console.WriteLine("1. Просмотр всех транзакций");
            Console.WriteLine("2. Добавление новой транзакции");
            Console.WriteLine("3. Удаление транзакции");
            Console.WriteLine("4. Редактировать транзакцию");
            Console.WriteLine("0. Назад");

            string? input = Console.ReadLine().Trim();
            exit = ProcessMenuChoice(input);
        }

        return salesManager;
    }

    /// <summary>
    /// Обработка выбора меню транзакций
    /// </summary>
    /// <param name="choice">Выбор пользователя в меню</param>
    /// <returns>true если выбран выход, иначе false</returns>
    public bool ProcessMenuChoice(string choice)
    {
        bool exit = false;

        switch (choice)
        {
            case "0":
                exit = true;
                break;
            case "1":
                Console.Clear();
                Console.WriteLine(salesManager.ToString());
                Console.ReadKey();
                break;
            case "2":
                Console.Clear();
                salesManager.AddSale(new Sale(Sale.CreateSale(false)));
                Console.WriteLine("Транзакция успешно добавлена!");
                Console.ReadKey();
                break;
            case "3":
                Console.Clear();
                salesManager.RemoveSale(GetUserInput.GetTransactionId());
                Console.WriteLine("Транзакция успешно удалена!");
                Console.ReadKey();
                break;
            case "4":
                Console.Clear();
                salesManager.UpdateSale(GetUserInput.GetTransactionId(), new Sale(Sale.CreateSale(true)));
                Console.WriteLine("Транзакция успешно изменена!");
                Console.ReadKey();
                break;
            default:
                Console.WriteLine("Некорректный выбор. Попробуйте ещё раз");
                Console.ReadKey();
                break;
        }

        return exit;
    }
}