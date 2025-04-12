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
        IList<Employee> employees = Data.GetEmployees();
        var filteredEmployees = employees.Filter(e => e.IsManager == true);
        filteredEmployees.ForEach(e => Console.WriteLine(e.ToString()));

        Console.WriteLine("-------------------------------------------------");

        string[] DepartmentShortNames = ["HR", "IT"];
        List<Department> departments = Data.GetDepartments();
        var filteredDepartments = departments.Filter(d => DepartmentShortNames.Contains(d.ShortName));

        filteredDepartments.ForEach(d => Console.WriteLine(d.ToString()));


        Console.WriteLine("-------------------------------------------------");

        var linq = from emp in employees
                   join dept in departments on emp.DepartmentId equals dept.Id
                   where emp.IsManager == true
                   select new { emp.FirstName, emp.LastName, emp.Position, Salary = emp.AnnualSalary, DepartmentName = dept.LongName };
        linq.ToImmutableList().ForEach(e => Console.WriteLine($"Name: {e.FirstName} {e.LastName}, Position: {e.Position}, Salary: {e.Salary}, Department: {e.DepartmentName}"));

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
}

