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
    /// Interaktionslogik für Artikelstamm.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        int permission;
        string user;
        public MainWindow(string user, int permission)
        {
            this.user = user;
            this.permission = permission;
            InitializeComponent();
            rechteVerwaltung();
        }

        private void rechteVerwaltung()
        {
            menuBenutzer.Header = user;
            //Grafiker
            if (permission > 1)
            {
                tabItemGrafik.Visibility = Visibility.Visible;
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

    }
}
