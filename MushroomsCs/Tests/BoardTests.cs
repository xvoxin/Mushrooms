using MushroomsCs.Models;
using NUnit.Framework;

namespace MushroomsCs.Tests
{
    [TestFixture]
    public class BoardTests
    {
        [Test]
        public void MovePlayer_MovesPlayerOneAboveTreshold_ShouldMoveProperly()
        {
            var target = new Board
            {
                Player1 = new Player
                {
                    Location = 2
                },
                Player2 = new Player
                {
                    Location = 1
                },
                Size = 7
            };
            target.MovePlayer(true, 2);

            Assert.AreEqual(-3, target.Player1.Location);
        }

        [Test]
        public void MovePlayer_MovesPlayerOnePositively_ShouldMoveProperly()
        {
            var target = new Board
            {
                Player1 = new Player
                {
                    Location = 2
                },
                Player2 = new Player
                {
                    Location = 1
                },
                Size = 7
            };
            target.MovePlayer(true, -3);

            Assert.AreEqual(-1, target.Player1.Location);
        }

        [Test]
        public void MovePlayer_MovesPlayerOneBelowTreshold_ShouldMoveProperly()
        {
            var target = new Board
            {
                Player1 = new Player
                {
                    Location = 2
                },
                Player2 = new Player
                {
                    Location = 1
                },
                Size = 7
            };
            target.MovePlayer(true, -6);

            Assert.AreEqual(3, target.Player1.Location);
        }
    }   
}