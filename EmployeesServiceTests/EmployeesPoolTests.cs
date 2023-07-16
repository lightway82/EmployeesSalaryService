using EmployeesService;
using EmployeesService.Employees;
using EmployeesService.EmployeesPool;
using FluentAssertions;

namespace EmployeesServiceTests;

public class EmployeesPoolTests
{
    [Test]
    public void AddTopLevelLeader_WhenEmployeeIsNotNull_ThenTopLevelLeaderReturnAddedEmployee()
    {
        IEmployeePool employeePool = new EmployeePool();
        Sales topLeader = new Sales{ Name = "s1", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate};
        Employee? topLevelLeaderResult = null;

        Action act = () =>
        {
            employeePool.AddTopLeader(topLeader);
            topLevelLeaderResult = employeePool.TopLevelLeader;
        };

        act.Should().NotThrow();
        topLevelLeaderResult.Should().NotBeNull().And.BeSameAs(topLeader);
    }
    
    [Test]
    public void AddTopLevelLeader_WhenEmployeeEmploymentDateGreaterThanNow_ThrowIncorrectDeploymentEmployeeDateException()
    {
        IEmployeePool employeePool = new EmployeePool();
        Sales topLeader = new Sales{ Name = "s1", BaseRate = 1000 , DateOfEmployment = DateTime.Now+TimeSpan.FromMinutes(10)};
     

        Action act = () =>
        {
            employeePool.AddTopLeader(topLeader);
        };

        act.Should().Throw<IncorrectEmploymentDateException>();
       
    }
  
    [Test]
    public void AddTopLevelLeader_WhenEmployeeIsSameThatTopLeader_ThrowRemoveTopLeaderAttemptException()
    {
        IEmployeePool employeePool = new EmployeePool();
        Sales topLeader = new Sales{ Name = "s1", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate};
        employeePool.AddTopLeader(topLeader);

        Action act = () =>
        {
            employeePool.AddTopLeader(topLeader);
         
        };

        act.Should().Throw<RemoveTopLeaderAttemptException>();
    }
    
    [Test]
    public void AddTopLevelLeader_WhenDuplicatedEmployee_Throw()
    {
        IEmployeePool employeePool = new EmployeePool();
        Sales topLeader = new Sales{ Name = "topLeader", BaseRate = 1000 , DateOfEmployment = Utils.CreateFirstYearEmploymentDate};
        employeePool.AddTopLeader(topLeader);

        Action act = () =>
        {
            employeePool.AddTopLeader(topLeader);
        };

        act.Should().Throw<RemoveTopLeaderAttemptException>();
       
    }
    
    
    [Test]
    public void GetLeader_WhenEmployeeAddedToLeader_ShouldReturnLeader()
    {
        IEmployeePool employeePool = new EmployeePool();
        Sales topLeader = new Sales{ Name = "topLeader", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate};
        employeePool.AddTopLeader(topLeader);
        Employee employee = new Employee{ Name = "employee", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate};
        employeePool.AddEmployee(employee, topLeader);

        Leader? leader = employeePool.GetLeader(employee);

        leader.Should().NotBeNull().And.BeSameAs(topLeader);
    }
    
    
    [Test]
    public void GetSubordinate_WhenEmployeesAddedToLeader_ShouldReturnSubordinates()
    {
        IEmployeePool employeePool = new EmployeePool();
        Sales topLeader = new Sales{ Name = "topLeader", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate};
        employeePool.AddTopLeader(topLeader);

        Employee[] employees = {
            new Employee{ Name = "employee1", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate},
            new Employee{ Name = "employee2", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate},
            new Employee{ Name = "employee3", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate}
        };
        foreach (var employee in employees)
        {
            employeePool.AddEmployee(employee, topLeader);
        }

        
        IEnumerable<Employee> subordinate = employeePool.GetSubordinate(topLeader);

        
        subordinate.Should().BeEquivalentTo(employees);
    }
    
