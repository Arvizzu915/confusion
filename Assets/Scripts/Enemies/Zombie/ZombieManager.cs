using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : Enemy
{
    public ZombieState currentState = null;
    public ZombieState stateIdle = new ZombieIdle();
    public ZombieState stateChasing = new ZombieChasing();
    public ZombieState stateWandering = new ZombieWandering();

    public Transform playerTrans;
    [SerializeField] private float detectionRange = 8f;

    public float detectionRangeSqr;

    public NavMeshAgent agent;

    public float idleSpeed = 0, walkingSpeed = .5f, RunningSpeed = 1.8f;

    public int health = 4;


    private void Start()
    {
        playerTrans = PlayerManager.instance.transform;
        detectionRangeSqr = detectionRange * detectionRange;

        currentState = stateIdle;
        currentState.ZombieEnter(this);
    }

    private void Update()
    {
        currentState.ZombieUpdate(this);
    }

    public void ChangeState(ZombieState state)
    {
        currentState.ZombieExit(this);
        currentState = state;
        currentState.ZombieEnter(this);
    }

    public override void GetShot(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }

        currentState.ZombieGetShot(this, damage);
    }

    public override void SetEnemyInsideRoom(bool value)
    {
        base.SetEnemyInsideRoom(value);
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
