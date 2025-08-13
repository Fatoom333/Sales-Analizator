namespace Sales;

using GetUserInput;
using Spectre.Console;
using System.Globalization;

/// <summary>
/// Класс для визуализации диаграмм продаж
/// </summary>
public class ChartRender
{
    private readonly SalesManager salesManager;

    /// <summary>
    /// Список цветов для диаграмм
    /// </summary>
    private readonly List<Color> colors = new List<Color>
    {
        Color.Red,
        Color.Green,
        Color.Blue,
        Color.Yellow,
        Color.Purple,
        Color.Orange1,
        Color.Cyan1,
        Color.Magenta1,
        Color.Lime,
        Color.Pink1,
        Color.White,
        Color.Grey,
        Color.Black,
        Color.Turquoise2,
        Color.Violet
    };

    /// <summary>
    /// Инициализирует новый экземпляр ChartRender с заданным менеджером продаж
    /// </summary>
    /// <param name="salesManager">Менеджер продаж для работы с данными</param>
    public ChartRender(SalesManager salesManager)
    {
        this.salesManager = salesManager;
    }

    /// <summary>
    /// Отображает столбчатую диаграмму с суммой продаж по дням
    /// </summary>
    public void DisplayChart()
    {
        var chart = new BarChart()
            .Width(200)
            .Label("\ud83d\udcca [yellow]Сумма продаж по дням[/]");

        List<DateTime> dates = new List<DateTime>(GetDates());
        var dailyTotal = salesManager.GetDailyTotal(dates[0], dates[1]);

        foreach (var dayTuple in dailyTotal)
        {
            chart.AddItem(dayTuple.Item1, dayTuple.Item2);
        }

        Console.Clear();
        AnsiConsole.Write(chart);
    }

    /// <summary>
    /// Отображает диаграмму распределения продаж по категориям
    /// </summary>
    public void DisplayBreakdownChart()
    {
        var breakdownChart = new BreakdownChart()
            .Width(200);
        var title = new Markup("\ud83d\uddc2\ufe0f [yellow]Распределение продаж по категориям[/]").Centered();
        Random rnd = new Random();

        List<DateTime> dates = new List<DateTime>(GetDates());
        var dailySolds = salesManager.GetDailySolds(dates[0], dates[1]);

        foreach (var dayTuple in dailySolds)
        {
            breakdownChart.AddItem(dayTuple.Item1.ToString(), dayTuple.Item2, colors[rnd.Next(0, colors.Count)]);
        }

        Console.Clear();
        AnsiConsole.MarkupLine("\ud83d\uddc2\ufe0f [yellow]Распределение продаж по категориям[/]", Justify.Center);
        AnsiConsole.Write(breakdownChart);
    }

    /// <summary>
    /// Запрашивает у пользователя начальную и конечную даты для выборки данных
    /// </summary>
    /// <returns>Список из двух дат: первая – начало периода, вторая – конец периода</returns>
    private List<DateTime> GetDates()
    {
        var dates = new List<DateTime>();

        DateTime date1 = DateTime.ParseExact(
            GetUserInput.GetUserStringInput(
                input => DateTime.TryParseExact(
                             input,
                             "dd.MM.yyyy",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.None,
                             out DateTime dateTime) && (dateTime.Year >= 2012 && dateTime.Year <= 2025) &&
                         dateTime <= DateTime.Now,
                "Введите начало периода (С 2012 по 2025) в формате DD.MM.YYYY:"),
            "dd.MM.yyyy",
            CultureInfo.InvariantCulture);

        DateTime date2 = DateTime.ParseExact(
            GetUserInput.GetUserStringInput(
                input => DateTime.TryParseExact(
                             input,
                             "dd.MM.yyyy",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.None,
                             out DateTime dateTime) &&
                         (dateTime.Year >= 2012 && dateTime.Year <= 2025 && dateTime >= date1) &&
                         dateTime <= DateTime.Now,
                "Введите конец периода (С 2012 по 2025) в формате DD.MM.YYYY:"),
            "dd.MM.yyyy",
            CultureInfo.InvariantCulture);

        dates.Add(date1);
        dates.Add(date2);

        return dates;
    }
}