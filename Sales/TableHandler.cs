namespace Sales;

using System;
using System.Collections.Generic;
using System.Linq;
using GetUserInput;
using Spectre.Console;
using System.Globalization;

/// <summary>
/// Класс для отображения данных о продажах в виде таблицы
/// </summary>
public class TableHandler
{
    /// <summary>
    /// Менеджер продаж, используемый для получения данных
    /// </summary>
    private readonly SalesManager salesManager;

    /// <summary>
    /// Базовые названия столбцов таблицы
    /// </summary>
    private readonly string[] baseColumnNames =
    {
        "[bold]Transaction ID[/]",
        "[bold]Date[/]",
        "[bold]Product ID[/]",
        "[bold]Name[/]",
        "[bold]Amount[/]",
        "[bold]Price[/]",
        "[bold]Total[/]",
        "[bold]Region[/]"
    };

    /// <summary>
    /// Названия столбцов для группировки по региону
    /// </summary>
    private readonly string[] regionColomNames =
    {
        "[bold]Region[/]",
        "[bold]Average[/]"
    };

    /// <summary>
    /// Инициализирует новый экземпляр TableHandler с заданным менеджером продаж
    /// </summary>
    /// <param name="salesManager">Менеджер продаж для получения данных</param>
    public TableHandler(SalesManager salesManager)
    {
        this.salesManager = salesManager;
    }

    /// <summary>
    /// Отображает таблицу с данными о продажах с возможностью передачи строк и названий столбцов
    /// </summary>
    /// <param name="rows">Строки таблицы в виде массива строковых массивов (необязательный параметр)</param>
    /// <param name="columnNames">Названия столбцов (необязательный параметр)</param>
    public void DisplayAsTable(List<string[]>? rows = null, string[] columnNames = null)
    {
        if (rows is null)
        {
            rows = new List<string[]>();
            foreach (var sale in salesManager.Sales)
            {
                rows.Add(sale.GetAllFieldsValues());
            }
        }

        if (columnNames is null) columnNames = baseColumnNames;

        var table = new Table();

        foreach (var column in columnNames) table.AddColumn(column);

        foreach (var row in rows)
        {
            table.AddRow(row);
        }

        table.Expand();
        table.Border(TableBorder.Rounded);
        table.Title("🛒 [yellow]Sales Transactions[/]");
        table.Alignment(Justify.Center);

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Отображает таблицу с отфильтрованными данными о продажах на основе выбранного поля и критерия
    /// </summary>
    public void DisplayFilteredTable()
    {
        string[] filedNames = salesManager.Sales.First().GetAllFieldsNames();
        string fieldToFilter = GetUserInput.GetUserStringInput(input => filedNames.Contains(input),
            $"Введите доступное поле для фильтрации:\n{string.Join(", ", filedNames)}");
        string criteriaStringInput = GetUserInput.GetUserStringInput(input =>
            {
                var propertyType = typeof(Sale).GetProperty(fieldToFilter)?.PropertyType;
                SalesManager.TryConvertToType(input, propertyType, out bool success);
                return propertyType is not null && success;
            },
            $"Введите значение для фильтрации по полю {fieldToFilter}:");

        Console.Clear();
        DisplayAsTable(salesManager.FilterSales(fieldToFilter, criteriaStringInput));
    }

    /// <summary>
    /// Отображает таблицу с данными о продажах, отсортированными по идентификатору транзакции в заданном порядке
    /// </summary>
    public void DisplaySortedTable()
    {
        string choice = GetUserInput.GetUserStringInput(
            input => input == "1" || input == "2",
            "1. По возрастанию\n2. По убыванию");

        switch (choice)
        {
            case "1":
                List<string[]> sortedSalesAsc = salesManager.Sales
                    .OrderBy(sale => sale.TransactionID)
                    .Select(sale => sale.GetAllFieldsValues())
                    .ToList();
                Console.Clear();
                DisplayAsTable(sortedSalesAsc);
                break;
            case "2":
                List<string[]> sortedSalesDesc = salesManager.Sales
                    .OrderByDescending(sale => sale.TransactionID)
                    .Select(sale => sale.GetAllFieldsValues())
                    .ToList();
                Console.Clear();
                DisplayAsTable(sortedSalesDesc);
                break;
        }
    }

    /// <summary>
    /// Отображает таблицу, сгруппированную по регионам с расчетом среднего значения продаж
    /// </summary>
    public void DisplayGroupedByRegionTable()
    {
        List<List<Sale>> groupedSales = salesManager.GroupSalesByRegion();
        List<double> averageSales = new List<double>();

        foreach (var sales in groupedSales)
        {
            double sumTotals = 0.0;
            int count = 0;

            foreach (var sale in sales)
            {
                sumTotals += sale.TotalByCurrency["RUB"];
                count++;
            }

            averageSales.Add(sumTotals / count);
        }

        DisplayAsTable(ConvertGroupedSalesToStringArray(groupedSales, averageSales), regionColomNames);
    }

    /// <summary>
    /// Преобразует сгруппированные продажи и соответствующие средние значения в список строковых массивов для отображения в таблице
    /// </summary>
    /// <param name="groupedSales">Список групп продаж, каждая группа представлена списком объектов Sale</param>
    /// <param name="averageSales">Список средних значений продаж для каждой группы</param>
    /// <returns>Список строковых массивов с отформатированными данными</returns>
    private List<string[]> ConvertGroupedSalesToStringArray(List<List<Sale>> groupedSales, List<double> averageSales)
    {
        List<string[]> formattedData = new List<string[]>();

        for (int i = 0; i < groupedSales.Count; i++)
        {
            formattedData.Add(new string[]
            {
                groupedSales[i].First().Region,
                averageSales[i].ToString("F2")
            });

            formattedData.Add(new string[] { "", "" });
        }

        return formattedData;
    }
}