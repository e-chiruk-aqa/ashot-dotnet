namespace AShotNet.Extentions
{
    using System.Collections.Generic;

    public static class CollectionExtention
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }

        public static bool IsEmpty<T>(this IList<T> collection)
        {
            return collection.Count == 0;
        }

        public static void AddAll<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                collection.Add(item);
            }
        }
    }
}
