using System.Text.Encodings.Web;
using System.Text.Json;

namespace Lab5.DAL;

/// <summary>Зберігання набору сутностей у JSON-файлі (серіалізація - єдиний спосіб DAL у цій роботі).</summary>
public class JsonFileRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly string _path;

    public JsonFileRepository(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Не вказано шлях до файлу.", nameof(path));
        }
        _path = path;
    }

    public IReadOnlyList<TEntity> LoadAll()
    {
        if (!File.Exists(_path))
        {
            return new List<TEntity>();
        }

        try
        {
            using FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            List<TEntity> result = JsonSerializer.Deserialize<List<TEntity>>(stream, Options);
            if (result == null)
            {
                return new List<TEntity>();
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Не вдалося прочитати файл '{_path}': {ex.Message}", ex);
        }
    }

    public void SaveAll(IReadOnlyCollection<TEntity> entities)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(_path)));

            using FileStream stream = new FileStream(_path, FileMode.Create, FileAccess.Write);
            JsonSerializer.Serialize(stream, entities, Options);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Не вдалося записати файл '{_path}': {ex.Message}", ex);
        }
    }
}
