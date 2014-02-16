using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttViewer.ViewModels
{
    /// <summary>
    /// Helper class.
    /// </summary>
    static class CollectionHelper
    {
        /// <summary>
        /// Extension method for adding items to an IList in correct order.
        /// </summary>
        /// <typeparam name="T">Type of the item beeing added.</typeparam>
        /// <param name="list">IList, the item is being added.</param>
        /// <param name="item">Item to add.</param>
        /// <param name="comparer">Comparer to determine the sequence.</param>
        public static void AddSorted<T>(this IList<T> list, T item, IComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            int i = 0;
            while (i < list.Count && comparer.Compare(list[i], item) < 0)
                i++;

            list.Insert(i, item);
        }
    }
}
