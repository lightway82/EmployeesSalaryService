namespace EmployeesService;

/// <summary>
/// Thrown when trying to add a previously added employee to the company tree into another position.
/// </summary>
public class DuplicatedEmployeeException : Exception
{
    public DuplicatedEmployeeException(string? message) : base(message)
    {
    }
}

/// <summary>
/// Thrown when trying to add a top-level leader if it already exists.
/// </summary>
public class RemoveTopLeaderAttemptException : Exception
{
    public RemoveTopLeaderAttemptException(string? message) : base(message)
    {
    }
}

/// <summary>
/// Thrown when no top level leader can be found
/// </summary>
public class TopLeaderNotFoundException : Exception
{
    public TopLeaderNotFoundException(string? message) : base(message)
    {
    }
}

/// <summary>
/// Thrown when the employment date is greater than the current one
/// </summary>
public class IncorrectEmploymentDateException : Exception
{
    public IncorrectEmploymentDateException(string? message) : base(message)
    {
    }
}

/// <summary>
/// Thrown when trying to add an employee to himself as a subordinate.
/// </summary>
public class SelfSubordinateException : Exception
{
    public SelfSubordinateException(string? message) : base(message)
    {
    }
}

/// <summary>
/// Thrown when base rate value is negative.
/// </summary>
public class IncorrectBaseRateException : Exception
{
    public IncorrectBaseRateException(string? message) : base(message)
    {
    }
}