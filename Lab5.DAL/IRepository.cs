namespace Lab5.DAL;


public interface IRepository<TEntity> where TEntity : class
{
    IReadOnlyList<TEntity> LoadAll();
    void SaveAll(IReadOnlyCollection<TEntity> entities);
}
