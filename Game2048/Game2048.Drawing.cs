using System;
using System.Collections.Generic;
using System.Linq;

namespace Game2048
{
    public partial class Game2048
    {
        private static readonly string NumberLineSeparator = string.Join("----", Enumerable.Repeat("|", Width + 1));

        private static readonly Dictionary<int, ConsoleColor> NumberColors = new()
        {
            { 2, ConsoleColor.White },
            { 4, ConsoleColor.DarkGreen },
            { 8, ConsoleColor.Green },
            { 16, ConsoleColor.Cyan },
            { 32, ConsoleColor.DarkCyan },
            { 64, ConsoleColor.DarkYellow },
            { 128, ConsoleColor.Yellow },
            { 256, ConsoleColor.DarkMagenta },
            { 512, ConsoleColor.Magenta },
            { 1024, ConsoleColor.DarkRed },
            { 2048, ConsoleColor.Red }
        };

        public void DrawToConsole()
        {
            Console.WriteLine(NumberLineSeparator);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write("|");

                    int cellNumber = this.gameCells[y, x];

                    string numberString = cellNumber != 0
                        ? cellNumber.ToString()
                        : "";

                    Console.ForegroundColor = NumberColors.TryGetValue(cellNumber, out ConsoleColor numberColor)
                        ? numberColor
                        : ConsoleColor.Red;

                    Console.Write(numberString.PadLeft(4));
                    Console.ResetColor();
                }

                Console.WriteLine("|");
                Console.WriteLine(NumberLineSeparator);
            }

            Console.WriteLine();
            Console.WriteLine($"Score: {this.score}");
            Console.WriteLine($"Best score: {this.bestScore}");

            switch (this.gameStatus)
            {
                case GameStatus.Victory:
                    Console.WriteLine("You win!");
                    break;
                case GameStatus.Defeat:
                    Console.WriteLine("You lose!");
                    break;
            }
        }
    }
}