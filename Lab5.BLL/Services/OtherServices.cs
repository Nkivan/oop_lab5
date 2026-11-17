using Lab5.BLL.Mapping;
using Lab5.BLL.Models;
using Lab5.DAL;

namespace Lab5.BLL.Services;

public class TaxiDriverService : PersonService<TaxiDriver, TaxiDriverEntity>
{
    public TaxiDriverService(IRepository<TaxiDriverEntity> repository)
        : base(repository, new TaxiDriverMapper())
    {
    }

    public void CompleteTrip(string key)
    {
        Modify(key, driver => driver.CompleteTrip());
    }
}

public class AcrobatService : PersonService<Acrobat, AcrobatEntity>
{
    public AcrobatService(IRepository<AcrobatEntity> repository)
        : base(repository, new AcrobatMapper())
    {
    }

    public void AddExperienceYear(string key)
    {
        Modify(key, acrobat => acrobat.AddExperienceYear());
    }
}
