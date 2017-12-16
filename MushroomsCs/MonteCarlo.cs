using System;
using MushroomsCs.Models;

namespace MushroomsCs
{
    public static class MonteCarlo
    {
        private const int NumberOfTries = 100000;

        public static double GetResultOfTheGame(Board board)
        {
            var numberOfPlayerOneWins = 0;
            for (var i = 0; i < NumberOfTries; i++)
            {
                var nobodyWon = true;
                var playerOneTurn = true;
                var random = new Random();
                while (nobodyWon)
                {
                    var index = random.Next(0, board.Cube.NumberOfWalls);
                    board.MovePlayer(playerOneTurn, board.Cube.PossibleResults[index]);
                    if (playerOneTurn && board.Player1.Location == 0)
                    {
                        numberOfPlayerOneWins++;
                        nobodyWon = false;
                    }
                    else if(!playerOneTurn && board.Player2.Location == 0)
                    {
                        nobodyWon = false;
                    }
                    playerOneTurn = !playerOneTurn;
                }
            }
            return (double) numberOfPlayerOneWins / NumberOfTries;
        }
    }
}