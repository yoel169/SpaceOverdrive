using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpriteSelector : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Sprite[] lifeSprites;
    GameSession sesh;

    void Start()
    {
        sesh = FindObjectOfType<GameSession>();

        if(sesh != null)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = lifeSprites[sesh.GetPlayerIndex()];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
