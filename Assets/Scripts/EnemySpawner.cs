using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Levels and waves")]
    [SerializeField] List<WaveHolder> levels;
    [SerializeField] int startingLevel = 0;
    [SerializeField] float timeBetweenLevels = 3f;
    [SerializeField] float timeBetweenWaves = 0f;
    //[SerializeField] bool looping = false;

    [Header("Audios")]
    [SerializeField] AudioClip nextLevelAudio;
    [SerializeField] AudioClip winAudio;
    [SerializeField] AudioClip spawnAudio;

    GameSession session;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        session = FindObjectOfType<GameSession>();
        yield return StartCoroutine(SpawnPlayer());
         
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private IEnumerator SpawnLevels()
    {
        for (int levelIndex = startingLevel; levelIndex < levels.Count; levelIndex++)
        {
            var currentLevel = levels[levelIndex];

            yield return StartCoroutine(SpawnAllWaves(currentLevel));

            AudioSource.PlayClipAtPoint(nextLevelAudio, Camera.main.transform.position);

            yield return new WaitForSeconds(timeBetweenLevels);

            session.IncreaseLevel();
        }

        print("all levels done");

        yield return new WaitForSeconds(5);

        AudioSource.PlayClipAtPoint(winAudio, Camera.main.transform.position);

        print("played win jingle");

        session.Win();

        yield return new WaitForSeconds(3);

        FindObjectOfType<Level>().LoadGameOver();

        print("loading game over");
    }

    private void SetLevel()
    {
        session.SetCurrentLevel(startingLevel);
    }


    private IEnumerator SpawnPlayer()
    {

        if (session.GetPlayerPreFab() != null)    
        {

            SetLevel();

            Instantiate(session.GetPlayerPreFab(), new Vector3(0f, -7f, 1f), Quaternion.identity);

            print("Player Spawned!");

            AudioSource.PlayClipAtPoint(spawnAudio, Camera.main.transform.position);

            yield return StartCoroutine(SpawnLevels());
        }
    }

    private IEnumerator SpawnAllWaves(WaveHolder currentLevel)

    {

        List<WaveConfig> waves = currentLevel.GetWaves();

        for(int waveIndex = 0; waveIndex < waves.Count; waveIndex++)
        {
            var currentWave = waves[waveIndex];

            yield return StartCoroutine(SpawnEnemies(currentWave));

            yield return new WaitForSeconds(timeBetweenWaves);
        }

    }

    private IEnumerator SpawnEnemies(WaveConfig config)
    {

        for(int enemyCount = 0; enemyCount < config.GetnumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(config.GetEnemyPrefab(),
            config.GetWaypoints()[0].transform.position,
            Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(config);

            yield return new WaitForSeconds(config.GetTimeBetweenSpawns());
        }

    }
}
