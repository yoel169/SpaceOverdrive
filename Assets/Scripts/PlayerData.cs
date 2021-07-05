
[System.Serializable]
public class PlayerData
{
    int slot;
    string playerName;

    //curent variables
    int currentLevel;
    int currentChapter;
    int currentShip;

    //counters
    int deathCounter;
    int totalEnemiesKilled;
    int [,] enemiesKilled; //balanced, speedy, fighter, bomber, orbital
    float playtime;

    //progression
    int chapterProgression;
    int levelProgression;
    int playeProgress;
    float [,] shipPogression; //play time, enemy kills, not used(for now)

    public PlayerData(int slotIndex, string playerName, int shipIndex)
    {
        slot = slotIndex;
        this.playerName = playerName;

        currentChapter = 1;
        currentShip = shipIndex;
        currentLevel = 1;
       
        deathCounter = 0;
        totalEnemiesKilled = 0;
        playtime = 0;
        enemiesKilled = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };

        chapterProgression = 1;
        levelProgression = 1;
        playeProgress = 0;
        shipPogression = new float[,] { { 0f, 0f, 0f }, { 0f, 0f, 0f }, { 0f, 0f, 0f } };
        
    }

    public int [] GetStats()
    {
        int[] currents = new int[] { currentChapter, chapterProgression, currentLevel, levelProgression, 
            currentShip, totalEnemiesKilled, playeProgress, deathCounter };

        return currents;
    }

    public float [,] GetShipProgression()
    {
        return shipPogression;
    }

    public void SetLevel(int lv)
    {
        currentLevel = lv;
    }

    public int GetLevel()
    {
        return currentLevel;
    }
    public int GetSlot()
    {
        return slot;
    }
    public void SetSlot(int slot)
    {
        this.slot = slot;
    }

    public string GetName()
    {
        return playerName;
    }

    //level, playtime, enemies killed
    public void OnGameUpdatePlayer(float [] update)
    {
        currentLevel =  (int) update[0];      
        playtime += update[1];
        totalEnemiesKilled += (int) update[2];

        shipPogression[currentShip, 0] = update[1];
        shipPogression[currentShip, 0] = update[2];

        if(currentLevel > levelProgression)
        {
            levelProgression = currentLevel;
        }
    }

    public void UpdateEnemiesKilled(int [,] killed)
    {
        enemiesKilled = killed;
    }

    public int[,] GetEnemiesKilled()
    {
        return enemiesKilled;
    }
}
