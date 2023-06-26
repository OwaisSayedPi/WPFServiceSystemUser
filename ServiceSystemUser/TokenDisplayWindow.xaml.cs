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

namespace ServiceSystemUser
{
    /// <summary>
    /// Interaction logic for TokenDisplayWindow.xaml
    /// </summary>
    public partial class TokenDisplayWindow : Window
    {
        public TokenDisplayWindow()
        {
            InitializeComponent();
        }

        internal void UpdateFields(string tokenID, int counterID)
        {
            CounterText.Content = counterID.ToString();
            TokenText.Content = tokenID;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
            Environment.Exit(0);
        }
    }
}
