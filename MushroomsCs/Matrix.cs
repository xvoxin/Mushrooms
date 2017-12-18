using System;
using System.Linq;

namespace MushroomsCs
{
    public class Matrix
    {
        public int NumberOfRows => MatrixValues.GetLength(0);
        public int NumberOfColumns => MatrixValues.GetLength(1);

        public double[,] MatrixValues { get; set; }
        public double[,] TempMatrixValues { get; set; }


        public Matrix(double[,] matrix)
        {
            MatrixValues = matrix;
        }

        public Matrix(int rows, int columns)
        {
            MatrixValues = InitializeWithRandomNumbers(rows, columns);
        }

        private void SetTempMatrix()
        {
            TempMatrixValues = (double[,])MatrixValues.Clone();
        }

        private double[,] InitializeWithRandomNumbers(int rows, int columns)
        {
            var random = new Random();
            var matrix = new double[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = random.NextDouble() * random.Next(Int32.MinValue, Int32.MaxValue);
                }
            }
            return matrix;
        }

        public static Matrix operator +(Matrix i, Matrix j)
        {
            return new Matrix (Add(i, j));
        }

        public static Matrix operator *(Matrix i, Matrix j)
        {
            return new Matrix(Multiply(i, j));
        }

        public static double[] operator *(Matrix a, double[] vector)
        {
            return MultiplyByVector(a, vector);
        }

