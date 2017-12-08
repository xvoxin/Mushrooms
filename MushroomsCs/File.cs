using System;
using System.IO;
using MushroomsCs;

namespace MatrixCalculator
{
    public class File
    {
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

        public double[] ReadVectorFromFile(int size, StreamReader sr)
        {
            var vector = new double[size];

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

        public double[,] ReadMatrixFromFile(int size, StreamReader sr)
        {
            double[,] matrix = new double[size, size];

            string line = "";
            line = sr.ReadLine();
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
                vector[i] = (dynamic)random.NextDouble() * random.Next(Int32.MinValue, Int32.MaxValue);
            }

            return vector;
        }
    }
}
