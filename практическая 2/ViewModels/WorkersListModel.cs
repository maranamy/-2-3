using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using практическая_2.Services;

namespace практическая_2.ViewModels
{
    internal class WorkersListModel: ObservableObject
    {
        public ObservableCollection<WorkerClass> Workers { get; }

        public WorkersListModel()
        {
            Workers = new ObservableCollection<WorkerClass>();
        }


        // Пример команд, которые вызываются из меню
        [RelayCommand]
        private void OpenProfile(WorkerClass emp) => MessageBox.Show($"Профиль: {emp?.FullName}");

        [RelayCommand]
        private void EditEmployee(WorkerClass emp) => MessageBox.Show($"Редактировать: {emp?.FullName}");

        [RelayCommand]
        private void DeleteEmployee(WorkerClass emp)
        {
            if (emp != null) Workers.Remove(emp);
        }
    }
}
