using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
        
    /// <summary>
    /// Welcome to the Basic AlexAI for the game HouseSiegeVR(working title)
    /// There are currently 3 states in this machine
    /// Roaming: Where the AI roams around in a certain area, Attacking: where the AI shoots the player, Reloading: When he runs out of ammo
    /// </summary>
    public enum State
    {
        ROAMING, ATTACKING, RELOADING
    }
    //AI STUFF
    [Header("AI Stuff")]
    public State state;
    public NavMeshAgent agent;
    [Range(0, 100)] public float speed;
    [Range(0, 100)] public float walkRadius;

    [Header("Random Number Generator 1 = hit")]
    public float oddsHigh;
    public float oddsLow;

    
    [Header("Shooting rate and Ammo")]
    [Range(0,2)] public float fireRateMin;
    [Range(0,5)] public float fireRateMax;
    public int AmmoCount;
    private float NextShootTime;
    
    
    [Header("Other Enemy Stuff")]
    public ParticleSystem Blood;
    public Transform Player;
    public GameObject LevelManager;

    [Header("Animation Stuff")] 
    public Animator ani;
    
    void Start()
    {
        state = State.ROAMING;
        agent.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = speed;
            agent.SetDestination(RandomNavMeshLocation());
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.ROAMING:
                ani.SetBool("ReadyArm",false);
                
                if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
                {
                    agent.SetDestination(RandomNavMeshLocation());
                }
                break;
            
            case State.ATTACKING:
                
                transform.LookAt(Player);
                agent.isStopped.Equals(true);
                AttackPlayer();
                break;
        }

    }
    //The AI will try to find a random spot and see if he can go there if yes goes there
    private Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosistion = Vector3.zero;
        Vector3 randomPosistion = Random.insideUnitSphere * walkRadius;
        randomPosistion += transform.position;
        if (NavMesh.SamplePosition(randomPosistion, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosistion = hit.position;
        }

        return finalPosistion;
    }
    //Attack the player and play the sound then in the shoot function there is a random number generator to see if the AI hits the player.
    private void AttackPlayer()
    {
        ani.SetBool("ReadyArm", true);
        
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 50f))
        {
            if (!(Time.time > NextShootTime))
                return;
            Shoot();
            NextShootTime = Time.time + Random.Range(fireRateMin, fireRateMax);
        }
    }

    public void Shoot()
    {
        
        AudioManager.instance.Play("Shot");
        var i =  Random.Range(oddsLow, oddsHigh);
        if (i >= 1)
        { 
            Debug.Log("I hit the player yay");
            LevelManager.GetComponent<LevelManager>().LoseLife();
        }
        else
        {
            Debug.Log("Missed");
        }

    }

    //If player is in the trigger aim at him and change the state to ATTACKING
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player = other.transform;
            state = State.ATTACKING;
        }
    }
    //If the player leaves set the state to ROAMING
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            state = State.ROAMING;
    }
    
    //Play some fancy blood effects after getting hit
    private void OnDestroy()
    {
        Blood.Play();
    }
    
}
