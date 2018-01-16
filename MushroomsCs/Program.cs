using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using MushroomsCs.Models;

namespace MushroomsCs
{
    internal class Program
    {
        private const string TimesGaussWithoutOptimization = "../../../Output/TimesGaussWithoutOptimization.txt";
        private const string TimesGaussWithOptimization = "../../../Output/TimesGaussWithOptimization.txt";
        private const string TimesSeidel = "../../../Output/TimesSeidel.txt";
        private const string TimesMatrixCreation = "../../../Output/TimesMatrixCreation.txt";
        private const string MatrixSize = "../../../Output/MatrixSize.txt";
        private const string TimesSparseCpp = "../../../Output/TimesSparseLuCpp.txt";
        private const string TimesSeidelCpp = "../../../Output/TimesSeidelCpp.txt";

        private static void Main()
        {
            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            CountStatistics();
            GetResultsAndApproximations();
        }

        private static void GetResultsAndApproximations()
        {
            var approximation = new Approximation();
            var expectedTimes = File.ReadAllLines(TimesGaussWithOptimization).Select(double.Parse).ToArray();
            Console.WriteLine("Optimized Gauss results and approximations:");
            for (int i = 10; i < 17; i++)
            {
                var approx = approximation.GetTimeForGivenBoardSize(TimesGaussWithOptimization, i, 2);
                Console.WriteLine($"Approx for {i - 5}: {approx}");
                Console.WriteLine($"Real value for {i - 5}: {expectedTimes[i - 5]}");
            }
            expectedTimes = File.ReadAllLines(TimesGaussWithoutOptimization).Select(double.Parse).ToArray();
            Console.WriteLine("Gauss without optimization results and approximations:");
            for (int i = 10; i < 17; i++)
            {
                var approx = approximation.GetTimeForGivenBoardSize(TimesGaussWithoutOptimization, i, 3);
                Console.WriteLine($"Approx for {i - 5}: {approx}");
                Console.WriteLine($"Real value for {i - 5}: {expectedTimes[i - 5]}");
            }
            
            expectedTimes = File.ReadAllLines(TimesSeidel).Select(double.Parse).ToArray();
            Console.WriteLine("Gauss-Seidel results and approximations:");
            for (int i = 10; i < 17; i++)
            {
                var approx = approximation.GetTimeForGivenBoardSize(TimesSeidel, i, 2);
                Console.WriteLine($"Approx for {i - 5}: {approx}");
                Console.WriteLine($"Real value for {i - 5}: {expectedTimes[i - 5]}");
            }
        }

        private static void CountStatistics()
        {
            File.Create(TimesMatrixCreation).Close();
            File.Create(TimesSeidel).Close();
            File.Create(TimesGaussWithOptimization).Close();
            File.Create(TimesGaussWithoutOptimization).Close();
            File.Create(MatrixSize).Close();
            var stopwatch = new Stopwatch();

            var prop = new Probability();

            var temp = FileManager.GenerateBoard(30);

            var fm = new FileManager();
            var matrixData = new StreamWriter("../../../Output/MatrixData.txt");
            var largeMatrixData = new StreamWriter("../../../Output/LargeMatrixData.txt");
            var largeMatrixSize = new StreamWriter("../../../Output/LargeMatrixSize.txt");
            var vectorData = new StreamWriter("../../../Output/VectorData.txt");

            stopwatch.Reset();
            stopwatch.Start();
            prop.CreateMatrixWithProbability(temp.Size, temp.Player1.Location, temp.Player2.Location,
                temp.Cube.PossibleResults, temp.Cube.ProbabilitiesOfCertainResult, true);
            stopwatch.Stop();
            File.AppendAllText(TimesMatrixCreation, $"{stopwatch.Elapsed.TotalMilliseconds}\n");

            var matrix = new Matrix(prop.ProbMatrix);
            fm.WriteToFile(matrix, prop.VectorB.Length, matrixData);
            fm.WriteLargeMatrixToFIle(prop.LargeMatrix, largeMatrixData);
            fm.WriteToFile(prop.LargeMatrix.Count, largeMatrixSize);
            fm.WriteToFile(prop.VectorB, prop.VectorB.Length, vectorData);

            var boards = FileManager.GenerateBoards(16);

            foreach (var board in boards)
            {

                stopwatch.Reset();
                stopwatch.Start();
                prop.CreateMatrixWithProbability(board.Size, board.Player1.Location, board.Player2.Location,
                    board.Cube.PossibleResults, board.Cube.ProbabilitiesOfCertainResult, false);
                stopwatch.Stop();
                File.AppendAllText(TimesMatrixCreation, $"{stopwatch.Elapsed.TotalMilliseconds}\n");

                matrix = new Matrix(prop.ProbMatrix);

                fm.WriteToFile(matrix, prop.VectorB.Length, matrixData);
                fm.WriteToFile(prop.VectorB, prop.VectorB.Length, vectorData);

                stopwatch.Reset();
                stopwatch.Start();
                var seidel = matrix.GaussSeidelMethod((double[])prop.VectorB.Clone(), 7000);
                stopwatch.Stop();
                File.AppendAllText(TimesSeidel, $"{stopwatch.Elapsed.TotalMilliseconds}\n");
                stopwatch.Reset();

                stopwatch.Start();
                var gaussOptimized = matrix.GaussWithPartialPivot((double[])prop.VectorB.Clone(), true);
                stopwatch.Stop();
                File.AppendAllText(TimesGaussWithOptimization, $"{stopwatch.Elapsed.TotalMilliseconds}\n");
                stopwatch.Reset();
                
                stopwatch.Start();
                var gauss = matrix.GaussWithPartialPivot((double[])prop.VectorB.Clone(), false);
                stopwatch.Stop();
                File.AppendAllText(TimesGaussWithoutOptimization, $"{stopwatch.Elapsed.TotalMilliseconds}\n");
                stopwatch.Reset();
                File.AppendAllText(MatrixSize, $"{gauss.Length}\n");
            }

            matrixData.Close();
            largeMatrixData.Close();
            largeMatrixSize.Close();
            vectorData.Close();
        }
    }
}