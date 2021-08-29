using System.Linq;

namespace Game2048
{
    public static class ArrayExtensions
    {
        public static bool SequenceEquals<T>(this T[,] a, T[,] b)
        {
            return a.Rank == b.Rank
                   && Enumerable.Range(0, a.Rank).All(dimension => a.GetLength(dimension) == b.GetLength(dimension))
                   && a.Cast<T>().SequenceEqual(b.Cast<T>());
        }
    }
}