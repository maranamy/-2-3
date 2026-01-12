using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;
using практическая_2.Models;
using практическая_2.Services;
using static практическая_2.Pages.WorkersListPage;

namespace практическая_2.Pages
{
    /// <summary>
    /// Логика взаимодействия для WorkerAddingPage.xaml
    /// </summary>
    public partial class WorkerAddingPage : Page
    {
        private List<Roles> positions = new List<Roles>();
        private WorkerClass currentWorker;
        private WorkerClass changedWorker;
        private int modeId;
        public WorkerAddingPage(int mode, WorkerClass worker)
        {
            InitializeComponent();

            currentWorker = worker;

            modeId = mode;

            string modeType = null;

            switch (mode)
            {
                case 1:
                    AddingMode();
                    modeType = "Добавление сотрудника";
                    break;
                case 2:
                    EditingMode(worker);
                    modeType = "Редакирование данных" ;
                    break;
                case 3:
                    ViewingMode(worker);
                    modeType = "Просмотр данных" ;
                    break;
            }

            DataContext = new ModeClass { ModeType = modeType };

            LoadData();
        }

        public void LoadData()
        {
            var db = EstateAgancyEntities.GetContext();

            var roles = db.Roles.OrderBy(r => r.RoleName).ToList();

            foreach(var role in roles)
            {
                positions.Add(role);
            }
            cmbPosition.ItemsSource = positions;
            cmbPosition.DisplayMemberPath = "RoleName";
            cmbPosition.SelectedValuePath = "Id";
        }

        public void AddingMode()
        {
            SaveAndExitBtn.Visibility = Visibility.Visible;
        }

        public void EditingMode(WorkerClass worker)
        {
            SaveAndExitBtn.Visibility = Visibility.Visible;
            employeeStackPanel.DataContext = worker;

        }

        public void ViewingMode(WorkerClass worker)
        {
            SaveAndExitBtn.Visibility = Visibility.Collapsed;
            EditPhotoBtn.Visibility = Visibility.Collapsed;
            txtFullName.IsEnabled = false;
            cmbPosition.IsEnabled = false;
            dpBirthDate.IsEnabled = false;
            txtAddress.IsEnabled = false;
            txtPhone.IsEnabled = false;
            txtEmail.IsEnabled = false;

            employeeStackPanel.DataContext = worker;
        }

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            switch (modeId)
            {
                case 1:
                    SaveNewData();
                    break;


                case 2:
                    if (IsChangedData())
                    {
                        SaveNewData();
                    }
                    else NavigationService?.GoBack();

                        break;

            }
        }

        public bool IsChangedData()
        {
            changedWorker = new WorkerClass
            {
                Id = currentWorker.Id,
                FullName = txtFullName.Text,
                Position = cmbPosition.Text,
                BirthDate = Convert.ToDateTime(dpBirthDate.Text),
                Address = txtAddress.Text,
                PhoneNumber = txtPhone.Text,
                Email = txtEmail.Text,
                AddedAt = currentWorker.AddedAt,
                Login = txtLogin.Text,
                Password = currentWorker.Password,
            };

            if (changedWorker == currentWorker) return false;
            
            return true;
        }

        public void SaveNewData()
        {
            if (txtFullName.Text == null || cmbPosition.SelectedValue == null ||
    txtPhone.Text == null || txtLogin.Text == null ||
    txtPassword.Text == null)
            {
                MessageBox.Show("Не заполнено одно из обязательных полей", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                string hashPassw = Hash.HashPassw(txtPassword.Text);
                var context = EstateAgancyEntities.GetContext();
                int authId = context.Authoriz.Any() ? context.Authoriz.Max(x => x.ID) + 1 : 1;
                int persDataId = context.PersonalData.Any() ? context.PersonalData.Max(x => x.ID) + 1 : 1;
                //int authId = 1;
                string[] fullName = txtFullName.Text.Split(' ');
                var account = new Authoriz
                {
                    ID = authId,
                    UserLogin = txtLogin.Text,
                    hashPassword = hashPassw,
                    addedAt = DateTime.Now,
                };
                context.Authoriz.Add(account);

                var persData = new PersonalData
                {
                    ID = persDataId,
                    BirthDate = Convert.ToDateTime(dpBirthDate.Text),
                    pAddress = txtAddress.Text ?? null,
                    Phone = txtPhone.Text,
                    Email = txtEmail.Text ?? null,
                    Photo = null,
                };
                context.PersonalData.Add(persData);

                string userType = null;
                var agent = new Workers
                {
                    ID = context.Workers.Any() ? context.Workers.Max(x => x.ID) + 1 : 1,
                    Surname = fullName[0],
                    aName = fullName[1],
                    Patronymic = fullName[2] ?? null,
                    AuthoID = authId,
                    PersonalDataID = persDataId,
                    RoleID = Convert.ToInt32(cmbPosition.SelectedValue)
                };
                context.Workers.Add(agent);
                userType = cmbPosition.SelectedValue.ToString();

                context.SaveChanges();
                try
                {
                    context.SaveChanges();
                    MessageBox.Show($"Сотрудник({userType}) успешно добавлен!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения в БД: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            NavigationService?.GoBack();
        }
    }
}
