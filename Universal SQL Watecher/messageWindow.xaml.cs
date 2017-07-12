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
using Universal_SQL_Watecher;

namespace Universal_SQL_Watecher
{
    /// <summary>
    /// Interaction logic for messageWindow.xaml
    /// </summary>
    public partial class messageWindow : Window
    {
        bool klikniecieTak;
        public messageWindow()
        {
            InitializeComponent();
        }

        private void takButton_Click(object sender, RoutedEventArgs e)
        {
            klikniecieTak = true; 
        }
        public bool czyKliknietoTak()
        {
            return klikniecieTak;
        }
    }
}
