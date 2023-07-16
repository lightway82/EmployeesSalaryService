using EmployeesService.Employees;
using EmployeesService.EmployeesPool;
using EmployeesService.SalaryCalculation.SalaryCalculators;

namespace EmployeesService.SalaryCalculation;

/// <summary>
/// The employees salary calculation service.
/// </summary>
public class SalaryService: ISalaryService
{
    private readonly IEmployeePool _employeePool;
    private readonly ISalaryCalculatorProvider _calculatorProvider;
    private readonly TotalSalaryCalculator _totalSalaryCalculator;

    /// <summary>
    /// Creates the service.
    /// </summary>
    /// <param name="employeePool">Employee Pool</param>
    public SalaryService(IEmployeePool employeePool)
    {
        _employeePool = employeePool;
        _calculatorProvider = new SalaryCalculatorProvider(employeePool);
        _totalSalaryCalculator = new TotalSalaryCalculator(_calculatorProvider, _employeePool);
    }
    
    /// <inheritdoc cref="ISalaryService.CalculateEmployeeSalary"/>
    public decimal CalculateEmployeeSalary(Employee employee)
    {
        return _calculatorProvider.GetCalculator(employee.GetType()).Calculate(employee);
    }


    /// <inheritdoc cref="ISalaryService.CalculateTotalSalaryOfCompany"/>
    /// <exception cref="TopLeaderNotFoundException">Thrown when trying to calculate the total salary if the employee tree is empty.</exception>
    public decimal CalculateTotalSalaryOfCompany()
    {
        if (_employeePool.TopLevelLeader is null) throw new TopLeaderNotFoundException("Top company's leader not found");
        return _totalSalaryCalculator.Calculate(_employeePool.TopLevelLeader).totalSalary;
    }
}

