namespace Lab5.DAL;

/// <summary>
/// Абстракція доступу до даних. Бізнес-логіка залежить лише від цього інтерфейсу,
/// тому в модульних тестах справжній файловий репозиторій підміняється заглушкою.
/// </summary>
public interface IRepository<TEntity> where TEntity : class
{
    IReadOnlyList<TEntity> LoadAll();
    void SaveAll(IReadOnlyCollection<TEntity> entities);
}
