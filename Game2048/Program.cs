using System;

namespace Game2048
{
    public class Program
    {
        private static Game2048 game;
        private static GameState currentGameState;

        public static void Main()
        {
            game = new Game2048();
            game.Victory += (_, _) => currentGameState = GameState.RestartOrQuit;
            game.Defeat  += (_, _) => currentGameState = GameState.RestartOrQuit;
            game.Restart();

            currentGameState = GameState.GameLoop;

            while (true)
            {
                Console.Clear();

                game.DrawToConsole();

                switch (currentGameState)
                {
                    case GameState.GameLoop:
                        UpdateGameLoop(game);
                        break;
                    case GameState.RestartConfirmation:
                        WaitingRestartConfirmation(game);
                        break;
                    case GameState.QuitConfirmation:
                        WaitingQuitConfirmation();
                        break;
                    case GameState.RestartOrQuit:
                        WaitingRestartOrQuit();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void UpdateGameLoop(Game2048 game)
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.DownArrow:
                    game.MoveDown();
                    break;
                case ConsoleKey.UpArrow:
                    game.MoveUp();
                    break;
                case ConsoleKey.LeftArrow:
                    game.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    game.MoveRight();
                    break;
                case ConsoleKey.R:
                    currentGameState = GameState.RestartConfirmation;
                    break;
                case ConsoleKey.Q:
                    currentGameState = GameState.QuitConfirmation;
                    break;
            }
        }

        private static void WaitingRestartConfirmation(Game2048 game)
        {
            Console.WriteLine("Do you want to restart the game? [Y/N]");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Y:
                    game.Restart();
                    currentGameState = GameState.GameLoop;
                    break;
                case ConsoleKey.N:
                    currentGameState = GameState.GameLoop;
                    break;
            }
        }

        private static void WaitingQuitConfirmation()
        {
            Console.WriteLine("Do you want to quit the game? [Y/N]");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Y:
                    Environment.Exit(0);
                    break;
                case ConsoleKey.N:
                    currentGameState = GameState.GameLoop;
                    break;
            }
        }

        private static void WaitingRestartOrQuit()
        {
            Console.WriteLine("Restart or quit the game? [R/Q]");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.R:
                    game.Restart();
                    currentGameState = GameState.GameLoop;
                    break;
                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}