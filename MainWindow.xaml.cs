using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ClickControl;


namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Top clicker portion
        private void Button_Start(object sender, RoutedEventArgs e)
        {
            clickerStart.IsEnabled = false;
            clickerStop.IsEnabled = true;
            Clicker.RunClickerAsync();
        }
        private void Button_Stop(object sender, RoutedEventArgs e)
        {
            clickerStop.IsEnabled = false;
            clickerStart.IsEnabled = true;
            Clicker.clickerState = false;
        }
        private void Delay_Slider(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Clicker.Slider_Pos = Convert.ToInt32(e.NewValue);
        }
        private void variance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Clicker.variance = Convert.ToInt32(e.NewValue);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clicker.spamKey = Spam_Key.Text;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Clicker.windowTitle = Window_Title.Text;
        }

        //Bottom bot portion

        private void Posting_Start(object sender, RoutedEventArgs e)
        {
            posterStart.IsEnabled = false;
            posterStop.IsEnabled = true;
            Clicker.RunAuctionPostAsync();
        }
        private void Posting_Stop(object sender, RoutedEventArgs e)
        {
            posterStop.IsEnabled = false;
            posterStart.IsEnabled = true;
            Clicker.posterState = false;
            Clicker.clickerState = false;
        }

    }
}




