using EmployeesService;
using EmployeesService.Employees;
using EmployeesService.EmployeesPool;
using EmployeesService.SalaryCalculation;
using FluentAssertions;

namespace EmployeesServiceTests;

public class SalaryServiceTests
{

    [Test]
    public void CalculateEmployeeSalary_ShouldReturnCorrectSalary()
    {
         int baseRate = 1000;
         decimal expectedTopLeaderSalary = 1051.222135M;
        
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


        decimal topLeaderSalary = salaryService.CalculateEmployeeSalary(s1);
         
         topLeaderSalary.Should().Be(expectedTopLeaderSalary);
    }
    
    [Test]
    public void CalculateTotalSalaryOfCompany_ShouldReturnCorrectTotalSalary()
    {
        int baseRate = 1000;
        decimal expectedTotalSalary = 8125.267135M;

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


        decimal totalSalary = salaryService.CalculateTotalSalaryOfCompany();
         
        totalSalary.Should().Be(expectedTotalSalary);
    }
    
     [Test]
    public void CalculateEmployeeSalary_WithSingleEmployeeInPool_ShouldReturnCorrectSalary()
    {
         int baseRate = 1000;
         decimal expectedTopLeaderSalary = baseRate;
        
        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

        Sales s1 = new Sales{ Name = "s1", BaseRate = baseRate , DateOfEmployment = DateTime.Today.Subtract(TimeSpan.FromDays(180))};
        employeePool.AddTopLeader(s1);
        
        
        decimal topLeaderSalary = salaryService.CalculateEmployeeSalary(s1);
         
         topLeaderSalary.Should().Be(expectedTopLeaderSalary);
    }
    
    [Test]
    public void CalculateTotalSalaryOfCompany_WithSingleEmployeeInPool_ShouldReturnCorrectTotalSalary()
    {
        int baseRate = 1000;
        decimal expectedTotalSalary = baseRate;

        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

        Sales s1 = new Sales{ Name = "s1", BaseRate = baseRate , DateOfEmployment = DateTime.Today.Subtract(TimeSpan.FromDays(180))};
       
        employeePool.AddTopLeader(s1);
       
        decimal totalSalary = salaryService.CalculateTotalSalaryOfCompany();
         
        totalSalary.Should().Be(expectedTotalSalary);

    }
    
    [Test]
    public void CalculateTotalSalaryOfCompany_WhenThereIsNoTopLeader_ThrowTopLeaderNotFoundException()
    {
        int baseRate = 1000;
        decimal expectedTotalSalary = baseRate;

        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);
      
        Action act = () => salaryService.CalculateTotalSalaryOfCompany();

        act.Should().Throw<TopLeaderNotFoundException>();

    }
    
    [Test]
    public void CalculateTotalSalaryOfCompany_WhenEmployeesPoolIsEmpty_ThrowTopLeaderNotFoundException()
    {
        IEmployeePool employeePool = new EmployeePool();
        ISalaryService salaryService = new SalaryService(employeePool);

        
        Action  act = () => salaryService.CalculateTotalSalaryOfCompany();
        

        act.Should().ThrowExactly<TopLeaderNotFoundException>();

    }
}