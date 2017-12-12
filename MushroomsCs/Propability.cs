using System;
using System.Dynamic;
using System.Globalization;
using System.Security.Policy;

namespace MushroomsCs
{
    public class Propability
    {
        public double[,] CreateMatrixWithPropability()
        {
            int mapLength = 5;
            bool isQubeEqual = true;

            double qubePropability = 1d / 2d;
            int[] qubeValues = {-1, 1};

            int playerOnePosition = -2;
            int playerTwoPosition = 2;

            int i = 0;

            int possibility = (mapLength - 1) * (mapLength - 1);

            Pi[] piTable = new Pi[possibility];

            double[,] resMatrix = new double[possibility, possibility];
            double[] vectorB = new double[possibility];

            //i++;

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
                        Console.WriteLine(j + " brejk");
                        break;
                    }
                    flag = true;
                }
            }

            for (int j = 0; j < possibility; j++)
            {
                Console.WriteLine(piTable[j].IsPlayerOneTurn + " - P(" + piTable[j].PlayerOnePosition + "," + piTable[j].PlayerTwoPosition + ")");
            }
            return null;
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
                        if (IsPlayerOneTurn == true)
                        {
                            if (pis[j].PlayerOnePosition == PlayerOnePosition
                                && pis[j].PlayerTwoPosition == CheckCosTam(PlayerTwoPosition, _qubeValues[i]))
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
                            if (pis[j].PlayerOnePosition == CheckCosTam(PlayerOnePosition,  _qubeValues[i])
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

        private int CheckCosTam(int pos, int qube)
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
            Console.WriteLine(ret);
            return ret;
        }
    }
}