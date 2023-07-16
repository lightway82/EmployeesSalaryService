using EmployeesService.Employees;
using EmployeesService.EmployeesPool;

namespace EmployeesService.SalaryCalculation.SalaryCalculators;

/// <summary>
/// The calculator for the company total salary (the sum of all employees' salaries).
/// </summary>
internal class TotalSalaryCalculator
{
    private readonly ISalaryCalculatorProvider _calculatorProvider;
    private readonly IEmployeePool _employeePool;

    /// <summary>
    /// Creates the calculator for the total company's salary.
    /// </summary>
    /// <param name="calculatorProvider">Calculator provider</param>
    /// <param name="employeePool">Employee pool</param>
    public TotalSalaryCalculator(ISalaryCalculatorProvider calculatorProvider, IEmployeePool employeePool)
    {
        _calculatorProvider = calculatorProvider;
        _employeePool = employeePool;
    }

    /// <summary>
    /// Calculates the total company's salary based on all other calculators.
    /// </summary>
    /// <param name="employee">Employee</param>
    /// <returns>Returns a tuple of the top leader salary and the total salary</returns>
    public  (decimal topLeaderSalary, decimal totalSalary) Calculate(Employee employee)
    {
        return WalkCompanyTreePostOrder(employee, 0M);
    }
    
    
    private  (decimal levelSalary, decimal totalSalary) WalkCompanyTreePostOrder(Employee employee, decimal underTreeTotalSalary)
    {
        if (employee is not Leader)
        {
           decimal salary =  _calculatorProvider.GetCalculator(employee.GetType()).Calculate(employee);
           return  (salary, salary);
        }
       
        Leader leader = (Leader)employee;
        
        decimal currentLevelSalary = 0;
        decimal currentUnderTreeTotalSalary = underTreeTotalSalary;
        foreach (var emp in _employeePool.GetSubordinate(leader))
        {
            var (levelSalary, totalSalary) = WalkCompanyTreePostOrder(emp, underTreeTotalSalary);
            currentLevelSalary = Decimal.Add(levelSalary, currentLevelSalary);
            currentUnderTreeTotalSalary = Decimal.Add(totalSalary, currentUnderTreeTotalSalary);
        }
    
        decimal employerSalary = _calculatorProvider.GetCalculator(employee.GetType()).Calculate(employee, currentLevelSalary, currentUnderTreeTotalSalary);
        return (
            employerSalary,
            Decimal.Add(currentUnderTreeTotalSalary, employerSalary)
        );
    }
}