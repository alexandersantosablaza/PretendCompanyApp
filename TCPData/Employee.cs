namespace TCPData;

public class Employee
{
    public int Id { get; set; } = 0;
    public required string LastName { get; set; } = string.Empty;
    public required string FirstName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public decimal AnnualSalary { get; set; } = 0m;
    public int DepartmentId { get; set; } = 0;
    public bool IsManager { get; set; } = false;
    public override string ToString()
    {
        return $" EID:{Id} , Name: {LastName}, {FirstName} , Position: {Position} , SALARY: {AnnualSalary} DEPTID: {DepartmentId} , MANAGER: {IsManager}";
    }
}
