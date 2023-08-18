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
            Clicker.Set_Settings();
            Search_Duration_text.Text = Convert.ToString(Clicker.Search_Duration);
            Clicker_Duration_text.Text = Convert.ToString(Clicker.Clicker_Duration);
            Wait_For_Login_text.Text = Convert.ToString(Clicker.Logon_Wait);
            Time_Between_Events_text.Text = Convert.ToString(Clicker.Time_Between_Events);
            Number_Of_Characters_text.Text = Convert.ToString(Clicker.Number_Characters);
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

        private void Search_Duration(object sender, RoutedEventArgs e)
        {
            Clicker.Search_Duration = Convert.ToInt32(Search_Duration_text.Text) * 1000;
        }

        private void Clicker_Duration(object sender, RoutedEventArgs e)
        {
            Clicker.Clicker_Duration = Convert.ToInt32(Clicker_Duration_text.Text) * 1000;
        }

        private void Wait_For_Login(object sender, RoutedEventArgs e)
        {
            Clicker.Logon_Wait = Convert.ToInt32(Wait_For_Login_text.Text) * 1000;
        }

        private void Time_Between_Events(object sender, RoutedEventArgs e)
        {
            Clicker.Time_Between_Events = Convert.ToInt32(Time_Between_Events_text.Text) * 1000;
        }

        private void Number_Of_Characters(object sender, RoutedEventArgs e)
        {
            Clicker.Number_Characters = Convert.ToInt32(Number_Of_Characters_text.Text);
        }

        private void Set_All(object sender, RoutedEventArgs e)
        {
            Clicker.Search_Duration = Convert.ToInt32(Search_Duration_text.Text) * 1000;
            Clicker.Clicker_Duration = Convert.ToInt32(Clicker_Duration_text.Text) * 1000;
            Clicker.Logon_Wait = Convert.ToInt32(Wait_For_Login_text.Text) * 1000;
            Clicker.Time_Between_Events = Convert.ToInt32(Time_Between_Events_text.Text) * 1000;
            Clicker.Number_Characters = Convert.ToInt32(Number_Of_Characters_text.Text);
        }
    }
}