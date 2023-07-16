using EmployeesService.Employees;

namespace EmployeesService.EmployeesPool;

/// <summary>
/// Provides the API contracts for employees pool
/// </summary>
public interface IEmployeePool
{
    /// <summary>
    /// The delegate for employees search filters.
    /// </summary>
    delegate bool SearchFilter(Employee employee);

    /// <summary>
    /// The top-level leader.
    /// </summary>
    Leader? TopLevelLeader { get; }

    /// <summary>
    /// Add a subordinate to the given leader.
    /// </summary>
    /// <param name="employee">Employee</param>
    /// <param name="leader">Leader</param>
    void AddEmployee(Employee employee, Leader leader);

    /// <summary>
    /// Add a top-level leader.
    /// </summary>
    /// <param name="employee">Leader</param>
    void AddTopLeader(Leader employee);

    /// <summary>
    /// Look up of an employee by name.
    /// </summary>
    /// <param name="name">Employee name</param>
    /// <returns>Returns a sequence of the employees.</returns>
    IEnumerable<Employee> GetEmployeeByName(string name);

    /// <summary>
    /// Search of the employees with a partial name match.
    /// </summary>
    /// <param name="name">Part of the name</param>
    /// <returns>Returns a sequence of employees which match the partial name.</returns>
    IEnumerable<Employee> FindEmployeeByNameFullText(string name);

    /// <summary>
    /// Employees look up with a given filter.
    /// </summary>
    /// <param name="searchFilter">Search filter</param>
    /// <returns>Returns a sequence with employees which match the given search filter.</returns>
    IEnumerable<Employee> FindEmployee(SearchFilter searchFilter);

    /// <summary>
    /// Obtains the all employees sequence.
    /// </summary>
    /// <returns>Employees sequence.</returns>
    IEnumerable<Employee> AllEmployee();

    /// <summary>
    /// Obtains a leader/manager of the given employee.
    /// </summary>
    /// <param name="employee">Employee</param>
    /// <returns>Employee's leader/manager or null if the employee is missing a leader.</returns>
    Leader? GetLeader(Employee employee);

    /// <summary>
    /// Obtain all subordinates of the given leader.
    /// </summary>
    /// <param name="employee">Leader</param>
    /// <returns>Subordinated employees sequence.</returns>
    IEnumerable<Employee> GetSubordinate(Leader employee);
}