using Lab5.BLL.Exceptions;
using Lab5.BLL.Models;

namespace Lab5.Tests;

public class StudentTests
{
 
    private static Student CreateStudent(
        string lastName = "Коваленко", string firstName = "Олена", int course = 1,
        string studentId = "KB123456", string city = "Львів", string passport = "AB123456",
        int dances = 0, string driverLicense = null, string dormitoryRoom = null)
        => new Student(lastName, firstName, course, studentId, city, passport, dances, driverLicense, dormitoryRoom);

  

    [Fact]
    public void Constructor_ValidData_SetsProperties()
    {
      
        Student student = CreateStudent();

      
        Assert.Equal("Коваленко", student.LastName);
        Assert.Equal("Олена", student.FirstName);
        Assert.Equal(1, student.Course);
        Assert.Equal("KB123456", student.StudentId);
        Assert.Equal("Львів", student.ArrivalCity);
        Assert.Equal("AB123456", student.Passport);
        Assert.Equal(0, student.Dances);
        Assert.Null(student.DriverLicense);
        Assert.Null(student.DormitoryRoom);
        Assert.False(student.LivesInDormitory);
        Assert.Equal("KB123456", student.Key);
    }

    [Fact]
    public void Constructor_RestoredState_SetsDancesLicenseAndDormitory()
    {
      
        Student student = CreateStudent(dances: 5, driverLicense: "ABC123456", dormitoryRoom: "3.215");

       
        Assert.Equal(5, student.Dances);
        Assert.Equal("ABC123456", student.DriverLicense);
        Assert.Equal("3.215", student.DormitoryRoom);
        Assert.True(student.LivesInDormitory);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    [InlineData(6)]
    public void Constructor_CourseFromOneToSix_Accepted(int course)
    {
      

     
        Student student = CreateStudent(course: course);

      
        Assert.Equal(course, student.Course);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    [InlineData(-1)]
    public void Constructor_CourseOutOfRange_Throws(int course)
    {
       
        Action act = () => CreateStudent(course: course);

       
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("А")]
    [InlineData("Ivan1")]
    public void Constructor_InvalidLastName_Throws(string lastName)
    {
       
        Action act = () => CreateStudent(lastName: lastName);

      
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("О")]
    [InlineData("Олена2")]
    public void Constructor_InvalidFirstName_Throws(string firstName)
    {
       
        Action act = () => CreateStudent(firstName: firstName);

       
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("kb123456")]
    [InlineData("KB12345")]
    [InlineData("KBA23456")]
    public void Constructor_InvalidStudentId_Throws(string studentId)
    {
     
        Action act = () => CreateStudent(studentId: studentId);

      
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("L")]
    [InlineData("Lviv1")]
    public void Constructor_InvalidArrivalCity_Throws(string city)
    {
       
        Action act = () => CreateStudent(city: city);

      
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("AB12345")]
    [InlineData("ab123456")]
    [InlineData("123456AB")]
    public void Constructor_InvalidPassport_Throws(string passport)
    {
        
        Action act = () => CreateStudent(passport: passport);

       
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Fact]
    public void Constructor_NegativeDances_Throws()
    {
     
        Action act = () => CreateStudent(dances: -1);

       
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Theory]
    [InlineData("abc123456")]
    [InlineData("AB123456")]
    public void Constructor_InvalidDriverLicense_Throws(string license)
    {
        
        Action act = () => CreateStudent(driverLicense: license);

       
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Theory]
    [InlineData("0.5")]
    [InlineData("3-215")]
    [InlineData("abc")]
    public void Constructor_InvalidDormitoryRoom_Throws(string room)
    {
       
        Action act = () => CreateStudent(dormitoryRoom: room);

      
        Assert.Throws<InvalidEntityDataException>(act);
    }

    

    [Fact]
    public void Dance_CalledTwice_IncrementsCounterTwice()
    {
       
        Student student = CreateStudent();

      
        student.Dance();
        student.Dance();

    
        Assert.Equal(2, student.Dances);
    }

   

    [Fact]
    public void MoveIntoDormitory_StudentFromOtherCity_AssignsRoom()
    {
      
        Student student = CreateStudent(city: "Львів");

     
        student.MoveIntoDormitory("Київ", 3, 215);

      
        Assert.True(student.LivesInDormitory);
        Assert.Equal("3.215", student.DormitoryRoom);
    }

    [Fact]
    public void MoveIntoDormitory_StudentFromUniversityCity_ThrowsAndStaysOutside()
    {
       
        Student student = CreateStudent(city: "Київ");
        Action act = () => student.MoveIntoDormitory("Київ", 3, 215);

       
        DormitoryException exception = Assert.Throws<DormitoryException>(act);

     
        Assert.Contains("з іншого міста", exception.Message);
        Assert.False(student.LivesInDormitory);
    }

    [Fact]
    public void MoveIntoDormitory_CityComparisonIgnoresCase_Throws()
    {
      
        Student student = CreateStudent(city: "київ");
        Action act = () => student.MoveIntoDormitory("КИЇВ", 3, 215);

     
        Assert.Throws<DormitoryException>(act);
    }

    [Fact]
    public void MoveIntoDormitory_AlreadyLivesInDormitory_ThrowsAndKeepsOldRoom()
    {
      
        Student student = CreateStudent(city: "Львів", dormitoryRoom: "2.101");
        Action act = () => student.MoveIntoDormitory("Київ", 3, 215);

       
        DormitoryException exception = Assert.Throws<DormitoryException>(act);

        
        Assert.Contains("уже проживає", exception.Message);
        Assert.Equal("2.101", student.DormitoryRoom);
    }

    [Fact]
    public void MoveIntoDormitory_InvalidUniversityCity_Throws()
    {
        
        Student student = CreateStudent(city: "Львів");
        Action act = () => student.MoveIntoDormitory("К", 3, 215);

      
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(5, 0)]
    [InlineData(-1, 5)]
    [InlineData(100, 5)]
    [InlineData(5, 1000)]
    public void MoveIntoDormitory_InvalidDormitoryOrRoom_ThrowsAndStaysOutside(int dormitory, int room)
    {
      
        Student student = CreateStudent(city: "Львів");
        Action act = () => student.MoveIntoDormitory("Київ", dormitory, room);

        
        Assert.Throws<InvalidEntityDataException>(act);

       
        Assert.False(student.LivesInDormitory);
    }
}
