using Lab5.BLL.Validation;

namespace Lab5.PL;

/// <summary>Читання з консолі та базове форматування (рівень представлення).</summary>
internal static class ConsoleIO
{
    public static string ReadLine(string prompt)
    {
        Console.Write(prompt);
        string line = Console.ReadLine();
        if (line == null)
        {
            throw new EndOfStreamException();
        }
        return line.Trim();
    }

    public static string ReadValid(string label, string pattern, string hint)
    {
        while (true)
        {
            string value = ReadLine($"{label} ({hint}): ");
            if (Validator.IsValid(pattern, value))
            {
                return value;
            }
            Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
        }
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            if (int.TryParse(ReadLine(prompt), out int value))
            {
                return value;
            }
            Console.WriteLine("  Введіть ціле число.");
        }
    }

    public static bool ReadYesNo(string prompt)
    {
        while (true)
        {
            string answer = ReadLine(prompt + " (так/ні): ").ToLowerInvariant();
            if (answer == "так" || answer == "т" || answer == "y")
            {
                return true;
            }
            if (answer == "ні" || answer == "н" || answer == "n")
            {
                return false;
            }
            Console.WriteLine("  Відповідайте «так» або «ні».");
        }
    }
}
