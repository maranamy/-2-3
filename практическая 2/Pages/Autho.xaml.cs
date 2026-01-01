using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Threading;
using практическая_2.Models;
using практическая_2.Services;

namespace практическая_2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {

        int click;
        public DispatcherTimer timer;
        int secondsCountdown = 10;
        public Autho()
        {
            InitializeComponent();
            click = 0;
            InitializeTimer();
        }

        private void btn_EnterAsGuest(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientPage(null));
        }

        private void GenCapcha()
        {
            tblockCapthca.Visibility = Visibility.Visible;
            tbxCaptcha.Visibility = Visibility.Visible;
            tblOutCapcha.Visibility = Visibility.Visible;

            tblOutCapcha.Text = CaptchaGen.GenCaptchaText(6);
            tblOutCapcha.TextDecorations = TextDecorations.Strikethrough;
        }

        private void btn_Enter(object sender, RoutedEventArgs e)
        {
            click += 1;
            string login = tbxLogin.Text.Trim();
            string password = Hash.HashPassw(tbxPassword.Text.Trim());

            EstateAgancyEntities db = EstateAgancyEntities.GetContext();
            if (click != 3)
            {
                var user = db.Authoriz.Where(x => x.UserLogin == login && x.hashPassword == password).FirstOrDefault();
                if (user != null && click == 1)
                {
                    string role = RoleIs(user);
                    LoadPage(role, user);
                    
                }

                else if (click > 1)
                {
                    if (user != null && tblOutCapcha.Text == tbxCaptcha.Text)
                    {
                        string role = RoleIs(user);
                        LoadPage(role, user);
                        

                    }
                    else
                    {
                        MessageBox.Show("Введите данные заново!");
                    }
                }

                else
                {
                    MessageBox.Show("Вы ввели логин или пароль неверно!");
                    GenCapcha();
                    tbxPassword.Clear();
                }
            }
            else
            {
                txblockTimer.Visibility = Visibility.Visible;
                txblockTimer.Text = "00:10";
                Blocking();
                timer.Start();
            }
            
        }

        public void InitializeTimer()
        {
            timer = new DispatcherTimer();
            txblockTimer.Text = "00:10";
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            secondsCountdown--;
            if (secondsCountdown >= 0)
            {
                UpdateTimeOnDisplay();

                if(secondsCountdown == 0)
                {
                    timer.Stop();
                    ResetTimer();
                }
            }

        }

        public void ResetTimer()
        {
            secondsCountdown = 10;
            UnBlocking();
            click = 0;
        }

        public void UpdateTimeOnDisplay()
        {
            int minutes = secondsCountdown / 60;
            int seconds = secondsCountdown % 60;
            txblockTimer.Text = $"{minutes:D2}:{seconds:D2}";
        }

        public void Blocking()
        {
            tbxCaptcha.IsEnabled = false;
            tbxLogin.IsEnabled = false;
            tbxPassword.IsEnabled = false;
            btnEnter.IsEnabled = false;
            btnEnterAsGuest.IsEnabled = false;
        }

        public void UnBlocking()
        {
            tbxCaptcha.IsEnabled = true;
            tbxLogin.IsEnabled = true;
            tbxPassword.IsEnabled = true;
            btnEnter.IsEnabled = true;
            btnEnterAsGuest.IsEnabled = true;
        }

        public string RoleIs(Authoriz user)
        {
            var worker = (EstateAgancyEntities.GetContext()).Workers.Where(x => x.AuthoID == user.ID).FirstOrDefault();
            /*int who = (user.UserLogin.StartsWith("agent_", StringComparison.OrdinalIgnoreCase) ? 1 : 2);
            string role = (who == 1 ? "агент" : "клиент");*/
            string role = null;
            if ((worker == null ? 1 : 2) == 1)
            {
                role = "клиент";
            }
            else
            {
                role = (EstateAgancyEntities.GetContext()).Roles.Where(x => x.Id == worker.RoleID).FirstOrDefault().RoleName;
            }
                return role;
        }

        private void LoadPage(string role, Authoriz user)
        {
            click = 0;
            switch (role)
            {
                case "клиент":
                    NavigationService.Navigate(new ClientPage(user));
                    //MessageBox.Show("Вы вошли как:" + role);
                    break;
                case "риелтор":
                    if (CheckWorkTime.CheckWork())
                    {
                        NavigationService.Navigate(new AgentPage(user, role));
                        //MessageBox.Show("Вы вошли как:" + role);
                    }
                    else MessageBox.Show("Вы не можете войти в систему в нерабочее время!", "Exception", MessageBoxButton.OK, MessageBoxImage.Stop);
                    break;
                case "Администратор":
                    //if (CheckWorkTime.CheckWork())
                    //{
                        NavigationService.Navigate(new AdminPage(user, role));
                        //MessageBox.Show("Вы вошли как:" + role);
                    //}
                    //else MessageBox.Show("Вы не можете войти в систему в нерабочее время!", "Exception", MessageBoxButton.OK, MessageBoxImage.Stop);
                    break;
            }
        }
    }
}
