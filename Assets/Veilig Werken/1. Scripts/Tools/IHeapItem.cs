using System;

namespace VeiligWerken.Tools
{
    /// <summary>
    /// When a class 
    /// <para>Created by Mathias on 20-05-2021</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHeapItem<in T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}