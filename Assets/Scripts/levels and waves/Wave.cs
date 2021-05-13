using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Wave")]
public class Wave : ScriptableObject
{

    [SerializeField] Part[] parts;

    public Part [] GetParts()
    {
        return parts;
    }
}
