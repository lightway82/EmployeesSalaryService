namespace EmployeesService.Employees;

public class Employee
{
    private int _baseRate;
    
    /// <summary>
    /// Employee name
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Employment date
    /// </summary>
    public required DateTime DateOfEmployment { get; set; }

    /// <summary>
    /// Base rate
    /// </summary>
    /// <exception cref="IncorrectBaseRateException">If value is negative.</exception>
    public required int BaseRate
    {
        get => _baseRate;
        set
        {
            if (value < 0) throw new IncorrectBaseRateException("Base rate value must be zero or positive integer.");
            _baseRate = value;
        }
    }

    /// <summary>
    /// Leader of employee
    /// </summary>
    public Leader? Leader { get; set; }
    
    
    /// <summary>
    /// Full years of employment.
    /// </summary>
    public int YearsInCompany => (int)(DateTime.Now - DateOfEmployment).TotalDays / 365;
}