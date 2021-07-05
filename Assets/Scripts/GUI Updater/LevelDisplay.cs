using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    int lv = 1;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();  
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void UpdateLevel()
    {
       

        try
        {
            lv++;
            scoreText.text = lv.ToString();
        }
        catch
        {
            scoreText = GetComponent<TextMeshProUGUI>();
        }
       
    }
}
