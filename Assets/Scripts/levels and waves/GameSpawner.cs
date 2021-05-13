using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawner : MonoBehaviour
{
    [Header("Levels and waves")]
    [SerializeField] Chapter [] chapterPack;
    //[SerializeField] int startingLevel = 0;
    //[SerializeField] float timeBetweenLevels = 3f;
    //[SerializeField] float timeBetweenWaves = 0f;
    //[SerializeField] bool looping = false;

    [Header("Audios")]
    [SerializeField] AudioClip nextLevelAudio;
    [SerializeField] AudioClip winAudio;
    [SerializeField] AudioClip spawnAudio;

    GameSession session;
    GameObject enemies;
    int partCounter = 0;


    // Start is called before the first frame update
    IEnumerator Start()
    {

        session = FindObjectOfType<GameSession>();
        enemies = GameObject.Find("Enemies");
        yield return StartCoroutine(SpawnPlayer());
         
    }

    //spawn all chapters of the game
    private IEnumerator SpawnChapters()
    {
       for(int levelIndex = 0; levelIndex < chapterPack.Length; levelIndex++)
        {
            var currentChapter = chapterPack[levelIndex];

            yield return StartCoroutine(SpawnLevels(currentChapter));

       }

        print("All chapters played");

        AudioSource.PlayClipAtPoint(winAudio, Camera.main.transform.position);

        yield return new WaitForSeconds(2);

        print("played win jingle");

        session.Win();

        FindObjectOfType<SceneSelector>().LoadGameOver();

        print("loading end screen");
    }

    //spawm all levels from a chapter
    private IEnumerator SpawnLevels(Chapter chapter)
    {

        var levels = chapter.GetLevels();
        var timeBetweenLevels = chapter.GetTimeBetweenLevels();

        for (int levelIndex = 0; levelIndex < levels.Length; levelIndex++)
        {
            var currentLevel = levels[levelIndex];

            yield return StartCoroutine(SpawnWaves(currentLevel));

            AudioSource.PlayClipAtPoint(nextLevelAudio, Camera.main.transform.position);  

            yield return DetectEnemies(timeBetweenLevels);

            if(levelIndex + 1 != levels.Length){
                session.IncreaseLevel();
            }
            
        }

        print("All levels done for chapter" );

    }

    //spawn all waves
    private IEnumerator SpawnWaves(Level currentLevel)

    {
         var waves = currentLevel.GetWaves();
        var timeBetweenWaves = currentLevel.GetTimeBetweenWaves();

        for (int waveIndex = 0; waveIndex < waves.Length; waveIndex++)
        {
            var currentWave = waves[waveIndex];

            yield return StartCoroutine(SpawnRounds(currentWave));

            yield return StartCoroutine(DetectEnemies(timeBetweenWaves));
        }

        print("All waves done for this level");

    }

    //spawn all rounds of enemies in a wave
    private IEnumerator SpawnRounds(Wave wave)

    {
        var parts = wave.GetParts();

        for (int waveIndex = 0; waveIndex < parts.Length; waveIndex++)
        {
            var currentEnemy = parts[waveIndex];

            StartCoroutine(SpawnEnemies(currentEnemy));
           
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
        while(enemies.transform.childCount > 0)
        {
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(time);
    }


    //spawn all enemies in a round
    private IEnumerator SpawnEnemies(Part config)
    {

        for(int enemyCount = 0; enemyCount < config.GetnumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(config.GetEnemyPrefab(),
            config.GetWaypoints()[0].transform.position,
            Quaternion.identity, enemies.transform);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(config);

            yield return new WaitForSeconds(config.GetTimeBetweenSpawns());
        }
        partCounter++;
    }

    private void SetLevel()
    {
        session.SetCurrentLevel(0);
    }

    private IEnumerator SpawnPlayer()
    {

        if (session.GetPlayerPreFab() != null)
        {

            SetLevel();

            yield return new WaitForSeconds(0.5f);

            Instantiate(session.GetPlayerPreFab(), new Vector3(0f, -7f, 1f), Quaternion.identity);

            print("Player Spawned!");

            AudioSource.PlayClipAtPoint(spawnAudio, Camera.main.transform.position);

            yield return new WaitForSeconds(0.5f);

            yield return StartCoroutine(SpawnChapters());
        }
    }
}
