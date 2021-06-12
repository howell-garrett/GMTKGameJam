using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMoveAir : MonoBehaviour
{
    public GameObject destination;
    public Transform destinationMoveToTarget;
    public GameObject player;
    public Transform playerMoveToTarget;
    NavMeshAgent navMeshAgent;
    EnemyStates state;

    public int attackPlayerRange = 6;
    public int attackObjectiveRange = 8;

    public float rateOfFire = 1f;
    float rateOfFireReset;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int shotDamage;
    public int projectileSpeed;
    bool shotDrops = false;
    public int stoppingDistance = 8;
    public int moveSpeed = 5;

    AudioClip shotSFX;

    // Start is called before the first frame update
    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("Target");
        destinationMoveToTarget = GameObject.FindGameObjectWithTag("FlyingTarget").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMoveToTarget = GameObject.FindGameObjectWithTag("PlayerFlyingTarget").transform;
        state = EnemyStates.ApproachingObjective;
    }

    // Update is called once per frame
    void Update()
    {
        if (destination)
        {
            if (state == EnemyStates.ApproachingObjective)
            {
                ApproachingObjective();
            }
            else if (state == EnemyStates.AttackingObjective)
            {
                AttackingObjective();
            }
            else if (state == EnemyStates.AttackingPlayer)
            {
                AttackingPlayer();
            }
        }
    }

    void ApproachingObjective()
    {
        transform.LookAt(destination.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, destinationMoveToTarget.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, player.transform.position) <= attackPlayerRange)
        {
            state = EnemyStates.AttackingPlayer;
            return;
        }
        if (Vector3.Distance(transform.position, destination.transform.position) <= attackObjectiveRange)
        {
            state = EnemyStates.AttackingObjective;
            return;
        }
    }

    void AttackingObjective()
    {
        if (rateOfFireReset <= 0)
        {

            GameObject shot = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            shot.GetComponent<ETFXProjectileScript>().isPlayerShot = false;
            shot.GetComponent<ETFXProjectileScript>().damage = shotDamage;
            shot.GetComponent<Rigidbody>().AddForce(shot.transform.forward * projectileSpeed); //Set the speed of the projectile by applying force to the rigidbody
            if (shotDrops)
            {
                shot.GetComponent<Rigidbody>().useGravity = true;
            }
        rateOfFireReset = rateOfFire;
        }
        rateOfFireReset -= Time.deltaTime;
        transform.LookAt(destination.transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) <= attackPlayerRange)
        {
            state = EnemyStates.AttackingPlayer;
            return;
        }
    }

    void AttackingPlayer()
    {
        if (Vector3.Distance(transform.position, playerMoveToTarget.position) >= stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerMoveToTarget.position, moveSpeed * Time.deltaTime);
        }
        transform.LookAt(player.transform.position);
        if (rateOfFireReset <= 0)
        {

            GameObject shot = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            shot.GetComponent<ETFXProjectileScript>().isPlayerShot = false;
            shot.GetComponent<ETFXProjectileScript>().damage = shotDamage;
            shot.GetComponent<Rigidbody>().AddForce(shot.transform.forward * projectileSpeed); //Set the speed of the projectile by applying force to the rigidbody
            if (shotDrops)
            {
                shot.GetComponent<Rigidbody>().useGravity = true;
            }

            rateOfFireReset = rateOfFire;
        }

        rateOfFireReset -= Time.deltaTime;
        transform.LookAt(player.transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) >= attackPlayerRange)
        {
            state = EnemyStates.ApproachingObjective;
        }
    }
}
