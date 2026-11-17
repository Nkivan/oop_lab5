namespace Lab5.DAL;

public abstract class PersonEntity
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int Dances { get; set; }
    public string DriverLicense { get; set; }
}

public class StudentEntity : PersonEntity
{
    public int Course { get; set; }
    public string StudentId { get; set; } = "";
    public string ArrivalCity { get; set; } = "";
    public string Passport { get; set; } = "";
    public string DormitoryRoom { get; set; }
}

public class TaxiDriverEntity : PersonEntity
{
    public int Trips { get; set; }
}

public class AcrobatEntity : PersonEntity
{
    public string CertificateId { get; set; } = "";
    public int ExperienceYears { get; set; }
}
