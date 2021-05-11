using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveHolder> levels;
    [SerializeField] int startingLevel = 0;
    [SerializeField] bool looping = false;
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

            session.IncreaseLevel();
        }
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
