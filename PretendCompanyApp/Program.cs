using System;
using System.Collections.Immutable;
using System.Linq;
using TCPData;
using TCPExtensions;
namespace PretendCompanyApp;
public static class Program
{
    public static void Main(string[] args)
    {
        RunExample3();
    }
    private static void RunExample1()
    {
        IList<Employee> employees = Data.GetEmployees();
        var filteredEmployees = employees.Filter(e => e.IsManager == true);
        filteredEmployees.ForEach(e => Console.WriteLine(e.ToString()));

        Console.WriteLine("-------------------------------------------------");

        string[] DepartmentShortNames = ["HR", "IT"];
        List<Department> departments = Data.GetDepartments();
        var filteredDepartments = departments.Filter(d => DepartmentShortNames.Contains(d.ShortName));

        filteredDepartments.ForEach(d => Console.WriteLine(d.ToString()));


        Console.WriteLine("-------------------------------------------------");
        // query syntax
        var linq = from emp in employees
                   join dept in departments on emp.DepartmentId equals dept.Id
                   where emp.IsManager == true
                   select new { emp.FirstName, emp.LastName, emp.Position, Salary = emp.AnnualSalary, DepartmentName = dept.LongName };
        linq.ToImmutableList().ForEach(e => Console.WriteLine($"Name: {e.FirstName} {e.LastName}, Position: {e.Position}, Salary: {e.Salary}, Department: {e.DepartmentName}"));

        //fluent syntax
        var query = employees
            .Where(emp => emp.IsManager && emp.AnnualSalary > 50_000m)
            .Join(departments,
                  emp => emp.DepartmentId,
                  dept => dept.Id,
                  (emp, dept) => new
                  {
                      emp.FirstName,
                      emp.LastName,
                      emp.Position,
                      Salary = emp.AnnualSalary,
                      DepartmentName = dept.LongName
                  });

        query.ToList().ForEach(e => Console.WriteLine($"Name: {e.FirstName} {e.LastName}, Position: {e.Position}, Salary: {e.Salary}, Department: {e.DepartmentName}"));

    }

    private static void RunExample2()
    {
        List<Employee> employees = Data.GetEmployees();
        List<Department> departments = Data.GetDepartments();

        // fluent syntax
        var result = employees.Where(e => e.AnnualSalary >= 50_000m).Select(e => new
        {
            FullName = e.FirstName + " " + e.LastName,
            e.Position,
            e.AnnualSalary,
        });
        result.ToList().ForEach(e => Console.WriteLine($"Name: {e.FullName}, Position: {e.Position}, Salary: {e.AnnualSalary,2}"));
        Console.WriteLine("-------------------------------------------------");

        // query syntax
        var query =
        from emp in employees
        where emp.AnnualSalary >= 50_000m
        select new
        {
            FullName = $"{emp.FirstName} {emp.LastName}",
            emp.Position,
            emp.AnnualSalary,
        };

        query.ToList().ForEach(e => Console.WriteLine($"Name: {e.FullName}, Position: {e.Position}, Salary: {e.AnnualSalary,2}"));

    }
    private static void RunExample3()
    {
        // deferred execution of linq 
        List<Employee> employees = Data.GetEmployees();
        var highSalaryEmployees = from employee in employees.GetHighSalaryEmployees(50_000m) select new { FullName = $"{employee.FirstName} {employee.LastName}", employee.AnnualSalary };
        employees.Add(new Employee() { FirstName = "John", LastName = "Doe", AnnualSalary = 60_000m, Position = "Manager", IsManager = true, DepartmentId = 1 });
        highSalaryEmployees.ToList().ForEach(e => Console.WriteLine($"Name: {e.FullName}, Salary: {e.AnnualSalary}"));

        // non deferred execution
        List<Employee> employees2 = Data.GetEmployees();
        var highSalaryEmployees2 = (from employee in employees2.GetHighSalaryEmployees(50_000m) select new { FullName = $"{employee.FirstName} {employee.LastName}", employee.AnnualSalary }).ToList();
        employees2.Add(new Employee() { FirstName = "Jane", LastName = "Smith", AnnualSalary = 70_000m, Position = "Manager", IsManager = true, DepartmentId = 2 });
        highSalaryEmployees2.ForEach(e => Console.WriteLine($"Name: {e.FullName}, Salary: {e.AnnualSalary}"));
        // john doe is not added here
    }
}

public static class EnumerableExtensions
{
    public static IEnumerable<Employee> GetHighSalaryEmployees(this IEnumerable<Employee> employees, decimal minSalary = 0)
    {
        foreach (var employee in employees)
        {
            System.Console.WriteLine($"Checking employee: {employee.FirstName} {employee.LastName}, Salary: {employee.AnnualSalary,20}");
            // Check if the employee's salary is greater than or equal to the minimum salary
            if (employee.AnnualSalary >= minSalary)
            {
                yield return employee;
            }
        }
    }
}

