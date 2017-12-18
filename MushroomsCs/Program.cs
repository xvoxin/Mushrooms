using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using MushroomsCs.Models;

namespace MushroomsCs
{
    internal class Program
    {
        private const string MatrixCsSymetric = "../../../Output/MatrixCsSymetric.txt";
        private const string VectorCsSymetric = "../../../Output/VectorCsSymetric.txt";
        private const string ResCsSymetric = "../../../Output/ResCsSymetric.txt";
        private const string TimeCsSymetric = "../../../Output/TimesCsSymetric.txt";
        private const string MatrixSizeSymetric = "../../../Output/MatrixSizeSymetric.txt";

        private static void Main()
        {
            CultureInfo culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;

            var prop = new Probability();
            var boards = new List<Board>
            {
                File.CreateBoard(4),
                File.CreateBoard(5),
                File.CreateBoard(6),
                File.CreateBoard(7),
                File.CreateBoard(8),
                File.CreateBoard(9),
                File.CreateBoard(44),
                File.CreateBoard(55),
                File.CreateBoard(66),
                File.CreateBoard(77),
                File.CreateBoard(88),
                File.CreateBoard(99)
            };

            foreach (var board in boards)
            {
                var random = new Random();
                prop.CreateMatrixWithProbability(board.Size, board.Player1.Location, board.Player2.Location,
                    board.Cube.PossibleResults, board.Cube.ProbabilitiesOfCertainResult);

                Matrix matrix = new Matrix(prop.ProbMatrix);

                var stopwatch = new Stopwatch();
                double[] times = new double[3];

                var gauss1 = matrix.GaussWithoutChoice((double[])prop.VectorB.Clone());

                stopwatch.Reset();
                stopwatch.Start();
                var gauss2 = matrix.GaussWithPartialPivot((double[])prop.VectorB.Clone());
                stopwatch.Stop();
                times[0] = stopwatch.Elapsed.TotalMilliseconds;

                var gauss3 = matrix.GaussWithCompletePivot((double[])prop.VectorB.Clone());

                stopwatch.Reset();
                stopwatch.Start();
                var jacoby = matrix.JacobyMethod((double[])prop.VectorB.Clone(), 7000);
                stopwatch.Stop();
                times[1] = stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();
                stopwatch.Start();
                var seidel = matrix.GaussSeidelMethod((double[])prop.VectorB.Clone(), 7000);
                stopwatch.Stop();
                var monteCarlo = MonteCarlo.GetResultOfTheGame(board, random);
                double[] output = { gauss1[0], gauss2[0], gauss3[0], jacoby[0], seidel[0]};
                times[2] = stopwatch.Elapsed.TotalMilliseconds;

                System.IO.File.AppendAllText(MatrixCsSymetric, $"{board.Size / 2}\n");
                System.IO.File.AppendAllText(MatrixCsSymetric, matrix.ToString());
                System.IO.File.AppendAllText(MatrixCsSymetric, "\n");

                System.IO.File.AppendAllText(VectorCsSymetric, $"{board.Size / 2}\n");
                foreach (var value in prop.VectorB)
                {
                    System.IO.File.AppendAllText(VectorCsSymetric, $"{value} ");
                }
                System.IO.File.AppendAllText(VectorCsSymetric, "\n");

                System.IO.File.AppendAllText(ResCsSymetric, $"{board.Size / 2}\n");
                foreach (var value in output)
                {
                    System.IO.File.AppendAllText(ResCsSymetric, $"{value} ");
                }
                System.IO.File.AppendAllText(ResCsSymetric, "\n");


                System.IO.File.AppendAllText(TimeCsSymetric, $"{board.Size / 2}\n");
                for (int i = 0; i < times.Length; i++)
                {
                    System.IO.File.AppendAllText(TimeCsSymetric, $"{times[i]} ");
                }
                System.IO.File.AppendAllText(TimeCsSymetric, "\n");
                
                System.IO.File.AppendAllText(MatrixSizeSymetric, $"Board size: {board.Size / 2}\n");
                System.IO.File.AppendAllText(MatrixSizeSymetric, $"{prop.VectorB.Length}\n");
                
                Console.WriteLine($"For board of size {board.Size / 2}");
                Console.WriteLine($"Gauss 1: {gauss1[0]}");
                Console.WriteLine($"Gauss 2: {gauss2[0]}");
                Console.WriteLine($"Gauss 3: {gauss3[0]}");
                Console.WriteLine($"Jacoby: {jacoby[0]}");
                Console.WriteLine($"Seidel: {seidel[0]}");
                Console.WriteLine($"Monte Carlo: {monteCarlo}");                
            }
        }
    }
}
