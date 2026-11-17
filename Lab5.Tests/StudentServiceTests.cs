using Lab5.BLL.Exceptions;
using Lab5.BLL.Mapping;
using Lab5.BLL.Models;
using Lab5.BLL.Services;
using Lab5.DAL;
using Lab5.Tests.Doubles;

namespace Lab5.Tests;

/// <summary>
/// Тести сервісу бізнес-логіки. Справжній файловий репозиторій замінено заглушкою (Stub)
/// або мок-об'єктом (Mock): це можливо завдяки ін'єкції залежностей через конструктор.
/// </summary>
public class StudentServiceTests
{
    // ---------- конструктор ----------

    [Fact]
    public void Constructor_NullRepository_ThrowsArgumentNullException()
    {
        // Arrange
        Action act = () => new StudentService(null);

        // Act + Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void Constructor_NullMapper_ThrowsArgumentNullException()
    {
        // Arrange
        IRepository<StudentEntity> repository = new StubRepository<StudentEntity>();
        Action act = () => new PersonService<Student, StudentEntity>(repository, null);

        // Act + Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    // ---------- GetAll ----------

    [Fact]
    public void GetAll_RepositoryHasEntities_ReturnsMappedModels()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity("KB123456"), TestData.NewStudentEntity("KB123457")));

        // Act
        IReadOnlyList<Student> students = service.GetAll();

