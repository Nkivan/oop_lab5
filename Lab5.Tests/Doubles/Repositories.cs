using Lab5.DAL;

namespace Lab5.Tests.Doubles;

/// <summary>
/// STUB (заглушка): повертає заготовлені дані, нічого не запам'ятовує. Потрібна, коли тесту
/// важливі лише вхідні дані, а не те, як із ними працювали.
/// </summary>
public sealed class StubRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly List<TEntity> _data;

    public StubRepository(params TEntity[] data)
    {
        _data = new List<TEntity>(data);
    }

    public IReadOnlyList<TEntity> LoadAll() => _data;

    public void SaveAll(IReadOnlyCollection<TEntity> entities)
    {
        // Заглушка нічого не робить.
    }
}

/// <summary>
/// MOCK (мок-об'єкт): окрім заготовлених даних, запам'ятовує звернення до себе,
/// щоб тест міг перевірити взаємодію (скільки разів і що було збережено).
/// </summary>
public sealed class MockRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly List<TEntity> _data;

    public MockRepository(params TEntity[] data)
    {
        _data = new List<TEntity>(data);
    }

    public int LoadAllCallCount { get; private set; }
    public int SaveAllCallCount { get; private set; }
    public IReadOnlyList<TEntity> LastSaved { get; private set; }

    public IReadOnlyList<TEntity> LoadAll()
    {
        LoadAllCallCount++;
        return _data;
    }

    public void SaveAll(IReadOnlyCollection<TEntity> entities)
    {
        SaveAllCallCount++;
        LastSaved = new List<TEntity>(entities);
    }
}

/// <summary>Заглушка, що імітує збій сховища (файл недоступний).</summary>
public sealed class FailingRepositoryStub<TEntity> : IRepository<TEntity> where TEntity : class
{
    public IReadOnlyList<TEntity> LoadAll()
    {
        throw new DataAccessException("Файл недоступний.");
    }

    public void SaveAll(IReadOnlyCollection<TEntity> entities)
    {
        throw new DataAccessException("Файл недоступний.");
    }
}
