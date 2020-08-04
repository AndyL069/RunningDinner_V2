using System.Collections.Generic;

namespace RunningDinner.Extensions
{
    public static class CollectionExtensions
    {
        public static void PushRange<T>(this Stack<T> source, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                source.Push(item);
            }
        }

        public static void EnqueueRange<T>(this Queue<T> source, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                source.Enqueue(item);
            }
        }
    }
}
