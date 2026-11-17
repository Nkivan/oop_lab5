using Lab5.BLL.Models;
using Lab5.DAL;

namespace Lab5.BLL.Mapping;

public interface IEntityMapper<TModel, TEntity>
{
    TModel ToModel(TEntity entity);
    TEntity ToEntity(TModel model);
}

public class StudentMapper : IEntityMapper<Student, StudentEntity>
{
    public Student ToModel(StudentEntity e)
        => new Student(e.LastName, e.FirstName, e.Course, e.StudentId, e.ArrivalCity, e.Passport,
                       e.Dances, e.DriverLicense, e.DormitoryRoom);

    public StudentEntity ToEntity(Student m) => new StudentEntity
    {
        LastName = m.LastName, FirstName = m.FirstName, Course = m.Course, StudentId = m.StudentId,
        ArrivalCity = m.ArrivalCity, Passport = m.Passport, Dances = m.Dances,
        DriverLicense = m.DriverLicense, DormitoryRoom = m.DormitoryRoom
    };
}

public class TaxiDriverMapper : IEntityMapper<TaxiDriver, TaxiDriverEntity>
{
    public TaxiDriver ToModel(TaxiDriverEntity e)
        => new TaxiDriver(e.LastName, e.FirstName, e.DriverLicense, e.Trips, e.Dances);

    public TaxiDriverEntity ToEntity(TaxiDriver m) => new TaxiDriverEntity
    {
        LastName = m.LastName, FirstName = m.FirstName, DriverLicense = m.DriverLicense,
        Trips = m.Trips, Dances = m.Dances
    };
}

public class AcrobatMapper : IEntityMapper<Acrobat, AcrobatEntity>
{
    public Acrobat ToModel(AcrobatEntity e)
        => new Acrobat(e.LastName, e.FirstName, e.CertificateId, e.ExperienceYears, e.Dances, e.DriverLicense);

    public AcrobatEntity ToEntity(Acrobat m) => new AcrobatEntity
    {
        LastName = m.LastName, FirstName = m.FirstName, CertificateId = m.CertificateId,
        ExperienceYears = m.ExperienceYears, Dances = m.Dances, DriverLicense = m.DriverLicense
    };
}
