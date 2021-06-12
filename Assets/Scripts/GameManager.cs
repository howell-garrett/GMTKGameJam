using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static HashSet<GameObject> enemiesCurrentlyInWave;
    public static int waveCount;
    bool isGameOver = true;
    // Start is called before the first frame update
    void Start()
    {
        waveCount = 1;
        isGameOver = true;
        enemiesCurrentlyInWave = new HashSet<GameObject>();
        StartWave();
        isGameOver = false;
    }

    public static void AddEnemyToWave(GameObject enemy)
    {
        enemiesCurrentlyInWave.Add(enemy);
    }

    public static void RemoveEnemyFromWave(GameObject enemy)
    {
        enemiesCurrentlyInWave.Remove(enemy);
    }

    public static void StartWave()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            go.GetComponent<WaveSpawner>().BeginWave(waveCount);
        }
        waveCount++;
    }

    public static void LoadCurrentScene()
    {
        //Invoke("LoadScene(" + SceneManager.GetActiveScene().name + ");", 2);
    }

    void LoadScene(string name)
    {

    }
}
