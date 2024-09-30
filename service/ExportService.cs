using System.Text.Json;
using System.Xml.Serialization;

public class ExportService
{
    public void ExportToJson<T>(List<T> data, string filePath)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string jsonString = JsonSerializer.Serialize(data, jsonOptions);
        File.WriteAllText(filePath, jsonString);
        Console.WriteLine($"Дані успішно збережено у JSON файл: {filePath}");
    }

    public void ExportToXml<T>(List<T> data, string filePath)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            xmlSerializer.Serialize(fileStream, data);
        }
        Console.WriteLine($"Дані успішно збережено у XML файл: {filePath}");
    }
}
