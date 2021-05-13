using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Chapter")]
public class Chapter : ScriptableObject
{
    [SerializeField] Level[] levels;
    [SerializeField] float timeBetweenLevels;
    public Level[] GetLevels(){
        return levels;
        }

    public float GetTimeBetweenLevels()
    {
        return timeBetweenLevels;
    }
}
