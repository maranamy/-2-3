using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using практическая_2.Models;
using практическая_2.Services;

namespace практическая_2.Pages
{
    /// <summary>
    /// Логика взаимодействия для WorkersListPage.xaml
    /// </summary>
    public partial class WorkersListPage : Page
    {
        public class Filtration
        {
            public int ID { get; set; }
            public string TypeSorting { get; set; }
        }

        public WorkersListPage()
        {
            InitializeComponent();

            var filtration = new List<Filtration> 
            {
                new Filtration{ID = 1, TypeSorting = "по дате добавления"},
                new Filtration{ID = 2, TypeSorting = "по должности"},
                new Filtration{ID = 3, TypeSorting = "по фамилии"}
            };
            cmbFilter.ItemsSource = filtration;
            cmbFilter.DisplayMemberPath = "TypeSorting";
            cmbFilter.SelectedValuePath = "ID";

            LoadEmployees();
        }
    
    
        private List<WorkerClass> allEmployees = new List<WorkerClass>();  // Полный список


        private void LoadEmployees()
        {
            allEmployees.Clear();

            EstateAgancyEntities db = EstateAgancyEntities.GetContext();

            var workersFromDb = db.Workers
                .Include("Roles").Include("Authoriz")  
                //.OrderBy(w => w.Surname)
                .ToList();

            foreach (var worker in workersFromDb)
            {
                // Формируем ФИО
                string fullName = $"{worker.Surname ?? ""} {worker.aName ?? ""} {worker.Patronymic ?? ""}".Trim();
                if (string.IsNullOrWhiteSpace(fullName))
                    fullName = "Не указано";

                
                string position = worker.Roles?.RoleName ?? "Должность не указана";
                
                DateTime addedAt = worker.Authoriz?.addedAt ?? DateTime.Now;

                allEmployees.Add(new WorkerClass
                {
                    Id = worker.ID,
                    FullName = fullName,
                    Position = position,
                    AddedAt = addedAt
                });
            }

            // карточки перерисуются автоматически
            EmployeesItemsControl.ItemsSource = allEmployees;

        }

        public void FilteringMode()
        {
            var filteringList = allEmployees.AsQueryable();
            string inputFio = txtSearch.Text.Trim();
            if (inputFio != null)
                filteringList = filteringList.Where(w => w.FullName.ToLower().Contains(inputFio.ToLower()));
            
            if (cmbFilter.SelectedValue is int filterValue)
            {
                switch (filterValue)
                {
                    case 1:
                        filteringList = filteringList.OrderBy(w => w.AddedAt);
                        break;
                    case 2:
                        filteringList = filteringList.OrderBy(w => w.Position);
                        break;
                    case 3:
                        filteringList = filteringList.OrderBy(w => w.FullName);
                        break;
                    default:
                        break;
                }
            }

            EmployeesItemsControl.ItemsSource = filteringList;  // обновляем источник для отрисовки данных
        }

        private void txtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            FilteringMode();   
        }

        private void cmbFilter_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            FilteringMode();
        }
    }
}
