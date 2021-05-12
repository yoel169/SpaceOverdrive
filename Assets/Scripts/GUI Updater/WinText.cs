using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameSession session = FindObjectOfType<GameSession>();

        bool win = session.GetWin();

        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        if (win)
        {
            text.text = "You Win";
        }
        else
        {
            text.text = "Game Over";
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
