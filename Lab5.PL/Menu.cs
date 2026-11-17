using System.Text;
using Lab5.PL.Menus;

namespace Lab5.PL;

/// <summary>Головне меню застосунку (рівень представлення).</summary>
internal sealed class Menu
{
    private readonly StudentMenu _students;
    private readonly TaxiDriverMenu _taxiDrivers;
    private readonly AcrobatMenu _acrobats;

    public Menu(StudentMenu students, TaxiDriverMenu taxiDrivers, AcrobatMenu acrobats)
    {
        _students = students;
        _taxiDrivers = taxiDrivers;
        _acrobats = acrobats;
    }

    public void Run()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        try
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== ГОЛОВНЕ МЕНЮ =====");
                Console.WriteLine("1. Студенти (в т.ч. завдання варіанта)");
                Console.WriteLine("2. Таксисти");
                Console.WriteLine("3. Акробати");
                Console.WriteLine("0. Вихід");

                switch (ConsoleIO.ReadLine("Ваш вибір: "))
                {
                    case "1": _students.Run(); break;
                    case "2": _taxiDrivers.Run(); break;
                    case "3": _acrobats.Run(); break;
                    case "0": return;
                    default: Console.WriteLine("Невідома команда."); break;
                }
            }
        }
        catch (EndOfStreamException)
        {
            // Вхідний потік закінчився - завершуємо роботу.
        }
    }
}
