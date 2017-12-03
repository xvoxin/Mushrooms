using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace MatrixCalculator
{
    public class TimeCalculator
    {
        private string Res1DoubleTimes = "../../../Output/Res1DoubleTimes.txt";
        private string Res2DoubleTimes = "../../../Output/Res2DoubleTimes.txt";
        private string Res3DoubleTimes = "../../../Output/Res3DoubleTimes.txt";
        private string Res1FloatTimes = "../../../Output/Res1FloatTimes.txt";
        private string Res2FloatTimes = "../../../Output/Res2FloatTimes.txt";
        private string Res3FloatTimes = "../../../Output/Res3FloatTimes.txt";
        private string Res1FractionTimes = "../../../Output/Res1FractionTimes.txt";
        private string Res2FractionTimes = "../../../Output/Res2FractionTimes.txt";
        private string Res3FractionTimes = "../../../Output/Res3FractionTimes.txt";
        private string GaussDoubleTimes = "../../../Output/GaussDoubleTimes.txt";
        private string PartialDoubleTimes = "../../../Output/PartialDoubleTimes.txt";
        private string FullDoubleTimes = "../../../Output/FullDoubleTimes.txt";
        private string GaussFloatTimes = "../../../Output/GaussFloatTimes.txt";
        private string PartialFloatTimes = "../../../Output/ParialFloatTimes.txt";
        private string FullFloatTimes = "../../../Output/FullFloatTimes.txt";

        private int StartingPoint = 5;
        private int NumberOfIterations = 101;
        private int Difference = 5;

        [Fact]
        public void CountTimeForGaussDouble()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<double>();
                var matrixA = new Matrix<double>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                matrixA.GaussWithoutChoice(vector);
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(GaussDoubleTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForPartialDouble()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<double>();
                var matrixA = new Matrix<double>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                matrixA.GaussWithPartialPivot(vector);
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(PartialDoubleTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForFullDouble()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<double>();
                var matrixA = new Matrix<double>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                matrixA.GaussWithCompletePivot(vector);
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(FullDoubleTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForGaussFloat()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<float>();
                var matrixA = new Matrix<float>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                matrixA.GaussWithoutChoice(vector);
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(GaussFloatTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForPartialFloat()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<float>();
                var matrixA = new Matrix<float>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                matrixA.GaussWithPartialPivot(vector);
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(PartialFloatTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForFullFloat()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<float>();
                var matrixA = new Matrix<float>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                matrixA.GaussWithCompletePivot(vector);
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(FullFloatTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForFirstOperationDouble()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<double>();
                var matrixA = new Matrix<double>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                var result = matrixA * vector;
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(Res1DoubleTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForSecondOperationDouble()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<double>();
                var matrixA = new Matrix<double>(i, i);
                var matrixB = new Matrix<double>(i, i);
                var matrixC = new Matrix<double>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                var result = (matrixA + matrixB + matrixC) * vector;
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(Res2DoubleTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForThirdOperationDouble()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var matrixA = new Matrix<double>(i, i);
                var matrixB = new Matrix<double>(i, i);
                var matrixC = new Matrix<double>(i, i);
                stopwatch.Start();
                var result = matrixA * (matrixB * matrixC);
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(Res3DoubleTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForFirstOperationFloat()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<float>();
                var matrixA = new Matrix<float>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                var result = matrixA * vector;
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(Res1FloatTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForSecondOperationFloat()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<float>();
                var matrixA = new Matrix<float>(i, i);
                var matrixB = new Matrix<float>(i, i);
                var matrixC = new Matrix<float>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                var result = (matrixA + matrixB + matrixC) * vector;
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(Res2FloatTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForThirdOperationFloat()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var matrixA = new Matrix<float>(i, i);
                var matrixB = new Matrix<float>(i, i);
                var matrixC = new Matrix<float>(i, i);
                stopwatch.Start();
                var result = matrixA * (matrixB * matrixC);
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(Res3FloatTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForFirstOperationFraction()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<Fraction>();
                var matrixA = new Matrix<Fraction>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                var result = matrixA * vector;
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(Res1FractionTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForSecondOperationFraction()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var generator = new File<Fraction>();
                var matrixA = new Matrix<Fraction>(i, i);
                var matrixB = new Matrix<Fraction>(i, i);
                var matrixC = new Matrix<Fraction>(i, i);
                var vector = generator.FillVectorWithRandom(i);
                stopwatch.Start();
                var result = (matrixA + matrixB + matrixC) * vector;
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(Res2FractionTimes, times.Select(x => x.ToString()));
        }

        [Fact]
        public void CountTimeForThirdOperationFraction()
        {
            var times = new List<double>();
            var stopwatch = new Stopwatch();
            for (int i = StartingPoint; i < NumberOfIterations; i += Difference)
            {
                var matrixA = new Matrix<Fraction>(i, i);
                var matrixB = new Matrix<Fraction>(i, i);
                var matrixC = new Matrix<Fraction>(i, i);
                stopwatch.Start();
                var result = matrixA * (matrixB * matrixC);
                stopwatch.Stop();
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            File.WriteAllLines(Res3FractionTimes, times.Select(x => x.ToString()));
        }
    }
}