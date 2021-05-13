using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpritePicker : MonoBehaviour
{
    [SerializeField] Sprite[] lifeSprites;
    Player player;
    int lives;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        if(player != null)
        {
            SetSprite();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = FindObjectOfType<Player>();
        }
        else
        {
            if (lives != player.GetCurrentLives())
            {
                SetSprite();
            }
        }
    }


    public void SetSprite()
    {
        lives =  player.GetCurrentLives();
        GetComponent<UnityEngine.UI.Image>().sprite = lifeSprites[lives];
    }
}
