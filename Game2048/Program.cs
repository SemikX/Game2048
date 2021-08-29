using System;

namespace Game2048
{
    public class Program
    {
        private static Game2048 game;
        private static PlayerInputControlState currentInputControlState;

        public static void Main()
        {
            game = new Game2048();
            game.Victory += (_, _) => currentInputControlState = PlayerInputControlState.RestartOrQuit;
            game.Defeat  += (_, _) => currentInputControlState = PlayerInputControlState.RestartOrQuit;
            game.Restart();

            currentInputControlState = PlayerInputControlState.GameLoop;

            while (true)
            {
                Console.Clear();

                game.DrawToConsole();

                switch (currentInputControlState)
                {
                    case PlayerInputControlState.GameLoop:
                        UpdateGameLoop(game);
                        break;
                    case PlayerInputControlState.RestartConfirmation:
                        WaitingRestartConfirmation(game);
                        break;
                    case PlayerInputControlState.QuitConfirmation:
                        WaitingQuitConfirmation();
                        break;
                    case PlayerInputControlState.RestartOrQuit:
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
                    currentInputControlState = PlayerInputControlState.RestartConfirmation;
                    break;
                case ConsoleKey.Q:
                    currentInputControlState = PlayerInputControlState.QuitConfirmation;
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
                    currentInputControlState = PlayerInputControlState.GameLoop;
                    break;
                case ConsoleKey.N:
                    currentInputControlState = PlayerInputControlState.GameLoop;
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
                    currentInputControlState = PlayerInputControlState.GameLoop;
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
                    currentInputControlState = PlayerInputControlState.GameLoop;
                    break;
                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}