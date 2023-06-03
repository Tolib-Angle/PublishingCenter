using PublishingCenter.PublishingCenter.Menu;
using System;

namespace PublishingCenter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Black; // text color
            Console.BackgroundColor = ConsoleColor.White; // baclground color

            Console.Clear();
            Menu.Start();
        }
    }
}
