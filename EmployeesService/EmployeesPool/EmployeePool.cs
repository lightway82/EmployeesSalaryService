using EmployeesService.Employees;
using static EmployeesService.EmployeesPool.IEmployeePool;

namespace EmployeesService.EmployeesPool;

/// <summary>
/// Implements employees pool. This implementation uses a dictionary of employees with a  name as a key to optimize the search.
/// </summary>
public class EmployeePool : IEmployeePool
{
    private readonly Dictionary<string, LinkedList<Employee>> _employeesTable = new();

    /// <inheritdoc cref="IEmployeePool.TopLevelLeader"/>
    public Leader? TopLevelLeader { get; private set; }

    /// <inheritdoc cref="IEmployeePool.AddEmployee"/>
    /// <exception cref="SelfSubordinateException">Thrown out when trying to add an employee to himself as a subordinate.</exception>
    /// <exception cref="IncorrectEmploymentDateException">Thrown when the employment date is greater than the current one</exception>
    /// <exception cref="DuplicatedEmployeeException">Thrown when trying to add a previously added employee to the company tree in a other position.</exception>
    public void AddEmployee(Employee employee, Leader leader)
    {
        if (ReferenceEquals(employee, leader)) throw new SelfSubordinateException("Employee can’t attach self as a parent node.");
        if (employee.DateOfEmployment > DateTime.Now) throw new IncorrectEmploymentDateException("Employment date must be earlier or equal to today’s date");
        
        if (_employeesTable.ContainsKey(employee.Name))
        {
            if (_employeesTable[employee.Name].Any(e => ReferenceEquals(e, employee)))
            {
                throw new DuplicatedEmployeeException("The employee to be added is already present in the company");
            }
        }
        else
        {
            _employeesTable[employee.Name] = new LinkedList<Employee>();
        }

        _employeesTable[employee.Name].AddLast(employee);

        leader.Employees.Add(employee);
        employee.Leader = leader;
    }

    /// <inheritdoc cref="IEmployeePool.AddTopLeader"/>
    /// <exception cref="RemoveTopLeaderAttemptException">Thrown when trying to add a top-level leader if it already exists.</exception>
    /// <exception cref="IncorrectEmploymentDateException">Thrown when the employment date is greater than the current one</exception>
    public void AddTopLeader(Leader topLeader)
    {
        if (TopLevelLeader is not null) throw new RemoveTopLeaderAttemptException("You can't remove the top leader");
        if (topLeader.DateOfEmployment > DateTime.Now) throw new IncorrectEmploymentDateException("Employment date must be less or equal to today’s date");
        
        _employeesTable[topLeader.Name] = new LinkedList<Employee>();
        _employeesTable[topLeader.Name].AddLast(topLeader);

        TopLevelLeader = topLeader;
    }

    /// <inheritdoc cref="IEmployeePool.FindEmployeeByNameFullText"/>
    public IEnumerable<Employee> FindEmployeeByNameFullText(string name)
    {
        return FindEmployee(employee => employee.Name.Contains(name));
    }

    /// <inheritdoc cref="IEmployeePool.FindEmployee"/>
    public IEnumerable<Employee> FindEmployee(SearchFilter searchFilter)
    {
        if (TopLevelLeader is null) return Enumerable.Empty<Employee>();
        return TraverseEmployees(TopLevelLeader).Where(employee => searchFilter(employee));
    }

    /// <inheritdoc cref="IEmployeePool.GetEmployeeByName"/>
    public IEnumerable<Employee> GetEmployeeByName(string name)
    {
        if (_employeesTable.TryGetValue(name, out var employees))
        {
            return employees;
        }

        return Enumerable.Empty<Employee>();
    }

    /// <inheritdoc cref="IEmployeePool.AllEmployee"/>
    public IEnumerable<Employee> AllEmployee()
    {
        return TopLevelLeader is null ? Enumerable.Empty<Employee>() : TraverseEmployees(TopLevelLeader);
    }

    /// <inheritdoc cref="IEmployeePool.GetLeader"/>
    public Leader? GetLeader(Employee employee) => employee.Leader;

    /// <inheritdoc cref="IEmployeePool.GetSubordinate"/>
    public IEnumerable<Employee> GetSubordinate(Leader employee) => employee.Employees;
     
    
    private IEnumerable<Employee> TraverseEmployees(Leader topLeader)
    {
        Queue<Employee> queue = new();
        queue.Enqueue(topLeader);

        while (queue.TryDequeue(out var employee))
        {
            yield return employee;

            if (employee is Leader leader)
            {
                foreach (var emp in leader.Employees)
                {
                    queue.Enqueue(emp);
                }
            }
        }
    }
}