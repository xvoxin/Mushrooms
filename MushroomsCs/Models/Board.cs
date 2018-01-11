using System.Collections.Generic;

namespace MushroomsCs.Models
{
    public class Board
    {
        public int Size { get; set; }
        public List<Mushroom> Muschrooms { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Cube Cube { get; set; }

        public void MovePlayer(bool isPlayerOne, int numberOfMoves)
        {
            if (numberOfMoves == 0)
            {
                return;
            }
            if (isPlayerOne)
            {
                if (Player1.Location + numberOfMoves > (Size - 1) / 2)
                {
                    Player1.Location += numberOfMoves - Size;
                }
                else if (Player1.Location + numberOfMoves < - (Size - 1) / 2)
                {
                    Player1.Location += numberOfMoves + Size;
                }
                else
                {
                    Player1.Location += numberOfMoves;
                }
            }
            else
            {
                if (Player2.Location + numberOfMoves > (Size - 1) / 2)
                {
                    Player2.Location += numberOfMoves - Size;
                }
                else if (Player2.Location + numberOfMoves < - (Size - 1) / 2)
                {
                    Player2.Location += numberOfMoves + Size;
                }
                else
                {
                    Player2.Location += numberOfMoves;
                }
            }
        }
    }
}