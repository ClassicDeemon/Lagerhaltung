using MySqlConnector;
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

namespace Lagerhaltung
{
    /// <summary>
    /// Interaktionslogik für Lagerplatzhinzufuegen.xaml
    /// </summary>
    public partial class Lagerplatzhinzufuegen : Window
    {
        private int permission;
        private string user;
        private MySqlConnection conn = new MySqlConnection("server=192.168.2.117;user=user;database=lagerhaltung;port=3306;password=user");
        public Lagerplatzhinzufuegen(int permission, string user)
        {
            InitializeComponent();
            this.permission = permission;
            this.user = user;
        }

        private void buttonClickOK(object sender, RoutedEventArgs e)
        {
            try
            {
                string command = "INSERT INTO lagerstamm (lag_bez, lag_nr, lag_regal, lag_fach, lag_platz)VALUES('" + textBezeichnung.Text.Trim() + "','" +
                    textNummer.Text.Trim() + "','" + textRegal.Text.Trim() + "','" + textFach.Text.Trim() + "','" + textPlatz.Text.Trim() + "');";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(command, conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                conn.Close();
                MainWindow main = new MainWindow(user, permission);
                main.Show();
                this.Close();
            }
        }

        private void buttonClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(user, permission);
            main.Show();
            this.Close();
        }
    }
}
