using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLoaderScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI [] slotText;

    PlayerData[] playerData;
    GameSession session;

    // Start is called before the first frame update
    void Start()
    {

        playerData = new PlayerData[3];

        session = FindObjectOfType<GameSession>();

        for(int x = 0; x < slotText.Length; x++)
        {
            PlayerData temp = DataSaver.LoadPlayer(x);

            if (temp != null)
            {
                playerData[x] = temp;
                slotText[x].text = temp.GetName();
            }
            else
            {
                slotText[x].text = "Create";
            }
        }  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(int slot)
    {

        if(playerData[slot] != null)
        {
           // session.SetPlayerData(playerData[slot]);
            FindObjectOfType<SceneSelector>().LoadPlayerHUb();
        }
        else
        {
          //  session.SetSlot(slot);
            FindObjectOfType<SceneSelector>().LoadPickPlayer();
        }

       
    }
}
