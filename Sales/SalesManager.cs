namespace Sales;

using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Linq;
using System.Globalization;
using System.ComponentModel;
using System.Collections;

public class SalesManager : ISalesManager, IEnumerable
{
    private List<Sale> sales;

    /// <summary>
    /// Инициализирует экземпляр SalesManager из строки в формате JSON
    /// </summary>
    /// <param name="jsonString">Строка в формате JSON, содержащая данные о продажах</param>
    public SalesManager(string jsonString)
    {
        using JsonDocument document = JsonDocument.Parse(jsonString);
        string salesJson = document.RootElement.GetProperty("sales").GetRawText();
        var salesDeserialize = JsonSerializer.Deserialize<List<Sale>>(salesJson);
        if (salesDeserialize is null || salesDeserialize.Any(sale => sale == default(Sale)) ||
            salesDeserialize.Any(sale => sale.GetAllFieldsValues().Contains(null)) ||
            salesDeserialize.Any(sale =>
                sale.GetAllFieldsValues().ToList().Any(fieldValue => fieldValue.Contains("-"))))
            throw new NullReferenceException("Данных нет или они повреждены");

        sales = new List<Sale>(salesDeserialize);
    }

    /// <summary>
    /// Возвращает список продаж в режиме только для чтения
    /// </summary>
    [JsonPropertyName("sales")]
    public IReadOnlyList<Sale> Sales => sales.AsReadOnly();

    /// <summary>
    /// Возвращает перечислитель для коллекции продаж
    /// </summary>
    /// <returns>Перечислитель для коллекции продаж</returns>
    public IEnumerator GetEnumerator()
    {
        return sales.GetEnumerator();
    }

    /// <summary>
    /// Возвращает перечислитель для коллекции продаж (явная реализация интерфейса IEnumerable)
    /// </summary>
    /// <returns>Перечислитель для коллекции продаж</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Добавляет новую продажу в коллекцию
    /// </summary>
    /// <param name="sale">Объект продажи для добавления</param>
    public void AddSale(Sale sale)
    {
        sales.Add(sale);
    }

    /// <summary>
    /// Удаляет продажу по указанному идентификатору транзакции
    /// </summary>
    /// <param name="transactionId">Идентификатор транзакции для удаления</param>
    public void RemoveSale(int transactionId)
    {
        sales.RemoveAll(sale => sale.TransactionID == transactionId);
    }

    /// <summary>
    /// Обновляет данные продажи по указанному идентификатору транзакции
    /// </summary>
    /// <param name="transactionId">Идентификатор транзакции для обновления</param>
    /// <param name="sale">Новый объект продажи</param>
    public void UpdateSale(int transactionId, Sale sale)
    {
        var index = sales.FindIndex(searchSale => searchSale.TransactionID == transactionId);
        if (index == -1) throw new ArgumentException("Такой TransactionID не существует");
        sales[index] = sale;
    }

    /// <summary>
    /// Группирует продажи по региону
    /// </summary>
    /// <returns>Список групп продаж, каждая группа представлена списком объектов Sale</returns>
    public List<List<Sale>> GroupSalesByRegion()
    {
        return sales.GroupBy(sale => sale.Region)
            .Select(group => group.ToList())
            .ToList();
    }

    /// <summary>
    /// Вычисляет суммарную стоимость продаж за каждый день в указанном диапазоне дат
    /// </summary>
    /// <param name="startDate">Дата начала диапазона</param>
    /// <param name="endDate">Дата окончания диапазона</param>
    /// <returns>Список кортежей с датой и суммарной стоимостью продаж в рублях</returns>
    public List<(string Date, double Total)> GetDailyTotal(DateTime startDate, DateTime endDate)
    {
        var result = sales
            .Where(sale => sale.Date >= startDate && sale.Date <= endDate)
            .OrderBy(sale => sale.Date)
            .GroupBy(sale => sale.Date.ToString("dd.MM.yyyy"))
            .Select(group =>
            {
                double total = group.Sum(sale => sale.TotalByCurrency["RUB"]);
                return (Date: group.Key, Total: total);
            })
            .ToList();

        return result;
    }

