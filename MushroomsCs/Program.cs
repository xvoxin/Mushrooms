using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
        private const string TimesPivLuCpp = "../../../Output/TimesPivLu.txt";

        private static void Main()
        {
            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            CountStatistics();

            /*var approximation = new Approximation();
            var timesForPivLu = File.ReadAllLines(TimesPivLuCpp).Select(double.Parse).ToArray();
            /*var mistake = 0.0d;
            for (int i = 10; i < 17; i++)
            {
                var timeForPivLuCpp = approximation.GetTimeForGivenBoardSize(TimesPivLuCpp, i, 3);
                mistake += Approximation.MyPow(timesForPivLu[i - 5] - timeForPivLuCpp, 2);
                Console.WriteLine(timeForPivLuCpp);
            }
            Console.WriteLine($"Mistake: {mistake}");*/
            /*var approximation = new Approximation();
            var size = 15;
            var timeForPivLuCpp = approximation.GetTimeForGivenBoardSize(TimesMatrixCreation, size, 1);
            Console.WriteLine(timeForPivLuCpp);
            var prop = new Probability();
            var board = FileManager.GenerateBoard(size);
            var stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            prop.CreateMatrixWithProbability(board.Size, board.Player1.Location, board.Player2.Location,
                board.Cube.PossibleResults, board.Cube.ProbabilitiesOfCertainResult);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
            var matrix = new Matrix(prop.ProbMatrix);
            
            //matrix.GaussWithPartialPivot((double[]) prop.VectorB.Clone(), true);
            
            /*var timeForGaussWithOptimization =
                approximation.GetTimeForGivenBoardSize(TimesGaussWithOptimization, 12, 3);
            Console.WriteLine(timeForGaussWithOptimization);*/
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
            //var boards = FileManager.GenerateBoards(16);


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
            //File.AppendAllText(TimesMatrixCreation, $"{stopwatch.Elapsed.TotalMilliseconds}\n");

            var matrix = new Matrix(prop.ProbMatrix);
            //fm.WriteToFile(matrix, prop.VectorB.Length, matrixData);
            fm.WriteLargeMatrixToFIle(prop.LargeMatrix, largeMatrixData);
            fm.WriteToFile(prop.LargeMatrix.Count, largeMatrixSize);
            fm.WriteToFile(prop.VectorB, prop.VectorB.Length, vectorData);

            //foreach (var board in boards)
            //{

            //    stopwatch.Reset();
            //    stopwatch.Start();
            //    prop.CreateMatrixWithProbability(board.Size, board.Player1.Location, board.Player2.Location,
            //        board.Cube.PossibleResults, board.Cube.ProbabilitiesOfCertainResult);
            //    stopwatch.Stop();
            //    File.AppendAllText(TimesMatrixCreation, $"{stopwatch.Elapsed.TotalMilliseconds}\n");

            //    var matrix = new Matrix(prop.ProbMatrix);

            //    fm.WriteToFile(matrix, prop.VectorB.Length, matrixData);
            //    fm.WriteToFile(prop.VectorB, prop.VectorB.Length, vectorData);

            //    stopwatch.Reset();
            //    stopwatch.Start();
            //    var seidel = matrix.GaussSeidelMethod((double[])prop.VectorB.Clone(), 7000);
            //    stopwatch.Stop();
            //    Console.WriteLine($"Seidel Result {seidel[0]}");
            //    File.AppendAllText(TimesSeidel, $"{stopwatch.Elapsed.TotalMilliseconds}\n");
            //    stopwatch.Reset();

            //    stopwatch.Start();
            //    var gaussOptimized = matrix.GaussWithPartialPivot((double[])prop.VectorB.Clone(), true);
            //    stopwatch.Stop();
            //    Console.WriteLine($"Gauss Optimized result {gaussOptimized[0]}");
            //    File.AppendAllText(TimesGaussWithOptimization, $"{stopwatch.Elapsed.TotalMilliseconds}\n");
            //    stopwatch.Reset();

            //    stopwatch.Start();
            //    var gauss = matrix.GaussWithPartialPivot((double[])prop.VectorB.Clone(), false);
            //    stopwatch.Stop();
            //    File.AppendAllText(TimesGaussWithoutOptimization, $"{stopwatch.Elapsed.TotalMilliseconds}\n");
            //    stopwatch.Reset();
            //    File.AppendAllText(MatrixSize, $"{gauss.Length}\n");
            //}

            matrixData.Close();
            largeMatrixData.Close();
            largeMatrixSize.Close();
            vectorData.Close();
        }
    }
}