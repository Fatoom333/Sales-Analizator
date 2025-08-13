namespace GetUserInput;

/// <summary>
/// Класс для получения пользовательского ввода с консоли
/// </summary>
public class GetUserInput
{
    /// <summary>
    /// Запрашивает у пользователя решение о перезапуске программы
    /// </summary>
    /// <returns>true если пользователь ввёл "y", иначе false</returns>
    public static bool GetUserRestartDecision()
    {
        string? input;
        Console.WriteLine("Вы хотите перезапустить программу? y/n");

        do
        {
            input = Console.ReadLine().ToLower().Trim();
        } while (input != "y" && input != "n");

        return input == "y";
    }

    /// <summary>
    /// Запрашивает у пользователя путь к JSON файлу
    /// </summary>
    /// <returns>Строка с полным путём к файлу</returns>
    public static string GetUserPath()
    {
        Console.WriteLine("Введите пожалуйста адрес JSON файла");
        string? input;

        do
        {
            input = Console.ReadLine();
        } while (!Path.IsPathFullyQualified(input));

        return input;
    }

    /// <summary>
    /// Запрашивает у пользователя строковый ввод с проверкой по заданному условию
    /// </summary>
    /// <param name="approveUserInput">Функция для проверки корректности ввода</param>
    /// <param name="message">Сообщение для пользователя</param>
    /// <returns>Введённая строка, удовлетворяющая условию</returns>
    public static string GetUserStringInput(Func<string, bool> approveUserInput, string message)
    {
        Console.WriteLine(message);
        string? input;

        do
        {
            input = Console.ReadLine();
        } while (!approveUserInput(input ?? string.Empty));

        return input;
    }

    /// <summary>
    /// Запрашивает у пользователя идентификатор транзакции в виде целого числа
    /// </summary>
    /// <returns>Целочисленный идентификатор транзакции</returns>
    public static int GetTransactionId()
    {
        Console.WriteLine("Введите TransactionId:");
        string? input;
        int transactionId;

        do
        {
            input = Console.ReadLine().Trim();
        } while (!int.TryParse(input, out transactionId));

        return transactionId;
    }
}