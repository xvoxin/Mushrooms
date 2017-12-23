namespace MushroomsCs.Models
{
    public class Cube
    {
        public int NumberOfWalls { get; set; }
        public int[] PossibleResults { get; set; }
        public double[] ProbabilitiesOfCertainResult { get; set; }
        public int[] NaturalProbabilities { get; set; }
    }
}