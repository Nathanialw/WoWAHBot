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

        public static int Search_Duration = 1;
        public static int Clicker_Duration = 1;
        public static int Logon_Wait = 1;

        public static int Time_Between_Events = 1;
        public static int Number_Characters = 1;

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

        public static async void RunAuctionPostAsync()
        {
            int numChars = 1;
            posterState = true;
            while (posterState)
            {
                // wait a moment for the game to load
                await Task.Delay(30000 + DelayCalc());

                // tar auctioneer
                _ = AutoItX.ControlSend(windowTitle, "", "", "1");
                await Task.Delay(Time_Between_Events + DelayCalc());
                if (!posterState) break;

                // open aucitoneer panel
                _ = AutoItX.ControlSend(windowTitle, "", "", "0");
                await Task.Delay(Time_Between_Events + DelayCalc());
                if (!posterState) break;

                // run post scan
                _ = AutoItX.ControlSend(windowTitle, "", "", "2");                                
                await Task.Delay(Search_Duration + DelayCalc());
                if (!posterState) break;

                // post                
                RunClickerAsync();
                await Task.Delay(Clicker_Duration + DelayCalc());
                clickerState = false;
                if (!posterState) break;

                // short delay before logging off
                await Task.Delay(Time_Between_Events + DelayCalc());                
                if (!posterState) break;

                if (Number_Characters > 1)
                {
                    // logout
                    _ = AutoItX.ControlSend(windowTitle, "", "", "3");
                    await Task.Delay(Logon_Wait + DelayCalc());
                    if (!posterState) break;

                    // check if last character
                    if (numChars < Number_Characters)
                    {
                        // next character
                        _ = AutoItX.ControlSend(windowTitle, "", "", "{down}");
                        await Task.Delay(Time_Between_Events + DelayCalc());
                        if (!posterState) break;
                        numChars++;
                    }
                    else { 
                        // reset char list loop
                        for (int i = 1; i < (Number_Characters); i++)
                        {
                            _ = AutoItX.ControlSend(windowTitle, "", "", "{up}");
                            await Task.Delay(Time_Between_Events + DelayCalc());
                            if (!posterState) break;
                        }
                        numChars = 1;
                    }

                     // login
                    _ = AutoItX.ControlSend(windowTitle, "", "", "{enter}");
                    await Task.Delay(Time_Between_Events + DelayCalc());
                    if (!posterState) break;
                }
                else
                {
                    // wait for next loop
                    _ = AutoItX.ControlSend(windowTitle, "", "", "{esc}");
                    await Task.Delay(Logon_Wait + DelayCalc());
                    if (!posterState) break;
                }
            }
        }
        public static void Set_Settings()
        {
            var path = "settings.txt";
            string[] lines = File.ReadAllLines(path);
            
            if (lines.Length > 0) { 
                Search_Duration = Convert.ToInt32(lines[0]);
                Clicker_Duration = Convert.ToInt32(lines[1]);
                Logon_Wait = Convert.ToInt32(lines[2]);
                Time_Between_Events = Convert.ToInt32(lines[3]);
                Number_Characters = Convert.ToInt32(lines[4]);
            }
        }
    }
}

