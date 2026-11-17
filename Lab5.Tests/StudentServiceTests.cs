using Lab5.BLL.Exceptions;
using Lab5.BLL.Mapping;
using Lab5.BLL.Models;
using Lab5.BLL.Services;
using Lab5.DAL;
using Lab5.Tests.Doubles;

namespace Lab5.Tests;


public class StudentServiceTests
{
 

    [Fact]
    public void Constructor_NullRepository_ThrowsArgumentNullException()
    {
        
        Action act = () => new StudentService(null);

   
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void Constructor_NullMapper_ThrowsArgumentNullException()
    {
        
        IRepository<StudentEntity> repository = new StubRepository<StudentEntity>();
        Action act = () => new PersonService<Student, StudentEntity>(repository, null);

    
        Assert.Throws<ArgumentNullException>(act);
    }

  

    [Fact]
    public void GetAll_RepositoryHasEntities_ReturnsMappedModels()
    {
       
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity("KB123456"), TestData.NewStudentEntity("KB123457")));

       
        IReadOnlyList<Student> students = service.GetAll();

       
        Assert.Equal(2, students.Count);
        Assert.Equal("KB123456", students[0].Key);
        Assert.Equal("KB123457", students[1].Key);
    }

    [Fact]
    public void GetAll_EmptyRepository_ReturnsEmptyList()
    {
       
        StudentService service = new StudentService(new StubRepository<StudentEntity>());

        
        IReadOnlyList<Student> students = service.GetAll();

      
        Assert.Empty(students);
    }

    [Fact]
    public void GetAll_StorageFails_PropagatesDataAccessException()
    {
        
        StudentService service = new StudentService(new FailingRepositoryStub<StudentEntity>());
        Action act = () => service.GetAll();

       
        Assert.Throws<DataAccessException>(act);
    }

    [Fact]
    public void GetAll_InvalidDataInStorage_ThrowsInvalidEntityData()
    {
       
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity(course: 9)));
        Action act = () => service.GetAll();

       
        Assert.Throws<InvalidEntityDataException>(act);
    }

    

    [Fact]
    public void Add_NewStudent_SavesExistingAndNewOnce()
    {
       
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        Student newStudent = new Student("Мельник", "Андрій", 2, "KB123457", "Одеса", "AB123457");

      
        service.Add(newStudent);

        
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal(2, repository.LastSaved.Count);
        Assert.Equal("KB123457", repository.LastSaved[1].StudentId);
    }

    [Fact]
    public void Add_NullModel_ThrowsArgumentNullException()
    {
      
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>();
        StudentService service = new StudentService(repository);
        Action act = () => service.Add(null);

       
        Assert.Throws<ArgumentNullException>(act);

       
        Assert.Equal(0, repository.SaveAllCallCount);
    }

    [Fact]
    public void Add_DuplicateKey_ThrowsAndDoesNotSave()
    {
    
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        Student duplicate = new Student("Інший", "Студент", 1, "KB123456", "Львів", "AB999999");
        Action act = () => service.Add(duplicate);

      
        DuplicateEntityException exception = Assert.Throws<DuplicateEntityException>(act);

      
        Assert.Contains("KB123456", exception.Message);
        Assert.Equal(0, repository.SaveAllCallCount);
    }

   

    [Fact]
    public void Remove_ExistingKey_SavesWithoutRemovedStudent()
    {
       
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(
            TestData.NewStudentEntity("KB123456"), TestData.NewStudentEntity("KB123457"));
        StudentService service = new StudentService(repository);

     
        service.Remove("KB123456");

    
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Single(repository.LastSaved);
        Assert.Equal("KB123457", repository.LastSaved[0].StudentId);
    }

    [Fact]
    public void Remove_UnknownKey_ThrowsNotFoundAndDoesNotSave()
    {
      
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        Action act = () => service.Remove("KB000000");

      
        Assert.Throws<EntityNotFoundException>(act);

      
        Assert.Equal(0, repository.SaveAllCallCount);
    }

  

    [Fact]
    public void GetByKey_KeyInDifferentCase_ReturnsStudent()
    {
    
        StudentService service = new StudentService(new StubRepository<StudentEntity>(TestData.NewStudentEntity("KB123456")));

    
        Student student = service.GetByKey("kb123456");

     
        Assert.Equal("KB123456", student.StudentId);
    }

    [Fact]
    public void GetByKey_UnknownKey_ThrowsNotFound()
    {
       
        StudentService service = new StudentService(new StubRepository<StudentEntity>(TestData.NewStudentEntity("KB123456")));
        Action act = () => service.GetByKey("KB000000");

      
        Assert.Throws<EntityNotFoundException>(act);
    }

    [Fact]
    public void FindByLastName_DifferentCase_ReturnsAllMatches()
    {
     
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity("KB123456", "Коваленко"),
            TestData.NewStudentEntity("KB123457", "Мельник"),
            TestData.NewStudentEntity("KB123458", "Коваленко")));

     
        IReadOnlyList<Student> found = service.FindByLastName("КОВАЛЕНКО");

     
        Assert.Equal(2, found.Count);
    }

    [Fact]
    public void FindByLastName_NoMatches_ReturnsEmptyList()
    {
     
        StudentService service = new StudentService(new StubRepository<StudentEntity>(TestData.NewStudentEntity()));

      
        IReadOnlyList<Student> found = service.FindByLastName("Невідомий");

      
        Assert.Empty(found);
    }



    [Fact]
    public void Dance_ExistingKey_IncrementsDancesAndSaves()
    {
       
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456", dances: 4));
        StudentService service = new StudentService(repository);

     
        service.Dance("KB123456");

    
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal(5, repository.LastSaved[0].Dances);
    }

    [Fact]
    public void Dance_UnknownKey_ThrowsNotFoundAndDoesNotSave()
    {
    
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        Action act = () => service.Dance("KB000000");

     
        Assert.Throws<EntityNotFoundException>(act);

       
        Assert.Equal(0, repository.SaveAllCallCount);
    }

    [Fact]
    public void ObtainDriverLicense_ConditionsMet_SavesLicense()
    {
        
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        DrivingExamResults results = new DrivingExamResults(19, true, true, true);

     
        service.ObtainDriverLicense("KB123456", results, "ABC123456");

        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal("ABC123456", repository.LastSaved[0].DriverLicense);
    }

    [Fact]
    public void ObtainDriverLicense_ConditionsNotMet_ThrowsAndDoesNotSave()
    {
     
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        DrivingExamResults results = new DrivingExamResults(16, true, true, true);
        Action act = () => service.ObtainDriverLicense("KB123456", results, "ABC123456");

    
        Assert.Throws<DriverLicenseException>(act);

    
        Assert.Equal(0, repository.SaveAllCallCount);
    }

   

    [Fact]
    public void MoveIntoDormitory_StudentFromOtherCity_SavesRoom()
    {
     
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456", city: "Львів"));
        StudentService service = new StudentService(repository);

     
        service.MoveIntoDormitory("KB123456", "Київ", 3, 215);

     
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal("3.215", repository.LastSaved[0].DormitoryRoom);
    }

    [Fact]
    public void MoveIntoDormitory_LocalStudent_ThrowsAndDoesNotSave()
    {
       
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456", city: "Київ"));
        StudentService service = new StudentService(repository);
        Action act = () => service.MoveIntoDormitory("KB123456", "Київ", 3, 215);

     
        Assert.Throws<DormitoryException>(act);

       
        Assert.Equal(0, repository.SaveAllCallCount);
    }

    [Fact]
    public void MoveIntoDormitory_UnknownKey_ThrowsNotFound()
    {
        
        StudentService service = new StudentService(new StubRepository<StudentEntity>());
        Action act = () => service.MoveIntoDormitory("KB000000", "Київ", 3, 215);

     
        Assert.Throws<EntityNotFoundException>(act);
    }



    [Fact]
    public void GetFirstYearFromOtherCities_MixedStudents_ReturnsPercentAndStudents()
    {
     
        StudentService service = new StudentService(new StubRepository<StudentEntity>(TestData.MixedStudents()));

      
        FirstYearReport report = service.GetFirstYearFromOtherCities("Київ");

   
        Assert.Equal("Київ", report.UniversityCity);
        Assert.Equal(4, report.FirstYearCount);
        Assert.Equal(3, report.FromOtherCitiesCount);
        Assert.Equal(75.0, report.Percent, 6);
        Assert.Equal(3, report.Students.Count);
        Assert.Equal("KB123456", report.Students[0].StudentId);
        Assert.Equal("KB123458", report.Students[1].StudentId);
        Assert.Equal("KB123459", report.Students[2].StudentId);
    }

    [Fact]
    public void GetFirstYearFromOtherCities_AllFromOtherCities_Returns100Percent()
    {
      
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity("KB123456", city: "Львів"),
            TestData.NewStudentEntity("KB123457", city: "Харків")));

     
        FirstYearReport report = service.GetFirstYearFromOtherCities("Київ");

     
        Assert.Equal(100.0, report.Percent, 6);
    }

    [Fact]
    public void GetFirstYearFromOtherCities_NoneFromOtherCities_ReturnsZeroPercent()
    {
        
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity("KB123456", city: "Київ"),
            TestData.NewStudentEntity("KB123457", city: "київ")));

     
        FirstYearReport report = service.GetFirstYearFromOtherCities("Київ");

     
        Assert.Equal(2, report.FirstYearCount);
        Assert.Equal(0, report.FromOtherCitiesCount);
        Assert.Equal(0.0, report.Percent, 6);
        Assert.Empty(report.Students);
    }

    [Fact]
    public void GetFirstYearFromOtherCities_NoFirstYearStudents_ReturnsZeroWithoutDivisionByZero()
    {
       
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity("KB223460", course: 2, city: "Львів")));

   
        FirstYearReport report = service.GetFirstYearFromOtherCities("Київ");

     
        Assert.Equal(0, report.FirstYearCount);
        Assert.Equal(0.0, report.Percent, 6);
    }

    [Fact]
    public void GetFirstYearFromOtherCities_InvalidUniversityCity_Throws()
    {
     
        StudentService service = new StudentService(new StubRepository<StudentEntity>());
        Action act = () => service.GetFirstYearFromOtherCities("");

      
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Fact]
    public void GetFirstYearFromOtherCities_ReadsStorageOnceAndDoesNotModifyIt()
    {
     
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.MixedStudents());
        StudentService service = new StudentService(repository);

       
        service.GetFirstYearFromOtherCities("Київ");

       
        Assert.Equal(1, repository.LoadAllCallCount);
        Assert.Equal(0, repository.SaveAllCallCount);
    }
}
