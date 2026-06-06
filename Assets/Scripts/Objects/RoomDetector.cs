using System.Collections.Generic;
using UnityEngine;

public class RoomDetector : MonoBehaviour
{
    public bool playerInside = false;

    public Enemy[] enemyList;

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;

        SetEnemiesPlayerInside(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        SetEnemiesPlayerInside(false);
    }

    private void SetEnemiesPlayerInside(bool value)
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyList[i].SetEnemyInsideRoom(value);
        }
    }
}
