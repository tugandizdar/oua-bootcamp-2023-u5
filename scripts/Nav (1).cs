using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Nav : MonoBehaviour
{
    Transform player;
    private NavMeshAgent enemyNavAgent;
    public float updateInterval = 0.5f; // Hedef güncelleme aralýðý

    private float elapsedTime = 0f; // Geçen süre

    void Start()
    {
        enemyNavAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Cube").transform;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime; // Geçen süreyi güncelle

        if (elapsedTime >= updateInterval)
        {
            UpdateDestination(); // Hedefi güncelle
            elapsedTime = 0f; // Geçen süreyi sýfýrla
        }
    }

    void UpdateDestination()
    {
        enemyNavAgent.SetDestination(player.position);
    }
}

