using System.Linq;
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
            this.game.Initialize();
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

            var testState = new Game2048State(initialCellsState, 500, 2500, GameStatus.WaitingForMove);

            // Act
            this.game.ApplyState(testState);
            Game2048State newState = this.game.GetState();

            // Assert
            // TODO: Would it be better to add method Equals to Game2048State?
            Assert.That(testState.GameCells.SequenceEquals(newState.GameCells));
            Assert.That(testState.Score, Is.EqualTo(newState.Score));
            Assert.That(testState.BestScore, Is.EqualTo(newState.BestScore));
            Assert.That(testState.GameStatus, Is.EqualTo(newState.GameStatus));
        }

        [Test]
        public void Two_plus_two_equals_four()
        {
            // Arrange
            int[,] initialCellsState =
            {
                { 0, 0, 0, 0 },
                { 2, 0, 2, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
            };

            this.game.ApplyState(new Game2048State(initialCellsState, 0, 0, GameStatus.WaitingForMove));

            // Act
            this.game.MoveRight();

            // Assert
            Assert.That(this.game.GetState().GameCells[1, 3], Is.EqualTo(4));
        }

        [Test]
        public void New_numbers_appear_after_move()
        {
            // Arrange
            int[,] initialCellsState =
            {
                { 0, 0, 0, 0 },
                { 0, 2, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
            };

            this.game.ApplyState(new Game2048State(initialCellsState, 0, 0, GameStatus.WaitingForMove));

            // Act
            this.game.MoveDown();

            // Assert
            Assert.That(this.game.GetState().GameCells.Cast<int>().Sum(), Is.EqualTo(4).Or.EqualTo(6));
        }

        [Test]
        public void Nothing_happens_after_useless_moves()
        {
            // Arrange
            int[,] initialCellsState =
            {
                { 0, 0, 0, 8 },
                { 0, 0, 0, 2 },
                { 0, 0, 0, 8 },
                { 0, 2, 4, 2 },
            };

            this.game.ApplyState(new Game2048State(initialCellsState, 0, 0, GameStatus.WaitingForMove));

            // Act
            this.game.MoveRight();
            this.game.MoveDown();

            // Assert
            Game2048State state = this.game.GetState();

            Assert.That(state.Score, Is.EqualTo(0));
            Assert.That(state.BestScore, Is.EqualTo(0));
            Assert.That(state.GameStatus, Is.EqualTo(GameStatus.WaitingForMove));
            Assert.That(state.GameCells.Cast<int>().Sum(), Is.EqualTo(26));
        }

        [Test]
        public void Score_increased_after_successful_move()
        {
            // Arrange
            int[,] initialCellsState =
            {
                { 0, 16, 16, 0 },
                { 0, 0,  0,  0 },
                { 0, 32, 0,  32 },
                { 0, 0,  0,  0 },
            };

            this.game.ApplyState(new Game2048State(initialCellsState, 0, 8, GameStatus.WaitingForMove));

            // Act
            this.game.MoveRight();

            // Assert
            Game2048State state = this.game.GetState();

            Assert.That(state.Score, Is.EqualTo(96));
            Assert.That(state.BestScore, Is.EqualTo(96));
        }

        [Test]
        public void Victory_when_2048()
        {
            // Arrange
            int[,] initialCellsState =
            {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 1024, 1024 },
            };

            this.game.ApplyState(new Game2048State(initialCellsState, 0, 0, GameStatus.WaitingForMove));

            // Act
            this.game.MoveLeft();

            // Assert
            Assert.That(this.game.GetState().GameStatus, Is.EqualTo(GameStatus.Victory));
        }

        [Test]
        public void Defeat_when_no_moves_available()
        {
            // Arrange
            int[,] initialCellsState =
            {
                { 128,  64,  32,  16 },
                { 256,  128, 64,  32 },
                { 512,  256, 128, 4 },
                { 1024, 512, 256, 4 },
            };

            this.game.ApplyState(new Game2048State(initialCellsState, 0, 0, GameStatus.WaitingForMove));

            // Act
            this.game.MoveUp();

            // Assert
            Assert.That(this.game.GetState().GameStatus, Is.EqualTo(GameStatus.Defeat));
        }

        [Test]
        public void Special_case1()
        {
            // Arrange
            int[,] initialCellsState =
            {
                { 4, 0, 0, 0 },
                { 2, 0, 0, 0 },
                { 2, 0, 0, 0 },
                { 2, 0, 0, 0 },
            };

            this.game.ApplyState(new Game2048State(initialCellsState, 0, 0, GameStatus.WaitingForMove));

            // Act
            this.game.MoveUp();

            // Assert
            int[,] gameCells = this.game.GetState().GameCells;

            Assert.That(gameCells[0, 0], Is.EqualTo(4));
            Assert.That(gameCells[1, 0], Is.EqualTo(4));
            Assert.That(gameCells[2, 0], Is.EqualTo(2));
        }

        [Test]
        public void Special_case2()
        {
            // Arrange
            int[,] initialCellsState =
            {
                { 0, 0, 4, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 2, 0 },
                { 0, 0, 2, 0 },
            };

            this.game.ApplyState(new Game2048State(initialCellsState, 0, 0, GameStatus.WaitingForMove));

            // Act
            this.game.MoveDown();

            // Assert
            int[,] gameCells = this.game.GetState().GameCells;

            Assert.That(gameCells[2, 2], Is.EqualTo(4));
            Assert.That(gameCells[3, 2], Is.EqualTo(4));
        }
    }
}