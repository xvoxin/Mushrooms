using System;
using MushroomsCs.Models;

namespace MushroomsCs
{
    public static class MonteCarlo
    {
        private const int NumberOfTries = 10000000;

        public static double GetResultOfTheGame(Board board, Random random)
        {
            var numberOfPlayerOneWins = 0;
            var newProbabilities = new int[board.Cube.NumberOfWalls];
            newProbabilities[0] = (int)(board.Cube.ProbabilitiesOfCertainResult[0] * 100);
            var playerOnePosition = board.Player1.Location;
            var playerTwoPosition = board.Player2.Location;
            for (int j = 1; j < board.Cube.NumberOfWalls; j++)
            {
                newProbabilities[j] = (int) (board.Cube.ProbabilitiesOfCertainResult[j] * 100) + newProbabilities[j - 1];
            }
            for (var i = 0; i < NumberOfTries; i++)
            {
                board.Player1.Location = playerOnePosition;
                board.Player2.Location = playerTwoPosition;
                var nobodyWon = true;
                var playerOneTurn = true;
                while (nobodyWon)
                {
                    var index = 0;
                    var randomValue = random.Next(0, 100);
                    for (int j = 1; j < board.Cube.NumberOfWalls; j++)
                    {
                        if (j == 0 && randomValue >= 0 && randomValue < newProbabilities[j])
                        {
                            index = j;
                            break;
                        }
                        if (randomValue >= newProbabilities[j - 1] && randomValue < newProbabilities[j])
                        {
                            index = j;
                            break;
                        }
                        if (j == board.Cube.NumberOfWalls - 1 && randomValue > newProbabilities[j])
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
            board.Player1.Location = playerOnePosition;
            board.Player2.Location = playerTwoPosition;
            return (double) numberOfPlayerOneWins / (double) NumberOfTries;
        }
    }
}