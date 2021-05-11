using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHolder : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;

    public List<WaveConfig> GetWaves()
    {
        return waveConfigs;
    }
}
