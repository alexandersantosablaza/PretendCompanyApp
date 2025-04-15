using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TCPData;
using TCPExtensions;
namespace PretendCompanyApp;
public static class Program
{
    private const string spacer = "-------------------------------------------------";

    public static void Main(string[] args)
    {
        RunExample6();
    }
    private static void RunExample1()
    {
        IList<Employee> employees = Data.GetEmployees();
        var filteredEmployees = employees.Filter(e => e.IsManager == true);
        filteredEmployees.ForEach(e => Console.WriteLine(e.ToString()));

        Console.WriteLine(spacer);

        string[] DepartmentShortNames = ["HR", "IT"];
        List<Department> departments = Data.GetDepartments();
        var filteredDepartments = departments.Filter(d => DepartmentShortNames.Contains(d.ShortName));

        filteredDepartments.ForEach(d => Console.WriteLine(d.ToString()));


        Console.WriteLine(spacer);
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
        Console.WriteLine(spacer);

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
    private static void RunExample4()
    {
        List<Employee> employees = Data.GetEmployees();
        List<Department> departments = Data.GetDepartments();
        // using query syntax
        var query =
        from employee in employees
        join department in departments on employee.DepartmentId equals department.Id
        where employee.AnnualSalary >= 50_000m
        select new { FullName = $"{employee.FirstName} {employee.LastName}", employee.AnnualSalary, Department = department.LongName };

        query.ToList().ForEach(e => Console.WriteLine($"Name: {e.FullName}, Salary: {e.AnnualSalary}, Department: {e.Department}"));

        Console.WriteLine(spacer);

        // using fluent syntax
        var result = employees.Where(e => e.AnnualSalary >= 50_000m)
        .Join(departments,
            emp => emp.DepartmentId,
            dept => dept.Id,
            (emp, dept) => new { FullName = $"{emp.FirstName} {emp.LastName}", emp.AnnualSalary, Department = dept.LongName });
        result.ToList().ForEach(e => Console.WriteLine($"Name: {e.FullName}, Salary: {e.AnnualSalary}, Department: {e.Department}"));
    }
    private static void RunExample5()
    {
        List<Employee> employees = Data.GetEmployees();
        List<Department> departments = Data.GetDepartments();

        var result = departments.GroupJoin(employees,
                dept => dept.Id,
                emp => emp.DepartmentId,
                (dept, employeesGroup) => new { Employees = employeesGroup ?? Enumerable.Empty<Employee>(), DepartmentName = dept.LongName });
        result.ToList().ForEach(e =>
        {
            Console.WriteLine($"Department: {e.DepartmentName}");
            if (!e.Employees.Any())
            {
                Console.WriteLine($"{"",5}No employees in this department.");
            }
            else
            {
                e.Employees.ToList().ForEach(emp => Console.WriteLine($"{"",5}Employee: {emp.FirstName} {emp.LastName}, Salary: {emp.AnnualSalary}"));
            }
        });

        Console.WriteLine(spacer);

        var qrs =
        from employee in employees
        join department in departments on employee.DepartmentId equals department.Id into departmentGroup
        select new { Department = departmentGroup, Employee = employee };
        qrs.ToList().ForEach(e =>
        {
            Console.WriteLine($"Employee: {e.Employee.FirstName} {e.Employee.LastName}, Salary: {e.Employee.AnnualSalary}");
            if (!e.Department.Any())
            {
                Console.WriteLine($"{"",5}No department for this employee.");
            }
            else
            {
                e.Department.ToList().ForEach(dept => Console.WriteLine($"{"",5}Department: {dept.LongName}"));
            }
        });

        Console.WriteLine(spacer);

        var query =
        from department in departments
        join employee in employees on department.Id equals employee.DepartmentId into employeeGroup
        //  where employeeGroup.Any(empl => empl.AnnualSalary >= 50_000m)
        select new { Employee = employeeGroup, Department = department.LongName };
        query.ToList().ForEach(q =>
        {
            Console.WriteLine($"Department: {q.Department}");
            if (!q.Employee.Any())
            {
                Console.WriteLine($"{"",5}No employees in this department.");
            }
            else
            {
                q.Employee.ToList().ForEach(emp => Console.WriteLine($"{"",5}Employee: {emp.FirstName} {emp.LastName}, Salary: {emp.AnnualSalary}"));
            }
        });

    }
    private static void RunExample6()
    {
        List<Employee> employees = Data.GetEmployees();
        List<Department> departments = Data.GetDepartments();
        var result = departments.GroupJoin(employees,
            dept => dept.Id,
            emp => emp.DepartmentId,
            (dept, employeeGrp) => new { Employees = employeeGrp ?? Enumerable.Empty<Employee>(), DepartmentName = dept.LongName });
        result.ToList().ForEach(e =>
        {
            Console.WriteLine($"Department: {e.DepartmentName}");
            if (!e.Employees.Any())
            {
                Console.WriteLine($"{"",5}No employees in this department.");
            }
            else
            {
                e.Employees.ToList().ForEach(emp => Console.WriteLine($"{"",5}Employee: {emp.FirstName} {emp.LastName}, Salary: {emp.AnnualSalary}"));
            }
        });
        Console.WriteLine(spacer);

        var qrs = from d in departments
                  join e in employees on d.Id equals e.DepartmentId into employeeGroup
                  select new { DepartmentName = d.LongName, Employee = employeeGroup };
        qrs.ToList().ForEach(e =>
        {
            Console.WriteLine($"Department: {e.DepartmentName}");
            if (!e.Employee.Any())
            {
                Console.WriteLine($"{"",5}No employees in this department.");
            }
            else
            {
                e.Employee.ToList().ForEach(emp => Console.WriteLine($"{"",5}Employee: {emp.FirstName} {emp.LastName}, Salary: {emp.AnnualSalary}"));
            }
        });

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

