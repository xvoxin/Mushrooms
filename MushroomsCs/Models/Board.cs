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
    }
}