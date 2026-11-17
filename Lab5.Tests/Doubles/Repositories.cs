using Lab5.DAL;

namespace Lab5.Tests.Doubles;


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
        
    }
}


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
