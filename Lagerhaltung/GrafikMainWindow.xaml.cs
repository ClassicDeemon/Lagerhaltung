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
    /// Interaktionslogik für GrafikMainWindow.xaml
    /// </summary>
    public partial class GrafikMainWindow : Window
    {
        private int permission;
        private string user;
        private MySqlConnection conn = new MySqlConnection("server=192.168.2.117;user=user;database=lagerhaltung;port=3306;password=user");
        private int art_id;
        private string path;
        private int lag_id;
        DataSet dataArt = new DataSet();
        DataSet dataBew = new DataSet();
        DataSet dataLag = new DataSet();
        public GrafikMainWindow(string user, int permission)
        {
            this.user = user;
            this.permission = permission;
            InitializeComponent();
            rechteVerwaltung();
            loadArtTable();
            loadBewTable();
            loadStorageTable();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Menu + Extras

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

        private void onClickAbmelden(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void onClickVerwaltung(object sender, EventArgs e)
        {
            Benutzerverwaltung verwaltung = new Benutzerverwaltung(user, permission);
            verwaltung.Show();
            this.Close();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Artikel

        private void loadArtTable()
        {

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT art_id, art_bez, art_nr, art_einheit, art_min_bestand, art_bild from artikelstamm", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dataArt = new DataSet();
                adapter.Fill(dataArt, "loadDataBinding");
                dataGridArticle.DataContext = dataArt;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conn.Close(); }

        }

        private void buttonArticleAdd_Click(object sender, RoutedEventArgs e)
        {
            Artikelhinzufuegen artikelhinzufuegen = new Artikelhinzufuegen(user, permission);
            artikelhinzufuegen.Show();
            this.Close();
        }

        private void buttonArticleDelete_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM artikelstamm WHERE art_id = " + art_id + ";", conn);
            imageArticle.Source = null;
            cmd.ExecuteNonQuery();
            dataGridArticle.Items.Refresh();
            conn.Close();
            loadArtTable();
        }

        private void buttonArticleEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGridRowArticle_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            var row = sender as DataGridRow;
            DataRowView dataView = (DataRowView)row.Item;

            art_id = int.Parse(dataView["art_id"].ToString());
            string bildpath = dataView["art_bild"].ToString();

            showArticlePicture(bildpath);

        }

        private void showArticlePicture(string path)
        {
            path = path.Replace('#', '/');
            this.path = path;
            imageArticle.Source = new BitmapImage(new Uri(path));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loadArtTable();
        }

        private void searchBarKeyUp(object sender, RoutedEventArgs e)
        {

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT art_id, art_bez, art_nr, art_einheit, art_min_bestand, art_bild from artikelstamm WHERE art_bez LIKE '%" +
                    textSearchBar.Text + "%';", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dataArt = new DataSet();
                adapter.Fill(dataArt, "loadDataBinding");
                dataGridArticle.DataContext = dataArt;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conn.Close(); }


        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Artikelbewegung
        private void buttonMoveArticle(object sender, RoutedEventArgs e)
        {
            Artikelbewegung artikelbewegung = new Artikelbewegung(user, permission);
            artikelbewegung.Show();
            this.Close();
        }
        private void loadBewTable()
        {

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from artikelbewegung LEFT JOIN artikelstamm ON bew_art_id = art_id", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dataBew = new DataSet();
                adapter.Fill(dataBew, "loadDataMoveBinding");
                dataGridMovement.DataContext = dataBew;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conn.Close(); }

        }

        private void searchBewegungKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from artikelbewegung LEFT JOIN artikelstamm ON bew_art_id = art_id WHERE art_bez LIKE '%"
                    + textSearchBewBar.Text + "%';", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dataBew = new DataSet();
                adapter.Fill(dataBew, "loadDataMoveBinding");
                dataGridMovement.DataContext = dataBew;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("oh");
            }
            finally { conn.Close(); }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Lagerstamm
        private void loadStorageTable()
        {

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM lagerstamm", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dataLag = new DataSet();
                adapter.Fill(dataLag, "loadDataStorageBinding");
                dataGridStorage.DataContext = dataLag;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conn.Close(); }

        }
        private void DataGridRowStorage_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            var row = sender as DataGridRow;
            DataRowView dataView = (DataRowView)row.Item;

            lag_id = int.Parse(dataView["lag_id"].ToString());

        }

        private void buttonStorageDelete(object sender, RoutedEventArgs e)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM lagerstamm WHERE lag_id = " + lag_id + ";", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            loadStorageTable();
        }

        private void buttonStorageAdd(object sender, RoutedEventArgs e)
        {
            Lagerplatzhinzufuegen lag = new Lagerplatzhinzufuegen(permission, user);
            lag.Show();
            this.Close();
        }

        private void searchLagerKeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("jo");
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from lagerstamm WHERE lag_bez LIKE '%" +
                    textSearchLagBar.Text + "%';", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dataLag = new DataSet();
                adapter.Fill(dataLag, "loadDataStorageBinding");
                dataGridStorage.DataContext = dataLag;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conn.Close(); }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    }

}