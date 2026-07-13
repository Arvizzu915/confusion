using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Berserker : MonoBehaviour
{
    [SerializeField] private int health = 4;

    private NavMeshAgent agent;

    [SerializeField] private float chasingSpeed = 3;
    [SerializeField] private float distanceToAttack = 3;
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private float eyeHeight = 1.6f;
    private bool chasing = false;

    private Transform playerPos;

    private GameObject eyeCover;

    private void Start()
    {
        playerPos = PlayerManager.instance.transform;

        StartCoroutine(Initializing());
    }

    private void Update()
    {
        if (!chasing) return;

        agent.destination = playerPos.position;

        if (CanAttackPlayer())
        {
            StartCoroutine(Attack());
        }
    }

    private bool CanAttackPlayer()
    {
        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 target = playerPos.position + Vector3.up * eyeHeight;

        Vector3 direction = target - origin;
        float distance = direction.magnitude;

        if (distance > distanceToAttack)
            return false;

        direction /= distance;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, attackMask, QueryTriggerInteraction.Ignore))
        {
            return hit.transform == playerPos;
        }

        return false;
    }

    public void GetShotOnEye(int Damage)
    {
        health -= Damage;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        chasing = false;

        yield return new WaitForSeconds(2);
    }

    public void GetShotOnBody()
    {

    }

    public void GetShotOnCover()
    {

    }

    private IEnumerator Initializing()
    {
        yield return new WaitForSeconds(1);

        agent.speed = chasingSpeed;
    }

    private IEnumerator Attack()
    {
        chasing = false;

        yield return new WaitForSeconds(1);

        chasing = true;
    }

    private IEnumerator CoverEye()
    {
        eyeCover.SetActive(true);
        chasing = false;

        yield return new WaitForSeconds(1);


        chasing = true;
    }
}
