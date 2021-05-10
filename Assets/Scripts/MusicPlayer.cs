using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {
        SetupSingleton();
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupSingleton()
    {
        if(FindObjectsOfType(GetType()).Length >1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
