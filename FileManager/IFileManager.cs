namespace FileManager;

/// <summary>
/// Интерфейс для работы с файлами (чтение и запись)
/// </summary>
public interface IFileManager
{
    /// <summary>
    /// Читает данные из файла и возвращает их в виде строки
    /// </summary>
    /// <param name="filePath">Путь к файлу</param>
    /// <returns>Строку из файла</returns>
    string ReadFile(string filePath);

    /// <summary>
    /// Записывает строку в файл
    /// </summary>
    /// <param name="filePath">Путь к файлу</param>
    /// <param name="content">Строка для записи</param>
    void WriteFile(string filePath, string content);
}