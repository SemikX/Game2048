namespace Game2048
{
    public record Game2048State(int[,] GameCells, int Score, int BestScore, GameStatus GameStatus);
}