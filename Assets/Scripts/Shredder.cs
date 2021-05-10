using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
