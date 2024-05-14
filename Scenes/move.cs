using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public Transform target; // The position the object should move towards
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Calculate the direction to move towards the target
            Vector3 direction = (target.position - transform.position).normalized;

            // Calculate the distance between current position and target position
            float distance = Vector3.Distance(transform.position, target.position);

            // Move towards the target position using Lerp
            transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime / distance);
        }
        //    transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
    }
}
