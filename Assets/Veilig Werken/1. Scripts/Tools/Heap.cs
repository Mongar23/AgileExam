namespace VeiligWerken.Tools
{
    /// <summary>
    ///     This class is a array-based generic min-heap data-structure. To learn more about heaps,
    ///     <see href="https://www.tutorialspoint.com/data_structures_algorithms/heap_data_structure.htm">this</see> is a
    ///     simple article on heaps.
    ///     <para>Created by Mathias on 20-05-2021</para>
    /// </summary>
    public class Heap<T> where T : IHeapItem<T>
    {
        /// <summary>
        ///     Current amount of <see cref="IHeapItem{T}" />s in the <see cref="Heap{T}" />
        /// </summary>
        public int Count { get; private set; }

        private readonly T[] items;

        public Heap(int maxHeapSize) { items = new T[maxHeapSize]; }

        /// <summary>
        ///     Add a item to the heap.
        /// </summary>
        /// <param name="item">Item that needs to be added.</param>
        public void Add(T item)
        {
            // Place the newly added item at the end of the heap. And set its index to the last item of the heap.
            item.HeapIndex = Count;
            items[Count] = item;

            // Find its correct place in the heap.
            SortUp(item);

            // Increase the item count by one.
            Count++;
        }

        /// <summary>
        ///     Remove the first item in the heap and return the removed item.
        /// </summary>
        /// <returns>The first and thus removed item.</returns>
        public T RemoveFirst()
        {
            // Save the first item and decrease the current item count.
            T first = items[0];
            Count--;

            // Replace the first item with the last item in the heap.
            items[0] = items[Count];
            items[0].HeapIndex = 0;

            // Find its correct place in the heap.
            SortDown(items[0]);

            // Return the removed item.
            return first;
        }

        public void UpdateItem(T item) { SortUp(item); }

        
        public bool Contains(T item) => Equals(items[item.HeapIndex], item);

        private void SortDown(T item)
        {
            while (true)
            {
                // Get the children's index of the heap item.
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;

                // If the item has to be swapped this value will contain the highest priority child's index.
                var swapIndex = 0;

                // Break out of the loop when the left child's index is outside the array bounds.
                if(childIndexLeft >= Count) { return; }

                // Set the swap index to the left child's index.
                swapIndex = childIndexLeft;

                // Check if the right child's index is in the array bounds.
                if(childIndexRight < Count)
                {
                    // Check if the right child has an higher priority, then set the swap index to the right child's index.
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) { swapIndex = childIndexRight; }
                }

                // Break out of the loop when the item has a similar or higher priority than its highest priority child.
                if(item.CompareTo(items[swapIndex]) >= 0) { return; }

                // Swap places with the highest priority child.
                Swap(item, items[swapIndex]);
            }
        }

        private void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                // Get the parent item.
                T parentItem = items[parentIndex];

                // If the item has an equal or lower priority than its parent, beak the loop.
                if(item.CompareTo(parentItem) <= 0) { break; }

                // Swap item with its parent. And calculate its new parent's index.
                Swap(item, parentItem);
                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        private void Swap(T a, T b)
        {
            // Swap items in the array.
            items[a.HeapIndex] = b;
            items[b.HeapIndex] = a;

            // Swap their indexes. 
            int temp = a.HeapIndex;
            a.HeapIndex = b.HeapIndex;
            b.HeapIndex = temp;
        }
    }
}