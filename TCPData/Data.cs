using System.Collections.Generic;

namespace TCPData;

public static class Data
{
    public static List<Employee> GetEmployees()
    {
        List<Employee> employees =
        [
            new Employee
            {
                Id = 1,
                LastName = "Smith",
                FirstName = "John",
                Position = "Manager",
                AnnualSalary = 60_000m,
                DepartmentId = 1,
                IsManager = true
            },
            new Employee
            {
                Id = 2,
                LastName = "Doe",
                FirstName = "Jane",
                Position = "Developer",
                AnnualSalary = 50_000m,
                DepartmentId = 1,
                IsManager = false
            },
            new Employee
            {
                Id = 3,
                LastName = "Brown",
                FirstName = "Charlie",
                Position = "Designer",
                AnnualSalary = 55_000m,
                DepartmentId = 2,
                IsManager = false
            },
            new Employee
            {
                Id = 4,
                LastName = "Johnson",
                FirstName = "Emily",
                Position = "Developer",
                AnnualSalary = 52_000m,
                DepartmentId = 2,
                IsManager = false
            },
            new Employee
            {
                Id = 5,
                LastName = "Williams",
                FirstName = "Michael",
                Position = "Manager",
                AnnualSalary = 70_000m,
                DepartmentId = 3,
                IsManager = true
            },
            new Employee
            {
                Id = 6,
                LastName = "Jones",
                FirstName = "Sarah",
                Position = "Developer",
                AnnualSalary = 48_000m,
                DepartmentId = 3,
                IsManager = false
            },
            new Employee
            {
                Id = 7,
                LastName = "Garcia",
                FirstName = "David",
                Position = "Designer",
                AnnualSalary = 53_000m,
                DepartmentId = 4,
                IsManager = false
            }
        ];
        return employees;
    }

    public static List<Department> GetDepartments()
    {
        List<Department> departments =
        [
            new Department
            {
                Id = 1,
                ShortName = "IT",
                LongName = "Information Technology"
            },
            new Department
            {
                Id = 2,
                ShortName = "Design",
                LongName = "Design Department"
            },
            new Department
            {
                Id = 3,
                ShortName = "HR",
                LongName = "Human Resources"
            },
            new Department
            {
                Id = 4,
                ShortName = "Marketing",
                LongName = "Marketing Department"
            },
            new Department
            {
                Id = 5,
                ShortName = "Finance",
                LongName = "Finance Department"
            }
        ];
        return departments;
    }
}