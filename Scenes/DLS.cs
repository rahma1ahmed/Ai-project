using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLS : MonoBehaviour
{
    public Transform target; // The position the object should move towards
    public float moveSpeed = 1f; // Speed at which the object moves
    public float depthLimit = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DepthLimitedSearch(transform.position, 0);
    }
    private void DepthLimitedSearch(Vector3 currentPosition, int depth)
    {
        if (depth >= depthLimit)
        {
            // Reached depth limit, stop searching
            return;
        }

        Vector3 direction = (target.position - currentPosition).normalized;
        float distance = Vector3.Distance(currentPosition, target.position);

        // Move towards the target position
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Check if the object has reached the target position
        if (distance < 0.1f)
        {
            Debug.Log("Object reached the target!");
            return;
        }

        // Recursive call to continue searching
        DepthLimitedSearch(transform.position, depth + 1);
    }
}
