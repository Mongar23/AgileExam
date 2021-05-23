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
        public int GCost { get; set; }
        public int HCost { get; set; }
        public Node Parent { get; set; }
        public Vector2 WorldPosition { get; }
        public Vector2Int GridPosition { get; }

        private int FCost => GCost + HCost;

        public Node(bool isWalkable, Vector2 worldPosition, Vector2Int gridPosition)
        {
            IsWalkable = isWalkable;
            WorldPosition = worldPosition;
            GridPosition = gridPosition;
        }

        public int HeapIndex { get; set; }

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