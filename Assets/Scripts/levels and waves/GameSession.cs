using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;
    bool win = false;
    int currentLevel = 1;

    [Header("Player")]
    
    [SerializeField] GameObject selectedPlayerPrefab;
    int playerIndex = 0;

    Player spawnedPlayer;
    GameObject SpawnedPlayerObject;

    //[SerializeField] Chapter [] chapterPack;

    //Level level;

    //PlayerData playerData;
   // int playerSlot;

   // int totalEnemiesKilled;
   // int[,] enemiesKilled;
   // int[] stats;

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
       // totalEnemiesKilled = 0;
    }


    public int GetScore()
    {
        return score;
    }

    //update score and enemies killed
    public void AddToScore(int data)
    {
        //print("sending score to player "+ data);
       score += data;
       spawnedPlayer.IncreaseSuper(data);

       // totalEnemiesKilled++;
        //enemiesKilled[data[1], data[2]]++;
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

    public void SetSelectedPlayer(GameObject player)
    {
        selectedPlayerPrefab = player;

        if (player.CompareTag("Normal")){
            playerIndex = 0;
        }else if (player.CompareTag("Speedy"))
        {
            playerIndex = 1;
        }else if (player.CompareTag("Fighter"))
        {
            playerIndex = 2;
        }
    }

    public void SetSpanwedPlayer(GameObject playerObject)
    {
        SpawnedPlayerObject = playerObject;
        spawnedPlayer = playerObject.GetComponent<Player>();
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public GameObject GetPlayerPreFab()
    {
        return selectedPlayerPrefab;
    }

    public Player GetPlayer()
    {
        return spawnedPlayer;
    }

    public void SetCurrentLevel(int lv)
    {
        currentLevel = lv;    
        //level = chapterPack[stats[0] - 1].GetLevels()[currentLevel - 1];      
    }

    //public void SetLevel(Level lv)
    //{
    //    level = lv;
    //}

    //public void SetPlayerData(PlayerData player)
    //{
    //    playerData = player;
    //    stats = playerData.GetStats();
    //    enemiesKilled = playerData.GetEnemiesKilled();
    //}

    //public PlayerData GetPlayerData()
    //{
    //    return playerData;
    //}

    public void ResetSesh()
    {

        score = 0;
        currentLevel = 1;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    //public Level GetLevel()
    //{
    //    return level;

    //}

    public void IncreaseLevel()
    {    
        currentLevel ++;
        //level = chapterPack[stats[0] - 1].GetLevels()[currentLevel - 1];
        //FindObjectOfType<LevelDisplay>().UpdateLevel();
    }

    public void Win()
    {
        win = true;
        //currentLevel++;
       // level = chapterPack[stats[0]].GetLevels()[currentLevel - 1];
        //SavePlayer(time);
        //totalEnemiesKilled = 0;
    }

    public void Lose()
    {
        win = false;
    }

    public bool GetWin()
    {
        return win;
    }

    //public void SetSlot(int slot)
    //{
    //    playerSlot = slot;
    //}

    public void SavePlayer(float playtime)
    {
        
       // playerData.UpdateEnemiesKilled(enemiesKilled);

        //playerData.OnGameUpdatePlayer(new float[] { currentLevel, playtime, totalEnemiesKilled });
    }
}
