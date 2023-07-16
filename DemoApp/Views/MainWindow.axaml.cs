using System;
using Avalonia.Controls;
using EmployeesService.Employees;

namespace DemoApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        SetupEmployeeTypeBox();
        SetupEmploymentDatePicker();

    }

    private void SetupEmploymentDatePicker()
    {
        employmentDatePicker.SelectedDate = DateTime.Now;
    }

    private void SetupEmployeeTypeBox()
    {
        typeEmployee.Items.Add(typeof(Employee));
        typeEmployee.Items.Add(typeof(Manager));
        typeEmployee.Items.Add(typeof(Sales));
        typeEmployee.SelectedIndex = 0;
    }
}