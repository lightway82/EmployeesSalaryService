using System.Collections.ObjectModel;
using EmployeesService.Employees;

namespace DemoApp;

public class TreeNode
{
    public Employee Value { get; set; }
    public ObservableCollection<TreeNode> Subordinates { get; set; } = new ObservableCollection<TreeNode>();
}