namespace Sales;

/// <summary>
/// Интерфейс для управления списком данных
/// </summary>
public interface ISalesManager
{
    /// <summary>
    /// Только для чтения коллекции продаж
    /// </summary>
    IReadOnlyList<Sale> Sales { get; }

    /// <summary>
    /// Добавляет новую продажу в список
    /// </summary>
    /// <param name="sale">Продажа для добавления</param>
    void AddSale(Sale sale);

    /// <summary>
    /// Удаляет продажу по указанному ID
    /// </summary>
    /// <param name="transactionID">ID удаляемого элемента</param>
    void RemoveSale(int transactionID);

    /// <summary>
    /// Изменяет продажу по указанному ID
    /// </summary>
    /// <param name="transactionID">ID изменяемого элемента</param>
    /// <param name="updatedSale">Обновленные данные для продажи</param>
    void UpdateSale(int transactionID, Sale updatedSale);
}