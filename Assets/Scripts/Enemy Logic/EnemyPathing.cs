using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] Part config;
    List<Transform> waypoints;
   
    int wayPointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = config.GetWaypoints();
        transform.position = waypoints[wayPointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToWaypoints();
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
}
