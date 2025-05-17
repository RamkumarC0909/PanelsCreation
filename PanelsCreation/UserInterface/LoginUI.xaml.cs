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
using System.Windows.Shapes;

namespace PanelsCreation.UserInterface
{
    /// <summary>
    /// Interaction logic for LoginUI.xaml
    /// </summary>
    public partial class LoginUI : Window
    {
        public LoginUI()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string userName = this.userName.Text;
            string password = this.password.Password;
            string rUserName = Properties.Settings.Default.UserName;
            string rPassword = Properties.Settings.Default.Password;

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                if (userName == rUserName && password == rPassword)
                {
                    PanelCreationCommand.IsLoggedIn = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
