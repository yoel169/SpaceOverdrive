using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;
    int currentLevelIndex = 0;
    bool levelSet = false; //to avoid reseting the level every time, only needed once

    [Header("Player")]
    [SerializeField]  Player myPlayer;
    [SerializeField] GameObject playerPrefab;

    private void Awake()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        if (FindObjectsOfType<GameSession>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
       
    }

    private void Start()
    {
        SetRatio(9, 19);
    }


    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int score)
    {
        this.score += score;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    private void SetRatio(float w, float h)
    {
        if ((((float)Screen.width) / ((float)Screen.height)) > w / h)
        {
            Screen.SetResolution((int)(((float)Screen.height) * (w / h)), Screen.height, true);
        }
        else
        {
            Screen.SetResolution(Screen.width, (int)(((float)Screen.width) * (h / w)), true);
        }
    }

    public void SetPlayer(GameObject player)
    {
        myPlayer = player.GetComponent<Player>();
        playerPrefab = player;
    }

    public GameObject GetPlayerPreFab()
    {
        return playerPrefab;
    }

    public Player GetPlayer()
    {
        return myPlayer;
    }

    public void SetCurrentLevel(int level)
    {
        if(!levelSet)
        {
            currentLevelIndex = level;
            levelSet = true;

        }
        
    }

    public void ResetSesh()
    {

        score = 0;
        currentLevelIndex = 0;
    }

    public int GetCurrentLevel()
    {
        return currentLevelIndex + 1;
    }

    public void IncreaseLevel()
    {    
        currentLevelIndex ++;
        FindObjectOfType<LevelDisplay>().UpdateLevel();
    }
}
