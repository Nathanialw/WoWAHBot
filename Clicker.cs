using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AutoIt;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ClickControl
{
    internal static class Clicker
    {
        //data
        private static readonly Random r = new Random();

        public static int variance = 50;
        public static int iterations = 0;
        public static string windowTitle = "World of Warcraft";
        public static string spamKey = "+{F12}";
        public static int Slider_Pos;
        public static bool clickerState = false;
        public static bool posterState = false;

        public static int Search_Duration;
        public static int Clicker_Duration;
        public static int Logon_Wait;
        public static int Time_Between_Events;
        public static int Number_Characters;
        public static int Number_Reposts;

        //methods
        public static async void RunClickerAsync()
        {
            clickerState = true;
            while (clickerState)
            {
                _ = AutoItX.ControlSend(windowTitle, "", "", spamKey);
                await Task.Delay(DelayCalc());
            }
        }

        private static int DelayCalc()
        {
            int a = Slider_Pos - variance;
            int b = Slider_Pos + variance;
            if (a < 1) { a = 1; };              //in case of zero or negative
            return r.Next(a, b);
        }

        private static async Task PostAuctions(int Number_of_Reposts)
        {
            //repeat x number of times
            for (int i = 0; i < Number_of_Reposts; i++)
            {
                // tar auctioneer
                _ = AutoItX.ControlSend(windowTitle, "", "", "1");
                await Task.Delay(Time_Between_Events + DelayCalc());

                // open aucitoneer panel
                _ = AutoItX.ControlSend(windowTitle, "", "", "0");
                await Task.Delay(Time_Between_Events + DelayCalc());

                // run post scan
                _ = AutoItX.ControlSend(windowTitle, "", "", "2");
                await Task.Delay(Search_Duration + DelayCalc());

                // post                
                RunClickerAsync();
                await Task.Delay(Clicker_Duration + DelayCalc());
                clickerState = false;

                // close auctioneer panel
                _ = AutoItX.ControlSend(windowTitle, "", "", "{esc}");
                await Task.Delay(Search_Duration + DelayCalc());
            }
        }

        private static async Task Login()
        {
            // logout
            _ = AutoItX.ControlSend(windowTitle, "", "", "{enter}");
            await Task.Delay(Logon_Wait + DelayCalc());
        }

        private static async Task Logout()
        {
            //take screenshot
            _ = AutoItX.ControlSend(windowTitle, "", "", "{}");

            // logout
            _ = AutoItX.ControlSend(windowTitle, "", "", "3");
            await Task.Delay(Time_Between_Events + DelayCalc());
        }

        private static async Task GoToGlyphChar(int currentChar)
        {
            for (int i = 0; i < currentChar; i++)
            {
                // return to top
                _ = AutoItX.ControlSend(windowTitle, "", "", "{down}");
                await Task.Delay(Time_Between_Events + DelayCalc());
            }
        }

        private static async Task ReturnToTop(int currentChar)
        {
            for (int i = 0; i < currentChar; i++)
            {
                // return to top
                _ = AutoItX.ControlSend(windowTitle, "", "", "{up}");
                await Task.Delay(Time_Between_Events + DelayCalc());
            }
        }

        public static async void RunAuctionPostAsync()
        {
            int numChars = Number_Characters;
            posterState = true;

            //needs to be able to pause or end
            while (posterState)
            {
                for (int currentChar = 1; currentChar <= numChars; currentChar++)
                {
                    //login enchants
                    await Login();
                    //post just once
                    await PostAuctions(1);
                    //logout
                    await Logout();

                    //Go to glyphs                
                    await GoToGlyphChar(currentChar);

                    //login glyphs
                    await Login();
                    //post
                    await PostAuctions(Number_Reposts);                   
                    //logout
                    await Logout();

                    //Reset
                    await ReturnToTop(currentChar);                    
                }
            }
            
        }

        public static void Set_Settings()
        {
            var path = "./settings.txt";
            
            try
            {
                string[] lines = File.ReadAllLines(path);

                if (lines.Length == 6)
                {
                    Search_Duration = Convert.ToInt32(lines[0]) * 1000;
                    Clicker_Duration = Convert.ToInt32(lines[1]) * 1000;
                    Logon_Wait = Convert.ToInt32(lines[2]) * 1000;
                    Time_Between_Events = Convert.ToInt32(lines[3]) * 1000;
                    Number_Characters = Convert.ToInt32(lines[4]);
                    Number_Reposts = Convert.ToInt32(lines[5]);
                }
            }
            catch
            {
                Search_Duration = 60 * 1000;
                Clicker_Duration = 180 * 1000;
                Logon_Wait = 30 * 1000;
                Time_Between_Events = 5 * 1000;
                Number_Characters = 5;
                Number_Reposts = 10;
            }
        }
    }
}

