namespace ConsoleMenu;

using Sales;

/// <summary>
/// Продвинутое меню для консольного приложения
/// </summary>
public class AdvancedMenu : IMenu
{
    /// <summary>
    /// Менеджер продаж
    /// </summary>
    public SalesManager salesManager { get; set; }

    /// <summary>
    /// Инициализация экземпляра
    /// </summary>
    public AdvancedMenu()
    {
        salesManager = null;
    }

    /// <summary>
    /// Заголовок меню
    /// </summary>
    public string Title => "Продвинутое меню";

    /// <summary>
    /// Запуск меню
    /// </summary>
    /// <returns>Экземпляр менеджера продаж</returns>
    public SalesManager Start()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine($"===== {Title} =====");
            Console.WriteLine("1. Визуализация таблицей");
            Console.WriteLine("2. Визуализация отсортированной таблицы");
            Console.WriteLine("3. Визуализация отфильтрованной таблицы");
            Console.WriteLine("4. Визуализация сгруппированных регионов в виде таблицы");
            Console.WriteLine("5. Визуализация гистограммы за выбранный период");
            Console.WriteLine("6. Визуализация Breakdown Chart за выбранный период");
            Console.WriteLine("0. Назад");

            string? input = Console.ReadLine().Trim();
            exit = ProcessMenuChoice(input);
        }

        return salesManager;
    }

    /// <summary>
    /// Обработка выбора меню
    /// </summary>
    /// <param name="choice">Выбор пользователя</param>
    /// <returns>true, если выбран выход, иначе false</returns>
    public bool ProcessMenuChoice(string choice)
    {
        bool exit = false;
        TableHandler tableHandler = new TableHandler(salesManager);
        ChartRender chartHandler = new ChartRender(salesManager);

        switch (choice)
        {
            case "0":
                exit = true;
                break;
            case "1":
                Console.Clear();
                tableHandler.DisplayAsTable();
                Console.ReadKey();
                break;
            case "2":
                Console.Clear();
                tableHandler.DisplaySortedTable();
                Console.ReadKey();
                break;
            case "3":
                Console.Clear();
                tableHandler.DisplayFilteredTable();
                Console.ReadKey();
                break;
            case "4":
                Console.Clear();
                tableHandler.DisplayGroupedByRegionTable();
                Console.ReadKey();
                break;
            case "5":
                Console.Clear();
                chartHandler.DisplayChart();
                Console.ReadKey();
                break;
            case "6":
                Console.Clear();
                chartHandler.DisplayBreakdownChart();
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