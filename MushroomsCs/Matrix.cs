using System;
using System.Linq;

namespace MatrixCalculator
{
    public class Matrix<T> where T : new()
    {
        public int NumberOfRows => MatrixValues.GetLength(0);
        public int NumberOfColumns => MatrixValues.GetLength(1);

        public T[,] MatrixValues { get; set; }
        public T[,] TempMatrixValues { get; set; }


        public Matrix(T[,] matrix)
        {
            MatrixValues = matrix;
        }

        public Matrix(int rows, int columns)
        {
            MatrixValues = InitializeWithRandomNumbers(rows, columns);
        }

        private T[,] InitializeWithRandomNumbers(int rows, int columns)
        {
            var random = new Random();
            var matrix = new T[rows, columns];

            if (matrix is double[,])
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        matrix[i, j] = (dynamic)random.NextDouble() * random.Next(Int32.MinValue, Int32.MaxValue);
                    }
                }
            }
            else if (matrix is float[,])
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        matrix[i, j] = (dynamic)(float)random.NextDouble() * random.Next(Int32.MinValue, Int32.MaxValue);
                    }
                }
            }
            else if (matrix is Fraction[,])
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        matrix[i, j] = (dynamic) new Fraction(random.Next(1, 10), random.Next(1, 10));
                    }
                }
            }
            return matrix;
        }

        public static Matrix<T> operator +(Matrix<T> i, Matrix<T> j)
        {
            return new Matrix<T>(Add(i, j));
        }

        public static Matrix<T> operator -(Matrix<T> i, Matrix<T> j)
        {
            return new Matrix<T>(Subtract(i, j));
        }

        public static Matrix<T> operator *(Matrix<T> i, Matrix<T> j)
        {
            return new Matrix<T>(Multiply(i, j));
        }

        public static T[] operator *(Matrix<T> a, T[] vector)
        {
            return MultiplyByVector(a, vector);
        }

        private static T[,] Multiply(Matrix<T> a, Matrix<T> b)
        {
            var numberOfRows = a.NumberOfRows;
            var numberOfColumns = a.NumberOfColumns;
            var result = new T[a.NumberOfRows, b.NumberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    var value = new T();
                    for (int k = 0; k < numberOfRows; k++)
                    {
                        value += (dynamic)b.MatrixValues[k, j] * (dynamic)a.MatrixValues[i, k];
                    }
                    result[i, j] = (dynamic)value;
                }
            }
            return result;
        }

        private static T[] MultiplyByVector(Matrix<T> a, T[] vector)
        {
            var numberOfRows = a.NumberOfRows;
            var numberOfColumns = a.NumberOfColumns;
            var result = new T[a.NumberOfRows];
            for(int i = 0; i < a.NumberOfRows; i++)
            {
                result[i] = new T();
            }

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    result[i] += (dynamic)a.MatrixValues[i, j] * vector[j];
                }
            }
            return result;
        }

        private static T[,] Add(Matrix<T> a, Matrix<T> b)
        {
            var numberOfRows = a.NumberOfRows;
            var numberOfColumns = a.NumberOfColumns;
            var matrix = new T[numberOfRows, numberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    matrix[i, j] = (dynamic)a.MatrixValues[i, j] + (dynamic)b.MatrixValues[i, j];
                }
            }
            return matrix;
        }

        private static T[,] Subtract(Matrix<T> a, Matrix<T> b)
        {
            var numberOfRows = a.NumberOfRows;
            var numberOfColumns = a.NumberOfColumns;
            var matrix = new T[numberOfRows, numberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    matrix[i, j] = (dynamic)a.MatrixValues[i, j] - (dynamic)b.MatrixValues[i, j];
                }
            }
            return matrix;
        }

        public T[] GaussWithoutChoice(T[] vector)
        {
            SetTempMatrix();
            var res = new T[NumberOfRows];

            for (int i = 0; i < NumberOfRows - 1; i++)
            {
                for (int j = i + 1; j < NumberOfRows; j++)
                {
                    T multiplier = (dynamic) TempMatrixValues[j, i] / (dynamic)TempMatrixValues[i, i] * (-1);
                    for (int k = i + 1; k < NumberOfRows; k++)
                    {
                        TempMatrixValues[j, k] += multiplier * (dynamic)TempMatrixValues[i, k];
                    }
                    vector[j] += (dynamic)vector[i] * multiplier;
                }
            }

            for (int i = NumberOfRows - 1; i >= 0; i--)
            {
                T sum = (dynamic)vector[i];
                for (int j = NumberOfRows - 1; j >= i + 1; j--)
                {
                    sum -= (dynamic)TempMatrixValues[i, j] * res[j];
                }
                res[i] = sum / (dynamic)TempMatrixValues[i, i];
            }

            return res;
        }

        public T[] GaussWithPartialPivot(T[] vector)
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

        public T[] GaussWithCompletePivot(T[] vector)
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

        public void SwapRows(T[] vector, int numberOfFirstRow, int numberOfSecondRow)
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

        private T[] GetResultsAfterGauss(T[] vector)
        {
            var resultsVector = new T[vector.Length];
            for (int i = vector.Length - 1; i >= 0; i--)
            {
                int j = i;
                var numerator = vector[i];
                while (j < NumberOfColumns - 1)
                {
                    numerator -= (dynamic)TempMatrixValues[i, j + 1] * resultsVector[j + 1];
                    j++;
                }
                resultsVector[i] = (dynamic) numerator / TempMatrixValues[i, i];
            }
            return resultsVector;
        }

        private T[] GetProperVector(T[] vector, int[] vectorHistory)
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
            return resultsVector; ;
        }

        private (int row, int column) GetGreatestElementPosition(int startingPoint)
        {
            var result = (row: startingPoint, column: startingPoint);
            for (int i = startingPoint; i < NumberOfRows; i++)
            {
                for (int j = startingPoint; j < NumberOfColumns; j++)
                {
                    if ((dynamic)TempMatrixValues[i, j] > TempMatrixValues[result.row, result.column])
                    {
                        result = (i, j);
                    }
                }
            }
            return result;
        }

        private int FindIndexOfRowWithGreatestNumberInGivenColumn(int rowNumber, int columnNumber)
        {
            var greatestColumn = (dynamic)TempMatrixValues[rowNumber, columnNumber];
            int index = rowNumber;
            for (int i = rowNumber; i < NumberOfRows; i++)
            {
                if ((dynamic)TempMatrixValues[i, columnNumber] > (dynamic)greatestColumn)
                {
                    greatestColumn = TempMatrixValues[i, columnNumber];
                    index = i;
                }
            }
            return index;
        }

        public void ResetAllColumnsBelow(T[] vector, int rowNumber, int columnNumber)
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

        private void SetTempMatrix()
        {
            TempMatrixValues = (T[,])MatrixValues.Clone();
        }
    }
}