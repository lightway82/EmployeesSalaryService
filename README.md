## Implementation description

The project implements a hierarchy of classes that model the types of employees. The Employee class implements the regular employee. Employees who can have subordinates (Sales and Manager) inherit the Leader class, which contains a list of subordinates. The Leader class inherits from Employee. The hierarchy of employees stores in the classes that describe them.

EmployeePool that implements the IEmployeePool interface, was created to interact with a hierarchy of employees. This implementation allows the separation of employee entities and operations on them. Also, it allows the creation of an implementation based on files, a database, or an external service to store employees and their relationships.

Payroll is performed by the SalaryService, which implements the ISalaryService interface. A calculator class that implements the ISalaryCalculator interface has been created for each type of employee. Each calculator marks with the CalculateSalaryForAttribute attribute to detect calculators and their corresponding employee classes automatically. Calculators are initialized and provided by the SalaryCalculatorProvider, which implements the ISalaryCalculatorProvider interface. When calculating employee payroll, calculators that require payroll levels below can call their calculators from the SalaryCalculatorProvider by descending the hierarchy tree.

The company's total salary calculator(TotalSalaryCalculator) is based on the employee tree post-order traversal algorithm, which allows you to collect salary data from the bottom layers and pass them to the levels above. The applied algorithm calculates the company's total salary in one pass through the tree. This class is also used for calculating Sales salaries, allowing you to receive total subtree salaries.

## Implementation notes:
1. The current implementation isn't thread-safe.


2. Salary calculators must go to the pool of employees and also receive other calculators (employee calculator does not do that), then these calculators are getting tested through the payroll service. If the employee pool was based on an external service or database, external dependencies could be mocked to speed up tests. Now all the data is in memory, and the tests are fast.


3. We could apply the Visitor pattern to calculate salaries, but this pattern is appropriate for an immutable type hierarchy. The suggested implementation makes it easy to expand the list of types of employees.


4. We assume that the company's hierarchy is unchanged, as well as the data of employees. Changing the name, base rate, employment date, deleting employees, and moving through the hierarchy are not implemented ( although the employees' data fields are changeable). Therefore, an incorrect addition of an employee is irreversible in this implementation.


5. There is a problem that you can add subordinates directly to an employee because the hierarchy information is stored in the employee classes. It can allow you to make direct changes to the hierarchy, bypassing restrictions, for example, using one employee in different places in the hierarchy. The solution is to encapsulate the hierarchical relationships in an EmployeePool. All manipulations with the hierarchy are done through the employee pool to prepare for such changes.


6. EmployeePool does not handle the situation of passing custom types for which there are no calculators. To solve the problem, allow adding calculators and types by registering them in the system.


7. In the current implementation, one has to consider that the employee could not be employed by that time. If you do not store changes to the tree and cannot restore it to the selected date, then calculating salary on any date will be impossible. It's possible to figure salary forecast for the future date. To get this feature in this solution, you must pass the date to the calculator for calculation, get the number of years of employment from Employee, and limit the ability to select dates less than the current one.


## Possible optimizations:
- To optimize the search for employees, you can rewrite the TraverseEmployees method in the EmployeePool class to iterate over dictionary entries which might give a slight performance boost. When traversing a tree, references may be stored in different areas of memory, which will cause a performance drop associated with the processor cache when reading from memory (so-called Cache Missings). In a dictionary, all entries are in an array, which can add performance in a sequential pass. For evaluation, you must conduct a benchmark with the BenchmarkDotNet library.

In the future, you can implement the following functions:
1. Removal of employees from the company structure. To do this, you must define the rules for moving employees of the removed manager. A possible solution could be moving employees to a higher level in the hierarchy.


2. Moving employees to another part of the hierarchy and reassigning their subordinates to another leader. The solution relies on the implemented rules for the movement of employees in the previous section.


3. Changing the type of employee, such as promotion to manager.


4. Replacing an employee with another employee from the company's hierarchy with the removal or swapping of the person being replaced. If the ability to change the type of employee is implemented, then this function can allow you to change the type of transferred employees.


5. Moving the leader along the hierarchy with their subordinates under the leadership of another leader. It is necessary to implement the possibility of deciding to transfer the entire hierarchy of employees or partial transfer with the reassignment of the remaining structure to enable this feature.


6. Encapsulation of hierarchical relations in an employee pool.


7. An implementation of an employee pool that relies on a database.


8. Thread safety implementation.
 
 
 
 
