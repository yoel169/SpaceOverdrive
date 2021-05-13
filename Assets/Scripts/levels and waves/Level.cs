using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Create Level")]
public class Level : ScriptableObject
{
    [SerializeField] Wave [] waves;
    [SerializeField] float timeBetweenWaves;

    public Wave [] GetWaves()
    {
        return waves;
    }

    public float GetTimeBetweenWaves()
    {
        return timeBetweenWaves;
    }
}
