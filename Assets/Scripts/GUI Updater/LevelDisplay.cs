using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    GameSession session;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        session = FindObjectOfType<GameSession>();
        scoreText.text = session.GetCurrentLevel().ToString();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void UpdateLevel()
    {
        scoreText.text = session.GetCurrentLevel().ToString();
    }
}
