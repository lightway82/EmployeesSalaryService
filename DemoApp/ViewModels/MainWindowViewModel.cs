using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using EmployeesService.Employees;
using EmployeesService.EmployeesPool;
using EmployeesService.SalaryCalculation;
using ReactiveUI;

namespace DemoApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    #region Properties

    public ObservableCollection<TreeNode> Employees { get; set; }

    public ReactiveCommand<Unit, Unit> AddEmployeeCommand { get; set; }
    public ReactiveCommand<Unit, Unit> AddTopLeaderCommand { get; set; }

    public ReactiveCommand<Unit, Unit> CalculateCompanySalaryCommand { get; set; }
    public ReactiveCommand<Unit, Unit> CalculateEmployeeSalaryCommand { get; set; }

    private decimal _employeeSalary;

    public decimal EmployeeSalary
    {
        get => _employeeSalary;
        set => this.RaiseAndSetIfChanged(ref _employeeSalary, value);
    }


    private decimal _totalSalary;

    public decimal TotalSalary
    {
        get => _totalSalary;
        set => this.RaiseAndSetIfChanged(ref _totalSalary, value);
    }

    private bool _hasTopLeader;

    public bool HasTopLeader
    {
        get => _hasTopLeader;
        set => this.RaiseAndSetIfChanged(ref _hasTopLeader, value);
    }

    private TreeNode? _selectedTreeNode;

    public TreeNode? SelectedTreeNode
    {
        get => _selectedTreeNode;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTreeNode, value);
            OnSelectedEmployee(value);
        }
    }

    private readonly Employee _emptyEmployee;

    private Employee _selectedEmployee;

    public Employee SelectedEmployee
    {
        get => _selectedEmployee;
        set => this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
    }

    private void OnSelectedEmployee(TreeNode? value)
    {
        EmployeeSalary = 0;
        if (value is null)
        {
            SelectedEmployee = _emptyEmployee;
            return;
        }

        SelectedEmployee = value.Value;
    }

    private string _employeeName = "";

    public string EmployeeName
    {
        get => _employeeName;
        set => this.RaiseAndSetIfChanged(ref _employeeName, value);
    }

    private int _employeeBaseRate;

    public int EmployeeBaseRate
    {
        get => _employeeBaseRate;
        set => this.RaiseAndSetIfChanged(ref _employeeBaseRate, value);
    }

    private DateTime _employmentDate = DateTime.Now;

    public DateTime EmploymentDate
    {
        get => _employmentDate;
        set => this.RaiseAndSetIfChanged(ref _employmentDate, value);
    }

    private Type _employeeType = typeof(Employee);

    public Type EmployeeType
    {
        get => _employeeType;
        set => this.RaiseAndSetIfChanged(ref _employeeType, value);
    }

    #endregion


    private readonly IEmployeePool _employeePool;
    private readonly ISalaryService _salaryService;

    public MainWindowViewModel()
    {
        _emptyEmployee = new Employee() { BaseRate = 0, Name = "", DateOfEmployment = DateTime.Now };
        _selectedEmployee = _emptyEmployee;
        _employeePool = new EmployeePool();
        _salaryService = new SalaryService(_employeePool);
        Employees = new ObservableCollection<TreeNode>();
        AddEmployeeCommand = ReactiveCommand.Create(AddEmployee);
        AddTopLeaderCommand = ReactiveCommand.Create(AddTopLeader);
        CalculateCompanySalaryCommand = ReactiveCommand.Create(CalculateCompanySalary);
        CalculateEmployeeSalaryCommand = ReactiveCommand.Create(CalculateEmployeeSalary);
    }

    private void CalculateEmployeeSalary()
    {
        if (SelectedTreeNode is null) return;
        EmployeeSalary = _salaryService.CalculateEmployeeSalary(SelectedTreeNode.Value);
    }

    private void CalculateCompanySalary()
    {
        if (_employeePool.TopLevelLeader is null) return;
        TotalSalary = _salaryService.CalculateTotalSalaryOfCompany();
    }


    private void AddTopLeader()
    {
        if (EmployeeType == typeof(Employee))
        {
            SukiUI.MessageBox.MessageBox.Info(GetMainWindow(), "Creating top leader",
                "Top leader type must be Manager or Sales");
            return;
        }

        if (EmployeeBaseRate <= 0)
        {
            SukiUI.MessageBox.MessageBox.Info(GetMainWindow(), "Creating top leader", "The base rate must be greater than zero");
            return;
        }
        
        if (EmployeeName.Length == 0)
        {
            SukiUI.MessageBox.MessageBox.Info(GetMainWindow(), "Creating top leader", "You need to set employee name.");
            return;
        }

        Leader? leader = Activator.CreateInstance(EmployeeType) as Leader;
        if (leader is null)
        {
            SukiUI.MessageBox.MessageBox.Error(GetMainWindow(), "Creating top leader",
                $"Instance of {EmployeeType.Name} not created.");
            return;
        }

        leader.Name = EmployeeName;
        leader.BaseRate = EmployeeBaseRate;
        leader.DateOfEmployment = EmploymentDate;
        try
        {
            _employeePool.AddTopLeader(leader);

            TreeNode treeNode = new TreeNode()
            {
                Value = leader
            };
            Employees.Add(treeNode);
            HasTopLeader = true;
        }
        catch (Exception e)
        {
            SukiUI.MessageBox.MessageBox.Error(GetMainWindow(), "Creating top leader", e.Message);
        }
    }


    private void AddEmployee()
    {
        if (SelectedTreeNode is null) return;

        if (SelectedTreeNode.Value is not Leader)
        {
            SukiUI.MessageBox.MessageBox.Info(GetMainWindow(), "Creating employee",
                "You can't add subordinate to employee");
            return;
        }

        if (EmployeeBaseRate <= 0)
        {
            SukiUI.MessageBox.MessageBox.Info(GetMainWindow(), "Creating top leader", "The base rate must be greater than zero");
            return;
        }
        
        if (EmployeeName.Length == 0)
        {
            SukiUI.MessageBox.MessageBox.Info(GetMainWindow(), "Creating employee", "You need to set employee name.");
            return;
        }

        Employee? employee = Activator.CreateInstance(EmployeeType) as Employee;
        if (employee is null)
        {
            SukiUI.MessageBox.MessageBox.Error(GetMainWindow(), "Creating employee",
                $"Instance of {EmployeeType.Name} not created.");
            return;
        }

        employee.Name = EmployeeName;
        employee.BaseRate = EmployeeBaseRate;
        employee.DateOfEmployment = EmploymentDate;
        try
        {
            _employeePool.AddEmployee(employee, (Leader)SelectedTreeNode.Value);

            TreeNode treeNode = new TreeNode()
            {
                Value = employee
            };

            SelectedTreeNode.Subordinates.Add(treeNode);
            EmployeeSalary = 0;
        }
        catch (Exception e)
        {
            SukiUI.MessageBox.MessageBox.Error(GetMainWindow(), "Creating employee", e.Message);
        }
    }

    private Window? GetMainWindow() =>
        Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
}