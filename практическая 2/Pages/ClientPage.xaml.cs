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

namespace практическая_2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage(Authoriz user)
        {
            InitializeComponent();
            Greeting(user);
        }

        public void Greeting(Authoriz autho)
        {
            EstateAgancyEntities db = EstateAgancyEntities.GetContext();
            if(autho !=null) 
            {
                var user = db.Clients.Where(x => x.AuthoID == autho.ID).FirstOrDefault();
                ClientGreeting.Text = CheckWorkTime.CheckDayTime() + NameSurname(user);
            }
            else
            {
                ClientGreeting.Text = "Добро пожаловать! Вы вошли как гость!";
            }
        }

        public string NameSurname(Clients user)
        {
            return user.cName + " " + user.Patronymic;
        }

    }
}
