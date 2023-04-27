﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

namespace SQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string kapcsolatLeiro = "datasource=127.0.0.1;port=3306;username=root;password=;database=hardver;charset=utf8;";
        List<Termek> termekek = new List<Termek>();
        MySqlConnection SQLkapcsolat;
        public MainWindow()
        {
            InitializeComponent();
            AdatbazisMegnyitas();
            KategoriakBetoltese();
            GyartokBetoltese();
            TermekekBetolteseListaba();
            AdatbazisLezarasa();
        }
        private void AdatbazisMegnyitas()
        {
            try
            {
                SQLkapcsolat = new MySqlConnection(kapcsolatLeiro);
                SQLkapcsolat.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Nem lehet kapcsolódni az adatbázishoz");
                this.Close();
            }
        }
        private void AdatbazisLezarasa()
        {
            SQLkapcsolat.Close();
            SQLkapcsolat.Dispose();
        }
        private void btnSzukit_Click(object sender, RoutedEventArgs e)
        {
            termekek.Clear();
            string SQLSzukitettLista = SzukitoLekerdezesEloallitasa();
            MySqlCommand SQLparancs = new MySqlCommand(SQLSzukitettLista, SQLkapcsolat);

            MySqlDataReader eredmenyOlvaso = SQLparancs.ExecuteReader();
            while (eredmenyOlvaso.Read())
            {
                Termek uj = new Termek(eredmenyOlvaso.GetString("Kategória"), eredmenyOlvaso.GetString("Gyártó"), eredmenyOlvaso.GetString("Név"), eredmenyOlvaso.GetInt32("Ár"), eredmenyOlvaso.GetInt32("Garidő"));
                termekek.Add(uj);
            }
            eredmenyOlvaso.Close();
            dgTermekek.Items.Refresh();
        }
        private void KategoriakBetoltese()
        {
            string SQLKategoriakRendezve = "SELECT DISTINCT kategória FROM termékek ORDER BY kategória;";
            MySqlCommand SQLparancs = new MySqlCommand(SQLKategoriakRendezve, SQLkapcsolat);
            MySqlDataReader eredmenyOlvaso = SQLparancs.ExecuteReader();

            cbKategoria.Items.Add(" - Nincs megadva -");
            while (eredmenyOlvaso.Read())
            {
                cbKategoria.Items.Add(eredmenyOlvaso.GetString("kategória"));
            }
            eredmenyOlvaso.Close();
            cbKategoria.SelectedIndex = 0;
        }
        private void GyartokBetoltese()
        {
            string SQLGyartokRendezve = "SELECT DISTINCT gyártó FROM termékek ORDER BY gyártó;";
            MySqlCommand SQLparancs = new MySqlCommand(SQLGyartokRendezve, SQLkapcsolat);
            MySqlDataReader eredmenyOlvaso = SQLparancs.ExecuteReader();
            cbGyarto.Items.Add("- Nincs megadva -");
            while (eredmenyOlvaso.Read())
            {
                cbGyarto.Items.Add(eredmenyOlvaso.GetString("Gyártó"));
            }
            eredmenyOlvaso.Close();
            cbGyarto.SelectedIndex = 0;
        }


        private void TermekekBetolteseListaba()
        {
            string SQLOsszesTermek = "SELECT * FROM termékek;";
            MySqlCommand SQLparancs = new MySqlCommand(SQLOsszesTermek, SQLkapcsolat);
            MySqlDataReader eredmenyOlvaso = SQLparancs.ExecuteReader();
            while (eredmenyOlvaso.Read())
            {
                Termek uj = new Termek(eredmenyOlvaso.GetString("Kategória"), eredmenyOlvaso.GetString("Gyártó"), eredmenyOlvaso.GetString("Név"), eredmenyOlvaso.GetInt32("Ár"), eredmenyOlvaso.GetInt32("Garidő"));
                termekek.Add(uj);
            }
            eredmenyOlvaso.Close();
            dgTermekek.ItemsSource = termekek;
        }
       
        private void btnMentes_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sw = new StreamWriter("blabla.csv");
            foreach(var item in termekek)
            {
                sw.WriteLine(item.ToCSVString());
            }
            sw.Close();
        }
        private string SzukitoLekerdezesEloallitasa()
        {
            throw new NotImplementedException();
        }

    }
}
