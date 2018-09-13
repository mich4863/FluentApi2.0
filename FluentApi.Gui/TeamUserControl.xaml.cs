﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FluentApi.Ef;

namespace FluentApi.Gui
{
    /// <summary>
    /// Interaction logic for TeamUserControl.xaml
    /// </summary>
    public partial class TeamUserControl :UserControl
    {
        protected Model model;
        private Team selectedTeam;
        private Employee selectedEmployee;
        private List<Employee> teamEmployees;

        public TeamUserControl()
        {
            InitializeComponent();
            model = new Model();
            dataGridTeams.ItemsSource = model.Teams.ToList();
            this.DataContext = selectedTeam;      
        }

        private void ReloadAllTeams()
        {
            dataGridAddOrDeleteFromTeam.ItemsSource = null;
            dataGridAddOrDeleteFromTeam.ItemsSource = teamEmployees;

            dataGridTeams.ItemsSource = model.Teams.ToList();        
        }
        
        private void Button_AddEmployeeToTeam_Click(object sender, RoutedEventArgs e)
        {
            selectedEmployee = comboBoxAllEmployees.SelectedItem as Employee;
            selectedEmployee.TeamId = selectedTeam.Id;
            selectedTeam.MembersOfTeam++;

            teamEmployees.Add(selectedEmployee);
            model.SaveChanges();
            ReloadAllTeams();
            
            comboBoxAllEmployees.Text = "Vælg medarbjeder";
        }

        private void Button_SaveNewTeam_Click(object sender, RoutedEventArgs e)
        {
            Team team = new Team();
            team.Name = textBoxTeamName.Text;
            team.StartDate = datePickerTeamStartDate.SelectedDate;
            team.Description = textBoxTeamDescription.Text;
            team.ExpectedEndDate = datePickerTeamExpectedEndDate.SelectedDate;

            model.Teams.Add(team);
            model.SaveChanges();
            ReloadAllTeams();
        }

        private void DataGrid_Teams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            if(selectedTeam != null)
            {
                /* Load only Employees that are not members of the selected team into ComboBox  */
                List<Employee> employeeCombo = model.Employees.ToList();
                List<Employee> elpo = employeeCombo.Where(employee => employee.TeamId != selectedTeam.Id).ToList();
                comboBoxAllEmployees.ItemsSource = elpo;

                /* Load the selected team into the textboxes and datepicker's */
                textBoxTeamName.Text = selectedTeam.Name;
                datePickerTeamStartDate.SelectedDate = selectedTeam.StartDate;
                textBoxTeamDescription.Text = selectedTeam.Description;
                datePickerTeamExpectedEndDate.SelectedDate = selectedTeam.ExpectedEndDate;

                /* Load the team's members into datagrid */
                List<Employee> employees = model.Employees.ToList();
                teamEmployees = employees.Where(employee => employee.TeamId == selectedTeam.Id).ToList();
                dataGridAddOrDeleteFromTeam.ItemsSource = teamEmployees;

                /* Count the members of selected team and set the property MembersOfTeam to the new member number. */
                int count = teamEmployees.Count();
                selectedTeam.MembersOfTeam = count;
            }
        }

        private void Button_UpdateTeam_Click(object sender, RoutedEventArgs e)
        {
            if(selectedTeam != null)
            {
                if(textBoxTeamName.Text != selectedTeam.Name || textBoxTeamDescription.Text != selectedTeam.Description || datePickerTeamStartDate.SelectedDate != selectedTeam.StartDate || datePickerTeamExpectedEndDate.SelectedDate != selectedTeam.ExpectedEndDate)
                {
                    selectedTeam.Name = textBoxTeamName.Text;
                    selectedTeam.Description = textBoxTeamDescription.Text;
                    selectedTeam.StartDate = datePickerTeamStartDate.SelectedDate.Value;
                    selectedTeam.ExpectedEndDate = datePickerTeamExpectedEndDate.SelectedDate.Value;
                }
                model.SaveChanges();
                ReloadAllTeams();
            }
        }

        private void DataGrid_Teams_KeyDown(object sender, KeyEventArgs e)
        {
            if(dataGridTeams.SelectedItem != null)
            {
                if(e.Key == Key.Escape)
                {
                    dataGridTeams.SelectedItem = selectedTeam = null;

                    textBoxTeamName.Text = String.Empty;
                    datePickerTeamStartDate.Text = null;
                    textBoxTeamDescription.Text = String.Empty;
                    datePickerTeamExpectedEndDate.Text = null;
                    comboBoxAllEmployees.Text = "Vælg medarbjeder";
                    dataGridAddOrDeleteFromTeam.ItemsSource = null;
                    comboBoxAllEmployees.ItemsSource = null;

                    textBoxTeamName.Focus();
                }
            }
        }

        private void Button_DeleteEmployeeFromTeam_Click(object sender, RoutedEventArgs e)
        {
            selectedEmployee = dataGridAddOrDeleteFromTeam.SelectedItem as Employee;
            selectedEmployee.TeamId = null;
            selectedTeam.MembersOfTeam--;
            
            model.SaveChanges();
            ReloadAllTeams();
        }
    }
}
