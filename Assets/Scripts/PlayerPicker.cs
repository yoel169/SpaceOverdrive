using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPicker : MonoBehaviour
{

    [Header("Player Prefabs")]
    [SerializeField] GameObject [] playerPrefabs;

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
        session.SetPlayer(playerPrefabs[index]);
        sceneSelector.LoadGameScene();
    }
}
