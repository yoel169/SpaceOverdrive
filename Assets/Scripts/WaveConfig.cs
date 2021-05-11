using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")] 
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeedAddition = 0f;

    public List<Transform> GetWaypoints() 
    {
        var waypoints = new List<Transform>();

        //add each waypoint to waypoints var from prefab parent
        foreach(Transform child in pathPrefab.transform)
        {

            waypoints.Add(child);

        }

        return waypoints;
    }

    public GameObject GetEnemyPrefab(){ return enemy; }
    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }
    public float GetspawnRandomFactor() { return spawnRandomFactor; }

    public int GetnumberOfEnemies() { return numberOfEnemies; }
    public float GetmoveSpeed() {

        return moveSpeedAddition + enemy.GetComponent<Enemy>().GetMoveSpeed(); 
    
    }


}
