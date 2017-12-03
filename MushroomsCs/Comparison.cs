using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MatrixCalculator
{
    public class Comparison
    {
        private readonly string Results1FloatCsPath = "../../../Output/Res1DataFloatCs.txt";
        private readonly string Results2FloatCsPath = "../../../Output/Res2DataFloatCs.txt";
        private readonly string Results3FloatCsPath = "../../../Output/Res3DataFloatCs.txt";
        private readonly string Results1FloatPath = "../../../Output/Res1DataFloat.txt";
        private readonly string Results2FloatPath = "../../../Output/Res2DataFloat.txt";
        private readonly string Results3FloatPath = "../../../Output/Res3DataFloat.txt";
        private readonly string Results1DoublePath = "../../../Output/Res1DataDoubleCs.txt";
        private readonly string Results2DoublePath = "../../../Output/Res2DataDoubleCs.txt";
        private readonly string Results3DoublePath = "../../../Output/Res3DataDoubleCs.txt";
        private readonly string Results1DoubleCsPath = "../../../Output/Res1DataDouble.txt";
        private readonly string Results2DoubleCsPath = "../../../Output/Res2DataDouble.txt";
        private readonly string Results3DoubleCsPath = "../../../Output/Res3DataDouble.txt";
        private readonly string ResultsNorms1Float = "../../../Output/ResultsNorms1Float.txt";
        private readonly string ResultsNorms2Float = "../../../Output/ResultsNorms2Float.txt";
        private readonly string ResultsNorms3Float = "../../../Output/ResultsNorms3Float.txt";
        private readonly string ResultsNorms1Double = "../../../Output/ResultsNorms1Double.txt";
        private readonly string ResultsNorms2Double = "../../../Output/ResultsNorms2Double.txt";
        private readonly string ResultsNorms3Double = "../../../Output/ResultsNorms3Double.txt";
        private readonly string GaussFloatCsPath = "../../../Output/GaussDataFloatCs.txt";
        private readonly string PartialFloatCsPath = "../../../Output/PartialDataFloatCs.txt";
        private readonly string FullFloatCsPath = "../../../Output/FullDataFloatCs.txt";
        private readonly string GaussDoubleCsPath = "../../../Output/GaussDataDoubleCs.txt";
        private readonly string PartialDoubleCsPath = "../../../Output/PartialDataDoubleCs.txt";
        private readonly string FullDoubleCsPath = "../../../Output/FullDataDoubleCs.txt";
        private readonly string GaussFloatPath = "../../../Output/GaussDataFloat.txt";
        private readonly string PartialFloatPath = "../../../Output/PartialDataFloat.txt";
        private readonly string FullFloatPath = "../../../Output/FullDataFloat.txt";
        private readonly string GaussDoublePath = "../../../Output/GaussDataDouble.txt";
        private readonly string PartialDoublePath = "../../../Output/PartialDataDouble.txt";
        private readonly string FullDoublePath = "../../../Output/FullDataDouble.txt";
        private readonly string NormsGaussFloat = "../../../Output/NormsGaussFloat.txt";
        private readonly string NormsPartialFloat = "../../../Output/NormsPartialFloat.txt";
        private readonly string NormsFullFloat = "../../../Output/NormsFullFloat.txt";
        private readonly string NormsGaussDouble = "../../../Output/NormsGaussDouble.txt";
        private readonly string NormsPartialDouble = "../../../Output/NormsPartialDouble.txt";
        private readonly string NormsFullDouble = "../../../Output/NormsFullDouble.txt";
        

        private void CompareDoubleCsCppAndFraction()
        {
            //tylko res 1 -3
            
        }

        public void CompareFloatCsAndCpp()
        {
            var results1CsFloat = ParseFile<float>(File.ReadAllLines(Results1FloatCsPath));
            var results1Float = ParseFile<float>(File.ReadAllLines(Results1FloatPath));
            var normsForResults =    
                results1CsFloat.Zip(results1Float, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(ResultsNorms1Float, normsForResults);

            var results2CsFloat = ParseFile<float>(File.ReadAllLines(Results2FloatCsPath));
            var results2Float = ParseFile<float>(File.ReadAllLines(Results2FloatPath));
            normsForResults =
                results2CsFloat.Zip(results2Float, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(ResultsNorms2Float, normsForResults);

            var results3CsFloat = ParseFile<float>(File.ReadAllLines(Results3FloatCsPath));
            var results3Float = ParseFile<float>(File.ReadAllLines(Results3FloatPath));
            normsForResults =
                results3CsFloat.Zip(results3Float, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(ResultsNorms3Float, normsForResults);
        }

        private void CompareDoubleCsAndCpp()
        {
            var results1CsDouble = ParseFile<double>(File.ReadAllLines(Results1DoubleCsPath));
            var results1Double = ParseFile<double>(File.ReadAllLines(Results1DoublePath));
            var normsForResults =
                results1CsDouble.Zip(results1Double, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(ResultsNorms1Double, normsForResults);

            var results2CsDouble = ParseFile<double>(File.ReadAllLines(Results2DoubleCsPath));
            var results2Double = ParseFile<double>(File.ReadAllLines(Results2DoublePath));
            normsForResults =
                results2CsDouble.Zip(results2Double, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(ResultsNorms2Double, normsForResults);

            var results3CsDouble = ParseFile<double>(File.ReadAllLines(Results3DoubleCsPath));
            var results3Double = ParseFile<double>(File.ReadAllLines(Results3DoublePath));
            normsForResults =
                results3CsDouble.Zip(results3Double, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(ResultsNorms3Double, normsForResults);
        }

        private void CompareGaussFloat()
        {
            var gaussCsFloat = ParseFile<float>(File.ReadAllLines(GaussFloatCsPath));
            var gaussFloat = ParseFile<float>(File.ReadAllLines(GaussFloatPath));
            var normsForResults =
                gaussCsFloat.Zip(gaussFloat, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(NormsGaussFloat, normsForResults);

            var partialCsFloat = ParseFile<float>(File.ReadAllLines(PartialFloatCsPath));
            var partialFloat = ParseFile<float>(File.ReadAllLines(PartialFloatPath));
            normsForResults =
                partialCsFloat.Zip(partialFloat, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(NormsPartialFloat, normsForResults);

            var fullCsFloat = ParseFile<float>(File.ReadAllLines(FullFloatCsPath));
            var fullFloat = ParseFile<float>(File.ReadAllLines(FullFloatPath));
            normsForResults =
                fullCsFloat.Zip(fullFloat, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(NormsFullFloat, normsForResults);
        }

        private void CompareGaussDoubleAndFraction()
        {
            var gaussCsDouble = ParseFile<double>(File.ReadAllLines(GaussDoubleCsPath));
            var gaussDouble = ParseFile<double>(File.ReadAllLines(GaussDoublePath));
            var normsForResults =
                gaussCsDouble.Zip(gaussDouble, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(NormsGaussDouble, normsForResults);

            var partialCsDouble = ParseFile<double>(File.ReadAllLines(PartialDoubleCsPath));
            var partialDouble = ParseFile<double>(File.ReadAllLines(PartialDoublePath));
            normsForResults =
                partialCsDouble.Zip(partialDouble, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(NormsPartialDouble, normsForResults);

            var fullCsDouble = ParseFile<double>(File.ReadAllLines(FullDoubleCsPath));
            var fullDouble = ParseFile<double>(File.ReadAllLines(FullDoublePath));
            normsForResults =
                fullCsDouble.Zip(fullDouble, (x, y) => x.Zip(y, (a, b) => a > b ? a - b : b - a).Max().ToString());
            File.WriteAllLines(NormsFullDouble, normsForResults);
        }

        private List<List<T>> ParseFile<T>(string[] fileContent) where T: new()
        {
            var result = new List<List<T>>();
            foreach (var line in fileContent)
            {
                var partialResult = new List<T>();
                var values = line.Split(' ');
                foreach (var value in values)
                {
                    if (value != string.Empty)
                    {
                        if (typeof(T) == typeof(float))
                        {
                            partialResult.Add((dynamic)float.Parse(value));
                        }
                        else if (typeof(T) == typeof(double))
                        {
                            partialResult.Add((dynamic)double.Parse(value));
                        }
                    }
                }
                result.Add(partialResult);
            }
            return result;
        }
    }
}
