namespace MushroomsCs
{
    public class Probability
    {
        public double[,] ProbMatrix;
        public double[] VectorB;

        public void CreateMatrixWithProbability(int mapLength, int playerOnePosition, int playerTwoPosition, 
            int[] qubeValues, double[] qubePropabilities)
        {

            int i = 0;
            int possibility = (mapLength - 1) * (mapLength - 1);

            Pi[] piTable = new Pi[possibility * 2];

            int inc = 1;

            if (playerOnePosition < playerTwoPosition)
            {
                inc = -1;
            }
            while (i < possibility * 2)
            {
                for (int j = playerTwoPosition; -j != playerTwoPosition - inc; j += inc)
                {
                    for (int k = playerOnePosition; -k != playerOnePosition + inc; k -= inc)
                    {
                        if (j != 0 && k != 0)
                        {
                            piTable[i] = new Pi(i, k, j, qubeValues, qubePropabilities, mapLength);
                            i++;
                        }
                    }
                }
            }

            for (int j = 0; j < possibility; j++)
            {
                piTable[j].IsPlayerOneTurn = true;
            }
            for (int j = possibility; j < possibility * 2; j++)
            {
                piTable[j].IsPlayerOneTurn = false;
            }

            for (int j = 0; j < possibility * 2; j++)
            {
                piTable[j].SetNextMove((Pi[])piTable.Clone());
            }
            
            ProbMatrix = new double[possibility * 2, possibility * 2];
            VectorB = new double[possibility * 2];
            for (int x = 0; x < possibility * 2; x++)
            {
                ProbMatrix[x, x] = 1;
                for (int j = 0; j < piTable[x].NextPlayerMoveId.Length; j++)
                {
                    var nextPlayerId = piTable[x].NextPlayerMoveId[j];
                    var prob = qubePropabilities[j];

                    if (nextPlayerId == -1 && piTable[x].IsPlayerOneTurn)
                    {
                        VectorB[x] += prob;
                    }
                    else if(nextPlayerId == -1 && piTable[x].IsPlayerOneTurn == false) { }
                    else
                    {
                        ProbMatrix[x, nextPlayerId] += prob;
                    }
                }
            }
        }
    }

    public class Pi
    {
        public int Id;
        public int PlayerOnePosition;
        public int PlayerTwoPosition;
        public bool IsPlayerOneTurn;
        public int[] NextPlayerMoveId;
        private readonly int _mapSize;
        private readonly int[] _qubeValues;
        public readonly double[] QubeProbs;

        public Pi(int id, int playerOnePosition, int playerTwoPosition, int[] qubeValues, double[] qubeProbs, int mapSize)
        {
            Id = id;

            PlayerOnePosition = playerOnePosition;
            PlayerTwoPosition = playerTwoPosition;
            NextPlayerMoveId = new int[qubeValues.Length];
            _mapSize = mapSize;
            _qubeValues = (int[]) qubeValues.Clone();
            QubeProbs = (double[]) qubeProbs.Clone();
        }

        public Pi(int id, int playerOnePosition, int playerTwoPosition, bool playerTurn, int[] nextMove, 
            int[] qubeValues, double[] qubeProbs, int mapSize)
        {
            Id = id;

            PlayerOnePosition = playerOnePosition;
            PlayerTwoPosition = playerTwoPosition;
            NextPlayerMoveId = (int[]) nextMove.Clone();
            IsPlayerOneTurn = playerTurn;
            _mapSize = mapSize;
            _qubeValues = (int[])qubeValues.Clone();
            QubeProbs = (double[])qubeProbs.Clone();
        }

        public bool SetNextMove(Pi[] pis)
        {
            int pislen = pis.Length / 2;

            for (int i = 0; i < _qubeValues.Length; i++)
            {
                for (int j = 0; j < pis.Length; j++)
                {
                    if (IsPlayerOneTurn == false)
                    {
                        if (_qubeValues[i] == 0)
                        {
                            int temp;
                            if (Id + pislen < pis.Length)
                            {
                                temp = Id + pislen;
                            }
                            else
                            {
                                temp = Id - pislen;
                            }
                            NextPlayerMoveId[i] = temp;
                        }
                        else if (pis[j].PlayerOnePosition == PlayerOnePosition
                            && pis[j].PlayerTwoPosition == CheckIsInRange(PlayerTwoPosition, _qubeValues[i])
                            && pis[j].IsPlayerOneTurn)
                        {
                            NextPlayerMoveId[i] = pis[j].Id;
                        }
                        else if (PlayerTwoPosition + _qubeValues[i] == 0)
                        {
                            NextPlayerMoveId[i] = -1;
                        }
                    }
                    else
                    {
                        if (_qubeValues[i] == 0)
                        {
                            int temp;
                            if (Id + pislen < pis.Length)
                            {
                                temp = Id + pislen;
                            }
                            else
                            {
                                temp = Id - pislen;
                            }
                            NextPlayerMoveId[i] = temp;
                        }
                        else if (pis[j].PlayerOnePosition == CheckIsInRange(PlayerOnePosition,  _qubeValues[i])
                            && pis[j].PlayerTwoPosition == PlayerTwoPosition
                            && !pis[j].IsPlayerOneTurn)
                        {
                            NextPlayerMoveId[i] = pis[j].Id;
                        }
                        else if (PlayerOnePosition + _qubeValues[i] == 0)
                        {
                            NextPlayerMoveId[i] = -1;
                        }
                    }
                }
            }
            return true;

        }

        private int CheckIsInRange(int pos, int qube)
        {
            int range = (_mapSize - 1) / 2;
            int ret;

            if (pos + qube > range)
            {
                ret = pos + qube - _mapSize;
            }
            else if (pos + qube < -range)
            {
                ret = pos + qube + _mapSize;
            }
            else
            {
                ret = pos + qube;
            }
            return ret;
        }

        public Pi Clone()
        {
            return new Pi(Id, PlayerOnePosition, PlayerTwoPosition, IsPlayerOneTurn, 
                NextPlayerMoveId, _qubeValues, QubeProbs, _mapSize);
        }
    }
}