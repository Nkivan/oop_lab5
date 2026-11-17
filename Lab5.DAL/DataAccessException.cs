namespace Lab5.DAL;

/// <summary>Помилка рівня доступу до даних.</summary>
public class DataAccessException : Exception
{
    public DataAccessException(string message) : base(message) { }
    public DataAccessException(string message, Exception inner) : base(message, inner) { }
}
