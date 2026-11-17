using Lab5.BLL.Services;
using Lab5.DAL;
using Lab5.Tests.Doubles;

namespace Lab5.Tests.Integration;


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
      
        Action act = () => new JsonFileRepository<StudentEntity>("  ");

        
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void LoadAll_FileDoesNotExist_ReturnsEmptyList()
    {
       
        JsonFileRepository<StudentEntity> repository = new JsonFileRepository<StudentEntity>(FilePath());

      
        IReadOnlyList<StudentEntity> result = repository.LoadAll();

      
        Assert.Empty(result);
    }

    [Fact]
    public void SaveAll_ThenLoadAll_RoundTripsEntities()
    {
       
        JsonFileRepository<StudentEntity> repository = new JsonFileRepository<StudentEntity>(FilePath());
        StudentEntity entity = TestData.NewStudentEntity("KB123456", "Коваленко", 1, "Львів", "3.215", 2);

       
        repository.SaveAll(new[] { entity });
        IReadOnlyList<StudentEntity> loaded = repository.LoadAll();

       
        Assert.Single(loaded);
        Assert.Equal("Коваленко", loaded[0].LastName);
        Assert.Equal("Львів", loaded[0].ArrivalCity);
        Assert.Equal("3.215", loaded[0].DormitoryRoom);
        Assert.Equal(2, loaded[0].Dances);
    }

    [Fact]
    public void LoadAll_CorruptedFile_ThrowsDataAccessException()
    {
       
        Directory.CreateDirectory(_folder);
        File.WriteAllText(FilePath(), "це не JSON");
        JsonFileRepository<StudentEntity> repository = new JsonFileRepository<StudentEntity>(FilePath());
        Action act = () => repository.LoadAll();

       
        Assert.Throws<DataAccessException>(act);
    }

    [Fact]
    public void LoadAll_FileContainsJsonNull_ReturnsEmptyList()
    {
        
        Directory.CreateDirectory(_folder);
        File.WriteAllText(FilePath(), "null");
        JsonFileRepository<StudentEntity> repository = new JsonFileRepository<StudentEntity>(FilePath());

       
        IReadOnlyList<StudentEntity> result = repository.LoadAll();

       
        Assert.Empty(result);
    }

    [Fact]
    public void SaveAll_PathIsExistingDirectory_ThrowsDataAccessException()
    {
       
        Directory.CreateDirectory(FilePath("occupied"));
        JsonFileRepository<StudentEntity> repository = new JsonFileRepository<StudentEntity>(FilePath("occupied"));
        Action act = () => repository.SaveAll(new List<StudentEntity>());

       
        Assert.Throws<DataAccessException>(act);
    }

    [Fact]
    public void StudentService_WithRealRepository_KeepsChangesBetweenInstances()
    {
        
        string path = FilePath();
        new StudentService(new JsonFileRepository<StudentEntity>(path))
            .Add(new Lab5.BLL.Models.Student("Коваленко", "Олена", 1, "KB123456", "Львів", "AB123456"));

       
        StudentService secondRun = new StudentService(new JsonFileRepository<StudentEntity>(path));
        secondRun.MoveIntoDormitory("KB123456", "Київ", 3, 215);
        Lab5.BLL.Models.Student restored = new StudentService(new JsonFileRepository<StudentEntity>(path)).GetByKey("KB123456");

        
        Assert.Equal("3.215", restored.DormitoryRoom);
    }
}
