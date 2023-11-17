using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] Enemies;

    public GameObject GetRandomEnemy()
    {
        int enemyToInstance = Random.Range(0, Enemies.Length);
        return Enemies[enemyToInstance];
    }
}
