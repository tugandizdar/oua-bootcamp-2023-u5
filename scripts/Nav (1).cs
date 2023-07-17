using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Nav : MonoBehaviour
{
    Transform player;
    private NavMeshAgent enemyNavAgent;
    public float updateInterval = 0.5f; // Hedef g�ncelleme aral���

    private float elapsedTime = 0f; // Ge�en s�re

    void Start()
    {
        enemyNavAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Cube").transform;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime; // Ge�en s�reyi g�ncelle

        if (elapsedTime >= updateInterval)
        {
            UpdateDestination(); // Hedefi g�ncelle
            elapsedTime = 0f; // Ge�en s�reyi s�f�rla
        }
    }

    void UpdateDestination()
    {
        enemyNavAgent.SetDestination(player.position);
    }
}

