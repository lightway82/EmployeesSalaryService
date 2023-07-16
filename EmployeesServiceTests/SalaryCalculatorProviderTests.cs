using EmployeesService.Employees;
using EmployeesService.EmployeesPool;
using EmployeesService.SalaryCalculation;
using FluentAssertions;
using Moq;

namespace EmployeesServiceTests;

public class SalaryCalculatorProviderTests
{

   [Test]
   public void WhenInitProvider_NotException()
   {
    Action act = () =>  new SalaryCalculatorProvider(new Mock<IEmployeePool>().Object);
    
    act.Should().NotThrow();
   }
   
   /// <summary>
   /// Test needs change if employees types changed
   /// </summary>
   [Test]
   public void WhenInitProvider_NotException1()
   {
       ISalaryCalculatorProvider salaryCalculatorProvider = new SalaryCalculatorProvider(new Mock<IEmployeePool>().Object);
       
       salaryCalculatorProvider.GetCalculator(typeof(Employee)).Should().NotBeNull();
       salaryCalculatorProvider.GetCalculator(typeof(Manager)).Should().NotBeNull();
       salaryCalculatorProvider.GetCalculator(typeof(Sales)).Should().NotBeNull();
   }

}