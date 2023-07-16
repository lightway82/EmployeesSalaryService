namespace EmployeesService.SalaryCalculation;

/// <summary>
/// A calculators provider interface. 
/// </summary>
internal interface ISalaryCalculatorProvider
{
     /// <summary>
     /// Returns the calculator for the given employee's type.
     /// </summary>
     /// <param name="employeeType">Employee type</param>
     /// <returns>Salary Calculator</returns>
     ISalaryCalculator GetCalculator(Type employeeType);
}