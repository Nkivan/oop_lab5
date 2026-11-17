using Lab5.BLL.Services;
using Lab5.DAL;
using Lab5.Tests.Doubles;

namespace Lab5.Tests.Integration;

/// <summary>
/// ІНТЕГРАЦІЙНІ тести: на відміну від модульних, працюють зі справжнім файлом на диску
/// (тому повільніші й залежать від середовища). Модульні тести бізнес-логіки файлів не торкаються.
/// </summary>
public class JsonFileRepositoryIntegrationTests : IDisposable
{
    private readonly string _folder;

    public JsonFileRepositoryIntegrationTests()
    {
        _folder = Path.Combine(Path.GetTempPath(), "lab5_tests_" + Guid.NewGuid().ToString("N"));
    }

    public void Dispose()
    {
        if (Directory.Exists(_folder))
        {
            Directory.Delete(_folder, true);
        }
    }

    private string FilePath(string name = "students.json") => Path.Combine(_folder, name);

    [Fact]
    public void Constructor_EmptyPath_Throws()
    {
        // Arrange
        Action act = () => new JsonFileRepository<StudentEntity>("  ");

        // Act + Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void LoadAll_FileDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        JsonFileRepository<StudentEntity> repository = new JsonFileRepository<StudentEntity>(FilePath());

        // Act
        IReadOnlyList<StudentEntity> result = repository.LoadAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SaveAll_ThenLoadAll_RoundTripsEntities()
    {
        // Arrange
        JsonFileRepository<StudentEntity> repository = new JsonFileRepository<StudentEntity>(FilePath());
        StudentEntity entity = TestData.NewStudentEntity("KB123456", "Коваленко", 1, "Львів", "3.215", 2);

        // Act
        repository.SaveAll(new[] { entity });
        IReadOnlyList<StudentEntity> loaded = repository.LoadAll();

        // Assert
        Assert.Single(loaded);
        Assert.Equal("Коваленко", loaded[0].LastName);
        Assert.Equal("Львів", loaded[0].ArrivalCity);
        Assert.Equal("3.215", loaded[0].DormitoryRoom);
        Assert.Equal(2, loaded[0].Dances);
    }

    [Fact]
    public void LoadAll_CorruptedFile_ThrowsDataAccessException()
    {
        // Arrange
        Directory.CreateDirectory(_folder);
        File.WriteAllText(FilePath(), "це не JSON");
        JsonFileRepository<StudentEntity> repository = new JsonFileRepository<StudentEntity>(FilePath());
        Action act = () => repository.LoadAll();

        // Act + Assert
        Assert.Throws<DataAccessException>(act);
    }

    [Fact]
    public void LoadAll_FileContainsJsonNull_ReturnsEmptyList()
    {
        // Arrange
        Directory.CreateDirectory(_folder);
        File.WriteAllText(FilePath(), "null");
        JsonFileRepository<StudentEntity> repository = new JsonFileRepository<StudentEntity>(FilePath());

        // Act
        IReadOnlyList<StudentEntity> result = repository.LoadAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SaveAll_PathIsExistingDirectory_ThrowsDataAccessException()
    {
        // Arrange
        Directory.CreateDirectory(FilePath("occupied"));
        JsonFileRepository<StudentEntity> repository = new JsonFileRepository<StudentEntity>(FilePath("occupied"));
        Action act = () => repository.SaveAll(new List<StudentEntity>());

        // Act + Assert
        Assert.Throws<DataAccessException>(act);
    }

    [Fact]
    public void StudentService_WithRealRepository_KeepsChangesBetweenInstances()
    {
        // Arrange
        string path = FilePath();
        new StudentService(new JsonFileRepository<StudentEntity>(path))
            .Add(new Lab5.BLL.Models.Student("Коваленко", "Олена", 1, "KB123456", "Львів", "AB123456"));

        // Act: «перезапуск застосунку» - новий сервіс читає той самий файл
        StudentService secondRun = new StudentService(new JsonFileRepository<StudentEntity>(path));
        secondRun.MoveIntoDormitory("KB123456", "Київ", 3, 215);
        Lab5.BLL.Models.Student restored = new StudentService(new JsonFileRepository<StudentEntity>(path)).GetByKey("KB123456");

        // Assert
        Assert.Equal("3.215", restored.DormitoryRoom);
    }
}
