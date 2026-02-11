using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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

namespace практическая_2.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для WorkerAddingPage.xaml
    /// </summary>
    public partial class WorkerAddingPage : Page
    {
        private List<Roles> positions = new List<Roles>();
        private WorkerClass currentWorker;
        //private WorkerClass changedWorker;

        private readonly string _fullname;
        private readonly string _position;
        private readonly DateTime? _birthdate;
        private readonly string _address;
        private readonly string _phone;
        private readonly string _email;
        private readonly string _login;

        private int modeId;
        public WorkerAddingPage(int mode, WorkerClass worker)
        {
            InitializeComponent();

            currentWorker = worker;

            modeId = mode;

            string modeType = null;

            _fullname = worker.FullName;
            _position = worker.Position;
            _birthdate = worker.BirthDate;
            _address = worker.Address;
            _phone = worker.PhoneNumber;
            _email = worker.Email;
            _login = worker.Login;

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

            var roles = db.Roles.ToList();

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
            txtPassword.Visibility = Visibility.Collapsed;
            tbPassword.Visibility = Visibility.Collapsed;
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
            txtLogin.IsEnabled = false;

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

                    //Debug.WriteLine("--- Маска отладка ---");
                    //Debug.WriteLine($"txtPhone.Text = '{txtPhone.Text ?? "(null)"}'");
                    //Debug.WriteLine($"txtPhone.Value = '{txtPhone.Value ?? "(null)"}'");
                    //Debug.WriteLine($"txtPhone.Value?.ToString() = '{txtPhone.Value?.ToString() ?? "(null)"}'");
                    //Debug.WriteLine($"_phone (твоя переменная) = '{_phone ?? "(null)"}'");
                    //Debug.WriteLine($"Normal(txtPhone.Value?.ToString()) = '{Normal(txtPhone.Value?.ToString()) ?? "(null)"}'");
                    //Debug.WriteLine($"Normal(_phone) = '{Normal(_phone) ?? "(null)"}'");
                    //Debug.WriteLine($"Изменение по Value? = {Normal(txtPhone.Value?.ToString()) != Normal(_phone)}");
                    if (IsChangedData())
                    {
                        SaveUpdatedData();
                    }
                    else NavigationService?.GoBack();

                        break;

            }
        }

        private static string CleanPhone(string value)
        {
            if (value == null) return null;

            string s = value.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(s)) return null;

            // оставляем только цифры
            string digits = new string(s.Where(char.IsDigit).ToArray());

            // не учитываем первую 7
            return digits.Length > 1 ? digits : null;
        }

        public bool IsChangedData()
        {
            EstateAgancyEntities db = EstateAgancyEntities.GetContext();
            string position = db.Roles?.FirstOrDefault(p => p.Id == cmbPosition.SelectedIndex+1).RoleName;


            if (Normal(txtFullName.Text) != Normal(_fullname))
                return true;
            if (Normal(position) != Normal(_position))
                return true;
            if (dpBirthDate.SelectedDate != _birthdate)
                return true;
            if(Normal(txtAddress.Text) != Normal(_address))
                return true;
            if (CleanPhone(txtPhone.Text) != CleanPhone(_phone))
                return true;
            if (Normal(txtEmail.Text) != Normal(_email))
                return true;
            if(Normal(txtLogin.Text) != Normal(_login))
                return true;

            return false;
        }


        //public bool IsChangedData()
        //{
        //    bool changed = false;

        //    changed = txtFullName.Text.Trim() == currentWorker.FullName;
        //    changed = cmbPosition.Text.Trim() == currentWorker.Position;
        //    changed = dpBirthDate.SelectedDate == currentWorker.BirthDate;
        //    changed = txtAddress.Text.Trim() == currentWorker.Address;
        //    changed = txtPhone.Text.Trim() == currentWorker.PhoneNumber;
        //    changed = txtEmail.Text.Trim() == currentWorker.Email;
        //    changed = txtLogin.Text.Trim() == currentWorker.Login;

        //    return changed;
        //}


        //public bool IsChangedData()
        //{
        //    bool changed = false;

        //    if (Normal(txtFullName.Text) != Normal(currentWorker.FullName) || Normal(cmbPosition.Text) != Normal(currentWorker.Position) ||
        //        dpBirthDate.SelectedDate != currentWorker.BirthDate || Normal(txtAddress.Text) != Normal(currentWorker.Address) ||
        //        Normal(txtPhone.Text) != Normal(currentWorker.PhoneNumber) || Normal(txtEmail.Text) != Normal(currentWorker.Email) ||
        //        Normal(txtLogin.Text) != Normal(currentWorker.Login))
        //        changed = true;


        //    return changed;
        //}

        private string Normal(string s)
        {
            return string.IsNullOrEmpty(s) ? null : s.Trim();
        }

        public bool IsEmptyData()
        {
            bool empty = false;

            if (modeId == 1)
                empty = txtFullName.Text.Trim().Equals(string.Empty) || cmbPosition.SelectedIndex == -1 ||
    txtPhone.Text.Trim().Equals(string.Empty) || txtLogin.Text.Trim().Equals(string.Empty) ||
    txtPassword.Text.Trim().Equals(string.Empty);
            else empty = txtFullName.Text.Trim().Equals(string.Empty) || cmbPosition.SelectedIndex == -1 ||
    txtPhone.Text.Trim().Equals(string.Empty) || txtLogin.Text.Trim().Equals(string.Empty);

            return empty;
        }

        public void SaveUpdatedData()
        {
            WorkerClass worker = new WorkerClass(currentWorker.Id, txtFullName.Text, cmbPosition.SelectedValue.ToString(), dpBirthDate.SelectedDate, 
                txtAddress.Text, txtPhone.Text, txtEmail.Text, currentWorker.AddedAt, txtLogin.Text, currentWorker.Password);
            var context = new ValidationContext(worker);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if(!Validator.TryValidateObject(worker, context, results, true))
            {
                MessageBox.Show(
                    "Не удалось сохранить:\n\n" +
                    string.Join("\n", results.Select(err => "• " + err)),
                    "Ошибка ввода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                var db = EstateAgancyEntities.GetContext();
                var user = db.Workers.FirstOrDefault(w => w.ID == currentWorker.Id);
                string[] fullName = txtFullName.Text.Split(' ');

                user.Surname = fullName[0];
                user.aName = fullName[1];
                user.Patronymic = fullName[2] ?? null;
                user.RoleID = cmbPosition.SelectedIndex + 1;

                user.PersonalData.BirthDate = dpBirthDate.SelectedDate ?? null;
                user.PersonalData.pAddress = txtAddress.Text.Trim() ?? null;
                user.PersonalData.Phone = txtPhone.Text.Trim() ?? null;
                user.PersonalData.Email = txtEmail.Text.Trim() ?? null;
                user.PersonalData.Photo = null;

                user.Authoriz.UserLogin = txtLogin.Text.Trim();

                try
                {
                    db.SaveChanges();
                    MessageBox.Show($"Сотрудник({cmbPosition.SelectedValue.ToString()}) успешно обновлён!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService?.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения в БД: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //NavigationService?.GoBack();
        }


        public void SaveNewData()
        {
            string hashPassw = Hash.HashPassw(txtPassword.Text);
            var db = EstateAgancyEntities.GetContext();
            int authId = db.Authoriz.Any() ? db.Authoriz.Max(x => x.ID) + 1 : 1;
            int persDataId = db.PersonalData.Any() ? db.PersonalData.Max(x => x.ID) + 1 : 1;

            WorkerClass worker = new WorkerClass(authId, txtFullName.Text, cmbPosition.SelectedValue.ToString(), dpBirthDate.SelectedDate,
                    txtAddress.Text, txtPhone.Text, txtEmail.Text, DateTime.Now, txtLogin.Text, txtPassword.Text);
            var context = new ValidationContext(worker);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (!Validator.TryValidateObject(worker, context, results, true))
            {
                MessageBox.Show(
                    "Не удалось сохранить:\n\n" +
                    string.Join("\n", results.Select(err => "• " + err)),
                    "Ошибка ввода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }

            //if (IsEmptyData())
            //{
            //    MessageBox.Show("Не заполнено одно из обязательных полей", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //}
            else
            {
                //int authId = 1;
                string[] fullName = txtFullName.Text.Split(' ');
                var account = new Authoriz
                {
                    ID = authId,
                    UserLogin = txtLogin.Text,
                    hashPassword = hashPassw,
                    addedAt = DateTime.Now,
                };
                db.Authoriz.Add(account);

                var persData = new PersonalData
                {
                    ID = persDataId,
                    BirthDate = Convert.ToDateTime(dpBirthDate.Text),
                    pAddress = txtAddress.Text ?? null,
                    Phone = txtPhone.Text,
                    Email = txtEmail.Text ?? null,
                    Photo = null,
                };
                db.PersonalData.Add(persData);

                string userType = null;
                var agent = new Workers
                {
                    ID = db.Workers.Any() ? db.Workers.Max(x => x.ID) + 1 : 1,
                    Surname = fullName[0],
                    aName = fullName[1],
                    Patronymic = fullName[2] ?? null,
                    AuthoID = authId,
                    PersonalDataID = persDataId,
                    RoleID = Convert.ToInt32(cmbPosition.SelectedValue)
                };
                db.Workers.Add(agent);
                userType = cmbPosition.SelectedValue.ToString();

                db.SaveChanges();
                try
                {
                    db.SaveChanges();
                    MessageBox.Show($"Сотрудник({userType}) успешно добавлен!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения в БД: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            NavigationService?.GoBack();
        }

        //private void txtPhone_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var mtb = sender as Xceed.Wpf.Toolkit.MaskedTextBox;
        //    if (mtb == null) return;

        //    // Получаем только цифры (без маски)
        //    string rawDigits = mtb.Value?.ToString() ?? "";  

        //    if (string.IsNullOrEmpty(rawDigits)) return;

        //    if (rawDigits.StartsWith("8"))
        //    {
        //        // Заменяем 8 на 7
        //        string corrected = "7" + rawDigits.Substring(1);

        //        // Форматируем обратно под маску
        //        mtb.Value = corrected;   // или mtb.Text = FormatToMask(corrected);
        //        mtb.CaretIndex = mtb.Text.Length;
        //    }
        //}
    }
}
