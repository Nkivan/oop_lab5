using Lab5.BLL.Exceptions;
using Lab5.BLL.Models;
using Lab5.BLL.Services;
using Lab5.BLL.Validation;
using Lab5.DAL;

namespace Lab5.PL.Menus;

internal sealed record MenuItem(string Title, Action Action);

internal abstract class EntityMenu<TModel, TEntity>
    where TModel : Person
    where TEntity : PersonEntity
{
    protected PersonService<TModel, TEntity> Service { get; }

    protected EntityMenu(PersonService<TModel, TEntity> service)
    {
        Service = service;
    }

    protected abstract string Title { get; }
    protected abstract TModel ReadModel();
    protected abstract string Describe(TModel model);
    protected abstract TModel[] Samples();
    protected virtual MenuItem[] ExtraItems() => Array.Empty<MenuItem>();

    public void Run()
    {
        MenuItem[] items = BuildItems();
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"===== {Title} =====");
            for (int i = 0; i < items.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {items[i].Title}");
            }
            Console.WriteLine("0. Назад");

            string choice = ConsoleIO.ReadLine("Ваш вибір: ");
            if (choice == "0")
            {
                return;
            }
            if (!int.TryParse(choice, out int number) || number < 1 || number > items.Length)
            {
                Console.WriteLine("Невідома команда.");
                continue;
            }

            try
            {
                items[number - 1].Action();
            }
            catch (BllException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            catch (DataAccessException ex)
            {
                Console.WriteLine($"Помилка доступу до даних: {ex.Message}");
            }
        }
    }

    private MenuItem[] BuildItems()
    {
        MenuItem[] common =
        {
            new MenuItem("Показати всіх", ShowAll),
            new MenuItem("Додати", Add),
            new MenuItem("Видалити за ключем", Remove),
            new MenuItem("Знайти за прізвищем", FindByLastName),
            new MenuItem("Виконати дію: танцювати", Dance),
            new MenuItem("Отримати водійське посвідчення", ObtainDriverLicense),
            new MenuItem("Додати тестові дані", AddSamples)
        };
        return common.Concat(ExtraItems()).ToArray();
    }

    // ---------- команди ----------

    protected void ShowAll()
    {
        IReadOnlyList<TModel> all = Service.GetAll();
        if (all.Count == 0)
        {
            Console.WriteLine("База порожня.");
            return;
        }
        foreach (TModel model in all)
        {
            Console.WriteLine(Describe(model));
        }
    }

    private void Add()
    {
        Service.Add(ReadModel());
        Console.WriteLine("Додано.");
    }

    private void Remove()
    {
        Service.Remove(ConsoleIO.ReadLine("Ключ запису: "));
        Console.WriteLine("Видалено.");
    }

    private void FindByLastName()
    {
        IReadOnlyList<TModel> found = Service.FindByLastName(ConsoleIO.ReadLine("Прізвище: "));
        if (found.Count == 0)
        {
            Console.WriteLine("Нічого не знайдено.");
            return;
        }
        foreach (TModel model in found)
        {
            Console.WriteLine(Describe(model));
        }
    }

    private void Dance()
    {
        string key = ConsoleIO.ReadLine("Ключ запису: ");
        Service.Dance(key);
        Console.WriteLine("Виконано. Новий стан:");
        Console.WriteLine(Describe(Service.GetByKey(key)));
    }

    private void ObtainDriverLicense()
    {
        string key = ConsoleIO.ReadLine("Ключ запису: ");
        DrivingExamResults results = new DrivingExamResults(
            ConsoleIO.ReadInt("Вік: "),
            ConsoleIO.ReadYesNo("Є медична довідка?"),
            ConsoleIO.ReadYesNo("Теоретичний іспит складено?"),
            ConsoleIO.ReadYesNo("Практичний іспит складено?"));
        string number = ConsoleIO.ReadValid("Номер посвідчення", Validator.DriverLicensePattern, "3 великі латинські літери + 6 цифр, напр. ABC123456");

        Service.ObtainDriverLicense(key, results, number);
        Console.WriteLine("Посвідчення отримано.");
        Console.WriteLine(Describe(Service.GetByKey(key)));
    }

    private void AddSamples()
    {
        int added = 0;
        int skipped = 0;
        foreach (TModel model in Samples())
        {
            try
            {
                Service.Add(model);
                added++;
            }
            catch (DuplicateEntityException)
            {
                skipped++;
            }
        }
        Console.WriteLine($"Додано: {added}, пропущено (вже є): {skipped}.");
    }
}
