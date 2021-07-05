using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class OverdriveText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;

    bool ready;

    // Start is called before the first frame update
    void Start()
    {
        ready = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle()
    {

        if (!ready)
        {
            text.color = Color.green;
            text.text = "ready";
        }
        else
        {
            text.color = Color.red;
            text.text = "Charge";
        }
    }
}
