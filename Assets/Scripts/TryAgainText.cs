using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TryAgainText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameSession session = FindObjectOfType<GameSession>();

        bool win = session.GetWin();

        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        if (win)
        {
            text.text = "Continue";
        }
        else
        {
            text.text = "Play Again";
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
