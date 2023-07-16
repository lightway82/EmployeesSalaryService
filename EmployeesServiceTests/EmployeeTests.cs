using EmployeesService;
using EmployeesService.Employees;
using FluentAssertions;

namespace EmployeesServiceTests;

public class EmployeeTests
{
    [Test]
    public void BaseRate_WhenPositiveValue_NoException()
    {
       
        Action act = () =>
        { 
            new Employee()
            {
                Name = "John Doe",
                BaseRate = 1000,
                DateOfEmployment = DateTime.Now
            };
        };
        act.Should().NotThrow();
    }
    
    [Test]
    public void BaseRate_WhenNegativeValue_ThrowIncorrectBaseRateException()
    {
        Action act = () =>
        { 
            new Employee
            {
                Name = "John Doe",
                BaseRate = -100,
                DateOfEmployment = DateTime.Now
            };
        };
        act.Should().ThrowExactly<IncorrectBaseRateException>();
    }
    
  
    [Test]
    public void YearsInCompany_WhenDateOfEmploymentLessOrEqualNow_ShouldReturnCorrectValue()
    {
        Employee employee = new Employee
        {
            Name = "John Doe",
            BaseRate = 1000,
            DateOfEmployment = DateTime.Now
        };
        employee.YearsInCompany.Should().Be(0);
        
        
        employee = new Employee
        {
            Name = "John Doe",
            BaseRate = 1000,
            DateOfEmployment = Utils.CreateYearEmploymentDate(1)
        };
        employee.YearsInCompany.Should().Be(1);
    }
}