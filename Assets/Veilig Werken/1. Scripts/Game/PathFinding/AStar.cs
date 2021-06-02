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
    public class AStar : Singleton<AStar>
    {
        private readonly Dictionary<int, List<Node>> paths = new Dictionary<int, List<Node>>();

        public PathFindingGrid Grid { get; private set; } = null;

        private int currentPathCost = int.MaxValue;
        private Transform playerTransform = null;

        protected override void Awake()
        {
            base.Awake();

            Grid = GetComponent<PathFindingGrid>();
            GameManager.Instance.PlayerSpawnedEvent += OnPlayerSpawned;
        }

        private void OnPlayerSpawned(Player player)
        {
            playerTransform = player.CachedTransform;

            //Find a path to each shelter.
            foreach (Shelter shelter in FindObjectsOfType<Shelter>())
            {
                Vector3 shelterPosition = shelter.transform.position;
                FindPath(playerTransform.position, shelterPosition);
                shelter.PathCost = currentPathCost;
            }

            // Draw best path.
            Grid.Path = paths.ValueFromLowestKey();
        }

        private void FindPath(Vector2 start, Vector2 destination)
        {
            // Determine the start and destination node based aon a world position.
            Node startNode = Grid.GetNodeFromWorldPoint(start);
            Node destinationNode = Grid.GetNodeFromWorldPoint(destination);

            // Set of open and closed nodes. And add start node to open set.
            var openNodes = new Heap<Node>(Grid.NodeCount);
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

                //
                foreach (Node neighbor in Grid.GetNeighbours(currentNode))
                {
                    if(!neighbor.IsWalkable || closedNodes.Contains(neighbor)) { continue; }

                    int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);

                    if(newMovementCostToNeighbor >= neighbor.GCost && openNodes.Contains(neighbor)) { continue; }

                    neighbor.GCost = newMovementCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, destinationNode);
                    neighbor.Parent = currentNode;

                    if(openNodes.Contains(neighbor)) { continue; }

                    openNodes.Add(neighbor);
                }
            }

            currentPathCost = int.MaxValue;
        }

        private void RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            Node currentNode = endNode;
            var totalPathCost = 0;

            while (currentNode != startNode)
            {
                totalPathCost += currentNode.FCost;
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            currentPathCost = totalPathCost;
            var pathPair = new KeyValuePair<int, List<Node>>(totalPathCost / path.Count, path);
            paths.Add(pathPair.Key, pathPair.Value);
        }

        private static int GetDistance(Node a, Node b)
        {
            // Calculate distance in nodes.
            var distance = new Vector2Int(Mathf.Abs(a.GridPosition.x - b.GridPosition.x), Mathf.Abs(a.GridPosition.y - b.GridPosition.y));

            // return distance with diagonal moves.
            return distance.x > distance.y ? 14 * distance.y + 10 * (distance.x - distance.y) : 14 * distance.x + 10 * (distance.y - distance.x);
        }
    }
}