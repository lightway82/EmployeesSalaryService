using EmployeesService.Employees;
using EmployeesService.EmployeesPool;
using EmployeesService.SalaryCalculation;
using FluentAssertions;

namespace EmployeesServiceTests.SalaryCalculators;

public class SalesSalaryCalculatorTests
{
     [Test]
    public void Calculate_ForFirstYearSales_ShouldReturnCorrectValue()
    {
        int baseRate = 1000;
        decimal expectedSalary = 1000M;
        
        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

        Sales topLeader = new Sales{Name = "topLeader", BaseRate = baseRate, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        employeePool.AddTopLeader(topLeader);

        decimal salary = salaryService.CalculateEmployeeSalary(topLeader);

        salary.Should().Be(expectedSalary);
    }

    [Test]
    public void Calculate_ForSeveralYearsSales_ShouldReturnCorrectValue()
    {
        int baseRate = 1000;
        decimal expectedSalary = 1030M;
        int employmentYears = 3;
        
        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

        Sales topLeader = new Sales {Name = "topLeader", BaseRate = baseRate, DateOfEmployment = Utils.CreateYearEmploymentDate(employmentYears) };
        employeePool.AddTopLeader(topLeader);

        decimal salary = salaryService.CalculateEmployeeSalary(topLeader);

        salary.Should().Be(expectedSalary);
    }

    [Test]
    public void Calculate_ForPremiumMoreThanLimit_ShouldReturnClampedValue()
    {
        int baseRate = 1000;
        decimal expectedSalary = 1350M;
        int employmentYears = 40;
        
        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

        Sales topLeader = new Sales {Name = "topLeader", BaseRate = baseRate, DateOfEmployment = Utils.CreateYearEmploymentDate(employmentYears) };
        employeePool.AddTopLeader(topLeader);

        decimal salary = salaryService.CalculateEmployeeSalary(topLeader);

        salary.Should().Be(expectedSalary);
    }
    
    [Test]
    public void Calculate_WhenHasSubordinates_ShouldReturnCorrectValue()
    {
        int baseRate = 1000;
        decimal expectedSalary = 1051.222135M;
        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

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
        
        
        decimal salary = salaryService.CalculateEmployeeSalary(s1);
        

        salary.Should().Be(expectedSalary);
    }
}