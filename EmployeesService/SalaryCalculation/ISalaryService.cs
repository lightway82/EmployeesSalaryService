using EmployeesService.Employees;

namespace EmployeesService.SalaryCalculation;

/// <summary>
/// Provides an API contracts for the salary service.
/// </summary>
public interface ISalaryService
{
    /// <summary>
    /// Calculates the employee's salary.
    /// </summary>
    /// <param name="employee">Employee</param>
    /// <returns>Employee salary</returns>
    public decimal CalculateEmployeeSalary(Employee employee);

    /// <summary>
    /// Calculates the total company salary from all employees salaries.
    /// </summary>
    /// <returns>Total company salary</returns>
    /// <exception cref="TopLeaderNotFoundException"></exception>
    public decimal CalculateTotalSalaryOfCompany();
}