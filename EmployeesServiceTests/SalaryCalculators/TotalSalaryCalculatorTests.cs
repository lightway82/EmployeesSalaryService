using EmployeesService.Employees;
using EmployeesService.EmployeesPool;
using EmployeesService.SalaryCalculation;
using EmployeesService.SalaryCalculation.SalaryCalculators;
using FluentAssertions;

namespace EmployeesServiceTests.SalaryCalculators;

public class TotalSalaryCalculatorTests
{
    [Test]
    public void Calculate_ShouldReturnCorrectValue()
    {
        int baseRate = 1000;
        decimal expectedTotalSalary = 8125.267135M;
        decimal expectedTopLeaderSalary = 1051.222135M;
        IEmployeePool employeePool = new EmployeePool();
        ISalaryCalculatorProvider salaryCalculatorProvider = new SalaryCalculatorProvider(employeePool);
        TotalSalaryCalculator totalSalaryCalculator = new TotalSalaryCalculator(salaryCalculatorProvider, employeePool);

        Sales s1 = new Sales{ Name = "s1", BaseRate = baseRate , DateOfEmployment = Utils.CreateYearEmploymentDate(3)};
        Sales s2 = new Sales { Name = "s2", BaseRate = baseRate, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Employee e1 = new Employee { Name = "e1", BaseRate = baseRate, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Employee e2 = new Employee { Name = "e2", BaseRate = baseRate, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Employee e3 = new Employee { Name = "e3", BaseRate = baseRate, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Employee e4 = new Employee { Name = "e4", BaseRate = baseRate, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Manager m1 = new Manager { Name = "m1", BaseRate = baseRate, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Manager m2 = new Manager { Name = "m2", BaseRate = baseRate, DateOfEmployment = Utils.CreateYearEmploymentDate(1) };

        employeePool.AddTopLeader(s1);
        employeePool.AddEmployee(m2, s1);
        employeePool.AddEmployee(s2, m2);
        employeePool.AddEmployee(e1, m2);
        employeePool.AddEmployee(e2, m2);
        employeePool.AddEmployee(m1, s2); 
        employeePool.AddEmployee(e3, s2); 
        employeePool.AddEmployee(e4, s2);


        var (topLeaderSalary, totalSalary) = totalSalaryCalculator.Calculate(s1);
        
        
        topLeaderSalary.Should().Be(expectedTopLeaderSalary);
        totalSalary.Should().Be(expectedTotalSalary);
    }
}