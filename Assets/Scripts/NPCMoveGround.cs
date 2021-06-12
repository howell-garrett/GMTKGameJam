using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMoveGround : MonoBehaviour
{
    public GameObject destination;
    public GameObject player;
    NavMeshAgent navMeshAgent;
    EnemyStates state;

    public GameObject[] wheels;

    public int attackPlayerRange = 6;
    public int attackObjectiveRange = 8;

    public float rateOfFire = 1f;
    float rateOfFireReset;
    public GameObject projectilePrefab;
    public Transform[] firePoints;
    public int shotDamage;
    public int projectileSpeed;
    bool shotDrops = false;
    // Start is called before the first frame update
    void Start()
    {
        rateOfFireReset = Random.Range(0f, rateOfFire);
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        state = EnemyStates.ApproachingObjective;
        if (navMeshAgent == null)
        {
            Debug.LogError("No Nav Mesh Agent Attached to Enemy");
        } else
        {

            destination = GameObject.FindGameObjectWithTag("Target");
            SetDestination();
        }
        navMeshAgent.stoppingDistance = 8;
        navMeshAgent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.velocity != Vector3.zero)
        {
            SpinWheels();
        }
        if (destination)
        {
            if (state == EnemyStates.ApproachingObjective)
            {
                ApproachingObjective();
            } else if (state == EnemyStates.AttackingObjective)
            {
                AttackingObjective();
            } else if (state == EnemyStates.AttackingPlayer)
            {
                AttackingPlayer();
            }
        }

    }

    void ApproachingObjective()
    {
        transform.LookAt(destination.transform.position);
        navMeshAgent.SetDestination(destination.transform.position);
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
            foreach (Transform t in firePoints)
            {
                GameObject shot = Instantiate(projectilePrefab, t.position, t.rotation);
                shot.GetComponent<ETFXProjectileScript>().isPlayerShot = false;
                shot.GetComponent<ETFXProjectileScript>().damage = shotDamage;
                shot.GetComponent<Rigidbody>().AddForce(shot.transform.forward * projectileSpeed); //Set the speed of the projectile by applying force to the rigidbody
                if (shotDrops)
                {
                    shot.GetComponent<Rigidbody>().useGravity = true;
                }
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
        if (rateOfFireReset <= 0)
        {
            foreach(Transform t in firePoints)
            {
                GameObject shot = Instantiate(projectilePrefab, t.position, t.rotation);
                shot.GetComponent<ETFXProjectileScript>().isPlayerShot = false;
                shot.GetComponent<ETFXProjectileScript>().damage = shotDamage;
                shot.GetComponent<Rigidbody>().AddForce(shot.transform.forward * projectileSpeed); //Set the speed of the projectile by applying force to the rigidbody
                if (shotDrops)
                {
                    shot.GetComponent<Rigidbody>().useGravity = true;
                }
            }

            rateOfFireReset = rateOfFire;
        }

        rateOfFireReset -= Time.deltaTime;
        transform.LookAt(player.transform.position);
        navMeshAgent.SetDestination(player.transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) >= attackPlayerRange)
        {
            state = EnemyStates.ApproachingObjective;
        }
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = (Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10));
    }


    void SetDestination()
    {
        if (destination != null)
        {
            Vector3 targetVector = destination.transform.position;
            navMeshAgent.SetDestination(targetVector);
        }
    }

    void SpinWheels()
    {
        foreach(GameObject g in wheels)
        {
            g.transform.Rotate(new Vector3(0,1,0), 1f, Space.Self);
        }
    }
}


enum EnemyStates
{
    ApproachingObjective, AttackingObjective, AttackingPlayer
}
