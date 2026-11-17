using Lab5.BLL.Exceptions;
using Lab5.BLL.Validation;

namespace Lab5.Tests;

public class ValidatorTests
{
    [Fact]
    public void IsValid_NullValue_ReturnsFalse()
    {
     
        string value = null;

      
        bool result = Validator.IsValid(Validator.NamePattern, value);

     
        Assert.False(result);
    }

    [Theory]
    [InlineData("Іван")]
    [InlineData("O'Brien")]
    [InlineData("Анна-Марія")]
    public void IsValid_CorrectName_ReturnsTrue(string name)
    {
   

      
        bool result = Validator.IsValid(Validator.NamePattern, name);

  
        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("А")]
    [InlineData("Ivan3")]
    [InlineData("Іван Петрович")]
    public void IsValid_IncorrectName_ReturnsFalse(string name)
    {
    

       
        bool result = Validator.IsValid(Validator.NamePattern, name);

       
        Assert.False(result);
    }

    [Fact]
    public void Require_ValidValue_DoesNotThrow()
    {
      
        string value = "KB123456";

  
        Exception exception = Record.Exception(() => Validator.Require(Validator.StudentIdPattern, value, "Студентський квиток"));

      
        Assert.Null(exception);
    }

    [Fact]
    public void Require_InvalidValue_ThrowsAndMessageContainsFieldName()
    {
       
        Action act = () => Validator.Require(Validator.StudentIdPattern, "bad", "Студентський квиток");

       
        InvalidEntityDataException exception = Assert.Throws<InvalidEntityDataException>(act);

  
        Assert.Contains("Студентський квиток", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(5)]
    public void RequireRange_ValueInsideRange_DoesNotThrow(int value)
    {
       
        Exception exception = Record.Exception(() => Validator.RequireRange(value, 0, 5, "Поле"));

        
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public void RequireRange_ValueOutsideRange_Throws(int value)
    {
     
        Action act = () => Validator.RequireRange(value, 0, 5, "Поле");

      
        Assert.Throws<InvalidEntityDataException>(act);
    }
}
