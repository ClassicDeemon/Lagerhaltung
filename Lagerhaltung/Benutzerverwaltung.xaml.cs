using MySqlConnector;
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
using System.Windows.Shapes;

namespace Lagerhaltung
{
    /// <summary>
    /// Interaktionslogik für Benutzerverwaltung.xaml
    /// </summary>
    public partial class Benutzerverwaltung : Window
    {
        private string user;
        private int permission;
        private MySqlConnection conn = new MySqlConnection("server=192.168.2.117;user=user;database=lagerhaltung;port=3306;password=user");
        private DataSet data;
        public Benutzerverwaltung(string user, int permission)
        {
            InitializeComponent();
            this.user = user;
            this.permission = permission;
            rechteVerwaltung();
            loadTable();
        }
        private void rechteVerwaltung()
        {
            menuBenutzer.Header = user;
            //Grafiker
            if (permission > 1)
            {
                menuGrafik.Visibility = Visibility.Visible;
            }

        }

        private void onClickAbmelden(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void onClickMain(object sender, EventArgs e)
        {
            MainWindow main = new MainWindow(user, permission);
            main.Show();
            this.Close();
        }

        private void loadTable()
        {

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from accounts", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data, "loadDataBinding");
                dataGridArticle.DataContext = data;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conn.Close(); }

        }

        private void buttonAddClick(object sender, RoutedEventArgs e)
        {
            Benutzerhinzufuegen benutzerhinzufuegen = new Benutzerhinzufuegen(user, permission);
            benutzerhinzufuegen.Show();
            this.Close();
        }

        private void buttonDeleteClick(object sender, RoutedEventArgs e)
        {

        }

        private void buttonEditClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
