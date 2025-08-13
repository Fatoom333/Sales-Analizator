namespace Sales;

using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using GetUserInput;
using System.Globalization;
using CbrApi;

/// <summary>
/// Структура, представляющая транзакцию продажи
/// </summary>
public struct Sale
{
    private int transactionID;
    private DateTime date;
    private int productID;
    private string name;
    private int amount;
    private Dictionary<string, double> priceByCurrency;
    private Dictionary<string, double> totalByCurrency;
    private string region;

    /// <summary>
    /// Инициализирует новую транзакцию с заданными параметрами
    /// </summary>
    /// <param name="transactionID">Идентификатор транзакции</param>
    /// <param name="date">Дата транзакции</param>
    /// <param name="productID">Идентификатор продукта</param>
    /// <param name="name">Наименование продукта</param>
    /// <param name="amount">Количество единиц продукта</param>
    /// <param name="priceByCurrency">Словарь цен по валютам</param>
    /// <param name="totalByCurrency">Словарь сумм по валютам</param>
    /// <param name="region">Регион продажи</param>
    public Sale(int transactionID, DateTime date, int productID, string name, int amount,
        Dictionary<string, double> priceByCurrency, Dictionary<string, double> totalByCurrency, string region)
    {
        this.transactionID = transactionID;
        this.date = date.Date;
        this.productID = productID;
        this.name = name;
        this.amount = amount;
        this.priceByCurrency = priceByCurrency;
        this.totalByCurrency = totalByCurrency;
        this.region = region;
    }

    /// <summary>
    /// Инициализирует новую транзакцию путем копирования другой транзакции
    /// </summary>
    /// <param name="sale">Транзакция для копирования</param>
    public Sale(Sale sale)
    {
        transactionID = sale.TransactionID;
        date = sale.Date;
        productID = sale.ProductID;
        name = sale.Name;
        amount = sale.Amount;
        priceByCurrency = new Dictionary<string, double>(sale.PriceByCurrency);
        totalByCurrency = new Dictionary<string, double>(sale.TotalByCurrency);
        region = sale.Region;
    }

    /// <summary>
    /// Получает или задает идентификатор транзакции
    /// </summary>
    [JsonPropertyName("transactionID")]
    public int TransactionID
    {
        get => transactionID;
        set => transactionID = value;
    }