    /// <summary>
    /// Вычисляет общее количество проданных единиц для каждого продукта за указанный диапазон дат
    /// </summary>
    /// <param name="startDate">Дата начала диапазона</param>
    /// <param name="endDate">Дата окончания диапазона</param>
    /// <returns>Список кортежей с идентификатором продукта и количеством проданных единиц</returns>
    public List<(int ProductId, double SoldAmount)> GetDailySolds(DateTime startDate, DateTime endDate)
    {
        var result = sales
            .Where(sale => sale.Date >= startDate && sale.Date <= endDate)
            .OrderBy(sale => sale.Date)
            .GroupBy(sale => sale.ProductID)
            .Select(group =>
            (
                ProductId: group.Key,
                SoldAmount: Convert.ToDouble(group.Sum(sale => sale.Amount))
            ))
            .ToList();

        return result;
    }

    /// <summary>
    /// Пытается преобразовать строку в указанный тип
    /// </summary>
    /// <param name="input">Входная строка для преобразования</param>
    /// <param name="targetType">Целевой тип для преобразования</param>
    /// <param name="succeeded">Выходной параметр, указывающий успешность преобразования</param>
    /// <returns>Преобразованное значение или null, если преобразование не удалось</returns>
    public static object? TryConvertToType(string input, Type targetType, out bool succeeded)
    {
        try
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingType == typeof(Dictionary<string, double>))
            {
                var dict = new Dictionary<string, double>();
                // Ожидаемый формат: "Rub: 110, usd: 1,1"
                var pairs = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var pair in pairs)
                {
                    var parts = pair.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                    {
                        succeeded = false;
                        return null;
                    }

                    var key = parts[0].Trim().ToUpperInvariant();
                    var valueStr = parts[1].Trim().Replace(',', '.');

                    if (!double.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                    {
                        succeeded = false;
                        return null;
                    }

                    dict[key] = value;
                }

                succeeded = true;
                return dict;
            }

            var converter = TypeDescriptor.GetConverter(underlyingType);
            if (converter != null && converter.IsValid(input))
            {
                var convertedValue = converter.ConvertFromString(input);
                succeeded = true;
                return convertedValue;
            }
        }
        catch
        {
            succeeded = false;
            return null;
        }

        succeeded = false;
        return null;
    }

    /// <summary>
    /// Фильтрует продажи по заданному полю и критерию
    /// </summary>
    /// <param name="fieldToFilter">Имя поля для фильтрации</param>
    /// <param name="criteriaStringInput">Строка с критерием фильтрации</param>
    /// <returns>Список строковых массивов, содержащих значения всех полей продаж, удовлетворяющих критерию</returns>
    public List<string[]> FilterSales(string fieldToFilter, string criteriaStringInput)
    {
        var propertyInfo = typeof(Sale).GetProperty(fieldToFilter);
        var criteria = TryConvertToType(criteriaStringInput, propertyInfo?.PropertyType, out bool _);

        List<string[]> rows = new List<string[]>();
        foreach (var sale in sales)
        {
            var saleValue = propertyInfo?.GetValue(sale);

            if (saleValue is Dictionary<string, double> saleDict && criteria is Dictionary<string, double> criteriaDict)
            {
                if (saleDict.OrderBy(kvp => kvp.Key)
                    .SequenceEqual(criteriaDict.OrderBy(kvp => kvp.Key)))
                {
                    rows.Add(sale.GetAllFieldsValues());
                }
            }
            else
            {
                if (object.Equals(saleValue, criteria))
                {
                    rows.Add(sale.GetAllFieldsValues());
                }
            }
        }

        return rows;
    }

    /// <summary>
    /// Возвращает строковое представление объекта SalesManager в формате JSON
    /// </summary>
    /// <returns>Строка в формате JSON с данными о продажах</returns>
    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        return JsonSerializer.Serialize(new { sales }, options);
    }
}