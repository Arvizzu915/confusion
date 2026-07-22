using System.Collections.Generic;
using UnityEngine;

public class RoomDetector : MonoBehaviour
{
    public bool playerInside = false;

    public Enemy[] enemyList;

    [SerializeField] private List<NoiseMaker> noiseMakers = new();

    public IReadOnlyList<NoiseMaker> NoiseMakers => noiseMakers;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        DetectPlayer();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        SetEnemiesPlayerInside(false);
    }

    private void SetEnemiesPlayerInside(bool value)
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyList[i].SetEnemyInsideRoom(value);
        }
    }

    public void RegisterNoiseMaker(NoiseMaker noiseMaker)
    {
        if (!noiseMakers.Contains(noiseMaker))
            noiseMakers.Add(noiseMaker);
    }

    public void UnregisterNoiseMaker(NoiseMaker noiseMaker)
    {
        noiseMakers.Remove(noiseMaker);
    }

    public virtual void DetectPlayer()
    {
        playerInside = true;

        SetEnemiesPlayerInside(true);

        SoundManager.Instance.SetCurrentRoom(this);
    }
}
