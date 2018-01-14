using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MushroomsCs.Models;

namespace MushroomsCs
{
    public class FileManager
    {
        public void WriteToFile(Matrix myMatrix, int size, StreamWriter sw)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    sw.Write(myMatrix.MatrixValues[i, j] + " ");
                }
            }
            sw.WriteLine();
        }

        public void WriteToFile(double[] vector, int size, StreamWriter sw)
        {
            for (int i = 0; i < size; i++)
            {
                sw.Write(vector[i] + " ");
            }
            sw.WriteLine();
        }

        public void WriteToFile(int x, StreamWriter sw)
        {
            sw.Write(x);
        }

        public double[] ReadVectorFromFile(int size, StreamReader sr)
        {
            var vector = new double[size];

            var line = sr.ReadLine();
            string[] splitLine = line.Split(' ');
            
            for (int j = 0; j < size; j++)
            {
                vector[j] = double.Parse(splitLine[j], System.Globalization.CultureInfo.InvariantCulture);
            }

            return vector;
        }

        public double[,] ReadMatrixFromFile(int size, StreamReader sr)
        {
            double[,] matrix = new double[size, size];

            var line = sr.ReadLine();
            string[] splitLine = line.Split(' ');

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int splitNumber = size * i + j;
                    matrix[i, j] = double.Parse(splitLine[splitNumber], System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return matrix;
        }

        public static IEnumerable<Board> GenerateBoards(int numberOfBoards)
        {
            for (var i = 5; i <= numberOfBoards; i++)
            {
                var board = new Board
                {
                    Size = 2 * i + 1,
                    Player1 = new Player
                    {
                        Location = i,
                        NumberOfCollectedMushrooms = 0
                    },
                    Player2 = new Player
                    {
                        Location = -i,
                        NumberOfCollectedMushrooms = 0
                    }
                };

                var cube = new Cube
                {
                    NumberOfWalls = 3
                };
                cube.ProbabilitiesOfCertainResult = new double[cube.NumberOfWalls];
                cube.PossibleResults = new []
                {
                    0, 1, -1
                };
                cube.NaturalProbabilities = new []
                {
                    1, 1, 1
                };
                
                for (int y = 0; y < cube.NumberOfWalls; y++)
                {
                    cube.ProbabilitiesOfCertainResult[y] = (double)cube.NaturalProbabilities[y] / cube.NaturalProbabilities.Sum();
                }
                board.Cube = cube;
                yield return board;
            }
        }

        public static Board GenerateBoard(int boardSize)
        {
            var board = new Board
            {
                Size = 2 * boardSize + 1,
                Player1 = new Player
                {
                    Location = boardSize,
                    NumberOfCollectedMushrooms = 0
                },
                Player2 = new Player
                {
                    Location = -boardSize,
                    NumberOfCollectedMushrooms = 0
                }
            };

            var cube = new Cube
            {
                NumberOfWalls = 3
            };
            cube.ProbabilitiesOfCertainResult = new double[cube.NumberOfWalls];
            cube.PossibleResults = new[]
            {
                0, 1, -1
            };
            cube.NaturalProbabilities = new[]
            {
                1, 1, 1
            };

            for (int y = 0; y < cube.NumberOfWalls; y++)
            {
                cube.ProbabilitiesOfCertainResult[y] = (double)cube.NaturalProbabilities[y] / cube.NaturalProbabilities.Sum();
            }
            board.Cube = cube;
            return board;
        }
    }
}
