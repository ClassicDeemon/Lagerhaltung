﻿using System;
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
        public Artikelhinzufuegen()
        {
            InitializeComponent();
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

        }
    }
}
