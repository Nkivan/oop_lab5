using Lab5.BLL.Services;
using Lab5.DAL;
using Lab5.PL.Menus;

namespace Lab5.PL;

internal static class Program
{

    private static void Main()
    {
        string folder = Path.Combine(AppContext.BaseDirectory, "data");

        StudentService students = new StudentService(new JsonFileRepository<StudentEntity>(Path.Combine(folder, "students.json")));
        TaxiDriverService taxiDrivers = new TaxiDriverService(new JsonFileRepository<TaxiDriverEntity>(Path.Combine(folder, "taxidrivers.json")));
        AcrobatService acrobats = new AcrobatService(new JsonFileRepository<AcrobatEntity>(Path.Combine(folder, "acrobats.json")));

        new Menu(new StudentMenu(students), new TaxiDriverMenu(taxiDrivers), new AcrobatMenu(acrobats)).Run();
    }
}
