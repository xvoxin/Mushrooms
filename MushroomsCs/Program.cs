using System;

namespace MushroomsCs
{
    class Program
    {
        static void Main(string[] args)
        {
//            double[,] matDoubl =
//            {
//                {10, -1, 2, 0},
//                {-1, 11, -1, 3},
//                {2, -1, 10, -1},
//                {0, 3, -1, 8}
//            };
//
//            double[] vector = {6, 25, -11, 15};
//            
//            Matrix mat = new Matrix(matDoubl);
//
//            var res = mat.JacobyMethod(vector, 15);
//
//            for (int i = 0; i < 4; i++)
//            {
//                Console.Write(res[i] + " , ");
//            }
//
//            var res2 = mat.GaussSeidelMethod(vector, 15);
//            Console.WriteLine();
//
//            for (int i = 0; i < 4; i++)
//            {
//                Console.Write(res2[i] + " , ");
//            }

            Probability prop = new Probability();

            prop.CreateMatrixWithProbability();

//            for (int i = 0; i < 16; i++)
//            {
//                for (int j = 0; j < 16; j++)
//                {
//                    Console.Write(prop.ProbMatrix[i, j] + "\t");
//                }
//                Console.Write(prop.VectorB[i]);
//                Console.WriteLine();
//            }

            Matrix matrix = new Matrix(prop.ProbMatrix);

            var gauss1 = matrix.GaussWithoutChoice((double[])prop.VectorB.Clone());
            var gauss2 = matrix.GaussWithPartialPivot((double[])prop.VectorB.Clone());
            var gauss3 = matrix.GaussWithCompletePivot((double[])prop.VectorB.Clone());
            var jacoby = matrix.JacobyMethod((double[])prop.VectorB.Clone(), 50);
            var seidel = matrix.GaussSeidelMethod((double[])prop.VectorB.Clone(), 50);

            Console.WriteLine(gauss1[0]);
            Console.WriteLine(gauss2[0]);
            Console.WriteLine(gauss3[0]);
            Console.WriteLine(jacoby[0]);
            Console.WriteLine(seidel[0]);

            Console.ReadKey();
        }
    }
}
