using System;
using System.IO;

namespace MatrixCalculator
{
    public class File<T> where T: new()
    {
        public void WriteToFile(Matrix<T> matrix, int size, StreamWriter sw)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    sw.Write(matrix.MatrixValues[i, j] + " ");
                }
            }
            sw.WriteLine();
        }

        public void WriteToFile(T[] vector, int size, StreamWriter sw)
        {
            for (int i = 0; i < size; i++)
            {
                sw.Write(vector[i] + " ");
            }
            sw.WriteLine();
        }

        public T[] ReadVectorFromFile(int size, StreamReader sr)
        {
            var vector = new T[size];

            string line = "";

            line = sr.ReadLine();
            string[] splitLine = line.Split(' ');
            
            for (int j = 0; j < size; j++)
            {
                if(vector is double[])
                    vector[j] = (dynamic) double.Parse(splitLine[j], System.Globalization.CultureInfo.InvariantCulture);
                else if (vector is float[])
                    vector[j] = (dynamic) float.Parse(splitLine[j], System.Globalization.CultureInfo.InvariantCulture);
            }

            return vector;
        }

        public T[,] ReadMatrixFromFile(int size, StreamReader sr)
        {
            T[,] matrix = new T[size, size];

            string line = "";
            line = sr.ReadLine();
            string[] splitLine = line.Split(' ');

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int splitNumber = size * i + j;

                    if (matrix is double[,])
                        matrix[i, j] = (dynamic) double.Parse(splitLine[splitNumber], System.Globalization.CultureInfo.InvariantCulture);
                    if (matrix is float[,])
                        matrix[i, j] = (dynamic)float.Parse(splitLine[splitNumber], System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return matrix;
        }

        public T[] FillVectorWithRandom(int size)
        {
            var vector = new T[size];
            var random = new Random();

            for (int i = 0; i < size; i++)
            {
                if(vector is float[])
                    vector[i] = (dynamic)(float)random.NextDouble() * random.Next(Int32.MinValue, Int32.MaxValue);

                if (vector is double[])
                    vector[i] = (dynamic)random.NextDouble() * random.Next(Int32.MinValue, Int32.MaxValue);
            }

            return vector;
        }
    }
}
