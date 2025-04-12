namespace TCPData;

public class Department
{
    public int Id { get; set; } = 0;
    public required string ShortName { get; set; } = string.Empty;
    public required string LongName { get; set; } = string.Empty;

    public override string ToString()
    {
        return $" DEPTID:{Id} , ShortName: {ShortName} , LongName: {LongName}";
    }
}
