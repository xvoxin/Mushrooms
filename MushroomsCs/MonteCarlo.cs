using System;
using MushroomsCs.Models;

namespace MushroomsCs
{
    public static class MonteCarlo
    {
        private const int NumberOfTries = 1000000;

        public static double GetResultOfTheGame(Board board)
        {
            var numberOfPlayerOneWins = 0;
            var newProbabilities = new double[board.Cube.NumberOfWalls];
            newProbabilities[0] = board.Cube.ProbabilitiesOfCertainResult[0];
            for (int j = 1; j < board.Cube.NumberOfWalls; j++)
            {
                newProbabilities[j] = board.Cube.ProbabilitiesOfCertainResult[j] + newProbabilities[j - 1];
            }
            for (var i = 0; i < NumberOfTries; i++)
            {
                var nobodyWon = true;
                var playerOneTurn = true;
                var random = new Random();
                while (nobodyWon)
                {
                    var index = 0;
                    var randomValue = random.NextDouble();
                    for (int j = 1; j < board.Cube.NumberOfWalls; j++)
                    {
                        if (randomValue > newProbabilities[j - 1] && randomValue < newProbabilities[j])
                        {
                            index = j;
                            break;
                        }
                    }
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