using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dfs : MonoBehaviour
{
    public Transform seeker; // Object seeking path
    public Transform target; // Target position
    public float movementSpeed = 5f; // Speed of movement

    Stack<Vector3> path; // Stack to store the path found by DFS

    void Start()
    {
        path = new Stack<Vector3>();
        FindPath();
    }

    void Update()
    {
        if (path.Count > 0 && seeker != null)
        {
            // Move the object towards the next point in the path
            seeker.position = Vector3.MoveTowards(seeker.position, path.Peek(), movementSpeed * Time.deltaTime);

            // Check if the object has reached the current point in the path
            if (Vector3.Distance(seeker.position, path.Peek()) < 0.01f)
            {
                path.Pop(); // Move to the next point in the path
            }
        }
    }

    void FindPath()
    {
        if (seeker == null || target == null)
        {
            Debug.LogError("Seeker or target not set!");
            return;
        }

        // Perform DFS to find the path
        DFS(seeker.position, target.position);
    }

    void DFS(Vector3 startPos, Vector3 targetPos)
    {
        Stack<Vector3> stack = new Stack<Vector3>();
        HashSet<Vector3> visited = new HashSet<Vector3>();
        Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();

        stack.Push(startPos);
        visited.Add(startPos);

        while (stack.Count > 0)
        {
            Vector3 current = stack.Pop();

            if (current == targetPos)
            {
                // Reconstruct the path
                ReconstructPath(cameFrom, startPos, targetPos);
                return;
            }

            foreach (Vector3 neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor))
                {
                    stack.Push(neighbor);
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        Debug.LogError("Path not found!");
    }

    void ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 start, Vector3 end)
    {
        Vector3 current = end;
        while (current != start)
        {
            path.Push(current);
            current = cameFrom[current];
        }
        path.Push(start);
    }

    List<Vector3> GetNeighbors(Vector3 position)
    {
        // Implement logic to get neighboring positions based on your grid system
        // For simplicity, let's assume a 2D grid with 4-connected neighbors

        List<Vector3> neighbors = new List<Vector3>();
        neighbors.Add(position + Vector3.up); // Up
        neighbors.Add(position + Vector3.down); // Down
        neighbors.Add(position + Vector3.right); // Right
        neighbors.Add(position + Vector3.left); // Left

        return neighbors;
    }
}
