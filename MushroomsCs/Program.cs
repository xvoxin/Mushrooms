using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushroomsCs
{
    class Program
    {
        static void Main(string[] args)
        {
            double[,] matDoubl =
            {
                {10, -1, 2, 0},
                {-1, 11, -1, 3},
                {2, -1, 10, -1},
                {0, 3, -1, 8}
            };

            double[] vector = {6, 25, -11, 15};
            
            Matrix mat = new Matrix(matDoubl);

            var res = mat.JacobyMethod(vector, 10);

            for (int i = 0; i < 4; i++)
            {
                Console.Write(res[i] + " , ");
            }

            var res2 = mat.GaussSeidelMethod(vector, 10);
            Console.WriteLine();

            for (int i = 0; i < 4; i++)
            {
                Console.Write(res2[i] + " , ");
            }
            Console.ReadKey();
        }
    }
}
