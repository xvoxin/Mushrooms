using System;

namespace MushroomsCs
{
    public class Probability
    {
        public double[,] ProbMatrix;
        public double[] VectorB;

        public void CreateMatrixWithPropability()
        {
            int mapLength = 5;
            bool isQubeEqual = true;

            double qubePropability = 1d / 2d;
            int[] qubeValues = {-1, 1};

            int playerOnePosition = -2;
            int playerTwoPosition = 2;

            int i = 0;

            int possibility = (mapLength - 1) * (mapLength - 1);

            ProbMatrix = new double[possibility, possibility];
            VectorB = new double[possibility];

            Pi[] piTable = new Pi[possibility];

            for (int k = playerOnePosition; k <= playerTwoPosition; k++) //all possibilities 
            {
                for (int j = playerTwoPosition; j >= playerOnePosition; j--)
                {
                    if (j != 0 && k != 0)
                    {
                        piTable[i] = new Pi(i, k, j, qubeValues, mapLength);
                        i++;
                    }
                }
            }

            piTable[0].IsPlayerOneTurn = true;

            bool[] notNullTable = new bool[possibility];
            notNullTable[0] = true;
            bool flag = false;

            while (!flag)
            {
                for (int j = 0; j < possibility; j++)
                {
                    if (piTable[j].SetNextMove((Pi[])piTable.Clone()) == true)
                    {
                        notNullTable[j] = true;
                        if (piTable[j].IsPlayerOneTurn == true)
                        {
                            foreach (int t in piTable[j].NextPlayerMoveId)
                            {
                                if (t > 0)
                                {
                                    piTable[t].IsPlayerOneTurn = false;
                                }
                            }
                        }
                        else
                        {
                            foreach (int t in piTable[j].NextPlayerMoveId)
                            {
                                if (t > 0)
                                {
                                    piTable[t].IsPlayerOneTurn = true;
                                }
                            }
                        }
                    }
                }
                for (int j = 0; j < possibility; j++)
                {
                    if (notNullTable[j] == false)
                    {
                        flag = false;
                        break;
                    }
                    flag = true;
                }
            }

            for (int j = 0; j < possibility; j++)
            {
                Console.WriteLine(piTable[j].IsPlayerOneTurn + " - P(" + piTable[j].PlayerOnePosition + "," + piTable[j].PlayerTwoPosition + ")");
            }

            for (int x = 0; x < possibility; x++)
            {
                ProbMatrix[x, x] = 1;
                foreach (int t in piTable[x].NextPlayerMoveId)
                {
                    if (t == -1 && piTable[x].IsPlayerOneTurn == true)
                    {
                        VectorB[x] = qubePropability;
                    }
                    else if(t == -1 && piTable[x].IsPlayerOneTurn == false) { } //not nice i know it
                    else
                    {
                        ProbMatrix[x, t] = -qubePropability;
                    }
                }
            }
        }
    }

    class Pi
    {
        public int Id;
        public int PlayerOnePosition;
        public int PlayerTwoPosition;
        public bool? IsPlayerOneTurn;
        public int[] NextPlayerMoveId; //-1 means other player win

        private readonly int _mapSize;
        private readonly int[] _qubeValues;

        public Pi(int id, int playerOnePosition, int playerTwoPosition, int[] qubeValues, int mapSize)
        {
            Id = id;

            PlayerOnePosition = playerOnePosition;
            PlayerTwoPosition = playerTwoPosition;
            NextPlayerMoveId = new int[qubeValues.Length];
            _mapSize = mapSize;
            _qubeValues = (int[]) qubeValues.Clone();
        }

        public bool SetNextMove(Pi[] pis)
        {
            if (IsPlayerOneTurn != null)
            {
                for (int i = 0; i < _qubeValues.Length; i++)
                {
                    for (int j = 0; j < pis.Length; j++)
                    {
                        if (IsPlayerOneTurn == false)
                        {
                            if (pis[j].PlayerOnePosition == PlayerOnePosition
                                && pis[j].PlayerTwoPosition == CheckIsInRange(PlayerTwoPosition, _qubeValues[i]))
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
                            if (pis[j].PlayerOnePosition == CheckIsInRange(PlayerOnePosition,  _qubeValues[i])
                                && pis[j].PlayerTwoPosition == PlayerTwoPosition)
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
            return false;
        }

        private int CheckIsInRange(int pos, int qube)
        {
            int range = (_mapSize - 1) / 2;
            int ret = 0;

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
    }
}