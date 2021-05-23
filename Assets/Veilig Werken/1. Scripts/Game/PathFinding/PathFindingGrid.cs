using System.Collections.Generic;
using MBevers;
using UnityEngine;

namespace VeiligWerken.PathFinding
{
    /// <summary>
    ///     This class handles the <see cref="Node" /> grid which used as the base of the A* algorithm.
    ///     <para>Created by Mathias on 19-05-2021</para>
    /// </summary>
    public class PathFindingGrid : ExtendedMonoBehaviour
    {
        private const float MAX_RAY_DISTANCE = 166.0f;

        [SerializeField] private bool onlyDrawPath;
        [SerializeField] private LayerMask unwalkableMask;
        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField] private float nodeRadius;

        public List<Node> Path { get; set; }
        public int nodeCount => gridSize.x * gridSize.y;

        private float nodeDiameter = 0;
        private Node[,] grid;
        private Vector2Int gridSize = new Vector2Int();

        private void Start()
        {
            nodeDiameter = nodeRadius * 2;

            // Calculate grid size.
            int x = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            int y = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            gridSize = new Vector2Int(x, y);

            CreateGrid();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(CachedTransform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

            if(onlyDrawPath)
            {
                if(Path == null) { return; }

                Gizmos.color = Color.green;
                foreach (Node node in Path) { Gizmos.DrawCube(node.WorldPosition, Vector3.one * (nodeDiameter - 0.1f)); }

                return;
            }

            if(grid == null) { return; }

            Node playerNode = GetNodeFromWorldPoint(GameManager.Instance.Player.CachedTransform.position);
            foreach (Node node in grid)
            {
                Gizmos.color = node.IsWalkable ? node == playerNode ? Color.cyan : new Color(0, 0, 0, 0) : Color.red;

                if(Path != null && Path.Contains(node)) { Gizmos.color = Color.green; }

                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }

        private void CreateGrid()
        {
            grid = new Node[gridSize.x, gridSize.y];
            Vector3 bottomLeft = CachedTransform.position - Vector3.right * gridWorldSize.x * 0.5f - Vector3.up * gridWorldSize.y * 0.5f;

            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    // Calculate the center of the node in the world position.
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);

                    // Add the node to the grid.
                    grid[x, y] = new Node(IsNodeWalkable(worldPoint), worldPoint, new Vector2Int(x, y));
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if(x == 0 && y == 0) { continue; }

                    var gridPosition = new Vector2Int(node.GridPosition.x + x, node.GridPosition.y + y);

                    if(gridPosition.x < 0 || gridPosition.x >= gridSize.x || gridPosition.y < 0 || gridPosition.y >= gridSize.y) { continue; }

                    neighbours.Add(grid[gridPosition.x, gridPosition.y]);
                }
            }

            return neighbours;
        }

        public Node GetNodeFromWorldPoint(Vector2 worldPoint)
        {
            // Calculate how many percent the worldPoint is off the lower left corner of the grid.
            float percentX = Mathf.Clamp01((worldPoint.x + gridWorldSize.x * 0.5f) / gridWorldSize.x);
            float percentY = Mathf.Clamp01((worldPoint.y + gridWorldSize.y * 0.5f) / gridWorldSize.y);

            // Based of the percent calculate the index on the x and y of the grid.
            int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
            int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);

            // return the node which covers the worldPoint.
            return grid[x, y];
        }

        private bool IsNodeWalkable(Vector3 nodeCenter)
        {
            // Check if the node collides with an object in the unwalkable mask.
            Collider2D checkCircle = Physics2D.OverlapCircle(nodeCenter, nodeRadius, unwalkableMask);

            // If it collided with an object in the unwalkable mask, return false.
            if(checkCircle != null) { return false; }

            // Convert the wind direction from degrees to radian. Then convert the radian to a vector 2.
            float windDirectionInRad = -GameManager.Instance.WindDirection * Mathf.Deg2Rad;
            var windDirection = new Vector2(Mathf.Cos(windDirectionInRad), Mathf.Sin(windDirectionInRad));

            // Return true when the raycast has hit an object int the unwalkable mask, which in this case are only walls.
            return Physics2D.Raycast(nodeCenter, windDirection, MAX_RAY_DISTANCE, unwalkableMask);;
        }
    }
}