using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] Part config;
    List<Transform> waypoints;
       
    int wayPointIndex = 0;
    bool looping = false;

    // Start is called before the first frame update
    void Start()
    {

        looping = config.GetLooping();
        waypoints = config.GetWaypoints();
        transform.position = waypoints[wayPointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (looping)
        {
            MoveToWaypointsLoop();
        }
        else
        {
            MoveToWaypoints();
        }
        
    }

    public void SetWaveConfig(Part config)
    {
        this.config = config;
    }

    private void MoveToWaypoints()
    {
        if (wayPointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[wayPointIndex].transform.position;
            var movementThisFrame = config.GetmoveSpeed() * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                wayPointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void MoveToWaypointsLoop()
    {
        if (wayPointIndex <= waypoints.Count - 1)
        {

            MoveLoop(wayPointIndex);
        }
        else
        {
            MoveLoop(1);
        }
    }

    private void MoveLoop(int index)
    {
        var targetPosition = waypoints[index].transform.position;
        var movementThisFrame = config.GetmoveSpeed() * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

        if (transform.position == targetPosition)
        {
            if (index != waypoints.Count - 1)
            {
                wayPointIndex++;
            }
            else
            {
                wayPointIndex = 1;
            }

        }
    }
}
