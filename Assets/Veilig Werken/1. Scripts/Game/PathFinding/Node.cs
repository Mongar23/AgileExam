using UnityEngine;
using VeiligWerken.Tools;

namespace VeiligWerken.PathFinding
{
	/// <summary>
	///     The <see cref="Node" /> <c>class</c> is used as the base of the <see cref="PathFindingGrid" />. It inherits from
	///     the <see cref="IHeapItem{T}" /> interface so it can be in a <see cref="Heap{T}" /> collection.
	///     <para>Created by Mathias on 19-05-2021</para>
	/// </summary>
	public class Node : IHeapItem<Node>
	{
		/// <summary>
		///     Determines whether the node is walkable and can therefore be added to a path.
		/// </summary>
		public bool IsWalkable { get; }

		/// <summary>
		///     Total value of a node, calculated by adding <c>g-cost</c> and <c>h-cost</c>.
		/// </summary>
		public int FCost => GCost + HCost;

		/// <summary>
		///     The cost of the path from the start node.
		/// </summary>
		public int GCost { get; set; }

		/// <summary>
		///     The estimated value of the cheapest path cost to the end node.
		/// </summary>
		public int HCost { get; set; }

		/// <summary>
		///     Previous node in the path.
		/// </summary>
		public Node Parent { get; set; }

		/// <summary>
		///     Position in the unity world.
		/// </summary>
		public Vector2 WorldPosition { get; }

		/// <summary>
		///     Position in the <see cref="PathFindingGrid" />
		/// </summary>
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
			if (compare == 0) compare = HCost.CompareTo(other.HCost);

			// Compare returns 1 if the value is higher, in this case we want to return 1 if the node has a higher priority and thus a lower compare value.
			// So we need to invert the compare value and return it.
			return -compare;
		}
	}
}