        // Assert
        Assert.Equal(2, students.Count);
        Assert.Equal("KB123456", students[0].Key);
        Assert.Equal("KB123457", students[1].Key);
    }

    [Fact]
    public void GetAll_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>());

        // Act
        IReadOnlyList<Student> students = service.GetAll();

        // Assert
        Assert.Empty(students);
    }

    [Fact]
    public void GetAll_StorageFails_PropagatesDataAccessException()
    {
        // Arrange
        StudentService service = new StudentService(new FailingRepositoryStub<StudentEntity>());
        Action act = () => service.GetAll();

        // Act + Assert
        Assert.Throws<DataAccessException>(act);
    }

    [Fact]
    public void GetAll_InvalidDataInStorage_ThrowsInvalidEntityData()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity(course: 9)));
        Action act = () => service.GetAll();

        // Act + Assert
        Assert.Throws<InvalidEntityDataException>(act);
    }

    // ---------- Add ----------

    [Fact]
    public void Add_NewStudent_SavesExistingAndNewOnce()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        Student newStudent = new Student("Мельник", "Андрій", 2, "KB123457", "Одеса", "AB123457");

        // Act
        service.Add(newStudent);

        // Assert
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal(2, repository.LastSaved.Count);
        Assert.Equal("KB123457", repository.LastSaved[1].StudentId);
    }

    [Fact]
    public void Add_NullModel_ThrowsArgumentNullException()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>();
        StudentService service = new StudentService(repository);
        Action act = () => service.Add(null);

        // Act
        Assert.Throws<ArgumentNullException>(act);

        // Assert
        Assert.Equal(0, repository.SaveAllCallCount);
    }

    [Fact]
    public void Add_DuplicateKey_ThrowsAndDoesNotSave()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        Student duplicate = new Student("Інший", "Студент", 1, "KB123456", "Львів", "AB999999");
        Action act = () => service.Add(duplicate);

        // Act
        DuplicateEntityException exception = Assert.Throws<DuplicateEntityException>(act);

        // Assert
        Assert.Contains("KB123456", exception.Message);
        Assert.Equal(0, repository.SaveAllCallCount);
    }

    // ---------- Remove ----------

    [Fact]
    public void Remove_ExistingKey_SavesWithoutRemovedStudent()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(
            TestData.NewStudentEntity("KB123456"), TestData.NewStudentEntity("KB123457"));
        StudentService service = new StudentService(repository);

        // Act
        service.Remove("KB123456");

        // Assert
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Single(repository.LastSaved);
        Assert.Equal("KB123457", repository.LastSaved[0].StudentId);
    }

    [Fact]
    public void Remove_UnknownKey_ThrowsNotFoundAndDoesNotSave()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        Action act = () => service.Remove("KB000000");

        // Act
        Assert.Throws<EntityNotFoundException>(act);

        // Assert
        Assert.Equal(0, repository.SaveAllCallCount);
    }

    // ---------- GetByKey / FindByLastName ----------

    [Fact]
    public void GetByKey_KeyInDifferentCase_ReturnsStudent()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>(TestData.NewStudentEntity("KB123456")));

        // Act
        Student student = service.GetByKey("kb123456");

        // Assert
        Assert.Equal("KB123456", student.StudentId);
    }

    [Fact]
    public void GetByKey_UnknownKey_ThrowsNotFound()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>(TestData.NewStudentEntity("KB123456")));
        Action act = () => service.GetByKey("KB000000");

        // Act + Assert
        Assert.Throws<EntityNotFoundException>(act);
    }

    [Fact]
    public void FindByLastName_DifferentCase_ReturnsAllMatches()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity("KB123456", "Коваленко"),
            TestData.NewStudentEntity("KB123457", "Мельник"),
            TestData.NewStudentEntity("KB123458", "Коваленко")));

        // Act
        IReadOnlyList<Student> found = service.FindByLastName("КОВАЛЕНКО");

        // Assert
        Assert.Equal(2, found.Count);
    }

    [Fact]
    public void FindByLastName_NoMatches_ReturnsEmptyList()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>(TestData.NewStudentEntity()));

        // Act
        IReadOnlyList<Student> found = service.FindByLastName("Невідомий");

        // Assert
        Assert.Empty(found);
    }

    // ---------- вміння ----------

    [Fact]
    public void Dance_ExistingKey_IncrementsDancesAndSaves()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456", dances: 4));
        StudentService service = new StudentService(repository);

        // Act
        service.Dance("KB123456");

        // Assert
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal(5, repository.LastSaved[0].Dances);
    }

    [Fact]
    public void Dance_UnknownKey_ThrowsNotFoundAndDoesNotSave()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        Action act = () => service.Dance("KB000000");

        // Act
        Assert.Throws<EntityNotFoundException>(act);

        // Assert
        Assert.Equal(0, repository.SaveAllCallCount);
    }

    [Fact]
    public void ObtainDriverLicense_ConditionsMet_SavesLicense()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        DrivingExamResults results = new DrivingExamResults(19, true, true, true);

        // Act
        service.ObtainDriverLicense("KB123456", results, "ABC123456");

        // Assert
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal("ABC123456", repository.LastSaved[0].DriverLicense);
    }

    [Fact]
    public void ObtainDriverLicense_ConditionsNotMet_ThrowsAndDoesNotSave()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456"));
        StudentService service = new StudentService(repository);
        DrivingExamResults results = new DrivingExamResults(16, true, true, true);
        Action act = () => service.ObtainDriverLicense("KB123456", results, "ABC123456");

        // Act
        Assert.Throws<DriverLicenseException>(act);

        // Assert
        Assert.Equal(0, repository.SaveAllCallCount);
    }

    // ---------- поселення в гуртожиток ----------

    [Fact]
    public void MoveIntoDormitory_StudentFromOtherCity_SavesRoom()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456", city: "Львів"));
        StudentService service = new StudentService(repository);

        // Act
        service.MoveIntoDormitory("KB123456", "Київ", 3, 215);

        // Assert
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal("3.215", repository.LastSaved[0].DormitoryRoom);
    }

    [Fact]
    public void MoveIntoDormitory_LocalStudent_ThrowsAndDoesNotSave()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.NewStudentEntity("KB123456", city: "Київ"));
        StudentService service = new StudentService(repository);
        Action act = () => service.MoveIntoDormitory("KB123456", "Київ", 3, 215);

        // Act
        Assert.Throws<DormitoryException>(act);

        // Assert
        Assert.Equal(0, repository.SaveAllCallCount);
    }

    [Fact]
    public void MoveIntoDormitory_UnknownKey_ThrowsNotFound()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>());
        Action act = () => service.MoveIntoDormitory("KB000000", "Київ", 3, 215);

        // Act + Assert
        Assert.Throws<EntityNotFoundException>(act);
    }

    // ---------- операція варіанта: відсоток студентів 1-го курсу з інших міст ----------

    [Fact]
    public void GetFirstYearFromOtherCities_MixedStudents_ReturnsPercentAndStudents()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>(TestData.MixedStudents()));

        // Act
        FirstYearReport report = service.GetFirstYearFromOtherCities("Київ");

        // Assert: серед 4 першокурсників троє приїхали не з Києва
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
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity("KB123456", city: "Львів"),
            TestData.NewStudentEntity("KB123457", city: "Харків")));

        // Act
        FirstYearReport report = service.GetFirstYearFromOtherCities("Київ");

        // Assert
        Assert.Equal(100.0, report.Percent, 6);
    }

    [Fact]
    public void GetFirstYearFromOtherCities_NoneFromOtherCities_ReturnsZeroPercent()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity("KB123456", city: "Київ"),
            TestData.NewStudentEntity("KB123457", city: "київ")));

        // Act (місто порівнюється без урахування регістру)
        FirstYearReport report = service.GetFirstYearFromOtherCities("Київ");

        // Assert
        Assert.Equal(2, report.FirstYearCount);
        Assert.Equal(0, report.FromOtherCitiesCount);
        Assert.Equal(0.0, report.Percent, 6);
        Assert.Empty(report.Students);
    }

    [Fact]
    public void GetFirstYearFromOtherCities_NoFirstYearStudents_ReturnsZeroWithoutDivisionByZero()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>(
            TestData.NewStudentEntity("KB223460", course: 2, city: "Львів")));

        // Act
        FirstYearReport report = service.GetFirstYearFromOtherCities("Київ");

        // Assert
        Assert.Equal(0, report.FirstYearCount);
        Assert.Equal(0.0, report.Percent, 6);
    }

    [Fact]
    public void GetFirstYearFromOtherCities_InvalidUniversityCity_Throws()
    {
        // Arrange
        StudentService service = new StudentService(new StubRepository<StudentEntity>());
        Action act = () => service.GetFirstYearFromOtherCities("");

        // Act + Assert
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Fact]
    public void GetFirstYearFromOtherCities_ReadsStorageOnceAndDoesNotModifyIt()
    {
        // Arrange
        MockRepository<StudentEntity> repository = new MockRepository<StudentEntity>(TestData.MixedStudents());
        StudentService service = new StudentService(repository);

        // Act
        service.GetFirstYearFromOtherCities("Київ");

        // Assert (перевірка взаємодії через мок)
        Assert.Equal(1, repository.LoadAllCallCount);
        Assert.Equal(0, repository.SaveAllCallCount);
    }
}
