using Lab5.BLL.Exceptions;
using Lab5.BLL.Mapping;
using Lab5.BLL.Models;
using Lab5.DAL;
using Lab5.Tests.Doubles;

namespace Lab5.Tests;

public class MapperTests
{
    [Fact]
    public void StudentMapper_ToModelThenToEntity_PreservesAllFields()
    {
     
        StudentMapper mapper = new StudentMapper();
        StudentEntity source = TestData.NewStudentEntity(dormitoryRoom: "3.215", dances: 2);
        source.DriverLicense = "ABC123456";

       
        StudentEntity result = mapper.ToEntity(mapper.ToModel(source));

       
        Assert.Equal(source.LastName, result.LastName);
        Assert.Equal(source.FirstName, result.FirstName);
        Assert.Equal(source.Course, result.Course);
        Assert.Equal(source.StudentId, result.StudentId);
        Assert.Equal(source.ArrivalCity, result.ArrivalCity);
        Assert.Equal(source.Passport, result.Passport);
        Assert.Equal(source.Dances, result.Dances);
        Assert.Equal(source.DriverLicense, result.DriverLicense);
        Assert.Equal(source.DormitoryRoom, result.DormitoryRoom);
    }

    [Fact]
    public void StudentMapper_ToModelWithInvalidEntity_ThrowsInvalidEntityData()
    {
       
        StudentMapper mapper = new StudentMapper();
        StudentEntity broken = TestData.NewStudentEntity(course: 9);
        Action act = () => mapper.ToModel(broken);

       
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Fact]
    public void TaxiDriverMapper_ToModelThenToEntity_PreservesAllFields()
    {
     
        TaxiDriverMapper mapper = new TaxiDriverMapper();
        TaxiDriverEntity source = TestData.NewTaxiEntity(trips: 7);
        source.Dances = 3;

        
        TaxiDriverEntity result = mapper.ToEntity(mapper.ToModel(source));

      
        Assert.Equal(source.DriverLicense, result.DriverLicense);
        Assert.Equal(source.Trips, result.Trips);
        Assert.Equal(source.Dances, result.Dances);
        Assert.Equal(source.LastName, result.LastName);
        Assert.Equal(source.FirstName, result.FirstName);
    }

    [Fact]
    public void AcrobatMapper_ToModelThenToEntity_PreservesAllFields()
    {
       
        AcrobatMapper mapper = new AcrobatMapper();
        AcrobatEntity source = TestData.NewAcrobatEntity(years: 9);
        source.Dances = 1;
        source.DriverLicense = "ABC123456";

       
        AcrobatEntity result = mapper.ToEntity(mapper.ToModel(source));

       
        Assert.Equal(source.CertificateId, result.CertificateId);
        Assert.Equal(source.ExperienceYears, result.ExperienceYears);
        Assert.Equal(source.Dances, result.Dances);
        Assert.Equal(source.DriverLicense, result.DriverLicense);
    }
}
