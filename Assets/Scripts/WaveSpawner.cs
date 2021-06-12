using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public float spawnStagger = 0.2f;
    public int spawnDegree = 3;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void BeginWave(int waveCount)
    {
        print("Round: " + waveCount);
        StartCoroutine(SpawnWave(waveCount));
        waveCount = GameManager.waveCount;
    }

    IEnumerator SpawnWave(int waveCount)
    {
        for (int i = 0; i < waveCount*spawnDegree; i++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
            GameObject enemy = Instantiate(enemyPrefab, point.position, point.rotation);
            GameManager.AddEnemyToWave(enemy);
            yield return new WaitForSeconds(spawnStagger);
        }
    }
}
