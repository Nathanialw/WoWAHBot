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
        private void Show_Settings()
        {
            Search_Duration_text.Text = Convert.ToString(Clicker.Search_Duration / 1000);
            Clicker_Duration_text.Text = Convert.ToString(Clicker.Clicker_Duration / 1000);
            Wait_For_Login_text.Text = Convert.ToString(Clicker.Logon_Wait / 1000);
            Delay_Between_Posts_text.Text = Convert.ToString(Clicker.Delay_Between_Posts / 1000);
            Time_Between_Events_text.Text = Convert.ToString(Clicker.Time_Between_Events / 1000);
            Number_Of_Characters_text.Text = Convert.ToString(Clicker.Number_Characters);
            Number_Of_Reposts_text.Text = Convert.ToString(Clicker.Number_Reposts);
        }

        private void Show_ETA()
        {
            int ETA_Seconds = ((((Clicker.Search_Duration / 1000) + (Clicker.Clicker_Duration / 1000) + (Clicker.Delay_Between_Posts / 1000)) * Clicker.Number_Reposts) * Clicker.Number_Characters) + ((Clicker.Logon_Wait * 2 / 1000) * Clicker.Number_Characters);

            int Hours = ETA_Seconds / 3600;
            int Remaining_Seconds = (ETA_Seconds - (Hours * 3600));
            int Minutes = Remaining_Seconds / 60;
            float Secs = (float)ETA_Seconds - ((float)Hours * 3600.0f) - ((float)Minutes * 60.0f);

            ETA_Value_Text.Content = Convert.ToString(Hours) + ":" + Convert.ToString(Minutes) + ":" + Convert.ToString(Secs);
        }

        public MainWindow()
        {
            InitializeComponent();
            Clicker.Set_Settings();
            Show_Settings();
            Show_ETA();

            //maybe we can put an async while loop that checks if the bot is done to control the start/stop buttons
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
        private void Set_Milling(object sender, RoutedEventArgs e)
        {
            Clicker.Slider_Pos = 1200;
            Milling.IsEnabled = false;
            Speed.IsEnabled = false;
            Speed.Value = 1200;

            Crafting.IsEnabled = true;
            Auction.IsEnabled = true;
        }
        private void Set_Crafting(object sender, RoutedEventArgs e)
        {
            Clicker.Slider_Pos = 5000;
            Crafting.IsEnabled = false;
            Speed.IsEnabled = false;
            Speed.Value = 5000;

            Milling.IsEnabled = true;
            Auction.IsEnabled = true;
        }
        private void Set_Auction(object sender, RoutedEventArgs e)
        {
            Clicker.Slider_Pos = 200;
            Auction.IsEnabled = false;
            Speed.IsEnabled = false;
            Speed.Value = 200;

            Milling.IsEnabled = true;
            Crafting.IsEnabled = true;
        }

        private void Reset_Delay(object sender, RoutedEventArgs e)
        {
            Clicker.Slider_Pos = 200;
            Milling.IsEnabled = true;
            Speed.IsEnabled = true;
            Speed.Value = 200;

            Crafting.IsEnabled = true;
            Auction.IsEnabled = true;
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
            if (clickerStop.IsEnabled && !clickerStart.IsEnabled)
            {
                clickerStop.IsEnabled = false;
                clickerStart.IsEnabled = true;
            }

            posterStart.IsEnabled = false;
            posterStop.IsEnabled = true;
            Clicker.RunAuctionPostAsync();
        }

        private async Task WaitForStop()
        {
            while (Clicker.running)
            {
                await Clicker.Delay(1);
            }
        }

        private async void Posting_Stop(object sender, RoutedEventArgs e)
        {
            Clicker.posterState = false;
            Clicker.clickerState = false;

            await WaitForStop();
            
            if (!Clicker.running)
            {
                if (clickerStop.IsEnabled && !clickerStart.IsEnabled)
                {
                    clickerStop.IsEnabled = false;
                    clickerStart.IsEnabled = true;
                }
                //async while loops is not yet complete wait

                posterStop.IsEnabled = false;
                posterStart.IsEnabled = true;
            }
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

        private void Delay_Between_Posts(object sender, RoutedEventArgs e)
        {
            Clicker.Delay_Between_Posts = Convert.ToInt32(Delay_Between_Posts_text.Text);
        }

        private void Number_Of_Reposts(object sender, RoutedEventArgs e)
        {
            Clicker.Number_Reposts = Convert.ToInt32(Number_Of_Reposts_text.Text);
        }

        private void Time_Between_Events(object sender, RoutedEventArgs e)
        {
            Clicker.Time_Between_Events = Convert.ToInt32(Time_Between_Events_text.Text) * 1000;
        }

        private void Number_Of_Characters(object sender, RoutedEventArgs e)
        {
            Clicker.Number_Characters = Convert.ToInt32(Number_Of_Characters_text.Text);
        }

        private void Save_Settings(object sender, RoutedEventArgs e)
        {
            Clicker.Search_Duration = Convert.ToInt32(Search_Duration_text.Text) * 1000;
            Clicker.Clicker_Duration = Convert.ToInt32(Clicker_Duration_text.Text) * 1000;
            Clicker.Logon_Wait = Convert.ToInt32(Wait_For_Login_text.Text) * 1000;
            Clicker.Delay_Between_Posts = Convert.ToInt32(Delay_Between_Posts_text.Text) * 1000;
            Clicker.Time_Between_Events = Convert.ToInt32(Time_Between_Events_text.Text) * 1000;
            Clicker.Number_Characters = Convert.ToInt32(Number_Of_Characters_text.Text);
            Clicker.Number_Reposts = Convert.ToInt32(Number_Of_Reposts_text.Text);
            Clicker.Save_Settings();
            Show_ETA();
            //write it to text file to save settings
        }

        private void Load_Settings(object sender, RoutedEventArgs e)
        {
            Clicker.Set_Settings();
            Show_Settings();
            Show_ETA();
        }

        private void Load_Defaults(object sender, RoutedEventArgs e)
        {
            Clicker.Load_Default_Settings();
            Show_Settings();
            Show_ETA();
        }

        private void Set_Continuous_Loop(object sender, RoutedEventArgs e)
        {
            Clicker.continuousLoop = true;
        }

        private void Unset_Continuous_Loop(object sender, RoutedEventArgs e)
        {
            Clicker.continuousLoop = false;
        }
  
    }
}