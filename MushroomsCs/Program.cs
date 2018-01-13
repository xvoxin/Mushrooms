using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
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
        
        private static void Main()
        {
            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;

            File.Create(TimesMatrixCreation).Close();
            File.Create(TimesSeidel).Close();
            File.Create(TimesGaussWithOptimization).Close();
            File.Create(TimesGaussWithoutOptimization).Close();
            File.Create(MatrixSize).Close();
            
            var prop = new Probability();
            var boards = new List<Board>
            {
                FileManager.CreateBoard(2),
                FileManager.CreateBoard(3),
                FileManager.CreateBoard(4),
                FileManager.CreateBoard(5),
                FileManager.CreateBoard(6),
                FileManager.CreateBoard(7),
                FileManager.CreateBoard(8),
                FileManager.CreateBoard(9),
                FileManager.CreateBoard(10),
                FileManager.CreateBoard(11),
                FileManager.CreateBoard(12)
            };

            var fm = new FileManager();
            var MatrixData = new StreamWriter("../../../Output/MatrixData.txt");
            var VectorData = new StreamWriter("../../../Output/VectorData.txt");
            
            foreach (var board in boards)
            {
                
                var stopwatch = new Stopwatch();
                stopwatch.Reset();
                stopwatch.Start();
                prop.CreateMatrixWithProbability(board.Size, board.Player1.Location, board.Player2.Location,
                    board.Cube.PossibleResults, board.Cube.ProbabilitiesOfCertainResult);
                stopwatch.Stop();
                File.AppendAllText(TimesMatrixCreation, $"{stopwatch.Elapsed.TotalMilliseconds}\n");
                
                var matrix = new Matrix(prop.ProbMatrix);

                fm.WriteToFile(matrix, prop.VectorB.Length, MatrixData);
                fm.WriteToFile(prop.VectorB, prop.VectorB.Length, VectorData);

                stopwatch.Reset();
                stopwatch.Start();
                var seidel = matrix.GaussSeidelMethod((double[])prop.VectorB.Clone(), 7000);
                stopwatch.Stop();
                File.AppendAllText(TimesSeidel, $"{stopwatch.Elapsed.TotalMilliseconds}\n");
                stopwatch.Reset();

                stopwatch.Start();
                var gaussOptimized = matrix.GaussWithPartialPivot((double[]) prop.VectorB.Clone(), true);
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

            MatrixData.Close();
            VectorData.Close();
        }
    }
}
