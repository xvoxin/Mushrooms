using System;
using System.Runtime.InteropServices;

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
            bool hasZero = false;

            foreach (var x in qubeValues)
            {
                if (x == 0)
                {
                    hasZero = true;
                    break;
                }
            }

            Pi[] piTable = new Pi[possibility];

            int inc = 1;

            if (playerOnePosition < playerTwoPosition)
            {
                inc = -1;
            }
            
            for (int j = playerTwoPosition; -j != playerTwoPosition - inc; j += inc)
            {
                for (int k = playerOnePosition; -k != playerOnePosition + inc; k -= inc) //all possibilities 
                {
                    if (j != 0 && k != 0)
                    {
                        piTable[i] = new Pi(i, k, j, qubeValues, qubePropabilities, mapLength);
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
                    if (piTable[j].SetNextMove((Pi[])piTable.Clone(), true) == true)
                    {
                        notNullTable[j] = true;
                        if (piTable[j].IsPlayerOneTurn == true)
                        {
                            foreach (int t in piTable[j].NextPlayerMoveId)
                            {
                                if (t > 0 && t < possibility && piTable[t].IsPlayerOneTurn == null)
                                {
                                    piTable[t].IsPlayerOneTurn = false;
                                }
                            }
                        }
                        else
                        {
                            foreach (int t in piTable[j].NextPlayerMoveId)
                            {
                                if (t > 0 && t < possibility && piTable[t].IsPlayerOneTurn == null)
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

            Pi[] pi = new Pi[possibility * 2];

            if (hasZero)
            {
                for (int j = 0; j < possibility; j++)
                {
                    pi[j] = new Pi(-10, 0, 0, qubeValues, qubePropabilities, 0); //temporary
                }

                int k = 0;
                for (int j = possibility; j < possibility * 2; j++)
                {
                    pi[j] = piTable[k].Clone();
                    pi[j].IsPlayerOneTurn = !piTable[k].IsPlayerOneTurn;
                    pi[j].Id += possibility;
                    k++;
                }
                for (int j = possibility; j < possibility * 2; j++)
                {
                    pi[j].SetNextMove((Pi[])pi.Clone(), false);
                }
                possibility *= 2;
            }

            for (int j = 0; j < possibility; j++)
            {
                if (hasZero && j < possibility / 2)
                    pi[j] = piTable[j].Clone();
                else if(!hasZero)
                    pi[j] = piTable[j].Clone();
            }

            ProbMatrix = new double[possibility, possibility];
            VectorB = new double[possibility];
            int counter = 0;
            for (int x = 0; x < possibility; x++)
            {
                ProbMatrix[x, x] = 1;
                for (int j = 0; j < pi[x].NextPlayerMoveId.Length; j++)
                {
                    var nextPlayerId = pi[x].NextPlayerMoveId[j];
                    var prob = pi[x].QubeProbs[j];

                    if (nextPlayerId == -1 && pi[x].IsPlayerOneTurn == true)
                    {
                        VectorB[x] = prob;
                        counter++;
                    }
                    else if(nextPlayerId == -1 && pi[x].IsPlayerOneTurn == false) { } //not nice i know it
                    else
                    {
                        ProbMatrix[x, nextPlayerId] = -prob;
                    }
                }
            }
            Console.WriteLine("Player one won " + counter + " times");
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

        public Pi(int id, int playerOnePosition, int playerTwoPosition, bool? playerTurn, int[] nextMove, 
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

        public bool SetNextMove(Pi[] pis, bool withoutZero) //1 is with, 2 is not with
        {
            int multiply = Convert.ToInt32(withoutZero) + 1;
            int pislen = pis.Length;
            if (!withoutZero)
            {
                pislen /= 2;
            }
            if (IsPlayerOneTurn != null)
            {
                for (int i = 0; i < _qubeValues.Length; i++)
                {
                    for (int j = 0; j < pis.Length; j++)
                    {
                        if (IsPlayerOneTurn == false)
                        {
                            if (_qubeValues[i] == 0)
                            {
                                int temp;
                                if (Id + pis.Length < pis.Length * multiply)
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
                            if (_qubeValues[i] == 0)
                            {
                                int temp;
                                if (Id + pis.Length < pis.Length * multiply)
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