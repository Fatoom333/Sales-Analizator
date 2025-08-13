namespace ConsoleMenu;

using FileManager;
using GetUserInput;
using Sales;
using System;
using ExceptionHandler;
using System.Text.Json;

/// <summary>
/// Главное меню консольного приложения
/// </summary>
public class MainMenu : IMenu
{
    /// <summary>
    /// Экземпляр продвинутого меню
    /// </summary>
    private AdvancedMenu advancedMenu { get; }

    /// <summary>
    /// Экземпляр меню транзакций
    /// </summary>
    private TransactionMenu transactionMenu { get; }

    /// <summary>
    /// Менеджер работы с файлами
    /// </summary>
    private FileManager fileManager { get; set; }

    /// <summary>
    /// Менеджер продаж
    /// </summary>
    private SalesManager salesManager { get; set; }

    /// <summary>
    /// Путь к файлу
    /// </summary>
    private string filePath { get; set; }

    /// <summary>
    /// Инициализация экземпляра MainMenu
    /// </summary>
    public MainMenu()
    {
        fileManager = new FileManager();
        advancedMenu = new AdvancedMenu();
        transactionMenu = new TransactionMenu();
    }

    /// <summary>
    /// Заголовок главного меню
    /// </summary>
    public string Title => "Главное меню";

    /// <summary>
    /// Запуск главного меню и выполнение выбранных операций
    /// </summary>
    /// <returns>Менеджер продаж после завершения работы меню</returns>
    public SalesManager Start()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine($"===== {Title} =====");
            Console.WriteLine("1. Ввести адрес файла");
            Console.WriteLine("2. Работа с транзакциями");
            Console.WriteLine("3. Продвинутое меню");
            Console.WriteLine("0. Выход");
            Console.WriteLine("Выберите пункты из меню, вводя соответсвующее цифры:");

            string? input = Console.ReadLine();
            exit = ProcessMenuChoice(input);
        }

        if (filePath != null)
            fileManager.WriteFile(filePath, salesManager.ToString());
        return null;
    }

    /// <summary>
    /// Обрабатывает выбор пользователя в главном меню
    /// </summary>
    /// <param name="choice">Выбор пользователя</param>
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
                try
                {
                    filePath = GetUserInput.GetUserPath();
                    salesManager = new SalesManager(fileManager.ReadFile(filePath));
                }
                catch (Exception e)
                {
                    IExceptionHandler fileExceptionHandler = new FileExceptionHandler();
                    fileExceptionHandler.Handle(e);
                    throw new JsonException();
                }

                transactionMenu.salesManager = salesManager;
                advancedMenu.salesManager = salesManager;
                break;
            case "2":
                if (salesManager == null)
                    throw new NullReferenceException("Вначале надо ввести данные");
                salesManager = transactionMenu.Start();
                advancedMenu.salesManager = salesManager;
                break;
            case "3":
                if (salesManager == null)
                    throw new NullReferenceException("Вначале надо ввести данные");
                salesManager = advancedMenu.Start();
                transactionMenu.salesManager = salesManager;
                break;
            default:
                Console.WriteLine("Некорректный выбор. Попробуйте ещё раз");
                Console.ReadKey();
                break;
        }

        return exit;
    }
}