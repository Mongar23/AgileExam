using System;

namespace VeiligWerken.Tools
{
    /// <summary>
    ///     In case a class has to be collected in a <see cref="Heap{T}" /> it should implement this interface. This interface
    ///     inherits from <see cref="IComparable{T}" />.
    ///     <para>Created by Mathias on 20-05-2021</para>
    /// </summary>
    /// <typeparam name="T">The class that should implement the <see cref="IHeapItem{T}" /> interface.</typeparam>
    public interface IHeapItem<in T> : IComparable<T>
    {
        /// <summary>
        ///     The <see cref="IHeapItem{T}" />'s current position in the heap.
        /// </summary>
        int HeapIndex { get; set; }
    }
}