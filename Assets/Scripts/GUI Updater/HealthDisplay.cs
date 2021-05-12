
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if(player != null)
        {
            scoreText.text = player.GetHealth().ToString();
        }
        else
        {
            player = FindObjectOfType<Player>();
        }
    }
}
