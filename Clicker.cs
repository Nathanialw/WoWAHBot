using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AutoIt;


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
            posterState = true;
            while (posterState)
            {               
                // tar auctioneer
                _ = AutoItX.ControlSend(windowTitle, "", "", "1");
                await Task.Delay(2000 + DelayCalc());
                // run post scan
                _ = AutoItX.ControlSend(windowTitle, "", "", "2");                                
                await Task.Delay(2000 + DelayCalc());
                // post
                RunClickerAsync();
                await Task.Delay(120000 + DelayCalc());
                clickerState = false;

                // logout
                _ = AutoItX.ControlSend(windowTitle, "", "", "3");
                await Task.Delay(10000 + DelayCalc());
                // next character
                _ = AutoItX.ControlSend(windowTitle, "", "", "{down}");
                await Task.Delay(900000 + DelayCalc());
                // login
                _ = AutoItX.ControlSend(windowTitle, "", "", "{enter}");
                await Task.Delay(20000 + DelayCalc());
            }
        }

    }
}

