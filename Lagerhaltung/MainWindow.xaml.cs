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
    /// Interaktionslogik für Artikelstamm.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int permission;
        string user;
        public MainWindow(string user, int permission)
        {
            this.user = user;
            this.permission = permission;
            InitializeComponent();
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
            //Mitarbeiter
            if (permission > 2)
            {
                tabItemLagerstamm.Visibility = Visibility.Visible;
            }
            //Admins
            if (permission > 3)
            {
                menuAdmin.Visibility = Visibility.Visible;
            }

        }

        private void loadTable()
        { 
            MySqlConnection conn = new MySqlConnection("server=localhost;user=user;database=lagerhaltung;port=3306;password=user");
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT art_id, art_bez, art_nr, art_einheit, art_min_bestand from artikelstamm", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataSet data = new DataSet();
                adapter.Fill(data, "loadDataBinding");
                dataGridArticle.DataContext = data;
                } catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conn.Close(); }

         } 

        private void onClickAbmelden(object sender, EventArgs e)
        {

        }

        private void onClickVerwaltung(object sender, EventArgs e)
        {

        }

    }

}
