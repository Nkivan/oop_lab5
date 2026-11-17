namespace Lab5.BLL.Exceptions;

public class BllException : Exception
{
    public BllException(string message) : base(message) { }
}

public class InvalidEntityDataException : BllException
{
    public InvalidEntityDataException(string message) : base(message) { }
}

public class EntityNotFoundException : BllException
{
    public EntityNotFoundException(string key) : base($"Запис із ключем '{key}' не знайдено.") { }
}

public class DuplicateEntityException : BllException
{
    public DuplicateEntityException(string key) : base($"Запис із ключем '{key}' уже існує.") { }
}

public class DormitoryException : BllException
{
    public DormitoryException(string message) : base(message) { }
}

public class DriverLicenseException : BllException
{
    public DriverLicenseException(string message) : base(message) { }
}
