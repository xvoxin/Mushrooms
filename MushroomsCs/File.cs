using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MushroomsCs.Models;

namespace MushroomsCs
{
    public class File
    {
        private const string DataFileName = "SampleData";

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

        public double[] FillVectorWithRandom(int size)
        {
            var vector = new double[size];
            var random = new Random();

            for (int i = 0; i < size; i++)
            {
                vector[i] = random.NextDouble() * random.Next(Int32.MinValue, Int32.MaxValue);
            }

            return vector;
        }

        public static Board CreateBoard(int nr)
        {
            var data = System.IO.File.ReadAllLines(DataFileName + nr + ".txt");
            var board = new Board
            {
                Size = 2 * int.Parse(data[0]) + 1,
                Player1 = new Player
                {
                    Location = int.Parse(data[2].Split(' ')[0]),
                    NumberOfCollectedMushrooms = 0
                },
                Player2 = new Player
                {
                    Location = int.Parse(data[2].Split(' ')[1]),
                    NumberOfCollectedMushrooms = 0
                }
            };

            var mushroomData = data[1].Split(' ');
            var numberOfMushrooms = int.Parse(mushroomData[0]);
            var mushrooms = new List<Mushroom>();
            for (int i = 1; i <= numberOfMushrooms; i++)
            {
                mushrooms.Add(new Mushroom { Location = int.Parse(mushroomData[i]) });
            }
            board.Muschrooms = mushrooms;

            var cube = new Cube
            {
                NumberOfWalls = int.Parse(data[3])
            };
            cube.ProbabilitiesOfCertainResult = new double[cube.NumberOfWalls];
            cube.PossibleResults = new int[cube.NumberOfWalls];
            cube.NaturalProbabilities = new int[cube.NumberOfWalls];
            var posibilites = data[4].Split(' ').Select(x => int.Parse(x)).ToList();
            var probabilities = data[5].Split(' ').Select(x => int.Parse(x)).ToList();
            var sumOfProbabilities = probabilities.Sum();
            for (int i = 0; i < cube.NumberOfWalls; i++)
            {
                cube.PossibleResults[i] = posibilites[i];
                cube.ProbabilitiesOfCertainResult[i] = (double)probabilities[i] / sumOfProbabilities;
                cube.NaturalProbabilities[i] = probabilities[i];
            }
            board.Cube = cube;

            return board;
        }
    }
}
