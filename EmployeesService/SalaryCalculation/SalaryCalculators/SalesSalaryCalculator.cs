using EmployeesService.Employees;
using EmployeesService.EmployeesPool;

namespace EmployeesService.SalaryCalculation.SalaryCalculators;

/// <summary>
/// Implements the calculator for sales employees.
/// </summary>
[CalculateSalaryFor<Sales>]
internal class SalesSalaryCalculator : BaseSalaryCalculator
{
    private readonly TotalSalaryCalculator _totalSalaryCalculator;
    private const decimal TotalPremiumLimitFromBaseRateCoefficient = 0.35M;
    private const decimal PremiumPerYearCoefficient = 0.01M;
    private const decimal PremiumFromSubordinatesCoefficient = 0.003M;
    
    /// <summary>
    /// Creates the calculator for sales employees.
    /// </summary>
    /// <param name="calculatorProvider">Calculator provider</param>
    /// <param name="employeePool">Employee pool</param>
    public SalesSalaryCalculator(ISalaryCalculatorProvider calculatorProvider, IEmployeePool employeePool) : base(
        calculatorProvider, employeePool)
    {
        _totalSalaryCalculator = new TotalSalaryCalculator(calculatorProvider, employeePool);
    }

    /// <summary>
    /// <inheritdoc cref="ISalaryCalculator.Calculate(Employee)"/>
    /// This method calculates the salary utilizing other calculators (subordinates calculators).
    /// </summary>
    /// <param name="employee">Employee</param>
    /// <returns>Employee's salary</returns>
    public override decimal Calculate(Employee employee)
    {
        return _totalSalaryCalculator.Calculate(employee).topLeaderSalary;
    }

    /// <inheritdoc cref="ISalaryCalculator.Calculate(Employee,decimal,decimal)"/>
    public override decimal Calculate(Employee employee, decimal underLevelTotalSalary, decimal underTreeTotalSalary)
    {
        decimal totalPremiumCoefficient =
            Math.Clamp(Decimal.Multiply(PremiumPerYearCoefficient, employee.YearsInCompany), min: 0M,
                max: TotalPremiumLimitFromBaseRateCoefficient);
        decimal selfSalary = Decimal.Add(Decimal.Multiply(employee.BaseRate, totalPremiumCoefficient), employee.BaseRate);
        
        return Decimal.Add(selfSalary, Decimal.Multiply(underTreeTotalSalary, PremiumFromSubordinatesCoefficient));
    }
}