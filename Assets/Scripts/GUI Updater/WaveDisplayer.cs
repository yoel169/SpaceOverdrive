using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class WaveDisplayer : MonoBehaviour
{
    TextMeshProUGUI text;

    int currentWave = 1;
    int maxWaves = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMaxWaves(int max)
    {
        currentWave = 1;
        maxWaves = max;
        text.text = currentWave.ToString() + "/" + maxWaves.ToString();
    }

    public void UpdateWave()
    {
        currentWave++;
        text.text = currentWave.ToString() + "/" + maxWaves.ToString();
    }
}
