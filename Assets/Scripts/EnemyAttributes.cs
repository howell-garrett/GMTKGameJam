using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributes : MonoBehaviour
{

    public int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.forward / 300;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            KillEnemy();
        }
    }

    void KillEnemy()
    {
        GameManager.RemoveEnemyFromWave(gameObject);
        if (GameManager.enemiesCurrentlyInWave.Count <= 0)
        {
            GameManager.StartWave();
        }
        Destroy(gameObject, 0.25f);
    }
}
