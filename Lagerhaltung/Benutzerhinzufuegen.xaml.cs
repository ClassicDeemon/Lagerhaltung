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
    /// Interaktionslogik für Benutzerhinzufuegen.xaml
    /// </summary>
    public partial class Benutzerhinzufuegen : Window
    {
        private string user;
        private int permission;
        private MySqlConnection conn = new MySqlConnection("server=192.168.2.117;user=user;database=lagerhaltung;port=3306;password=user");
        private int getPermission = 0;
        public Benutzerhinzufuegen(string user, int permission)
        {
            InitializeComponent();
            this.user = user;
            this.permission = permission;
        }

        private void buttonAddClick(object sender, RoutedEventArgs e)
        {
            if (eingabeCheck() == true)
            {
                conn.Open();
                try
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO accounts (acc_benutzername, acc_passwort, acc_rechte)VALUES('" +
                        textUser.Text.Trim() + "','" + textPassword.Text.Trim() + "','" + getCreatedPermission() + "');", conn);
                    cmd.ExecuteNonQuery();
                    Benutzerhinzufuegen benutzerhinzufuegen = new Benutzerhinzufuegen(user, permission);
                    benutzerhinzufuegen.Show();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                    this.Close();
                }
            } 
        }

        private bool eingabeCheck()
        {
            bool check = true;
            if ((textUser.Text.Trim().Equals("") || textPassword.Text.Trim().Equals("")) && getCreatedPermission() == 0)
            {
                MessageBox.Show("Eingabe vergessen!", "Fehler!");
                check = false;
            }
            else
            {
                if (textUser.Text.Trim().Equals("") || textPassword.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Benutzername oder Passwort fehlen!", "Fehler!");
                    check = false;
                }
                if (getCreatedPermission() == 0)
                {
                    MessageBox.Show("Rechte auswählen!", "Fehler!");
                    check = false;
                }
            }
            return check;
        }

        private void buttonbackClick(object sender, RoutedEventArgs e)
        {
            Benutzerverwaltung benutzerverwaltung = new Benutzerverwaltung(user, permission);
            benutzerverwaltung.Show();
            this.Close();
        }

        private int getCreatedPermission()
        {
            if (comboBoxPermission.SelectedIndex == 0)
            {
                return 1;
            }
            else if (comboBoxPermission.SelectedIndex == 1)
            {
                return 2;
            }
            else if (comboBoxPermission.SelectedIndex == 2)
            {
                return 3;
            }
            else if (comboBoxPermission.SelectedIndex == 3)
            {
                return 4;
            }
            return 0;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBoxPermission.SelectedIndex == 0)
            {
                getPermission = 1;
            } else if(comboBoxPermission.SelectedIndex == 1)
            {
                getPermission = 2;
            } else if(comboBoxPermission.SelectedIndex == 2)
            {
                getPermission = 3;
            } else if(comboBoxPermission.SelectedIndex == 3)
            {
                getPermission = 4;
            }
            getPermission = 0;
        }
    }
}
