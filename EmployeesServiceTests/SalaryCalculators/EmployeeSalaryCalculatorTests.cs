using EmployeesService.Employees;
using EmployeesService.EmployeesPool;
using EmployeesService.SalaryCalculation;
using EmployeesService.SalaryCalculation.SalaryCalculators;
using FluentAssertions;
using Moq;

namespace EmployeesServiceTests.SalaryCalculators;

public class EmployeeSalaryCalculatorTests
{

    [Test]
    public void Calculate_ForFirstYearEmployee_ShouldReturnCorrectValue()
    {
        int baseRate = 1000;

        EmployeeSalaryCalculator salaryCalculator = new EmployeeSalaryCalculator(
            new Mock<ISalaryCalculatorProvider>().Object,
            new Mock<IEmployeePool>().Object);

        Employee employee = new Employee
        {
            Name = "John Doe",
            BaseRate = baseRate,
            DateOfEmployment = Utils.CreateFirstYearEmploymentDate
        };


        decimal salary = salaryCalculator.Calculate(employee);


        salary.Should().Be(baseRate);
    }

    [Test]
    public void Calculate_ForSeveralYearsEmployee_ShouldReturnCorrectValue()
    {
        int baseRate = 1000;
        int employmentYears = 3;
        decimal expectedSalary = 1090M;

        EmployeeSalaryCalculator salaryCalculator = new EmployeeSalaryCalculator(
            new Mock<ISalaryCalculatorProvider>().Object,
            new Mock<IEmployeePool>().Object);

        Employee employee = new Employee
        {
            Name = "John Doe",
            BaseRate = baseRate,
            DateOfEmployment = Utils.CreateYearEmploymentDate(employmentYears)
        };


        decimal salary = salaryCalculator.Calculate(employee);


        salary.Should().Be(expectedSalary);
    }

    [Test]
    public void Calculate_ForPremiumMoreThanLimit_ShouldReturnClampedValue()
    {
        int baseRate = 1000;
        int employmentYears = 15;
        decimal expectedSalary = 1300M;

        EmployeeSalaryCalculator salaryCalculator = new EmployeeSalaryCalculator(
            new Mock<ISalaryCalculatorProvider>().Object,
            new Mock<IEmployeePool>().Object);

        Employee employee = new Employee
        {
            Name = "John Doe",
            BaseRate = baseRate,
            DateOfEmployment = Utils.CreateYearEmploymentDate(employmentYears)
        };


        decimal salary = salaryCalculator.Calculate(employee);


        salary.Should().Be(expectedSalary);
    }
}