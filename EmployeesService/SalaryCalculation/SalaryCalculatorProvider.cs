using EmployeesService.Employees;
using EmployeesService.EmployeesPool;

namespace EmployeesService.SalaryCalculation;

/// <summary>
/// The employee's calculator provider.
/// Calculators are determined by the attribute <see cref="CalculateSalaryForAttribute{TEmployeeType}"/>
/// </summary>
internal class SalaryCalculatorProvider: ISalaryCalculatorProvider
{
    private readonly IEmployeePool _employeePool;
    private readonly Dictionary<Type, ISalaryCalculator> _salaryCalculators;

    /// <summary>
    /// Creates the calculator's provider.
    /// </summary>
    /// <param name="employeePool">Employee Pool</param>
    public SalaryCalculatorProvider(IEmployeePool employeePool)
    {
        _employeePool = employeePool;
        _salaryCalculators = SetupCalculators();
    }
    
    private Dictionary<Type, ISalaryCalculator> SetupCalculators()
    {
        var calculatorsTypes = typeof(ISalaryCalculator).Assembly.GetTypes()
            .Where(t => t.IsDefined(typeof(CalculateSalaryForAttribute<>), inherit:false) &&
                        t.IsAssignableTo(typeof(ISalaryCalculator)));

        Dictionary<Type, ISalaryCalculator> result = new Dictionary<Type, ISalaryCalculator>();

        foreach (var type in calculatorsTypes)
        {
            Attribute? attribute =
                Attribute.GetCustomAttribute(type, typeof(CalculateSalaryForAttribute<>), inherit: false);
            if (attribute is not null &&
                attribute.GetType().GetGenericArguments()[0].IsAssignableTo(typeof(Employee)))
            {
                // At this point, the client should not receive a null reference because the type is received programmatically from existing code.
                // We control what types there are taking them from existing classes.
                // This situation should be verified by tests and may appear at the development stage.
                result[attribute.GetType().GetGenericArguments()[0]] =
                    (Activator.CreateInstance(type, this, _employeePool) as ISalaryCalculator)!;
            }
        }

        return result;
    }
    
    ///<inheritdoc cref="ISalaryCalculatorProvider.GetCalculator"/>
    public ISalaryCalculator GetCalculator(Type employeeType)
    {
        return _salaryCalculators[employeeType];
    }
}