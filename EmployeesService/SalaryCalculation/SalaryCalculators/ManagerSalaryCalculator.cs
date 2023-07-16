using EmployeesService.Employees;
using EmployeesService.EmployeesPool;

namespace EmployeesService.SalaryCalculation.SalaryCalculators;

/// <summary>
/// Implements the calculator for the manager employee type.
/// </summary>
[CalculateSalaryFor<Manager>]
internal class ManagerSalaryCalculator : BaseSalaryCalculator
{
    private const decimal TotalPremiumLimitFromBaseRateCoefficient = 0.4M;
    private const decimal PremiumPerYearCoefficient = 0.05M;
    private const decimal PremiumFromSubordinatesCoefficient = 0.005M;

    /// <summary>
    /// Create the calculator for the manager employee type.
    /// </summary>
    /// <param name="calculatorProvider">Calculator Provider</param>
    /// <param name="employeePool">Employee Pool</param>
    public ManagerSalaryCalculator(ISalaryCalculatorProvider calculatorProvider, IEmployeePool employeePool) : base(
        calculatorProvider, employeePool)
    {
    }

   /// <summary>
   ///<inheritdoc cref="ISalaryCalculator.Calculate(EmployeesService.Employees.Employee)"/>.
   /// This method calculates the salary utilizing other calculators (subordinates calculators).
   /// </summary>
   /// <param name="employee">Employee</param>
   /// <returns>Employee's salary</returns>
    public override decimal Calculate(Employee employee)
    {
        decimal totalPremiumCoefficient =
            Math.Clamp(Decimal.Multiply(PremiumPerYearCoefficient, employee.YearsInCompany), min: 0M,
                max: TotalPremiumLimitFromBaseRateCoefficient);
        decimal selfSalary =
            Decimal.Add(Decimal.Multiply(employee.BaseRate, totalPremiumCoefficient), employee.BaseRate);

        Manager manager = (Manager)employee;

        decimal subordinatesTotalSalary = 0M;
        foreach (var emp in Pool.GetSubordinate(manager))
        {
            subordinatesTotalSalary = Decimal.Add(
                CalculatorProvider.GetCalculator(emp.GetType()).Calculate(emp),
                subordinatesTotalSalary);
        }

        return Decimal.Add(selfSalary, Decimal.Multiply(subordinatesTotalSalary, PremiumFromSubordinatesCoefficient));
    }

   /// <inheritdoc cref="ISalaryCalculator.Calculate(Employee,decimal,decimal)"/>
    public override decimal Calculate(Employee employee, decimal underLevelTotalSalary, decimal underTreeTotalSalary)
    {
        decimal totalPremiumCoefficient =
            Math.Clamp(Decimal.Multiply(PremiumPerYearCoefficient, employee.YearsInCompany), min: 0M,
                max: TotalPremiumLimitFromBaseRateCoefficient);
        decimal selfSalary = Decimal.Add(Decimal.Multiply(employee.BaseRate, totalPremiumCoefficient), employee.BaseRate);
        
        return Decimal.Add(selfSalary, Decimal.Multiply(underLevelTotalSalary, PremiumFromSubordinatesCoefficient));
        
    }
}