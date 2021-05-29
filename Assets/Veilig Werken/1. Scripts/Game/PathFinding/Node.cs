using UnityEngine;
using VeiligWerken.Tools;

namespace VeiligWerken.PathFinding
{
    /// <summary>
    ///     <para>Created by Mathias on 19-05-2021</para>
    /// </summary>
    public class Node : IHeapItem<Node>
    {
        public bool IsWalkable { get; }

        public int FCost => GCost + HCost;
        public int GCost { get; set; }
        public int HCost { get; set; }
        public Node Parent { get; set; }
        public Vector2 WorldPosition { get; }
        public Vector2Int GridPosition { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Node" /> class with parameters.
        /// </summary>
        /// <param name="isWalkable">Determines whether the node is walkable and can therefore be added to a path.</param>
        /// <param name="worldPosition">Center of the node.</param>
        /// <param name="gridPosition">Position in the <see cref="PathFindingGrid" /> in ints.</param>
        public Node(bool isWalkable, Vector2 worldPosition, Vector2Int gridPosition)
        {
            IsWalkable = isWalkable;
            WorldPosition = worldPosition;
            GridPosition = gridPosition;
        }

        /// <summary>
        ///     Current place in the heap.
        /// </summary>
        public int HeapIndex { get; set; }

        /// <summary>
        ///     The <c>CompareTo</c> method compares the <see cref="Node" />'s priority in a <see cref="Heap{T}" />.
        ///     This is based on the <see cref="Node.FCost" /> and <see cref="Node.HCost" />.
        /// </summary>
        /// <param name="other">The <see cref="Node" /> that it has to be compared to.</param>
        /// <returns>
        ///     Returns <c>1</c> if it has a higher priority, <c>0</c> if the priority is the same and <c>-1</c> if it has a
        ///     lower priority.
        /// </returns>
        public int CompareTo(Node other)
        {
            // First compare the FCost between the nodes.
            int compare = FCost.CompareTo(other.FCost);

            // If both nodes have the same F-cost, take the H-cost in consideration.
            if(compare == 0) { compare = HCost.CompareTo(other.HCost); }

            // Compare returns 1 if the value is higher, in this case we want to return 1 if the node has a higher priority and thus a lower compare value.
            // So we need to invert the compare value and return it.
            return -compare;
        }
    }
}