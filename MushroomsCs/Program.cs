using System;

namespace MushroomsCs
{
    internal class Program
    {
        private static void Main()
        {
            var board = File.CreateBoard();
            Console.WriteLine($"{MonteCarlo.GetResultOfTheGame(board):.######}");
        }
    }
}
