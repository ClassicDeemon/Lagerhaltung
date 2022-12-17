using MySqlConnector;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace Lagerhaltung
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        Boolean check = false;
        public static string conString = "server=192.168.2.117;user=user;database=lagerhaltung;port=3306;password=user";
        int permission;
        string user;
        public Login()
        {
            InitializeComponent();
            checkDirectoryExists();
        }

        private Boolean checkDirectoryExists()
        {
            if(!(Directory.Exists("\\\\mipocloud\\Justin\\Lagerhaltungsdateien")))
            {
                Directory.CreateDirectory("\\\\mipocloud\\Justin\\Lagerhaltungsdateien");
                if(Directory.Exists("\\\\mipocloud\\Justin\\Lagerhaltungsdateien\\Artikelbilder"))
                {
                    return true;
                } else
                {
                    Directory.CreateDirectory("\\\\mipocloud\\Justin\\Lagerhaltungsdateien\\Artikelbilder");
                    return true;
                }
                
            } else
            {
                if (Directory.Exists("\\\\mipocloud\\Justin\\Lagerhaltungsdateien\\Artikelbilder"))
                {
                    return true;
                }
                else
                {
                    Directory.CreateDirectory("\\\\mipocloud\\Justin\\Lagerhaltungsdateien\\Artikelbilder");
                    return true;
                }
            }
        }

        private void loginOnClick(object sender, RoutedEventArgs e)
        {
            if (checkLogin())
            {
                user = textUser.Text;
                MainWindow main = new MainWindow(user, permission);
                main.Show();
                this.Close();
            }
        }

        private void onClickCheckBox(object sender, RoutedEventArgs e)
        {
            if (checkBoxPassword.IsChecked == true)
            {
                textPassword.Visibility = Visibility.Hidden;
                textPasswordHint.Visibility = Visibility.Visible;
                textPasswordHint.Text = textPassword.Password.ToString();
                textPassword.Visibility = Visibility.Hidden;
                check = true;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
                textPassword.Password = textPasswordHint.Text;
                textPasswordHint.Visibility = Visibility.Hidden;
                check = false;
            }

        }

        private string getPasswordFromDatabase()
        {
            if (!(textPassword.Password == null && textUser.Text.Equals(null)))
            {
                String query = "SELECT acc_passwort, acc_rechte FROM accounts WHERE acc_benutzername = '" + textUser.Text.Trim() + "';";
                MySqlConnection connection = new MySqlConnection(conString);
                String password = "";
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        password = reader.GetValue(0).ToString();
                        permission = int.Parse(reader.GetValue(1).ToString());
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                }
                return password;
            }
            else
            {
                return null;
            }

        }

        private Boolean checkLogin()
        {
            Boolean check = false;
            if (getPasswordFromDatabase() != null)
            {

                if (checkBoxPassword.IsChecked == true)
                {
                    if (getPasswordFromDatabase().Equals(textPasswordHint.Text)) { check = true; }
                }
                else
                {
                    if (getPasswordFromDatabase().Equals(textPassword.Password.ToString())) { check = true; }
                }
                return check;
            }
            Error error = new Error();
            error.Show();
            return check;
        }

        private void hintUser_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
