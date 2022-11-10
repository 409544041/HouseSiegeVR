using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAITest : MonoBehaviour
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
    
    [Header("Other Enemy Stuff")]
    public ParticleSystem Blood;
    public Transform Player;
    public float TimeDelayBetweenBullets;
    public int AmmoCount;
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
                
                if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
                {
                    agent.SetDestination(RandomNavMeshLocation());
                }
                break;
            
            case State.ATTACKING:
                
                transform.LookAt(Player);
                agent.SetDestination(transform.position);
                AttackPlayer();
                break;
        }

    }

    public Vector3 RandomNavMeshLocation()
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

    public void AttackPlayer()
    {
        ///Make here the shoot script alex;
        /// 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player = other.transform;
            state = State.ATTACKING;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            state = State.ROAMING;
    }

    private void OnDestroy()
    {
        Blood.Play();
    }
}
