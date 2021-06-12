using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{

    public int health;
    public Text playerHealth;
    public Text alert;
    // Start is called before the first frame update
    void Start()
    {
        alert.gameObject.SetActive(false);
        playerHealth.text = health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            playerHealth.text = "0";
            PlayerDie();
        } else
        {
            playerHealth.text = health.ToString();
        }
    }

    void PlayerDie()
    {
        alert.gameObject.SetActive(true);
        alert.text = "Game Over";
        //GameManager.LoadCurrentScene();
    }
}
