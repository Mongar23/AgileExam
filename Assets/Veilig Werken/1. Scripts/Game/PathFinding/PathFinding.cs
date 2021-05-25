using System.Collections.Generic;
using MBevers;
using UnityEngine;
using VeiligWerken.Tools;

namespace VeiligWerken.PathFinding
{
    /// <summary>
    ///     This class finds a the fastest path from a to b using the grid made in the <see cref="PathFindingGrid" /> class.
    ///     The path is found using the A* algorithm. As a guidance for this, I used
    ///     <see href="https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW">this</see> video series.
    ///     <para>Created by Mathias on 19-05-2021</para>
    /// </summary>
    [RequireComponent(typeof(PathFindingGrid))]
    public class PathFinding : ExtendedMonoBehaviour
    {
        [SerializeField] private Transform destinationTransform;
        private PathFindingGrid grid = null;

        private Transform playerTransform;

        private void Awake()
        {
            grid = GetComponent<PathFindingGrid>();
            playerTransform = GameManager.Instance.Player.CachedTransform;
        }

        private void Update()
        {
            if(playerTransform == null && destinationTransform == null) { return; }

            FindPath(playerTransform.position, destinationTransform.position);
        }

        private void FindPath(Vector2 start, Vector2 destination)
        {
            // Determine the start and destination node based aon a world position.
            Node startNode = grid.GetNodeFromWorldPoint(start);
            Node destinationNode = grid.GetNodeFromWorldPoint(destination);

            // Set of open and closed nodes. And add start node to open set.
            var openNodes = new Heap<Node>(grid.NodeCount);
            var closedNodes = new HashSet<Node>();
            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                // Remove the item with the highest priority from the heap and add it to the closed nodes.
                Node currentNode = openNodes.RemoveFirst();
                closedNodes.Add(currentNode);

                //if the current node is the destination node a path as been found.
                if(currentNode == destinationNode)
                {
                    RetracePath(startNode, destinationNode);
                    return;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if(!neighbour.IsWalkable || closedNodes.Contains(neighbour)) { continue; }

                    int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);

                    if(newMovementCostToNeighbour >= neighbour.GCost && openNodes.Contains(neighbour)) { continue; }

                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, destinationNode);
                    neighbour.Parent = currentNode;

                    if(openNodes.Contains(neighbour)) { continue; }

                    openNodes.Add(neighbour);
                }
            }
        }

        private void RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            grid.Path = path;
        }

        private int GetDistance(Node a, Node b)
        {
            // Calculate distance in nodes.
            var distance = new Vector2Int(Mathf.Abs(a.GridPosition.x - b.GridPosition.x), Mathf.Abs(a.GridPosition.y - b.GridPosition.y));

            // return distance with diagonal moves.
            return distance.x > distance.y ? 14 * distance.y + 10 * (distance.x - distance.y) : 14 * distance.x + 10 * (distance.y - distance.x);
        }
    }
}