using Microsoft.Win32;
using MySqlConnector;
using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace Lagerhaltung
{
    /// <summary>
    /// Interaktionslogik für Artikelhinzufuegen.xaml
    /// </summary>
    public partial class Artikelhinzufuegen : Window
    {
        private int id;
        private int menge;
        private string bezeichnung;
        private string bild;
        private string text;
        private string artikel;
        private string einheit;
        private int lager_id;
        private int min_bestand;
        private int min_bestell;

        private MySqlConnection conn = new MySqlConnection("server=192.168.2.117;user=user;" +
                    "database=lagerhaltung;port=3306;password=user");
        public Artikelhinzufuegen()
        {
            InitializeComponent();
            loadTable();
        }

        public Artikelhinzufuegen(int id, int menge, string bezeichnung, string bild, string text, string artikel, string einheit, int lager_id, int min_bestand, int min_bestell)
        {
            this.id = id;
            this.menge = menge;
            this.bezeichnung = bezeichnung;
            this.bild = bild;
            this.text = text;
            this.artikel = artikel;
            this.einheit = einheit;
            this.lager_id = lager_id;
            this.min_bestand = min_bestand;
            this.min_bestell = min_bestell;

            InitializeComponent();

            textMenge.Text = menge.ToString();
            textBezeichnung.Text = bezeichnung;
            textBeschreibung.Text = text;
            textEinheit.Text = einheit;
            textMinbestand.Text = min_bestand.ToString();
            textMinBestell.Text = min_bestell.ToString();
        }

        private void clickedButtonAdd(object sender, RoutedEventArgs e)
        {
            if (checkSetEingabe())
            {
                
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO artikelstamm (art_nr, art_bez, art_bild, " +
                        "art_text, art_einheit, art_lag_id, art_min_bestand, art_min_bestell)VALUES('" +
                        menge + "','" + bezeichnung + "','" + bild + "','" + text + "','" + einheit +
                        "','" + lager_id + "','" + min_bestand + "','" + min_bestell + "');", conn);
                    cmd.ExecuteNonQuery();

                    int givenId = 0;
                    MySqlCommand command = new MySqlCommand("SELECT LAST_INSERT_ID();", conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetValue(0).ToString());
                        givenId = int.Parse(reader.GetValue(0).ToString());
                    }
                    picInDirectory(givenId);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally { conn.Close(); }
            }
        }

        private void picInDirectory(int givenId)
        {
            conn.Close();
            conn.Open();
            String path = "\\\\mipocloud\\Justin\\Lagerhaltungsdateien\\Artikelbilder\\" + givenId + ".jpg";
            File.Copy(bild, path);
            MySqlCommand cmd = new MySqlCommand("UPDATE artikelstamm SET art_bild = '" + path + "' WHERE art_id = " + givenId + ";", conn);
            cmd.ExecuteNonQuery();
        }

        private void DataGridRow_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            var row = sender as DataGridRow;
            DataRowView dataView = (DataRowView)row.Item;

            lager_id = int.Parse(dataView["lag_id"].ToString());
            Console.WriteLine(lager_id);

        }

        private Boolean checkSetEingabe()
        {
            if (!(textBezeichnung.Text.Equals("") && textMenge.Text.Equals("") && textEinheit.Text.Equals("") &&
                textMinbestand.Text.Equals("") && textMinBestell.Text.Equals("") && bild.Equals("")))
            {
                if (textBeschreibung.Text.Equals("")) 
                {
                    text = "---";
                } else
                {
                    text = textBeschreibung.Text.Trim();
                }

                menge = int.Parse(textMenge.Text.Trim());
                bezeichnung = textBezeichnung.Text.Trim();
                einheit = textEinheit.Text.Trim();
                min_bestand = int.Parse(textMinbestand.Text.Trim());
                min_bestell = int.Parse(textMinBestell.Text.Trim());

                return true;
            }
            return false;
        }

        private void buttonFileChooser_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Wähle das Bild des Artikels aus";
            op.Filter = "Alle unterstützten Grafikformate|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imageArticle.Source = new BitmapImage(new Uri(op.FileName));
                bild = op.FileName;
                Console.WriteLine(bild);
            }
        }

        private void loadTable()
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT lag_id, lag_bez, lag_nr, lag_regal, lag_fach, lag_platz from lagerstamm", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataSet data = new DataSet();
                adapter.Fill(data, "loadDataBinding");
                dataGridArticle.DataContext = data;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conn.Close(); }

        }

        private void DataGridRow_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
