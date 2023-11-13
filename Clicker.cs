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
        public static bool continuousLoop = false;

        public static int Search_Duration;
        public static int Clicker_Duration;
        public static int Delay_Between_Posts;
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

        private static async Task PressButton(string key)
        {
            _ = AutoItX.ControlSend(windowTitle, "", "", key);
            await Task.Delay(Time_Between_Events + DelayCalc());
        }

        private static async Task PostAuctions(int Number_of_Reposts)
        {
            //repeat x number of times
            for (int i = 0; i < Number_of_Reposts; i++)
            {
                // tar auctioneer
                await PressButton("1");
                if (!posterState) // if stopped break
                {
                    break;
                }

                // open aucitoneer panel
                await PressButton("0");
                if (!posterState) // if stopped break
                {
                    break;
                }

                // run post scan
                await PressButton("2");
                if (!posterState) // if stopped break
                {
                    break;
                }

                //hit pause, run clicker

                // post                
                RunClickerAsync();
                await Task.Delay(Clicker_Duration + DelayCalc());
                clickerState = false;
                if (!posterState) // if stopped break
                {
                    break;
                }

                // close auctioneer panel
                await PressButton("{esc}");
                if (!posterState) // if stopped break
                {
                    break;
                }

                // wait between posts
                await Task.Delay(Delay_Between_Posts);
                if (!posterState) // if stopped break
                {
                    break;
                }
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
            //_ = AutoItX.ControlSend(windowTitle, "", "", "{}");

            // logout
            _ = AutoItX.ControlSend(windowTitle, "", "", "3");
            await Task.Delay(Logon_Wait + DelayCalc());
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
        
        // [+] write to text file on set all press to save settings
        // [] add keystrokes during auction search to press pause on search and start posting from there

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
                    if (!posterState) // if stopped break
                    {
                        break;
                    }
                    //post just once
                    await PostAuctions(1);
                    if (!posterState) // if stopped break
                    {
                        break;
                    }
                    //logout
                    await Logout();
                    if (!posterState) // if stopped break
                    {
                        break;
                    }
                    //Go to glyphs                
                    await GoToGlyphChar(currentChar);
                    if (!posterState) // if stopped break
                    {
                        break;
                    }
                    //login glyphs
                    await Login();
                    if (!posterState) // if stopped break
                    {
                        break;
                    }
                    //post
                    await PostAuctions(Number_Reposts);
                    if (!posterState) // if stopped break
                    {
                        break;
                    }
                    //logout
                    await Logout();
                    if (!posterState) // if stopped break
                    {
                        break;
                    }

                    //Reset
                    await ReturnToTop(currentChar);
                    if (!posterState) // if stopped break
                    {
                        break;
                    }
                }

                if (!continuousLoop) // if false only run once
                {
                    posterState = false;
                }
            }
        }

        public static void Save_Settings()
        {
            var userPath = "./user_settings.txt";

            string[] wlines = {
                    Convert.ToString(Search_Duration / 1000),
                    Convert.ToString(Clicker_Duration / 1000),
                    Convert.ToString(Logon_Wait / 1000),
                    Convert.ToString(Delay_Between_Posts / 1000),
                    Convert.ToString(Time_Between_Events  / 1000),
                    Convert.ToString(Number_Characters),
                    Convert.ToString(Number_Reposts)
            };

            File.WriteAllLines("./user_settings.txt", wlines);
        }

        public static void Load_Default_Settings()
        {
            var defaultPath = "./default_settings.txt";

            string[] lines = File.ReadAllLines(defaultPath);

            if (lines.Length == 7)
            {
                Search_Duration = Convert.ToInt32(lines[0]) * 1000;
                Clicker_Duration = Convert.ToInt32(lines[1]) * 1000;
                Logon_Wait = Convert.ToInt32(lines[2]) * 1000;
                Delay_Between_Posts = Convert.ToInt32(lines[3]) * 1000;
                Time_Between_Events = Convert.ToInt32(lines[4]) * 1000;
                Number_Characters = Convert.ToInt32(lines[5]);
                Number_Reposts = Convert.ToInt32(lines[6]);
            }
            else
            {
                Search_Duration = 120 * 1000;
                Clicker_Duration = 240 * 1000;
                Logon_Wait = 130 * 1000;
                Delay_Between_Posts = 5 * 1000;
                Time_Between_Events = 5 * 1000;
                Number_Characters = 5;
                Number_Reposts = 20;
            }
        }

        public static void Set_Settings()
        {            
            var userPath = "./user_settings.txt";

            try
            {
                string[] lines = File.ReadAllLines(userPath);

                if (lines.Length == 7)
                {
                    Search_Duration = Convert.ToInt32(lines[0]) * 1000;
                    Clicker_Duration = Convert.ToInt32(lines[1]) * 1000;
                    Logon_Wait = Convert.ToInt32(lines[2]) * 1000;
                    Delay_Between_Posts = Convert.ToInt32(lines[3]) * 1000;
                    Time_Between_Events = Convert.ToInt32(lines[4]) * 1000;
                    Number_Characters = Convert.ToInt32(lines[5]);
                    Number_Reposts = Convert.ToInt32(lines[6]);
                }
            }
            catch
            {
                try
                {
                    Load_Default_Settings();
                }
                catch
                {
                    Search_Duration = 120 * 1000;
                    Clicker_Duration = 240 * 1000;
                    Logon_Wait = 130 * 1000;
                    Delay_Between_Posts = 5 * 1000;
                    Time_Between_Events = 5 * 1000;
                    Number_Characters = 5;
                    Number_Reposts = 20;
                }
            }
        }
    }
}
