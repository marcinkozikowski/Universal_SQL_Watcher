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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using System.Data;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace Universal_SQL_Watecher
{

    public partial class MainWindow : Window
    {
        #region Zmienne globalne

        List<Watek> listaWatkow = new List<Watek>();    // Lista Watkow klasy Watek do zarzadzania poszczegolnymi watkami
        List<Thread> listawatkow = new List<Thread>();  // Lista watkow systemowych do zarzadzania ktore zatrzymac usunac ktore wznowic wykorzystuje watki z listaWatkow
        Watek nowy; // Aplikacja wielowatkowa musi posiadac mozliwosc obslugi wielu watkow jednoczesnie
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        NpgsqlDataAdapter dataAdapter;
        NpgsqlConnection connection;
        List<Przypadek> listaPrzypadkow = new List<Przypadek>();    // Lista przypadkow zdefiniowanych
        string fileName = "bazaPrzypadkow.usw";     // nazwa pliku bazo danowego
        StreamWriter writer;    // Instancja zapisu do bazy danych
        StreamReader reader;    // Czytanie z bazy danych
        DataTable cacheTable = new DataTable();
        int ile;
        int ileOld;

        #endregion

        #region Definicje Klas

        class Watek
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string server, port, database, login, pass;
            public string nazwaWatku { get; set; }
            string sqlQuestion;
            string sqlUpdate;
            string tabela;
            string kolumnaIf, kolumnaThen, wyrazenieIf, wartoscThen;
            NpgsqlConnection connection;
            NpgsqlDataAdapter dataAdapter;
            Int64 interwal;
            System.Windows.Threading.DispatcherTimer TimerWatkow;
            DataTable cacheTable;
            public bool dziala{get;set;}

            
            public Watek(string _server,string _port,string _database,string _login,string _pass,string _nazwa, string _sqlQuestion, string _sqlUpdate, string _kolumnaIf, string _kolumnaThen,
                string _wyrazenieIf, string _wartoscThen, NpgsqlConnection _connection, NpgsqlDataAdapter _dataAdapter, Int64 _interwal)
            {
                server = _server;
                port = _port;
                database = _database;
                login = _login;
                pass = _pass;

                nazwaWatku = _nazwa;
                sqlQuestion = _sqlQuestion;
                sqlUpdate = _sqlUpdate;
                cacheTable = new DataTable();
              

                string[] rozbity = sqlQuestion.Split(' ');
                tabela = rozbity[rozbity.Count() - 1].Replace("\"","");

                kolumnaIf = _kolumnaIf;
                kolumnaThen = _kolumnaThen;
                wyrazenieIf = _wyrazenieIf;
                wartoscThen = _wartoscThen;
                connection = _connection;
                dataAdapter = _dataAdapter;
                interwal = _interwal;
                dziala = true;
                

                //this.Dispatcher.Invoke(() =>
                //{
                    TimerWatkow = new System.Windows.Threading.DispatcherTimer();
                    TimerWatkow.Tick += new EventHandler(dispatcherTimer_Tick);
                    TimerWatkow.Interval = new TimeSpan(interwal);
                //});
            }

            public Watek()
            {
                // pusty konstruktor wykorzystywany podczas dodawania watkow na liste
            }
            
            public void stopThread()
            {
                TimerWatkow.Stop();
                dziala = false;
            }
            private void updateSqlDataBase()
            {
                dataAdapter = new NpgsqlDataAdapter(sqlUpdate, connection);
                //dataAdapter.Update(ds);
                ds.Reset();
                ds.Clear();
                dataAdapter.Fill(ds);
                dataAdapter.Update(dt);
                connection.Close();

                cacheTable.Clear();
                cacheTable = dt.Copy();
            }
            /** Metoda typu void zawiera ciag instrukcji
             * podlaczenia do bazy i zebrania danych
             * do objektu DataSet oraz DataTabel*/
            private void podlaczDobazySql()
            {
                // Connection String
                string connectionString = String.Format("Server={0};Port={1};" +
                    "User Id={2};Password={3};Database={4};",
                    server, port, login,
                    pass, database);
                // Ustanowienie polaczenia z baza danych 
                connection = new NpgsqlConnection(connectionString);
                connection.Open();
                // Wyrazenie SQL
                //string sql = "select \"LoanSecurity\",\"SalesIncomeTax2YearsBack\" from \"Loans\"";
                String sql = sqlQuestion;
                // Adapter odpowiedzialny za polaczenie 
                dataAdapter = new NpgsqlDataAdapter(sql, connection);
                ds.Reset();
                ds.Clear();
                // Uzupelnianie obiektu dataSet danymi przechwyconymi przez dataAdapter
                dataAdapter.Fill(ds);
                // since it C# DataSet can handle multiple tables, we will select first
                dt.Clear();
                //dt = ds.Tables[0];
                // Uzupelniania DataGrid tabela z bazy danych
                dataAdapter.Fill(dt);
                //dataGridView.ItemsSource = dt.DefaultView;
                // Ilosc wierszy w danej tabeli dt(DataTable)
                connection.Close();
            }
            /** Metoda uruchamiana cyklicznie poprzez obiekt
             * dispatcherTime co okreslony interwal czasu bedzie przeprowadzac
             * nastepujące funkcje */
            private void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                podlaczDobazySql();
                /* Jezeli nastapi zmiana w ilosci wierszy pobierz nowe dane do tabeli cache */
                if (cacheTable.Rows.Count != dt.Rows.Count)
                {
                    cacheTable.Clear(); // czyszczenie cachu jesli ma nastapic przeladowanie
                    // Dodawanie danych z bazy do tabeli cache
                        cacheTable = dt.Copy();
                        MessageBox.Show(nazwaWatku); 
                }             
                else
                {
                    /* Tutaj trzeba zrobic odpytywanie tabeli cache
                     * jesli nastapi zmiana w tabeli cache wtedy trzeba zrobic update bazy danych */
                    string wyrazenie = wyrazenieIf.Substring(0, 1);
                    switch (wyrazenie)
                    {
                        case "=":
                            {
                                /* Jezeli w trakcie przegladania tabeli cache wykryje nieprawidlowe dane
                                 * to zrob update bazy danych oraz zaktualizuj cache */
                                
                                for (int i = 0; i < cacheTable.Rows.Count; i++)
                                {
                                    if (cacheTable.Rows[i][0].ToString().Equals(wyrazenieIf.Substring(1).ToString()))
                                    {
                                        if (!cacheTable.Rows[i][1].ToString().Equals(wartoscThen))
                                        {
                                            updateSqlDataBase();
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "<":
                            {
                                for (int i = 0; i < cacheTable.Rows.Count; i++)
                                {

                                    if (double.Parse(cacheTable.Rows[i][0].ToString()) < double.Parse(wyrazenieIf.Substring(1)))
                                    {
                                        if (cacheTable.Rows[i][1].ToString() != wartoscThen)
                                        {
                                            updateSqlDataBase();
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case ">":
                            {
                                for (int i = 0; i < cacheTable.Rows.Count; i++)
                                {

                                    if (double.Parse(cacheTable.Rows[i][0].ToString()) > double.Parse(wyrazenieIf.Substring(1)))
                                    {
                                        if (cacheTable.Rows[i][1].ToString() != wartoscThen)
                                        {
                                            updateSqlDataBase();
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "<=":
                            {
                                for (int i = 0; i < cacheTable.Rows.Count; i++)
                                {

                                    if (double.Parse(cacheTable.Rows[i][0].ToString()) <= double.Parse(wyrazenieIf.Substring(1)))
                                    {
                                        if (cacheTable.Rows[i][1].ToString() != wartoscThen)
                                        {
                                            updateSqlDataBase();
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case ">=":
                            {
                                for (int i = 0; i < cacheTable.Rows.Count; i++)
                                {

                                    if (double.Parse(cacheTable.Rows[i][0].ToString()) >= double.Parse(wyrazenieIf.Substring(1)))
                                    {
                                        if (cacheTable.Rows[i][1].ToString() != wartoscThen)
                                        {
                                            updateSqlDataBase();
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        default:
                            {
                                MessageBox.Show("Nie rozpoznano wyrażenia");
                                break;
                            }
                    }  
                }
                
            }
            private bool shouldStop()
            {
                return true;
            }
            public void startThred()
            {
                
                TimerWatkow.Start();
                dziala = true;
            }
            public string getNazwaWatku()
            {
                return nazwaWatku;
            }
            public override string ToString()
            {
                return nazwaWatku.ToString();
            }
            public DataTable getCacheList()
            {
                return cacheTable;
            }
            public bool czyDzialaWatek()
            {
                return dziala;
            }
        }

        private class Tabela
        {
            String kolumnaIf;
            String kolumnaThen;
            public Tabela(string _kolumnaIf,string _kolumnaThen)
            {
                kolumnaIf = _kolumnaIf;
                kolumnaThen = _kolumnaThen;
            }
            public string getKolumnaIf()
            {
                return kolumnaIf;
            }
            public  string getKolumnaThen()
            {
                return kolumnaThen;
            }
        }

        private class Item
        {
            String name, server, port, login, pass, database;
            public Item(String nazwa,String host,String portN,String loginUser,String passUser,String databaseName)
            {
                name = nazwa;
                server = host;
                port = portN;
                login = loginUser;
                pass = passUser;
                database = databaseName;
            }
            public String getServer(){
                return server;
            }
            public String getPort()
            {
                return port;
            }
            public String getLogin()
            {
                return login;
            }
            public String getPass()
            {
                return pass;
            }
            public String getDatabase()
            {
                return database;
            }
            public override string ToString()
            {
                return name.ToString();
            }
        }

        private class Przypadek
        {
            string nazwaPrzypadku;
            string server, port, login, pass, database;
            Int64 interwal;
            string kolumnaIF;
            string kolumnaThen; 
            string wyrazenieIf; 
            string wartosThen;
            string sql;

            public Przypadek(string _nazwaPrzypadku,
                string _server, string _port,
                string _login, string _pass, string _database,
                string _kolumnaIF, string _kolumnaThen, string _wyrazenieIf,
                string _wartoscThen,string _sql,Int64 _interwal)
            {
                nazwaPrzypadku = _nazwaPrzypadku;
                server = _server;
                port = _port;
                login = _login;
                pass = _pass;
                database = _database;
                kolumnaIF = _kolumnaIF;
                kolumnaThen = _kolumnaThen;
                wyrazenieIf = _wyrazenieIf;
                wartosThen = _wartoscThen;
                sql = _sql;
                interwal = _interwal;
            }
            public string getNazwaPrzypadku()
            {
                return nazwaPrzypadku;
            }
            public string getServer()
            {
                return server;
            }
            public string getPort()
            {
                return port;
            }
            public string getLogin()
            {
                return login;
            }
            public string getPass()
            {
                return pass;
            }
            public string getDatabase()
            {
                return database;
            }
            public string getKolumnaIf()
            {
                return kolumnaIF;
            }
            public string getKolumnaThen()
            {
                return kolumnaThen;
            }
            public string getWyrazenieIf(){
                return wyrazenieIf;
            }
            public string getWartoscThen()
            {
                return wartosThen;
            }
            public string getSqlQuestion()
            {
                return sql;
            }
            public Int64 getInterwal()
            {
                return interwal;
            }

            public override string ToString()
            {
                return nazwaPrzypadku.ToString();
            }
        }
        #endregion

        #region MainWidow Inicjalizacja
        public MainWindow()
        {
            InitializeComponent();
            comboWybor.Items.Add(new Item("Capitales DataBase", "109.231.37.8", "5432", "bank_db", "bank_db123", "bankDB"));
            readBazaDanych();
        }
        #endregion

        public void podlaczDoBazy()
        {
            // Connection String
            string connectionString = String.Format("Server={0};Port={1};" +
                "User Id={2};Password={3};Database={4};",
                serverText.Text, portText.Text, loginText.Text,
                passText.Text, databaseText.Text);
            // Ustanowienie polaczenia z baza danych 
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
            // Wyrazenie SQL
            //string sql = "select \"LoanSecurity\",\"SalesIncomeTax2YearsBack\" from \"Loans\"";
            String sql = sqlQuestionText.Text;
            // Adapter odpowiedzialny za polaczenie 
            dataAdapter = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            // Uzupelnianie obiektu dataSet danymi przechwyconymi przez dataAdapter
            dataAdapter.Fill(ds);
            // since it C# DataSet can handle multiple tables, we will select first
            dt = ds.Tables[0];
            // Uzupelniania DataGrid tabela z bazy danych
            dataAdapter.Fill(dt);
            //dataGridView.ItemsSource = dt.DefaultView;
            // Ilosc wierszy w danej tabeli dt(DataTable)
            ileOld = ile;
            ile = dt.Rows.Count;
            // zakonczenie polaczenia
            connection.Close();
        }

        #region Reakcje na Przyciski

        #region Przyciski na 1 karcie

        private void wybierzListeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listView.SelectedIndex;

                serverText.Text = listaPrzypadkow.ElementAt(index).getServer();
                portText.Text = listaPrzypadkow.ElementAt(index).getPort();
                loginText.Text = listaPrzypadkow.ElementAt(index).getLogin();
                passText.Text = listaPrzypadkow.ElementAt(index).getPass();
                nazwaPrzypadkuText.Text = listaPrzypadkow.ElementAt(index).getNazwaPrzypadku();
                kolumnaIfText.Text = listaPrzypadkow.ElementAt(index).getKolumnaIf();
                kolumnaThenText.Text = listaPrzypadkow.ElementAt(index).getKolumnaThen();
                wartoscThenText.Text = listaPrzypadkow.ElementAt(index).getWartoscThen();
                wyrazenieIfText.Text = listaPrzypadkow.ElementAt(index).getWyrazenieIf();
                databaseText.Text = listaPrzypadkow.ElementAt(index).getDatabase();
                timeText.Text = listaPrzypadkow.ElementAt(index).getInterwal().ToString();
                sqlQuestionText.Text = listaPrzypadkow.ElementAt(index).getSqlQuestion();

                tabControl.SelectedIndex = 1;
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }

        }
        /* Usun button, usun zdefiniowane zapytanie z listy */
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            listaPrzypadkow.RemoveAt(index);
            listView.Items.Clear();
            for (int i = 0; i < listaPrzypadkow.Count; i++)
            {
                listView.Items.Add(listaPrzypadkow.ElementAt(i).ToString());
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            listaPrzypadkow.Clear();
            listView.Items.Clear();
        }

        /* Zapisz liste przypadkow */
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            /** Zapis listy zdefiniowanych przypadkow
             * do zewnetrznej bazy danych w postaci pliku
             * tekstowego*/
            writeDoBazy();
        }


        #endregion

        #region Przyciski 2 karty
        /** Przycisk rozpocznij */
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string[] rozbity = sqlQuestionText.Text.Split(' ');
            tabela = rozbity[rozbity.Count() - 1].Replace("\"", "");
            tabela = this.WrapWithQuoutes(tabela);
            string column_if_text = this.WrapWithQuoutes(this.kolumnaIfText.Text);
            string column_then_text = this.WrapWithQuoutes(this.kolumnaThenText.Text);
            string wyrazenieIf_quoutes = this.WrapWithSingleQuoutes(this.wyrazenieIfText.Text);

            //string sqlUpdate = "UPDATE \"" + tabela + "\" SET \"" + kolumnaThenText.Text + "\" = " + wartoscThenText.Text + " WHERE \"" + kolumnaIfText.Text + "\""+wyrazenieIfText.Text;
            string sqlUpdate = String.Format(@"UPDATE {0} SET {1} = {2} WHERE {3} {4}", tabela, column_then_text, wartoscThenText.Text, column_if_text, wyrazenieIf_quoutes); 
            nowy = new Watek(serverText.Text,portText.Text,databaseText.Text,loginText.Text,passText.Text, nazwaPrzypadkuText.Text,
                sqlQuestionText.Text, sqlUpdate, kolumnaIfText.Text, kolumnaThenText.Text, 
                wyrazenieIfText.Text, wartoscThenText.Text, connection, dataAdapter,Int64.Parse(timeText.Text));
            
            listaWatkow.Add(nowy);
            
            Thread watek = new Thread(nowy.startThred); /* Metoda startThread startuje Dispatchera*/
            watek.Name = "Thread " + nowy.getNazwaWatku();
            listawatkow.Add(watek);
            listaWatkowView.Items.Clear();
            watek.Start();
            for (int i = 0; i < listaWatkow.Count; i++)
            {
                listaWatkowView.Items.Add(new Watek{nazwaWatku = listaWatkow.ElementAt(i).getNazwaWatku(),dziala=listaWatkow.ElementAt(i).czyDzialaWatek() });
            }
            
        }

        private string WrapWithSingleQuoutes(string input)
        {
            return String.Format(input.Substring(0,1)+"'{0}'", input.Substring(1));
        }

        private string WrapWithQuoutes(string Input)
        {
            return String.Format("\"{0}\"", Input);
        }

        private void DodajPrzypadek_Click(object sender, RoutedEventArgs e)
        {
            listaPrzypadkow.Add(new Przypadek(nazwaPrzypadkuText.Text.ToString(),
                serverText.Text.ToString(), portText.Text.ToString(), loginText.Text.ToString(), passText.Text.ToString(),
                databaseText.Text.ToString(), kolumnaIfText.Text.ToString(), kolumnaThenText.Text.ToString(),
                wyrazenieIfText.Text.ToString(), wartoscThenText.Text.ToString(), sqlQuestionText.Text.ToString(), Int64.Parse(timeSlider.Value.ToString())));
            /* dodano do listy przypadkow teraz trzeba wrzucic do obiektu listView */
            listView.Items.Clear();
            for (int i = 0; i < listaPrzypadkow.Count; i++)
            {
                listView.Items.Add(listaPrzypadkow.ElementAt(i));
            }
            MessageBox.Show("Poprawnie dodano zdefiniowny przypadek do listy.\n\nNa koniec nie zapomnij zapisać wprowadzonych zmian !");
        }

        private void comboWybor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item item = (Item)comboWybor.SelectedItem;
            serverText.Text = item.getServer();
            portText.Text = item.getPort();
            loginText.Text = item.getLogin();
            passText.Text = item.getPass();
            databaseText.Text = item.getDatabase();
        }

        #endregion

        #region Obsluga Watkow Przyciski 3 karty
        private void zatrzymajWatekButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string index = listaWatkowView.SelectedItem.ToString();
                
                for (int i = 0; i < listawatkow.Count; i++)
                {
                    /* na liscie klasy Watek stworzonej przeze mnie 
                     * jest metoda ktora zatrzymuje dispatchera z wykonywania ciagle tej samej funkcji
                     * zatrzymanie watku polaga zatem na zatrzymaniu dispatchera*/

                    if (listaWatkow.ElementAt(i).getNazwaWatku().Equals(index))
                    {
                        listaWatkow.ElementAt(i).stopThread();
                        break;
                    }
                }
                listaWatkowView.Items.Clear();
                for (int i = 0; i < listaWatkow.Count; i++)
                {
                    listaWatkowView.Items.Add(new Watek { nazwaWatku = listaWatkow.ElementAt(i).getNazwaWatku(), dziala = listaWatkow.ElementAt(i).czyDzialaWatek() });
                }

            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
        }

        private void wznowButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string index = listaWatkowView.SelectedItem.ToString();
                string[] rozbity = index.Split(' ');
                for (int i = 0; i < listawatkow.Count; i++)
                {
                    /* na liscie klasy Watek stworzonej przeze mnie 
                     * jest metoda ktora zatrzymuje dispatchera z wykonywania ciagle tej samej funkcji
                     * zatrzymanie watku polaga zatem na zatrzymaniu dispatchera*/

                    if (listaWatkow.ElementAt(i).getNazwaWatku().Equals(rozbity[0]))
                    {
                        listaWatkow.ElementAt(i).startThred();
                        break;
                    }
                }
                listaWatkowView.Items.Clear();
                for (int i = 0; i < listaWatkow.Count; i++)
                {
                    listaWatkowView.Items.Add(new Watek { nazwaWatku = listaWatkow.ElementAt(i).getNazwaWatku(), dziala = listaWatkow.ElementAt(i).czyDzialaWatek() });
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
        }
        private void usunWatekButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string index = listaWatkowView.SelectedItem.ToString();
                string[] rozbity = index.Split(' ');
                for (int i = 0; i < listawatkow.Count; i++)
                {
                    /* na liscie klasy Watek stworzonej przeze mnie 
                     * jest metoda ktora zatrzymuje dispatchera z wykonywania ciagle tej samej funkcji
                     * zatrzymanie watku polaga zatem na zatrzymaniu dispatchera*/

                    if (listaWatkow.ElementAt(i).getNazwaWatku().Equals(index))
                    {
                        listaWatkow.ElementAt(i).stopThread();
                        listaWatkow.RemoveAt(i);
                        break;
                    }
                }
                listaWatkowView.Items.Clear();
                for (int i = 0; i < listaWatkow.Count; i++)
                {
                    listaWatkowView.Items.Add(new Watek { nazwaWatku = listaWatkow.ElementAt(i).getNazwaWatku(), dziala = listaWatkow.ElementAt(i).czyDzialaWatek() });
                }

            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
        }
        private void stopAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < listaWatkow.Count; i++)
                {
                    listaWatkow.ElementAt(i).stopThread();
                }
                listaWatkowView.Items.Clear();
                for (int i = 0; i < listaWatkow.Count; i++)
                {
                    listaWatkowView.Items.Add(new Watek { nazwaWatku = listaWatkow.ElementAt(i).getNazwaWatku(), dziala = listaWatkow.ElementAt(i).czyDzialaWatek() });
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
        }
        private void wznowWszystkieButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < listaWatkow.Count; i++)
                {
                    listaWatkow.ElementAt(i).startThred();
                }
                listaWatkowView.Items.Clear();
                for (int i = 0; i < listaWatkow.Count; i++)
                {
                    listaWatkowView.Items.Add(new Watek { nazwaWatku = listaWatkow.ElementAt(i).getNazwaWatku(), dziala = listaWatkow.ElementAt(i).czyDzialaWatek() });
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
        }
        #endregion  


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /* jesli w pliku jest mniej zapisane niz aktualnie przypadkow na liscie to zapytaj czy zapisac
             * zmiany lub poinformuj uzytkownika o takiej mozliwosci */
            messageWindow okno = new messageWindow();
            okno.Show();
            Thread.Sleep(10000);
            if (okno.czyKliknietoTak())
            {
                writeDoBazy();
            }
            Application.Current.Shutdown();

            //Application.Current.Shutdown();

        }

        #endregion

        
        #region Wpisywanie Danych do pól wyjątki

        private void timeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string time = timeSlider.Value.ToString();
            string[] rozbity = time.Split(',');
            timeText.Text = rozbity[0];
            secondsText.Text = (Int64.Parse(rozbity[0])/10000000).ToString();
           
        }

        private void serverText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(serverText.Text, "\\.[^0-9]"))
            {
                MessageBox.Show("Możesz wpisac tylko cyfry i kropki");
                serverText.Text.Remove(serverText.Text.Length - 1);
            }
        }

        private void portText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(portText.Text, "[^0-9]"))
            {
                MessageBox.Show("Możesz wpisac tylko cyfry");
                portText.Text.Remove(portText.Text.Length - 1);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(timeText.Text, "[^0-9]"))
            {
                MessageBox.Show("Możesz wpisac tylko cyfry i kropki");
                timeText.Text.Remove(timeText.Text.Length - 1);
            }
            else
            {
                if (!timeText.Text.Equals("0"))
                {
                    timeSlider.Value = double.Parse(timeText.Text);
                    secondsText.Text = (double.Parse(timeText.Text) / 10000000).ToString();
                }
            }
        }

        private void secondsText_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(secondsText.Text, "\\.[^0-9]"))
                {
                    MessageBox.Show("Możesz wpisac tylko cyfry i kropki");
                    secondsText.Text.Remove(secondsText.Text.Length - 1);
                }
                else
                {
                    if (!secondsText.Text.Equals("0"))
                    {
                        timeSlider.Value = (double.Parse(secondsText.Text) * 10000000);
                        timeText.Text = (double.Parse(secondsText.Text) * 10000000).ToString();
                    }
                }
            }
            catch(Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
        }

        #endregion


        #region Pisanie i Czytanie Bazy Danych 

        /* Metoda czytania z bazy danych 
         * predefiniowanych zapytań do bazy sql */
        private void readBazaDanych()
        {
            try
            {
                reader = new StreamReader(fileName);
                string linia;
                while((linia=reader.ReadLine())!=null)
                {
                    string[] rozbity = linia.Split(';');
                    listaPrzypadkow.Add(new Przypadek(rozbity[0],
                        rozbity[1],
                        rozbity[2],
                        rozbity[3],
                        rozbity[4],
                        rozbity[5],
                        rozbity[6],
                        rozbity[7],
                        rozbity[8],
                        rozbity[9],
                        rozbity[10],
                        Int64.Parse(rozbity[11])));
                }
                reader.Close();
                listView.Items.Clear();
                for(int i=0;i<listaPrzypadkow.Count;i++)
                {
                    listView.Items.Add(listaPrzypadkow.ElementAt(i));
                }
            }
            catch(Exception msg)
            {
                MessageBox.Show("Błąd podczas odczytu z bazy danych\n\n"+msg.ToString());
            }
        }
        /* Metoda wczytania do programu wczesniej
         * zapisanych zapytan */
        public void writeDoBazy()
        {
            try
            {
                writer = new StreamWriter(fileName);
                for (int i = 0; i < listaPrzypadkow.Count; i++)
                {
                    string przypadek = listaPrzypadkow.ElementAt(i).getNazwaPrzypadku() + ";" +
                                        listaPrzypadkow.ElementAt(i).getServer() + ";" +
                                        listaPrzypadkow.ElementAt(i).getPort() + ";" +
                                        listaPrzypadkow.ElementAt(i).getLogin() + ";" +
                                        listaPrzypadkow.ElementAt(i).getPass() + ";" +
                                        listaPrzypadkow.ElementAt(i).getDatabase() + ";" +
                                        listaPrzypadkow.ElementAt(i).getKolumnaIf() + ";" +
                                        listaPrzypadkow.ElementAt(i).getKolumnaThen() + ";" +
                                        listaPrzypadkow.ElementAt(i).getWyrazenieIf() + ";" +
                                        listaPrzypadkow.ElementAt(i).getWartoscThen() + ";" +
                                        listaPrzypadkow.ElementAt(i).getSqlQuestion() + ";" +
                                        listaPrzypadkow.ElementAt(i).getInterwal().ToString();

                    //public Przypadek(string _nazwaPrzypadku,
                    //string _server, string _port,
                    //string _login, string _pass, string _database,
                    //string _kolumnaIF, string _kolumnaThen, string _wyrazenieIf,
                    //string _wartoscThen,string _sql,Int64 _interwal)
                    writer.WriteLine(przypadek);
                }
                writer.Close();
                MessageBox.Show("Pomyślnie zapisano liste do bazy danych");
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
        }

        #endregion
        // Odswiez button
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            DataTable cache = new DataTable();
            try
            {
                cache = listaWatkow.ElementAt(0).getCacheList();
                listViewCache.Items.Clear();
                for (int i = 0; i < cache.Rows.Count; i++)
                {
                    listViewCache.Items.Add(i + "\t" + cache.Rows[i][0] + "\t" + cache.Rows[i][1]);
                }
            }
            catch(Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
        }

        public string tabela { get; set; }      
    }
}