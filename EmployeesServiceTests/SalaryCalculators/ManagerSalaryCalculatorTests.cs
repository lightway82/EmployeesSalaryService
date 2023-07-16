using EmployeesService.Employees;
using EmployeesService.EmployeesPool;
using EmployeesService.SalaryCalculation;
using FluentAssertions;

namespace EmployeesServiceTests.SalaryCalculators;

public class ManagerSalaryCalculatorTests
{
    [Test]
    public void Calculate_ForFirstYearManager_ShouldReturnCorrectValue()
    {
        int baseRate = 1000;
        decimal expectedSalary = 1000M;
        
        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

        Manager topLeader = new Manager() {Name = "topLeader", BaseRate = baseRate, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        employeePool.AddTopLeader(topLeader);

        decimal salary = salaryService.CalculateEmployeeSalary(topLeader);

        salary.Should().Be(expectedSalary);
    }

    [Test]
    public void Calculate_ForSeveralYearsManager_ShouldReturnCorrectValue()
    {
        int baseRate = 1000;
        decimal expectedSalary = 1150M;
        int employmentYears = 3;
        
        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

        Manager topLeader = new Manager() {Name = "topLeader", BaseRate = baseRate, DateOfEmployment = Utils.CreateYearEmploymentDate(employmentYears) };
        employeePool.AddTopLeader(topLeader);

        decimal salary = salaryService.CalculateEmployeeSalary(topLeader);

        salary.Should().Be(expectedSalary);
    }

    [Test]
    public void Calculate_ForPremiumMoreThanLimit_ShouldReturnClampedValue()
    {
        int baseRate = 1000;
        decimal expectedSalary = 1400M;
        int employmentYears = 10;
        
        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

        Manager topLeader = new Manager() {Name = "topLeader", BaseRate = baseRate, DateOfEmployment = Utils.CreateYearEmploymentDate(employmentYears) };
        employeePool.AddTopLeader(topLeader);

        decimal salary = salaryService.CalculateEmployeeSalary(topLeader);

        salary.Should().Be(expectedSalary);
    }
    
    [Test]
    public void Calculate_WhenHasSubordinates_ShouldReturnCorrectValue()
    {
        int baseRate = 1000;
        decimal expectedSalary = 1165.025475M;
        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

        Manager m1 = new Manager {Name = "m1", BaseRate = baseRate, DateOfEmployment = Utils.CreateYearEmploymentDate(3) };
       
        Manager m2 = new Manager {Name = "m2", BaseRate = baseRate, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Manager m3 = new Manager {Name = "m3", BaseRate = baseRate, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
       
        Employee e1 = new Employee {Name = "e1", BaseRate = baseRate, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e2 = new Employee {Name = "e2", BaseRate = baseRate, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e3 = new Employee {Name = "e3", BaseRate = baseRate, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e4 = new Employee {Name = "e4", BaseRate = baseRate, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
      
        Sales s = new Sales {Name = "s", BaseRate = baseRate, DateOfEmployment = Utils.CreateYearEmploymentDate(1)  };
        
        employeePool.AddTopLeader(m1);
        employeePool.AddEmployee(m2, m1);
        employeePool.AddEmployee(e1, m1);
        employeePool.AddEmployee(e2, m1);
        employeePool.AddEmployee(s, m2);
        
        employeePool.AddEmployee(m3, s);
        employeePool.AddEmployee(e3, s);
        employeePool.AddEmployee(e4, s);


        decimal salary = salaryService.CalculateEmployeeSalary(m1);
        

        salary.Should().Be(expectedSalary);
    }

}