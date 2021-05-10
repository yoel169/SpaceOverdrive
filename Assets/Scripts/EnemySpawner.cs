using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnAllWaves()
    {
        for(int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];

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