        private static double[,] Multiply(Matrix a, Matrix b)
        {
            var numberOfRows = a.NumberOfRows;
            var numberOfColumns = a.NumberOfColumns;
            var result = new double[a.NumberOfRows, b.NumberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    double value = 0;
                    for (int k = 0; k < numberOfRows; k++)
                    {
                        value += b.MatrixValues[k, j] * a.MatrixValues[i, k];
                    }
                    result[i, j] = value;
                }
            }
            return result;
        }

        private static double[] MultiplyByVector(Matrix a, double[] vector)
        {
            var numberOfRows = a.NumberOfRows;
            var numberOfColumns = a.NumberOfColumns;
            var result = new double[a.NumberOfRows];

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    result[i] += a.MatrixValues[i, j] * vector[j];
                }
            }
            return result;
        }

        private static double[,] Add(Matrix a, Matrix b)
        {
            var numberOfRows = a.NumberOfRows;
            var numberOfColumns = a.NumberOfColumns;
            var matrix = new double[numberOfRows, numberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    matrix[i, j] = a.MatrixValues[i, j] + b.MatrixValues[i, j];
                }
            }
            return matrix;
        }

        private static double[] AddVectors(double[] a, double[] b)
        {
            double[] ret = new double[a.Length];
            for(int i = 0; i < a.Length; i++)
            {
                ret[i] = a[i] + b[i];
            }
            return ret;
        }

        private static double[] SubtractVectors(double[] a, double[] b)
        {
            double[] ret = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                ret[i] = a[i] - b[i];
            }
            return ret;
        }
        public double[] GaussWithoutChoice(double[] vector)
        {
            SetTempMatrix();
            var res = new double[NumberOfRows];

            for (int i = 0; i < NumberOfRows - 1; i++)
            {
                for (int j = i + 1; j < NumberOfRows; j++)
                {
                    double multiplier = TempMatrixValues[j, i] / TempMatrixValues[i, i] * (-1);
                    for (int k = i + 1; k < NumberOfRows; k++)
                    {
                        TempMatrixValues[j, k] += multiplier * TempMatrixValues[i, k];
                    }
                    vector[j] += vector[i] * multiplier;
                }
            }

            for (int i = NumberOfRows - 1; i >= 0; i--)
            {
                double sum = vector[i];
                for (int j = NumberOfRows - 1; j >= i + 1; j--)
                {
                    sum -= TempMatrixValues[i, j] * res[j];
                }
                res[i] = sum / TempMatrixValues[i, i];
            }

            return res;
        }

        public double[] GaussWithPartialPivot(double[] vector)
        {
            SetTempMatrix();
            var currentRow = 0;
            int currentColumn;
            for (currentColumn = 0; currentColumn < NumberOfColumns - 1; currentColumn++, currentRow++)
            {
                var smallestRowIndex = FindIndexOfRowWithGreatestNumberInGivenColumn(currentRow, currentColumn);
                SwapRows(vector, currentRow, smallestRowIndex);
                ResetAllColumnsBelow(vector, currentRow, currentColumn);
            }
            return GetResultsAfterGauss(vector);
        }

        public double[] GaussWithCompletePivot(double[] vector)
        {
            SetTempMatrix();
            var vectorHistory = Enumerable.Range(1, vector.Length).ToArray();
            for (int i = 0; i < NumberOfRows; i++)
            {
                var greatestElementPosition = GetGreatestElementPosition(i);
                if (greatestElementPosition.row != i)
                {
                    SwapRows(vector, i, greatestElementPosition.row);
                }
                if (greatestElementPosition.column != i)
                {
                    vectorHistory = SwapColumns(i, greatestElementPosition.column, vectorHistory);
                }
                ResetAllColumnsBelow(vector, i, i);
            }
            return GetProperVector(vector, vectorHistory);

        }

        public int[] SwapColumns(int numberOfFirstColumn, int numberOfSecondColumn, int[] vectorChange)
        {
            for (int i = 0; i < NumberOfRows; i++)
            {
                var temp = TempMatrixValues[i, numberOfFirstColumn];
                TempMatrixValues[i, numberOfFirstColumn] = TempMatrixValues[i, numberOfSecondColumn];
                TempMatrixValues[i, numberOfSecondColumn] = temp;
            }
            var tempVector = vectorChange[numberOfFirstColumn];
            vectorChange[numberOfFirstColumn] = vectorChange[numberOfSecondColumn];
            vectorChange[numberOfSecondColumn] = tempVector;
            return vectorChange;
        }

        public void SwapRows(double[] vector, int numberOfFirstRow, int numberOfSecondRow)
        {
            for (int i = 0; i < NumberOfColumns; i++)
            {
                var temp = TempMatrixValues[numberOfFirstRow, i];
                TempMatrixValues[numberOfFirstRow, i] = TempMatrixValues[numberOfSecondRow, i];
                TempMatrixValues[numberOfSecondRow, i] = temp;
            }
            var oldValue = vector[numberOfFirstRow];
            vector[numberOfFirstRow] = vector[numberOfSecondRow];
            vector[numberOfSecondRow] = oldValue;
        }

        private double[] GetResultsAfterGauss(double[] vector)
        {
            var resultsVector = new double[vector.Length];
            for (int i = vector.Length - 1; i >= 0; i--)
            {
                int j = i;
                var numerator = vector[i];
                while (j < NumberOfColumns - 1)
                {
                    numerator -= TempMatrixValues[i, j + 1] * resultsVector[j + 1];
                    j++;
                }
                resultsVector[i] =  numerator / TempMatrixValues[i, i];
            }
            return resultsVector;
        }

        private double[] GetProperVector(double[] vector, int[] vectorHistory)
        {
            var resultsVector = GetResultsAfterGauss(vector);
            for (int i = 0; i < vector.Length; i++)
            {
                if (vectorHistory[i] != i + 1)
                {
                    var indexToReplace = i;
                    for (int j = i; j < vector.Length; j++)
                    {
                        if (vectorHistory[j] == i + 1)
                        {
                            indexToReplace = j;
                        }
                    }

                    var tempIndex = vectorHistory[i];
                    vectorHistory[i] = vectorHistory[indexToReplace];
                    vectorHistory[indexToReplace] = tempIndex;

                    var tempValue = resultsVector[i];
                    resultsVector[i] = resultsVector[indexToReplace];
                    resultsVector[indexToReplace] = tempValue;
                }
            }
            return resultsVector;
        }

        private (int row, int column) GetGreatestElementPosition(int startingPoint)
        {
            var result = (row: startingPoint, column: startingPoint);
            for (int i = startingPoint; i < NumberOfRows; i++)
            {
                for (int j = startingPoint; j < NumberOfColumns; j++)
                {
                    if (TempMatrixValues[i, j] > TempMatrixValues[result.row, result.column])
                    {
                        result = (i, j);
                    }
                }
            }
            return result;
        }

        private int FindIndexOfRowWithGreatestNumberInGivenColumn(int rowNumber, int columnNumber)
        {
            var greatestColumn = TempMatrixValues[rowNumber, columnNumber];
            int index = rowNumber;
            for (int i = rowNumber; i < NumberOfRows; i++)
            {
                if (TempMatrixValues[i, columnNumber] > greatestColumn)
                {
                    greatestColumn = TempMatrixValues[i, columnNumber];
                    index = i;
                }
            }
            return index;
        }

        public void ResetAllColumnsBelow(double[] vector, int rowNumber, int columnNumber)
        {
            for (int i = rowNumber + 1; i < NumberOfRows; i++)
            {
                var multiplier = (dynamic)TempMatrixValues[i, columnNumber] / TempMatrixValues[rowNumber, columnNumber] * (-1);
                for (int j = columnNumber; j < NumberOfColumns; j++)
                {
                    TempMatrixValues[i, j] += multiplier * TempMatrixValues[rowNumber, j];
                }
                vector[i] += vector[rowNumber] * multiplier;
            }
        }

        public double[] JacobyMethod(double[] vectorB, int numberOfIterations)
        {
            SetTempMatrix();
            Matrix D = new Matrix(new double[NumberOfColumns, NumberOfRows]);
            Matrix LU = new Matrix(new double[NumberOfColumns, NumberOfRows]);
            double[] x1 = new double[NumberOfColumns];
            double[] x2 = new double[NumberOfColumns];

            for (int i = 0; i < NumberOfColumns; i++)
            {
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    if (i == j)
                    {
                        D.MatrixValues[i, i] = InverseNumber(TempMatrixValues[i, i]);
                        LU.MatrixValues[i, i] = 0;
                    }
                    else
                    {
                        D.MatrixValues[i, j] = 0;
                        LU.MatrixValues[i, j] = -TempMatrixValues[i, j];
                    }
                }
            }
            for (int i = 0; i < numberOfIterations; i++)
            {
                x2 = D * AddVectors(LU * x1, vectorB);
                if (i > 10 && i % 2 == 0)
                {
                    if (x2[0] - x1[0] < 0.0000000000000001 && x2[0] > 0)
                    {
                        Console.WriteLine("Jacoby break on iteration nr - " + i);
                        break;
                    }
                }
                for (int j = 0; j < x1.Length; j++)
                {
                    x1[j] = x2[j];
                }
            }
            
            return x1;
        }

        public double[] GaussSeidelMethod(double[] vectorB, int numberOfIterations) 
        {
            SetTempMatrix();
            Matrix U = new Matrix(new double[NumberOfColumns, NumberOfRows]); 
            Matrix D = new Matrix(new double[NumberOfColumns, NumberOfRows]); 
            Matrix L = new Matrix(new double[NumberOfColumns, NumberOfRows]);
            double[] x1 = new double[NumberOfColumns];
            double[] x2 = new double[NumberOfColumns];

            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    if (i == j)
                    {
                        D.MatrixValues[i, j] = InverseNumber(TempMatrixValues[i, j]);
                    }
                    else if (i > j)
                    {
                        L.MatrixValues[i, j] = TempMatrixValues[i, j];
                    }
                    else if (i < j)
                    {
                        U.MatrixValues[i, j] = TempMatrixValues[i, j];
                    }
                }
            }
            for (int k = 0; k < numberOfIterations; k++)
            {
                for (int i = 0; i < x1.Length; i++)
                {
                    x1[i] = vectorB[i] * D.MatrixValues[i, i];
                    for (int j = 0; j < i; j++)
                    {
                        x1[i] -= D.MatrixValues[i,i] * L.MatrixValues[i, j] * x1[j];
                    }
                    for (int j = i + 1; j < x1.Length; j++)
                    {
                        x1[i] -= D.MatrixValues[i, i] * U.MatrixValues[i, j] * x1[j];
                    }
                }
                if (k > 10 && k % 2 == 0)
                {
                    if (x1[0] - x2[0] < 0.0000000000000001 && x2[0] > 0)
                    {
                        Console.WriteLine("Seidel break on iteration nr - " + k);
                        break;
                    }
                }
                for (int j = 0; j < x1.Length; j++)
                {
                    x2[j] = x1[j];
                }
            }
            return x1;
        }

        private double InverseNumber(double x)
        {
            if (Math.Abs(x) < 0.0000000000001)
                return 0;

            return 1 / x;
        }

        public override string ToString()
        {
            var matrix = string.Empty;
            for (int i = 0; i < MatrixValues.GetLength(0); i++)
            {
                for (int j = 0; j < MatrixValues.GetLength(1); j++)
                {
                    matrix += MatrixValues[i, j] + " ";
                }
            }
            return matrix;
        }
    }
}