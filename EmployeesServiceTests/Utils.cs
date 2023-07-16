namespace EmployeesServiceTests;

public static class Utils
{
  public static DateTime CreateFirstYearEmploymentDate => CreateYearEmploymentDate(0);

  public static DateTime CreateSecondYearEmploymentDate => CreateYearEmploymentDate(1);

  public static DateTime CreateThirdYearEmploymentDate => CreateYearEmploymentDate(2);

  public static DateTime CreateFourthYearEmploymentDate => CreateYearEmploymentDate(3);

  /// <summary>
  /// Create employment date by full employment years.
  /// </summary>
  /// <param name="countFullYear">Full employment years</param>
  /// <returns></returns>
  public static DateTime CreateYearEmploymentDate(int countFullYear) => DateTime.Today.Subtract(TimeSpan.FromDays(365 * countFullYear));
}