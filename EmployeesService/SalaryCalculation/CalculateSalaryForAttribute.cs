using EmployeesService.Employees;

namespace EmployeesService.SalaryCalculation;

/// <summary>
/// The attribute that marks employees salary calculators to bind with the employee type.
/// </summary>
/// <typeparam name="TEmployeeType">Employee type</typeparam>
[AttributeUsage(AttributeTargets.Class)]
internal class CalculateSalaryForAttribute<TEmployeeType> : Attribute where TEmployeeType: Employee { }