    /// <summary>
    /// Получает или задает дату транзакции
    /// </summary>
    [JsonPropertyName("date")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date
    {
        get => date;
        set => date = value;
    }

    /// <summary>
    /// Получает или задает идентификатор продукта
    /// </summary>
    [JsonPropertyName("productID")]
    public int ProductID
    {
        get => productID;
        set => productID = value;
    }

    /// <summary>
    /// Получает или задает наименование продукта
    /// </summary>
    [JsonPropertyName("name")]
    public string Name
    {
        get => name;
        set => name = value;
    }

    /// <summary>
    /// Получает или задает количество единиц продукта
    /// </summary>
    [JsonPropertyName("amount")]
    public int Amount
    {
        get => amount;
        set => amount = value;
    }

    /// <summary>
    /// Получает или задает словарь цен по валютам
    /// </summary>
    [JsonPropertyName("price")]
    public Dictionary<string, double> PriceByCurrency
    {
        get => priceByCurrency;
        set => priceByCurrency = value;
    }

    /// <summary>
    /// Получает или задает словарь сумм по валютам
    /// </summary>
    [JsonPropertyName("total")]
    public Dictionary<string, double> TotalByCurrency
    {
        get => totalByCurrency;
        set => totalByCurrency = value;
    }

    /// <summary>
    /// Получает или задает регион продажи
    /// </summary>
    [JsonPropertyName("region")]
    public string Region
    {
        get => region;
        set => region = value;
    }

    /// <summary>
    /// Возвращает массив строковых представлений всех полей транзакции
    /// </summary>
    /// <returns>Массив значений полей</returns>
    public string[] GetAllFieldsValues()
    {
        return new string[]
        {
            TransactionID.ToString(),
            Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
            ProductID.ToString(),
            Name,
            Amount.ToString(),
            string.Join(", ", PriceByCurrency.Select(kvp => $"{kvp.Key}:{kvp.Value}")),
            string.Join(", ", TotalByCurrency.Select(kvp => $"{kvp.Key}:{kvp.Value}")),
            Region
        };
    }

    /// <summary>
    /// Возвращает массив названий всех полей транзакции
    /// </summary>
    /// <returns>Массив названий полей</returns>
    public string[] GetAllFieldsNames()
    {
        return new string[]
        {
            nameof(TransactionID),
            nameof(Date),
            nameof(ProductID),
            nameof(Name),
            nameof(Amount),
            nameof(PriceByCurrency),
            nameof(TotalByCurrency),
            nameof(Region)
        };
    }

    /// <summary>
    /// Создает новую транзакцию с вводом данных от пользователя
    /// </summary>
    /// <param name="indicator">Флаг, указывающий необходимость предупредить пользователя о вводе новых данных</param>
    /// <returns>Новая транзакция</returns>
    public static Sale CreateSale(bool indicator)
    {
        if (indicator) Console.WriteLine("Вводите новые данные:");
        int transactionId = int.Parse(GetUserInput.GetUserStringInput(
            input => int.TryParse(input, out int id) && id.ToString().Length == 9,
            "Введите TransactionId (9 цифр):"));
        DateTime date = DateTime.ParseExact(
            GetUserInput.GetUserStringInput(
                input => DateTime.TryParseExact(
                             input,
                             "dd.MM.yyyy",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.None,
                             out DateTime dateTime) && (dateTime.Year >= 2012 && dateTime.Year <= 2025) &&
                         dateTime <= DateTime.Now,
                "Введите Date (С 2012 по 2025) в формате DD.MM.YYYY:"),
            "dd.MM.yyyy",
            CultureInfo.InvariantCulture);
        int productId = int.Parse(GetUserInput.GetUserStringInput(
            input => int.TryParse(input, out int id) && id.ToString().Length == 9,
            "Введите ProductID (9 цифр):"));
        string name = GetUserInput.GetUserStringInput(
            input => !string.IsNullOrWhiteSpace(input),
            "Введите Name:");
        int amount = int.Parse(GetUserInput.GetUserStringInput(
            input => int.TryParse(input, out _),
            "Введите Amount:"));
        string inputPairs = GetUserInput.GetUserStringInput(
            input =>
            {
                var pairs = input.Split(',').Select(p => p.Trim());
                return pairs.All(pair =>
                {
                    var parts = pair.Split(':');
                    return parts.Length == 2 && double.TryParse(parts[0].Trim(), out _) &&
                           !string.IsNullOrWhiteSpace(parts[1]);
                });
            },
            "Введите пары Price:Currency через запятую (например, '100:USD, 200:RUB'):");
        Dictionary<string, double> priceByCurrency = inputPairs
            .Split(',')
            .Select(pair => pair.Split(':'))
            .ToDictionary(
                parts => parts[1].Trim().ToUpperInvariant(),
                parts => double.Parse(parts[0].Trim())
            );
        string region = GetUserInput.GetUserStringInput(
            input => !string.IsNullOrWhiteSpace(input),
            "Введите Region:");
        if (priceByCurrency.Any(p => p.Key != "RUB"))
        {
            CbrApiClient client = new CbrApiClient();
            var notRubPrice = priceByCurrency.First(p => p.Key != "RUB");
            var requestResult = client.GetCurrencyRateAsync(notRubPrice.Key, date).Result;
            double rubPrice = Convert.ToDouble(requestResult);
            priceByCurrency.Add("RUB", rubPrice * notRubPrice.Value);
            client.Dispose();
        }

        Dictionary<string, double> total = new Dictionary<string, double>(priceByCurrency.ToDictionary(
            kvp => kvp.Key,
            kvp => amount * kvp.Value
        ));
        return new Sale(transactionId, date, productId, name, amount, priceByCurrency, total, region);
    }

    /// <summary>
    /// Определяет, равны ли две транзакции
    /// </summary>
    /// <param name="left">Левая транзакция</param>
    /// <param name="right">Правая транзакция</param>
    /// <returns>true если транзакции равны, иначе false</returns>
    public static bool operator ==(Sale left, Sale right)
    {
        bool s = (left.TransactionID.Equals(right.TransactionID) &&
                  left.Date.Equals(right.Date) &&
                  left.ProductID.Equals(right.ProductID) &&
                  left.Name.Equals(right.Name) &&
                  left.Amount.Equals(right.Amount) &&
                  left.PriceByCurrency.OrderBy(kvp => kvp.Key)
                      .SequenceEqual(right.PriceByCurrency.OrderBy(kvp => kvp.Key)) &&
                  left.Region.Equals(right.Region));
        return s;
    }

    /// <summary>
    /// Определяет, не равны ли две транзакции
    /// </summary>
    /// <param name="left">Левая транзакция</param>
    /// <param name="right">Правая транзакция</param>
    /// <returns>true если транзакции не равны, иначе false</returns>
    public static bool operator !=(Sale left, Sale right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Проверяет равенство объекта с данной транзакцией
    /// </summary>
    /// <param name="obj">Объект для сравнения</param>
    /// <returns>true если объекты равны, иначе false</returns>
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Sale other = (Sale)obj;
        return this == other;
    }

   
    /// <summary>
    /// Возвращает хэш-код для текущего объекта продажи
    /// </summary>
    /// <returns>Хэш-код объекта</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(TransactionID, Date, ProductID, Name, Amount,
            PriceByCurrency.Aggregate(0, (hash, pair) => HashCode.Combine(hash, pair.Key, pair.Value)),
            TotalByCurrency.Aggregate(0, (hash, pair) => HashCode.Combine(hash, pair.Key, pair.Value)),
            Region);
    }

    /// <summary>
    /// Возвращает строковое представление транзакции в формате JSON
    /// </summary>
    /// <returns>Строка в формате JSON</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}