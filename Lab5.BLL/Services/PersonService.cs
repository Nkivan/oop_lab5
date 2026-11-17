using Lab5.BLL.Exceptions;
using Lab5.BLL.Mapping;
using Lab5.BLL.Models;
using Lab5.DAL;

namespace Lab5.BLL.Services;


public class PersonService<TModel, TEntity>
    where TModel : Person
    where TEntity : PersonEntity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IEntityMapper<TModel, TEntity> _mapper;

    public PersonService(IRepository<TEntity> repository, IEntityMapper<TModel, TEntity> mapper)
    {
        if (repository == null)
        {
            throw new ArgumentNullException(nameof(repository));
        }
        if (mapper == null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }
        _repository = repository;
        _mapper = mapper;
    }

    public IReadOnlyList<TModel> GetAll()
    {
        return LoadModels();
    }

    public void Add(TModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        List<TModel> models = LoadModels();
        if (IndexOf(models, model.Key) >= 0)
        {
            throw new DuplicateEntityException(model.Key);
        }

        models.Add(model);
        Save(models);
    }

    public void Remove(string key)
    {
        List<TModel> models = LoadModels();
        int index = IndexOf(models, key);
        if (index < 0)
        {
            throw new EntityNotFoundException(key);
        }

        models.RemoveAt(index);
        Save(models);
    }

    public TModel GetByKey(string key)
    {
        List<TModel> models = LoadModels();
        int index = IndexOf(models, key);
        if (index < 0)
        {
            throw new EntityNotFoundException(key);
        }
        return models[index];
    }

    public IReadOnlyList<TModel> FindByLastName(string lastName)
    {
        List<TModel> result = new List<TModel>();
        foreach (TModel model in LoadModels())
        {
            if (string.Equals(model.LastName, lastName, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(model);
            }
        }
        return result;
    }

    public void Dance(string key)
    {
        Modify(key, model => model.Dance());
    }

    public void ObtainDriverLicense(string key, DrivingExamResults results, string licenseNumber)
    {
        Modify(key, model => model.ObtainDriverLicense(results, licenseNumber));
    }


    protected void Modify(string key, Action<TModel> change)
    {
        List<TModel> models = LoadModels();
        int index = IndexOf(models, key);
        if (index < 0)
        {
            throw new EntityNotFoundException(key);
        }

        change(models[index]);
        Save(models);
    }

    protected List<TModel> LoadModels()
    {
        List<TModel> models = new List<TModel>();
        foreach (TEntity entity in _repository.LoadAll())
        {
            models.Add(_mapper.ToModel(entity));
        }
        return models;
    }

    private void Save(List<TModel> models)
    {
        List<TEntity> entities = new List<TEntity>();
        foreach (TModel model in models)
        {
            entities.Add(_mapper.ToEntity(model));
        }
        _repository.SaveAll(entities);
    }

    private static int IndexOf(List<TModel> models, string key)
    {
        for (int i = 0; i < models.Count; i++)
        {
            if (string.Equals(models[i].Key, key, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        return -1;
    }
}
