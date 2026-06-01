using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyManager : MonoBehaviour, IShootable
{
    public abstract void GetShot();

    public abstract void Attack();
}
