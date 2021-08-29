using NUnit.Framework;

namespace Game2048.UnitTests
{
    public class Game2048Tests
    {
        private Game2048 game;

        [SetUp]
        public void Setup()
        {
            this.game = new Game2048();
            this.game.Restart();
        }

        [Test]
        public void Game_state_does_not_change_on_sequential_loading_and_saving()
        {
            // Arrange
            int[,] initialCellsState =
            {
                { 2, 0,  4, 1024 },
                { 0, 4,  0,    0 },
                { 0, 0,  8,    0 },
                { 0, 32, 0,   16 },
            };

            var testState = new Game2048State(initialCellsState, 500, 2500, IsVictoryNumberReached: false, NoLegalMove: false);

            // Act
            this.game.LoadState(testState);
            Game2048State newState = this.game.GetState();

            // Assert
            // TODO: Would it be better to add method Equals to Game2048State?
            Assert.That(testState.GameCells.SequenceEquals(newState.GameCells));
            Assert.That(testState.Score, Is.EqualTo(newState.Score));
            Assert.That(testState.BestScore, Is.EqualTo(newState.BestScore));
            Assert.That(testState.NoLegalMove, Is.EqualTo(newState.NoLegalMove));
            Assert.That(testState.IsVictoryNumberReached, Is.EqualTo(newState.IsVictoryNumberReached));
        }
    }
}