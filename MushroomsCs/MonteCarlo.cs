using System;
using System.Linq;
using MushroomsCs.Models;

namespace MushroomsCs
{
    public static class MonteCarlo
    {
        private const int NumberOfTries = 1000000;

        public static double GetResultOfTheGame(Board board, Random random)
        {
            var numberOfPlayerOneWins = 0;
            var numberOfElementsForArray = board.Cube.NaturalProbabilities.Sum();
            var possibilities = new int[numberOfElementsForArray];
            var point = 0;
            for (int i = 0; i < board.Cube.NumberOfWalls; i++)
            {
                for (int j = 0; j < board.Cube.NaturalProbabilities[i]; j++)
                {
                    possibilities[point++] = board.Cube.PossibleResults[i];
                }
            }

            var playerOnePosition = board.Player1.Location;
            var playerTwoPosition = board.Player2.Location;
            for (var i = 0; i < NumberOfTries; i++)
            {
                board.Player1.Location = playerOnePosition;
                board.Player2.Location = playerTwoPosition;
                var nobodyWon = true;
                var playerOneTurn = true;
                while (nobodyWon)
                {
                    var index = random.Next(0, numberOfElementsForArray);
                    board.MovePlayer(playerOneTurn, possibilities[index]);
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
            board.Player1.Location = playerOnePosition;
            board.Player2.Location = playerTwoPosition;
            return (double) numberOfPlayerOneWins / (double) NumberOfTries;
        }
    }
}