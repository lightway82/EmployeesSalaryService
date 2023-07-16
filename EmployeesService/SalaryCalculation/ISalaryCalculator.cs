using EmployeesService.Employees;

namespace EmployeesService.SalaryCalculation;

/// <summary>
/// Provides an API contracts for the salary calculators.
/// </summary>
internal interface ISalaryCalculator
{
    /// <summary>
    /// Calculates employees' salaries
    /// 
    /// </summary>
    /// <param name="employee">Employee.</param>
    /// <returns>Salary of employee.</returns>
    decimal Calculate(Employee employee);
    
    /// <summary>
    /// Calculates the employee's salary.
    /// The method doesn't invoke other calculators, but uses values from the sub levels of the hierarchy.
    /// </summary>
    /// <param name="employee">Employee</param>
    /// <param name="underLevelTotalSalary">Sum of all direct subordinates' salaries</param>
    /// <param name="underTreeTotalSalary">Sum of all direct and the rest of the employees down the tree</param>
    /// <returns>Salary of employee.</returns>
    decimal Calculate(Employee employee, decimal underLevelTotalSalary, decimal underTreeTotalSalary);
}
