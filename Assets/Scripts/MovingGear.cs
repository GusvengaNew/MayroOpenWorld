using UnityEngine;
using System.Collections;

public class MovingGear : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints
    public float speed = 5.0f; // Speed of movement
    public float waitTime = 2.0f; // Time to wait at each waypoint

    private int currentWaypointIndex = 0;
    private bool movingForward = true;
    private bool isWaiting = false;

    void Update()
    {
        if (!isWaiting)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            transform.position = targetWaypoint.position;
            StartCoroutine(WaitAtWaypoint());
            UpdateWaypointIndex();
        }
        else
        {
            transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }

    void UpdateWaypointIndex()
    {
        if (movingForward)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = waypoints.Length - 2;
                movingForward = false;
            }
        }
        else
        {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0)
            {
                currentWaypointIndex = 1;
                movingForward = true;
            }
        }
    }
}
