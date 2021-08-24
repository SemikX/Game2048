using System;
using System.Collections.Generic;
using System.Linq;

namespace Game2048
{
    public class Game2048
    {
        private const int Width = 4;
        private const int Height = 4;
        private const int VictoryNumber = 2048;
        private readonly Random random = new Random();

        public event EventHandler Victory = delegate { };
        public event EventHandler Defeat  = delegate { };

        // [y, x]
        private int[,] gameCells;
        private int bestScore;
        private int score;

        private bool isVictoryNumberReached;
        private bool noLegalMove;

        private readonly Dictionary<int, ConsoleColor> numberColors = new()
        {
            { 2,    ConsoleColor.White },
            { 4,    ConsoleColor.DarkGreen },
            { 8,    ConsoleColor.Green },
            { 16,   ConsoleColor.Cyan },
            { 32,   ConsoleColor.DarkCyan },
            { 64,   ConsoleColor.DarkYellow },
            { 128,  ConsoleColor.Yellow },
            { 256,  ConsoleColor.DarkMagenta },
            { 512,  ConsoleColor.Magenta },
            { 1024, ConsoleColor.DarkRed },
            { 2048, ConsoleColor.Red }
        };

        public Game2048()
        {
            this.bestScore = 0;
        }

        public void Restart()
        {
            this.gameCells = new int[Height, Width];
            this.score = 0;
            this.noLegalMove = false;
            this.isVictoryNumberReached = false;

            const int startingTwos = 2;

            for (int i = 0; i < startingTwos; i++)
                this.GenerateNumberInFreeCell();
        }

        public void DrawToConsole()
        {
            string numberLineSeparator = string.Join("----", Enumerable.Repeat("|", Width + 1));

            Console.WriteLine(numberLineSeparator);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write("|");

                    int cellNumber = this.gameCells[y, x];

                    string numberString = cellNumber != 0
                        ? cellNumber.ToString()
                        : "";

                    Console.ForegroundColor = this.numberColors.TryGetValue(cellNumber, out ConsoleColor numberColor)
                        ? numberColor
                        : ConsoleColor.Red;

                    Console.Write(numberString.PadLeft(4));
                    Console.ResetColor();
                }

                Console.WriteLine("|");
                Console.WriteLine(numberLineSeparator);
            }

            Console.WriteLine();
            Console.WriteLine($"Score: {this.score}");
            Console.WriteLine($"Best score: {this.bestScore}");

            if (this.isVictoryNumberReached)
                Console.WriteLine($"You win!");

            if (this.noLegalMove)
                Console.WriteLine($"You lose!");
        }

        public void MoveUp()    => this.Move(0, -1);
        public void MoveDown()  => this.Move(0,  1);
        public void MoveLeft()  => this.Move(-1, 0);
        public void MoveRight() => this.Move(1,  0);

        private void Move(int offsetX, int offsetY)
        {
            bool isMovementHappened = false;

            var cellProcessingOrderXs = offsetX < 1
                ? Enumerable.Range(0, Width)
                : Enumerable.Range(0, Width).Reverse();

            var cellProcessingOrderYs = offsetY < 1
                ? Enumerable.Range(0, Height)
                : Enumerable.Range(0, Height).Reverse();

            foreach (int accumulatorCellX in cellProcessingOrderXs)
            {
                foreach (int accumulatorCellY in cellProcessingOrderYs)
                {
                    int dimensionInDirection = offsetX != 0
                        ? Width
                        : Height;

                    for (int i = 1; i < dimensionInDirection; i++)
                    {
                        int movingCellX = accumulatorCellX - i * offsetX;
                        int movingCellY = accumulatorCellY - i * offsetY;

                        if (this.IsCellOutOfBounds(movingCellX, movingCellY))
                            continue;

                        int accumulatorCellNumber = this.gameCells[accumulatorCellY, accumulatorCellX];
                        int movingCellNumber      = this.gameCells[movingCellY, movingCellX];

                        if (movingCellNumber == 0)
                            continue;

                        if (accumulatorCellNumber == 0)
                        {
                            this.gameCells[accumulatorCellY, accumulatorCellX] = movingCellNumber;
                            this.gameCells[movingCellY, movingCellX] = 0;
                            isMovementHappened = true;
                            continue;
                        }

                        if (accumulatorCellNumber == movingCellNumber)
                        {
                            int finalNumber = accumulatorCellNumber + movingCellNumber;

                            this.gameCells[accumulatorCellY, accumulatorCellX] = finalNumber;
                            this.gameCells[movingCellY, movingCellX] = 0;

                            this.score += finalNumber;
                            if (this.score > this.bestScore)
                                this.bestScore = this.score;

                            if (finalNumber >= VictoryNumber)
                                this.isVictoryNumberReached = true;

                            isMovementHappened = true;
                        }

                        break;
                    }
                }
            }

            if (this.isVictoryNumberReached)
            {
                this.Victory(this, EventArgs.Empty);
                return;
            }

            if (isMovementHappened)
            {
                this.GenerateNumberInFreeCell();

                if (!this.IsLegalMoveAvailable())
                {
                    this.noLegalMove = true;
                    this.Defeat(this, EventArgs.Empty);
                }
            }
        }

        private bool IsCellOutOfBounds(int x, int y)
        {
            return x < 0 || x >= Width ||
                   y < 0 || y >= Height;
        }

        private bool IsLegalMoveAvailable()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    int currentCellNumber = this.gameCells[y, x];
                    if (currentCellNumber == 0)
                        return true;

                    if (!this.IsCellOutOfBounds(x + 1, y))
                    {
                        int rightCellNumber = this.gameCells[y, x + 1];
                        if (currentCellNumber == rightCellNumber)
                            return true;
                    }

                    if (!this.IsCellOutOfBounds(x, y + 1))
                    {
                        int bottomCellNumber = this.gameCells[y + 1, x];
                        if (currentCellNumber == bottomCellNumber)
                            return true;
                    }
                }

            return false;
        }

        private void GenerateNumberInFreeCell()
        {
            if (this.TryFindFreeCell(out int freeCellX, out int freeCellY))
            {
                // 10% chance to drop 4 instead of 2
                const double chanceToDrop4 = 0.1;

                int number = this.random.NextDouble() < chanceToDrop4
                    ? 4
                    : 2;

                this.gameCells[freeCellY, freeCellX] = number;
            }
        }

        private bool TryFindFreeCell(out int freeCellX, out int freeCellY)
        {
            int yRnd = this.random.Next(0, Height);
            int xRnd = this.random.Next(0, Width);

            for (int y = 0; y < Height; ++y)
            {
                int checkY = (yRnd + y) % Height;

                for (int x = 0; x < Width; ++x)
                {
                    int checkX = (xRnd + x) % Width;

                    if (this.gameCells[checkY, checkX] == 0)
                    {
                        freeCellX = checkX;
                        freeCellY = checkY;
                        return true;
                    }
                }
            }

            freeCellX = 0;
            freeCellY = 0;
            return false;
        }
    }
}
