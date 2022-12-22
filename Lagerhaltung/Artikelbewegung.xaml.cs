using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static System.Net.Mime.MediaTypeNames;

namespace Lagerhaltung
{
    /// <summary>
    /// Interaktionslogik für Artikelbewegung.xaml
    /// </summary>
    public partial class Artikelbewegung : Window
    {

        private int permission;
        private string user;
        private string path;
        private int art_id;
        private string optionType;
        private MySqlConnection conn = new MySqlConnection("server=192.168.2.117;user=user;database=lagerhaltung;port=3306;password=user");
        DateTime time = DateTime.Now;
        public Artikelbewegung(string user, int permission)
        {
            this.user = user;
            this.permission = permission;
            InitializeComponent();
            setLabelDatum();
            loadArtTable();
        }

        private void loadArtTable()
        {

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT art_id, art_bez, art_nr, art_min_bestand, art_bild from artikelstamm", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataSet data = new DataSet();
                adapter.Fill(data, "loadDataBinding");
                dataGridArticle.DataContext = data;
                Console.WriteLine(data);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conn.Close(); }

        }

        private void DataGridRowArticle_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            var row = sender as DataGridRow;
            DataRowView dataView = (DataRowView)row.Item;

            art_id = int.Parse(dataView["art_id"].ToString());
            string bildpath = dataView["art_bild"].ToString();
            Console.WriteLine(art_id);

            showArticlePicture(bildpath);

        }

        private void showArticlePicture(string path)
        {
            path = path.Replace('#', '/');
            Console.WriteLine(path);
            imageArticle.Source = new BitmapImage(new Uri(path));
        }

        private void setLabelDatum()
        {
            labelDate.Content = "";
            labelDate.Content = time.ToString();
        }

        private void buttonClickBuchen(object sender, RoutedEventArgs e)
        {
            
            string command = "INSERT INTO artikelbewegung (bew_art_id, bew_datum, bew_typ, bew_menge, bew_bestand, bew_user)" +
                "VALUES('" + art_id + "','" + time + "','" + optionType + "','" + textMenge.Text + "','" + getNowAmount() + "','" + user + "');";
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(command, conn);
                cmd.ExecuteNonQuery();
            } catch(SqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
            } finally
            {
                conn.Close();
                updateBestandArtikel();
                MainWindow main = new MainWindow(user, permission);
                main.Show();
                this.Close();
            }
            
        }
        
        private void updateBestandArtikel()
        {
            
            string command = "UPDATE artikelstamm SET art_nr = " + getNowAmount() + " WHERE art_id = " + art_id + ";";
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(command, conn);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        private int getNowAmount()
        {
            MySqlConnection connection = new MySqlConnection("server=192.168.2.117;user=user;database=lagerhaltung;port=3306;password=user");
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT art_nr FROM artikelstamm WHERE art_id = " + art_id + ";", connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                int amount = 0;
                while (reader.Read())
                {
                    amount = int.Parse(reader.GetValue(0).ToString());
                }
                if (optionType.Equals("Abgang"))
                {
                    return amount - int.Parse(textMenge.Text);
                }
                if (optionType.Equals("Zugang"))
                {
                    return amount + int.Parse(textMenge.Text);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                connection.Close();
            }
            return 0;
        }

        private void buttonClickAbbrechen(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(user, permission);
            main.Show();
            this.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(comboBoxType.SelectedIndex);
            if (comboBoxType.SelectedIndex == 0)
            {
                optionType = "Zugang";
                
            } else if(comboBoxType.SelectedIndex == 1)
            {
                optionType = "Abgang";
                
            }
        }
    }
}
