using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bfs : MonoBehaviour
{
    public Transform targetPosition; // The position object you want to reach
    public float moveSpeed = 5f; // Speed of movement
    private List<Vector3> path; // List of positions representing the path
    private int currentWaypointIndex = 0;

    void Start()
    {
        // Calculate path using BFS
        path = BFSPathfinding(transform.position, targetPosition.position);
    }

    void Update()
    {
        // If path is calculated
        if (path != null && path.Count > 0)
        {
            // Move towards the current waypoint
            Vector3 currentWaypoint = path[currentWaypointIndex]; 
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);

            // If close enough to the current waypoint, move to the next one
            if (Vector3.Distance(transform.position, currentWaypoint) < 0.1f)
            {
                currentWaypointIndex++;
                // If reached the end of the path, stop moving
                if (currentWaypointIndex >= path.Count)
                {
                    Debug.Log("Object reached the target position.");
                    enabled = false; // Disable ObjectMover script
                }
            }
        }
    }

    // Breadth-first search for pathfinding
    List<Vector3> BFSPathfinding(Vector3 startPos, Vector3 targetPos)
    {
        List<Vector3> path = new List<Vector3>();

        // Queue for BFS traversal
        Queue<Vector3> queue = new Queue<Vector3>();
        queue.Enqueue(startPos);

        // Dictionary to store parent positions for path reconstruction
        Dictionary<Vector3, Vector3> parentMap = new Dictionary<Vector3, Vector3>();
        parentMap[startPos] = startPos;

        // Visited set to avoid revisiting positions
        HashSet<Vector3> visited = new HashSet<Vector3>();
        visited.Add(startPos);

        // BFS traversal
        while (queue.Count > 0)
        {
            Vector3 currentPos = queue.Dequeue();

            // If target position is found, reconstruct path and return
            if (currentPos == targetPos)
            {
                Vector3 backtrackPos = targetPos;
                while (backtrackPos != startPos)
                {
                    path.Insert(0, backtrackPos); // Insert at the beginning for correct order
                    backtrackPos = parentMap[backtrackPos];
                }
                path.Insert(0, startPos); // Insert start position
                return path;
            }

            // Get neighbors of current position (assuming 4-connected grid)
            Vector3[] neighbors = {
                currentPos + Vector3.forward,
                currentPos - Vector3.forward,
                currentPos + Vector3.right,
                currentPos - Vector3.right
            };

            foreach (Vector3 neighbor in neighbors)
            {
                // If neighbor is walkable and not visited
                if (!visited.Contains(neighbor) && IsPositionWalkable(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    parentMap[neighbor] = currentPos;
                }
            }
        }

        // If no path is found
        Debug.LogWarning("No valid path found.");
        return path;
    }

    // Check if a position is walkable (you can implement your own logic here)
    bool IsPositionWalkable(Vector3 position)
    {
        // Example: Check if position is within bounds and not colliding with obstacles
        return true;
    }

}
