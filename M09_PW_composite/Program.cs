using System;
using System.Collections.Generic;
using System.Linq;

namespace c;

public abstract class OrganizationComponent
{
    public string Name { get; protected set; }

    public abstract void Display(int depth);
    public abstract decimal GetBudget();
    public abstract int GetStaffCount();
    public abstract void ChangeSalary(string employeeName, decimal newSalary);
    public abstract OrganizationComponent SearchEmployee(string employeeName);
    public abstract List<string> GetAllEmployees();
}

public class Employee : OrganizationComponent
{
    public string Position { get; private set; }
    public decimal Salary { get; private set; }

    public Employee(string name, string position, decimal salary)
    {
        Name = name;
        Position = position;
        Salary = salary;
    }

    public override void Display(int depth)
    {
        Console.WriteLine(new string('-', depth) + $" Военнослужащий: {Name}, {Position} (Денежное довольствие: {Salary:C})");
    }

    public override decimal GetBudget() => Salary;
    public override int GetStaffCount() => 1;

    public override void ChangeSalary(string employeeName, decimal newSalary)
    {
        if (Name == employeeName) Salary = newSalary;
    }

    public override OrganizationComponent SearchEmployee(string employeeName)
    {
        return Name == employeeName ? this : null;
    }

    public override List<string> GetAllEmployees() => new List<string> { Name };
}

public class Contractor : OrganizationComponent
{
    public string Position { get; private set; }
    public decimal FixedPay { get; private set; }

    public Contractor(string name, string position, decimal fixedPay)
    {
        Name = name;
        Position = position;
        FixedPay = fixedPay;
    }

    public override void Display(int depth)
    {
        Console.WriteLine(new string('-', depth) + $" Гражданский специалист: {Name}, {Position} (Фикс. оплата: {FixedPay:C} - вне бюджета)");
    }

    public override decimal GetBudget() => 0;
    public override int GetStaffCount() => 1;

    public override void ChangeSalary(string employeeName, decimal newSalary)
    {
        if (Name == employeeName) FixedPay = newSalary;
    }

    public override OrganizationComponent SearchEmployee(string employeeName)
    {
        return Name == employeeName ? this : null;
    }

    public override List<string> GetAllEmployees() => new List<string> { Name + " (Гражданский)" };
}

public class Department : OrganizationComponent
{
    private List<OrganizationComponent> _components = new List<OrganizationComponent>();

    public Department(string name)
    {
        Name = name;
    }

    public void Add(OrganizationComponent component) => _components.Add(component);
    public void Remove(OrganizationComponent component) => _components.Remove(component);

    public override void Display(int depth)
    {
        Console.WriteLine(new string('-', depth) + $" Подразделение: {Name} (Бюджет: {GetBudget():C}, Личный состав: {GetStaffCount()})");
        foreach (var component in _components)
        {
            component.Display(depth + 2);
        }
    }

    public override decimal GetBudget() => _components.Sum(c => c.GetBudget());
    public override int GetStaffCount() => _components.Sum(c => c.GetStaffCount());

    public override void ChangeSalary(string employeeName, decimal newSalary)
    {
        foreach (var component in _components)
        {
            component.ChangeSalary(employeeName, newSalary);
        }
    }

    public override OrganizationComponent SearchEmployee(string employeeName)
    {
        foreach (var component in _components)
        {
            var found = component.SearchEmployee(employeeName);
            if (found != null) return found;
        }
        return null;
    }

    public override List<string> GetAllEmployees()
    {
        var all = new List<string>();
        foreach (var component in _components)
        {
            all.AddRange(component.GetAllEmployees());
        }
        return all;
    }
}

public class CorporateClient
{
    public static void Main()
    {
        Department command = new Department("Главное командование");
        Department infantry = new Department("Пехотный батальон");
        Department logistics = new Department("Тыловое обеспечение");

        Employee emp1 = new Employee("Бекзат Сембаев", "Командир взвода", 150000);
        Employee emp2 = new Employee("Кирилл Метельников", "Ефрейтор", 1000);
        Employee emp3 = new Employee("Тамерлан Негодин", "Гражданский", 1);
        Contractor cont1 = new Contractor("Тобиас Фокс", "Игродел-композитор", 999);

        infantry.Add(emp1);
        infantry.Add(emp2);
        infantry.Add(cont1);
        logistics.Add(emp3);

        command.Add(infantry);
        command.Add(logistics);

        Console.WriteLine("\n--- Структура военной организации ---");
        command.Display(1);

        Console.WriteLine("\n--- Поиск военнослужащего ---");
        var found = command.SearchEmployee("Бекзат Сембаев");
        found?.Display(1);

        Console.WriteLine("\n--- Изменение довольствия Тобиаса Фокса ---");
        command.ChangeSalary("Тобиас Фокс", 666);
        command.Display(1);

        Console.WriteLine("\n--- Личный состав пехотного батальона ---");
        var itStaff = infantry.GetAllEmployees();
        foreach (var name in itStaff)
        {
            Console.WriteLine($"- {name}");
        }
    }
}