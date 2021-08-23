using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AutoIt;


namespace ClickControl
{
    static class Clicker
    {
        //data
        private static readonly Random r = new Random();

        public static int variance = 50;
        public static int iterations = 0;
        public static string windowTitle = "World of Warcraft";
        public static string spamKey = "+{F12}";
        public static int Slider_Pos;
        public static bool state = false;

        //methods
        public static async void RunClickerAsync()
        {
            state = true;
            while (state)
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
    }
}

