using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySpawner : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField] AudioClip winAudio;
    [SerializeField] AudioClip spawnAudio;

    GameSession session;
    GameObject enemies; //hold all enemies
    WaveDisplayer waveDisplayer;
    Player player;

    //counters
    int partCounter = 0;
    float timeCounter;
    //int experienceCounter;

    // Start is called before the first frame update
    void Start()
    {
        //get session object and enemy parent
        session = FindObjectOfType<GameSession>();
        enemies = GameObject.Find("Enemies");

        //get wave displayer and set max waves
        waveDisplayer = FindObjectOfType<WaveDisplayer>();

        StartCoroutine(SpawnPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
    }
    private IEnumerator SpawnPlayer()
    {

        if (session.GetPlayerPreFab() != null)
        {

            //SetLevel();
            FindObjectOfType<LevelDisplay>().UpdateLevel();

            yield return new WaitForSeconds(0.5f);

            GameObject playero = Instantiate(session.GetPlayerPreFab(), new Vector3(0f, -7f, 1f), Quaternion.identity);

            player = playero.GetComponent<Player>();

            print("Player Spawned!");

            AudioSource.PlayClipAtPoint(spawnAudio, Camera.main.transform.position);

            yield return new WaitForSeconds(0.5f);

            yield return StartCoroutine(SpawnLevel());
        }
    }

    private IEnumerator SpawnLevel()
    {
       // var level = session.GetLevel();

        //yield return StartCoroutine(SpawnWaves(level));

        print("Level Complete.");

        AudioSource.PlayClipAtPoint(winAudio, Camera.main.transform.position);

        yield return new WaitForSeconds(2);

        print("played win jingle");

        session.Win();

        FindObjectOfType<SceneSelector>().LoadGameOver();

        print("loading end screen");
    }

    private IEnumerator SpawnWaves(Level currentLevel)

    {
        var waves = currentLevel.GetWaves();
        var timeBetweenWaves = currentLevel.GetTimeBetweenWaves();

        for (int waveIndex = 0; waveIndex < waves.Length; waveIndex++)
        {
            var currentWave = waves[waveIndex];

            yield return StartCoroutine(SpawnParts(currentWave));

            yield return StartCoroutine(DetectEnemies(timeBetweenWaves));

            if(waveIndex != waves.Length - 1)
            {
                UpdateWaveCounter();
            }
          
        }

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

        yield return StartCoroutine(WaitForParts(parts.Length));

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
            var newEnemy = Instantiate(config.GetEnemyPrefab(),
            config.GetWaypoints()[0].transform.position,
            Quaternion.identity, enemies.transform);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(config);

            yield return new WaitForSeconds(config.GetTimeBetweenSpawns());
        }
        partCounter++;
    }

    public void UpdateWaveCounter()
    {
      
        waveDisplayer.UpdateWave();
    }
}
