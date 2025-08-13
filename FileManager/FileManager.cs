namespace FileManager;

/// <summary>
/// Класс для работы с файлами
/// </summary>
public class FileManager : IFileManager
{
    /// <summary>
    /// Читает содержимое файла по заданному пути
    /// </summary>
    /// <param name="filePath">Путь к файлу</param>
    /// <returns>Строка с содержимым файла</returns>
    public string ReadFile(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    /// <summary>
    /// Записывает заданное содержимое в файл по указанному пути
    /// </summary>
    /// <param name="filePath">Путь к файлу</param>
    /// <param name="content">Содержимое для записи</param>
    public void WriteFile(string filePath, string content)
    {
        File.WriteAllText(filePath, content);
    }
}