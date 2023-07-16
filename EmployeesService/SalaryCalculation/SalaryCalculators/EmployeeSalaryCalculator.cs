using EmployeesService.Employees;
using EmployeesService.EmployeesPool;

namespace EmployeesService.SalaryCalculation.SalaryCalculators;

/// <summary>
/// The implementation for the calculator for the employees without leaders/managers.
/// </summary>
[CalculateSalaryFor<Employee>]
internal class EmployeeSalaryCalculator : BaseSalaryCalculator
{
    private const decimal TotalPremiumLimitFromBaseRateCoefficient = 0.3M;
    private const decimal PremiumPerYearCoefficient = 0.03M;

    /// <summary>
    /// Creates the salary calculator for this type of the employee. 
    /// </summary>
    /// <param name="calculatorProvider">Calculator provider</param>
    /// <param name="employeePool">Employee pool</param>
    public EmployeeSalaryCalculator(ISalaryCalculatorProvider calculatorProvider, IEmployeePool employeePool) : base(
        calculatorProvider, employeePool)
    {
    }

    /// <inheritdoc cref="ISalaryCalculator.Calculate(EmployeesService.Employees.Employee)"/>
    public override decimal Calculate(Employee employee)
    {
       decimal totalPremiumCoefficient =  Math.Clamp(Decimal.Multiply(PremiumPerYearCoefficient, employee.YearsInCompany), min: 0M, max: TotalPremiumLimitFromBaseRateCoefficient);
       return Decimal.Add(Decimal.Multiply(employee.BaseRate, totalPremiumCoefficient), employee.BaseRate);
    }
}