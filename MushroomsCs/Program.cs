using System;

namespace MushroomsCs
{
    internal class Program
    {
        private static void Main()
        {

            Probability prop = new Probability();

            var board = File.CreateBoard();
            prop.CreateMatrixWithProbability(board.Size, board.Player1.Location, board.Player2.Location, 
                board.Cube.PossibleResults, board.Cube.ProbabilitiesOfCertainResult);

            Matrix matrix = new Matrix(prop.ProbMatrix);

            var gauss1 = matrix.GaussWithoutChoice((double[]) prop.VectorB.Clone());
            var gauss2 = matrix.GaussWithPartialPivot((double[]) prop.VectorB.Clone());
            var gauss3 = matrix.GaussWithCompletePivot((double[]) prop.VectorB.Clone());
            var jacoby = matrix.JacobyMethod((double[]) prop.VectorB.Clone(), 50);
            var seidel = matrix.GaussSeidelMethod((double[]) prop.VectorB.Clone(), 50);

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
