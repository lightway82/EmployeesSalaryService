namespace EmployeesService.Employees;

public abstract class Leader: Employee
{
    /// <summary>
    /// Subordinates
    /// </summary>
    public List<Employee> Employees { get; } = new();
}