namespace Sales;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Преобразователь DateTime для работы с JSON в формате dd.MM.yyyy
/// </summary>
public class DateTimeConverter : JsonConverter<DateTime>
{
    /// <summary>
    /// Формат даты
    /// </summary>
    private readonly string format = "dd.MM.yyyy";

    /// <summary>
    /// Читает значение даты из JSON строки
    /// </summary>
    /// <param name="reader">Читатель JSON</param>
    /// <param name="typeToConvert">Тип, который нужно преобразовать</param>
    /// <param name="options">Параметры сериализации JSON</param>
    /// <returns>Объект DateTime, созданный на основе строки</returns>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string dateString = reader.GetString();
        return DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Записывает значение DateTime в JSON строку с заданным форматом
    /// </summary>
    /// <param name="writer">Писатель JSON</param>
    /// <param name="value">Дата для записи</param>
    /// <param name="options">Параметры сериализации JSON</param>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(format));
    }
}