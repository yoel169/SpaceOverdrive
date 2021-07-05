using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerPicker : MonoBehaviour
{

    [Header("Player Prefabs")]
    [SerializeField] GameObject [] playerPrefabs;
    
    //[SerializeField] TMP_InputField nameInput;
    GameSession session;
    SceneSelector sceneSelector;

    // Start is called before the first frame update
    void Start()
    {
        session = FindObjectOfType<GameSession>();
        sceneSelector = FindObjectOfType<SceneSelector>();
    }

    /**
     * Select who the player will be. Provide the index to the player prefab.
     */
    public void OnPlayerSelect(int index)
    {
        //get player name and print it
        //string name = nameInput.text;
        //print(name);

        //GameObject selectedShip = playerPrefabs[index];

        //create new player data and save it
        // PlayerData player = new PlayerData(index, name, selectedShip.GetComponent<Player>().GetShipType());
        //DataSaver.SavePlayer(player);

        //print("player saved");

        //get the ship prefab selected and load game scene
        //session.SetPlayerData(player);
        //session.SetCurrentLevel(1);

        session.SetSelectedPlayer(playerPrefabs[index]);
        sceneSelector.LoadGameScene();

        print("player selected ship.");

        //if (PhotonNetwork.IsConnected)
        //{
        //    PhotonNetwork.LoadLevel("M Game");
        //}
        //else
        //{
        //    sceneSelector.LoadGameScene();
        //}
        
    }
}