    [Test]
    public void AddEmployee_WhenAddedEmployee_NotException()
    {
        IEmployeePool employeePool = new EmployeePool();
        Sales topLeader = new Sales{ Name = "topLeader", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate};
        employeePool.AddTopLeader(topLeader);
        Employee employee = new Employee{ Name = "employee", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate};
        
        
       Action act = () => employeePool.AddEmployee(employee, topLeader);

       act.Should().NotThrow();
       employeePool.GetLeader(employee).Should().NotBeNull().And.BeSameAs(topLeader);
    }
    
    [Test]
    public void AddEmployee_WhenAddedLeaderToThemself_ThrowSelfSubordinateException()
    {
        IEmployeePool employeePool = new EmployeePool();
        Sales topLeader = new Sales{ Name = "topLeader", BaseRate = 1000 , DateOfEmployment = Utils.CreateFourthYearEmploymentDate};
        employeePool.AddTopLeader(topLeader);
       
        Action act = () => employeePool.AddEmployee(topLeader, topLeader);

        act.Should().ThrowExactly<SelfSubordinateException>();
    }
    
    [Test]
    public void AddEmployee_WhenEmployeeEmploymentDateGreaterThanNow_ThrowIncorrectDeploymentEmployeeDateException()
    {
        IEmployeePool employeePool = new EmployeePool();
        Sales topLeader = new Sales{ Name = "topLeader", BaseRate = 1000 , DateOfEmployment = Utils.CreateFirstYearEmploymentDate};
        employeePool.AddTopLeader(topLeader);
        Employee employee = new Employee{ Name = "employee", BaseRate = 1000 ,  DateOfEmployment = DateTime.Now+TimeSpan.FromMinutes(10)};
       

        Action act = () =>
        {
            employeePool.AddEmployee(employee, topLeader);
        };

        act.Should().Throw<IncorrectEmploymentDateException>();
       
    }
    
