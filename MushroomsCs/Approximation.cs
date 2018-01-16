using System;
using System.IO;
using System.Linq;

namespace MushroomsCs
{
    public class Approximation
    {
        public double GetTimeForGivenBoardSize(string timesFileName, int boardSize, int polynomialDegree)
        {
            const int startingBoardSize = 5;
            var times = File.ReadAllLines(timesFileName).Select(double.Parse).ToArray();
            var pointsForPolynomial = new double[times.Length, 2];
            for (int i = 0; i < times.Length; i++)
            {
                pointsForPolynomial[i, 0] = i + startingBoardSize;
                pointsForPolynomial[i, 1] = times[i];
            }
            var coefficients = CountCoefficientsOfPolynomial(pointsForPolynomial, polynomialDegree);
            var sum = 0.0;
            for (int i = 0; i < coefficients.Length; i++)
            {
                sum += coefficients[i] * MyPow(boardSize, i);
            }

            return sum;
        }

        public double[] CountCoefficientsOfPolynomial(double[,] tableOfPoints, int polynomialDegree)
        {
            var tableForPolynomialOperations = FillTableForPolynomial(tableOfPoints, polynomialDegree);
            var firstIndexOfMultiplyElements = 2 + (2 * polynomialDegree) - 1;
            var amountOfPoints = tableForPolynomialOperations.GetLength(0);
            var matrixCoefficientTable = new double[polynomialDegree + 1, polynomialDegree + 1];
            var sTable = new double[2 * polynomialDegree + 1];
            var tTable = new double[polynomialDegree + 1];

            sTable[0] = amountOfPoints;
            for (int i = 0; i < amountOfPoints; i++)
            {
                int k = 0;
                for (int j = 2; j < tableForPolynomialOperations.GetLength(1); j++)
                {

                    if (j < firstIndexOfMultiplyElements)
                    {
                        sTable[j] += tableForPolynomialOperations[i, j];
                    }
                    else
                    {
                        k++;
                        tTable[k] += tableForPolynomialOperations[i, j];
                    }

                }

                sTable[1] += tableForPolynomialOperations[i, 0];
                tTable[0] += tableForPolynomialOperations[i, 1];
            }

            for (var i = 0; i < polynomialDegree + 1; i++)
            {
                for (var j = 0; j < polynomialDegree + 1; j++)
                {
                    matrixCoefficientTable[i, j] = sTable[i + j];
                }
            }

            var coefficientsMatrix = new Matrix(matrixCoefficientTable);
            var coefficients = coefficientsMatrix.GaussWithPartialPivot(tTable, true);

            return coefficients;
        }

        private double[,] FillTableForPolynomial(double[,] points, int polynomialDegree)
        {
            var amountOfColumns = 2 + polynomialDegree * 2 - 1 + polynomialDegree;
            var tableForPolynomial = new double[points.GetLength(0), amountOfColumns];


            for (var i = 0; i < points.GetLength(0); i++)
            {
                tableForPolynomial[i, 0] = points[i, 0];
                tableForPolynomial[i, 1] = points[i, 1];

                GeneratePowElementsForPolynomialTable(tableForPolynomial, polynomialDegree, i);
                GenerateMultiplyElementsForPolynomialTable(tableForPolynomial, polynomialDegree, i);
            }

            return tableForPolynomial;
        }


        private void GeneratePowElementsForPolynomialTable(double[,] tableForPolynomial, int polynomialDegree, int rowNumber)
        {
            const int startingColumn = 2;
            var argument = tableForPolynomial[rowNumber, 0];

            for (var i = 0; i < polynomialDegree * 2 - 1; i++)
            {
                tableForPolynomial[rowNumber, startingColumn + i] = MyPow(argument, i + 2);
            }
        }

        private void GenerateMultiplyElementsForPolynomialTable(double[,] tableForPolynomial, int polynomialDegree, int rowNumber)
        {
            var startingColumn = 2 + polynomialDegree * 2 - 1;
            var value = tableForPolynomial[rowNumber, 1];
            var argument = tableForPolynomial[rowNumber, 0];

            for (var i = 0; i < polynomialDegree; i++)
            {
                tableForPolynomial[rowNumber, startingColumn + i] = value * MyPow(argument, i + 1);
            }
        }

        public static double MyPow(double number, int exponent)
        {
            var result = number;
            if (exponent == 0) return 1;
            for (var i = 1; i < exponent; i++)
            {
                result = result * number;
            }
            return result;
        }
    }
}