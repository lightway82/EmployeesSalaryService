using EmployeesService.Employees;
using EmployeesService.EmployeesPool;

namespace EmployeesService.SalaryCalculation.SalaryCalculators;

/// <summary>
/// The base class for all employee's calculators
/// </summary>
internal abstract class BaseSalaryCalculator: ISalaryCalculator
{
    protected ISalaryCalculatorProvider CalculatorProvider { get; }
    protected IEmployeePool Pool { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="calculatorProvider">Calculator Provider</param>
    /// <param name="employeePool">Employee Pool</param>
    protected BaseSalaryCalculator(ISalaryCalculatorProvider calculatorProvider, IEmployeePool employeePool)
    {
        CalculatorProvider = calculatorProvider;
        Pool = employeePool;
    }

    public virtual decimal Calculate(Employee employee) => 0;
    
    public virtual decimal Calculate(Employee employee, decimal underLevelTotalSalary, decimal underTreeTotalSalary) => 0;

}