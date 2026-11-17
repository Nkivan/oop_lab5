using Lab5.BLL.Exceptions;
using Lab5.BLL.Validation;

namespace Lab5.Tests;

public class ValidatorTests
{
    [Fact]
    public void IsValid_NullValue_ReturnsFalse()
    {
        // Arrange
        string value = null;

        // Act
        bool result = Validator.IsValid(Validator.NamePattern, value);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("Іван")]
    [InlineData("O'Brien")]
    [InlineData("Анна-Марія")]
    public void IsValid_CorrectName_ReturnsTrue(string name)
    {
        // Arrange (дані передає InlineData)

        // Act
        bool result = Validator.IsValid(Validator.NamePattern, name);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("А")]
    [InlineData("Ivan3")]
    [InlineData("Іван Петрович")]
    public void IsValid_IncorrectName_ReturnsFalse(string name)
    {
        // Arrange (дані передає InlineData)

        // Act
        bool result = Validator.IsValid(Validator.NamePattern, name);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Require_ValidValue_DoesNotThrow()
    {
        // Arrange
        string value = "KB123456";

        // Act
        Exception exception = Record.Exception(() => Validator.Require(Validator.StudentIdPattern, value, "Студентський квиток"));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Require_InvalidValue_ThrowsAndMessageContainsFieldName()
    {
        // Arrange
        Action act = () => Validator.Require(Validator.StudentIdPattern, "bad", "Студентський квиток");

        // Act
        InvalidEntityDataException exception = Assert.Throws<InvalidEntityDataException>(act);

        // Assert
        Assert.Contains("Студентський квиток", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(5)]
    public void RequireRange_ValueInsideRange_DoesNotThrow(int value)
    {
        // Arrange (дані передає InlineData)

        // Act
        Exception exception = Record.Exception(() => Validator.RequireRange(value, 0, 5, "Поле"));

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public void RequireRange_ValueOutsideRange_Throws(int value)
    {
        // Arrange
        Action act = () => Validator.RequireRange(value, 0, 5, "Поле");

        // Act + Assert
        Assert.Throws<InvalidEntityDataException>(act);
    }
}
