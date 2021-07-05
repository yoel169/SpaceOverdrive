using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MGame : MonoBehaviourPunCallbacks
{
    [Header("Levels and waves")]
    [SerializeField] Chapter[] chapterPack;
    [SerializeField] int startingChapter = 1;
    [SerializeField] int startingLevel = 1;

    //[SerializeField] bool looping = false;

    [Header("Audios")]
    [SerializeField] AudioClip nextLevelAudio;
    [SerializeField] AudioClip winAudio;
    [SerializeField] AudioClip spawnAudio;
    [SerializeField] AudioClip livesUp;

    GameSession session;
    GameObject enemies;
    Player player;

    //counters
    int partCounter = 0;
    float healthCounter;
    int livesCounter;

    WaveDisplayer waveDisp;
    LevelDisplay lvDisp;

    //int playersJoined = 0;
    bool gameStarted = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("Lobby");
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        waveDisp = FindObjectOfType<WaveDisplayer>();
        lvDisp = FindObjectOfType<LevelDisplay>();
        session = FindObjectOfType<GameSession>();
        enemies = GameObject.Find("Enemies");
        yield return null; // StartCoroutine(SpawnPlayer());

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        
        print("Starting game!");
        
        StartCoroutine(SpawnPlayer(PhotonNetwork.IsMasterClient));      
        
    }

    //spawn all chapters of the game
    private IEnumerator SpawnChapters()
    {
        for (int levelIndex = startingChapter - 1; levelIndex < chapterPack.Length; levelIndex++)
        {
            var currentChapter = chapterPack[levelIndex];

            yield return StartCoroutine(SpawnLevels(currentChapter));

        }

        print("All chapters played");

        AudioSource.PlayClipAtPoint(winAudio, Camera.main.transform.position);

        yield return new WaitForSeconds(2);

        print("played win jingle");

        //session.Win();

        FindObjectOfType<SceneSelector>().LoadGameOver();

        print("loading end screen");
    }

    //spawm all levels from a chapter
    private IEnumerator SpawnLevels(Chapter chapter)
    {

        var levels = chapter.GetLevels();
        var timeBetweenLevels = chapter.GetTimeBetweenLevels();

        for (int levelIndex = startingLevel - 1; levelIndex < levels.Length; levelIndex++)
        {
            var currentLevel = levels[levelIndex];

            livesCounter = player.GetCurrentLives();
            healthCounter = player.GetHealth();

            yield return StartCoroutine(SpawnWaves(currentLevel));

            AudioSource.PlayClipAtPoint(nextLevelAudio, Camera.main.transform.position);

            yield return DetectEnemies(timeBetweenLevels);

            if (levelIndex + 1 != levels.Length)
            {
                session.IncreaseLevel();
                lvDisp.UpdateLevel();
            }

            if (player.GetCurrentLives() >= livesCounter && player.GetHealth() >= healthCounter)
            {
                player.IncreaseLives();
                print("Player won a life!");
                AudioSource.PlayClipAtPoint(livesUp, Camera.main.transform.position);
            }

        }

        print("All levels done for chapter");

    }

    //spawn all waves
    private IEnumerator SpawnWaves(Level currentLevel)

    {
        var waves = currentLevel.GetWaves();

        waveDisp.SetMaxWaves(waves.Length);

        var timeBetweenWaves = currentLevel.GetTimeBetweenWaves();

        for (int waveIndex = 0; waveIndex < waves.Length; waveIndex++)
        {
            var currentWave = waves[waveIndex];

            yield return StartCoroutine(SpawnParts(currentWave));

            yield return StartCoroutine(DetectEnemies(timeBetweenWaves));

            waveDisp.UpdateWave();
        }

        print("All waves done for this level");

    }

    //spawn all rounds of enemies in a wave
    private IEnumerator SpawnParts(Wave wave)

    {
        var parts = wave.GetParts();

        for (int waveIndex = 0; waveIndex < parts.Length; waveIndex++)
        {
            var currentPart = parts[waveIndex];

            yield return new WaitForSeconds(currentPart.GetDelay());

            StartCoroutine(SpawnEnemies(currentPart));

            print("part spawned");
        }

       // yield return StartCoroutine(WaitForParts(parts.Length));

    }

    private IEnumerator WaitForParts(int numOfParts)
    {

        while (partCounter != numOfParts)
        {
            if (partCounter >= numOfParts)
            {
                break;
            }
            yield return null;
        }

        print("All parts done for this level");
        partCounter = 0;
        yield return null;
    }

    private IEnumerator DetectEnemies(float time)
    {

       // PhotonNetwork.FindGameObjectsWithComponent<>;

        while (enemies.transform.childCount > 0)
        {
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(time);
    }


    //spawn all enemies in a round
    private IEnumerator SpawnEnemies(Part config)
    {

        for (int enemyCount = 0; enemyCount < config.GetnumberOfEnemies(); enemyCount++)
        {
            var newEnemy = PhotonNetwork.Instantiate(config.GetEnemyPrefab().name,
            config.GetWaypoints()[0].transform.position,
            Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(config);

            yield return new WaitForSeconds(config.GetTimeBetweenSpawns());
        }
        partCounter++;
    }

    private void SetLevel()
    {
        session.SetCurrentLevel(startingLevel);
        FindObjectOfType<LevelDisplay>().UpdateLevel();
    }

    private IEnumerator SpawnPlayer(bool master)
    {

        if (session.GetPlayerPreFab() != null)
        {
            print("spawning player");

            yield return new WaitForSeconds(0.5f);

            GameObject playero;

            if (master)
            {
                playero = PhotonNetwork.Instantiate(session.GetPlayerPreFab().name, new Vector3(0f, -2f, 1f), Quaternion.identity);
            }
            else{

                playero = PhotonNetwork.Instantiate(session.GetPlayerPreFab().name, new Vector3(0f, 2f, 1f), Quaternion.identity);
            }      

            player = playero.GetComponent<Player>();

            print("Player Spawned!");

            AudioSource.PlayClipAtPoint(spawnAudio, Camera.main.transform.position);

            yield return new WaitForSeconds(0.5f);

            if (master)
            {
                yield return StartCoroutine(SpawnChapters());
            }
            
        }
        else
        {
            print("player prefab null!");
        }
    }
}
