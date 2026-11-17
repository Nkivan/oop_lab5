namespace Lab5.BLL.Exceptions;

/// <summary>Базовий виняток бізнес-логіки.</summary>
public class BllException : Exception
{
    public BllException(string message) : base(message) { }
}

/// <summary>Дані не відповідають вимогам предметної області.</summary>
public class InvalidEntityDataException : BllException
{
    public InvalidEntityDataException(string message) : base(message) { }
}

/// <summary>Запис із вказаним ключем не знайдено.</summary>
public class EntityNotFoundException : BllException
{
    public EntityNotFoundException(string key) : base($"Запис із ключем '{key}' не знайдено.") { }
}

/// <summary>Запис із таким ключем уже існує.</summary>
public class DuplicateEntityException : BllException
{
    public DuplicateEntityException(string key) : base($"Запис із ключем '{key}' уже існує.") { }
}

/// <summary>Поселення в гуртожиток неможливе.</summary>
public class DormitoryException : BllException
{
    public DormitoryException(string message) : base(message) { }
}

/// <summary>Отримання водійського посвідчення неможливе.</summary>
public class DriverLicenseException : BllException
{
    public DriverLicenseException(string message) : base(message) { }
}
