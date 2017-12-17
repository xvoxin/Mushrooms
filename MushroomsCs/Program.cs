using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace MushroomsCs
{
    internal class Program
    {
        private static void Main()
        {
            CultureInfo culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;

            var prop = new Probability();
            var board = File.CreateBoard(1);
            var file = new File();

            var matrixCs = new StreamWriter("../../../Output/MatrixCs.txt");
            var vectorCs = new StreamWriter("../../../Output/VectorCs.txt");
            var resCs = new StreamWriter("../../../Output/ResCs.txt");
            var timesCs = new StreamWriter("../../../Output/TimesCs.txt");
            var matrixSize = new StreamWriter("../../../Output/MatrixSize.txt");
            

            prop.CreateMatrixWithProbability(board.Size, board.Player1.Location, board.Player2.Location, 
                board.Cube.PossibleResults, board.Cube.ProbabilitiesOfCertainResult);

            Matrix matrix = new Matrix(prop.ProbMatrix);

            var stopwatch = new Stopwatch();
            double[] times = new double[3];

            var gauss1 = matrix.GaussWithoutChoice((double[]) prop.VectorB.Clone());

            stopwatch.Reset();
            stopwatch.Start();
            var gauss2 = matrix.GaussWithPartialPivot((double[]) prop.VectorB.Clone());
            stopwatch.Stop();
            times[0] = stopwatch.Elapsed.TotalMilliseconds;

            var gauss3 = matrix.GaussWithCompletePivot((double[]) prop.VectorB.Clone());

            stopwatch.Reset();
            stopwatch.Start();
            var jacoby = matrix.JacobyMethod((double[]) prop.VectorB.Clone(), 600);
            stopwatch.Stop();
            times[1] = stopwatch.Elapsed.TotalMilliseconds;

            stopwatch.Reset();
            stopwatch.Start();
            var seidel = matrix.GaussSeidelMethod((double[]) prop.VectorB.Clone(), 600);
            stopwatch.Stop();
            double[] output = {gauss2[0], jacoby[0], seidel[0]};
            times[2] = stopwatch.Elapsed.TotalMilliseconds;

            file.WriteToFile(matrix, matrix.NumberOfColumns, matrixCs);
            file.WriteToFile(prop.VectorB, prop.VectorB.Length, vectorCs);
            file.WriteToFile(output, output.Length, resCs);
            file.WriteToFile(times, times.Length, timesCs);
            file.WriteToFile(prop.VectorB.Length, matrixSize);

            matrixCs.Close();
            vectorCs.Close();
            resCs.Close();
            timesCs.Close();
            matrixSize.Close();
            
            Console.WriteLine(gauss1[0]);
            Console.WriteLine(gauss2[0]);
            Console.WriteLine(gauss3[0]);
            Console.WriteLine(jacoby[0]);
            Console.WriteLine(seidel[0]);

            Console.WriteLine($"{MonteCarlo.GetResultOfTheGame(board):.######}");

            Console.ReadKey();
        }
    }
}