    [Test]
    public void AddEmployee_WhenDuplicatedEmployee_ThrowDuplicatedEmployeeException()
    {
        IEmployeePool employeePool = new EmployeePool();
        Sales topLeader = new Sales{ Name = "topLeader", BaseRate = 1000 , DateOfEmployment = Utils.CreateFirstYearEmploymentDate};
        employeePool.AddTopLeader(topLeader);
        Employee employee = new Employee{ Name = "employee", BaseRate = 1000 ,  DateOfEmployment = Utils.CreateFirstYearEmploymentDate};
        employeePool.AddEmployee(employee, topLeader);
       

        Action act = () =>
        {
            employeePool.AddEmployee(employee, topLeader);
        };

        act.Should().Throw<DuplicatedEmployeeException>();
       
    }
    
    
    [Test]
    public void GetEmployeeByName_ShouldReturnEmployeesWithRequestedName()
    {
        IEmployeePool employeePool = new EmployeePool();

        Manager m1 = new Manager {Name = "m1", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
       
        Manager m2 = new Manager {Name = "m2", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Manager m3 = new Manager {Name = "m3", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
       
        Employee e1 = new Employee {Name = "e1", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e2 = new Employee {Name = "e2", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e3 = new Employee {Name = "Same name", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e4 = new Employee {Name = "Same name", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
      
        Sales s = new Sales {Name = "s", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate  };
        
        employeePool.AddTopLeader(m1);
        employeePool.AddEmployee(m2, m1);
        employeePool.AddEmployee(e1, m1);
        employeePool.AddEmployee(e2, m1);
        employeePool.AddEmployee(s, m2);
        
        employeePool.AddEmployee(m3, s);
        employeePool.AddEmployee(e3, s);
        employeePool.AddEmployee(e4, s);

        var employeeByName = employeePool.GetEmployeeByName("Same name");

        employeeByName.Should().Contain(new[] { e3, e4 });
    }
    
    [Test]
    public void GetEmployeeByName_WhenThereIsNoEmployees_ShouldReturnEmptySequence()
    {
        IEmployeePool employeePool = new EmployeePool();

        var employeeByName = employeePool.GetEmployeeByName("Same name");

        employeeByName.Should().BeEmpty();
    }
    
    [Test]
    public void FindEmployeeByNameFullText_ShouldReturnEmployeesWithRequestedNamePart()
    {
        IEmployeePool employeePool = new EmployeePool();

        Manager m1 = new Manager {Name = "m1", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
       
        Manager m2 = new Manager {Name = "m2", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Manager m3 = new Manager {Name = "Not same name", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
       
        Employee e1 = new Employee {Name = "e1", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e2 = new Employee {Name = "e2", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e3 = new Employee {Name = "e3", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e4 = new Employee {Name = "Same name", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
      
        Sales s = new Sales {Name = "s", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate  };
        
        employeePool.AddTopLeader(m1);
        employeePool.AddEmployee(m2, m1);
        employeePool.AddEmployee(e1, m1);
        employeePool.AddEmployee(e2, m1);
        employeePool.AddEmployee(s, m2);
        
        employeePool.AddEmployee(m3, s);
        employeePool.AddEmployee(e3, s);
        employeePool.AddEmployee(e4, s);

        var employeeByName = employeePool.FindEmployeeByNameFullText("ame name");

        employeeByName.Should().Contain(new[] { m3, e4 });
    }
    
    [Test]
    public void FindEmployee_ShouldReturnEmployeesByPredicate()
    {
        IEmployeePool employeePool = new EmployeePool();

        Manager m1 = new Manager {Name = "m1", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
       
        Manager m2 = new Manager {Name = "m2", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Manager m3 = new Manager {Name = "Not same name", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
       
        Employee e1 = new Employee {Name = "e1", BaseRate = 10000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e2 = new Employee {Name = "e2", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e3 = new Employee {Name = "e3", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e4 = new Employee {Name = "Same name", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
      
        Sales s = new Sales {Name = "s", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate  };
        
        employeePool.AddTopLeader(m1);
        employeePool.AddEmployee(m2, m1);
        employeePool.AddEmployee(e1, m1);
        employeePool.AddEmployee(e2, m1);
        employeePool.AddEmployee(s, m2);
        
        employeePool.AddEmployee(m3, s);
        employeePool.AddEmployee(e3, s);
        employeePool.AddEmployee(e4, s);

        var employeeByName = employeePool.FindEmployee(e => e.BaseRate > 1000 || e.Name=="Same name");

        employeeByName.Should().Contain(new[] { e1, e4 });
    }
    
    [Test]
    public void FindEmployee_WhenThereIsNoTopLeader_ShouldReturnEmptySequence()
    {
        IEmployeePool employeePool = new EmployeePool();
        

        var employees = employeePool.FindEmployee(e => e.BaseRate > 1000 || e.Name == "Same name");


        employees.Should().BeEmpty();


    }

    [Test] public void AllEmployee_ShouldReturnAllEmployees()
    {
        IEmployeePool employeePool = new EmployeePool();

        Manager m1 = new Manager {Name = "m1", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
       
        Manager m2 = new Manager {Name = "m2", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate };
        Manager m3 = new Manager {Name = "Not same name", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
       
        Employee e1 = new Employee {Name = "e1", BaseRate = 10000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e2 = new Employee {Name = "e2", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e3 = new Employee {Name = "e3", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
        Employee e4 = new Employee {Name = "Same name", BaseRate = 1000, DateOfEmployment =  Utils.CreateFirstYearEmploymentDate };
      
        Sales s = new Sales {Name = "s", BaseRate = 1000, DateOfEmployment = Utils.CreateFirstYearEmploymentDate  };
        
        employeePool.AddTopLeader(m1);
        employeePool.AddEmployee(m2, m1);
        employeePool.AddEmployee(e1, m1);
        employeePool.AddEmployee(e2, m1);
        employeePool.AddEmployee(s, m2);
        
        employeePool.AddEmployee(m3, s);
        employeePool.AddEmployee(e3, s);
        employeePool.AddEmployee(e4, s);
        

        var allEmployees = employeePool.AllEmployee();
        
        
        Employee[] expectedEmployees =  new[] { m1, m2, m3, e1, e2, e3, e4, s };
        allEmployees.Should().Contain(expectedEmployees).And.HaveCount(expectedEmployees.Length);
    }
    
    
    [Test] public void AllEmployee_ThereIsNoTopLeader_ShouldReturnEmptySequence()
    {
        IEmployeePool employeePool = new EmployeePool();

        var allEmployees = employeePool.AllEmployee();

        allEmployees.Should().BeEmpty();
    }

}
