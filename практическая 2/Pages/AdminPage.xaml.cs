using System;
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
using практическая_2.Models;
using практическая_2.Services;
using практическая_2.Pages;

namespace практическая_2.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage(Authoriz user, string role)
        {
            InitializeComponent();
            Greeting(user);
        }

        public void Greeting(Authoriz autho)
        {
            EstateAgancyEntities db = EstateAgancyEntities.GetContext();
            var user = db.Workers.Where(x => x.AuthoID == autho.ID).FirstOrDefault();

            if (user != null)
            {
                AdminGreeting.Text = CheckWorkTime.CheckDayTime() + NameSurname(user);
            }
        }

        public string NameSurname(Workers user)
        {
            return user.aName + " " + user.Patronymic;
        }

        private void WorkWithUsers_ClickBtn(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WorkersListPage());
        }
    }
}
