using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum AIState { Patrol, Attack, Chase } 

    [Header("States")]
    [SerializeField] private AIState currentState = AIState.Patrol;

    [Header("AI settings")]
    [SerializeField] private float radiusDetection = 10f;
    [SerializeField] private float radiusAttack = 2f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackWindup = 0.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private Transform[] controlPoints;

    private int currentPointIndex;
    private bool isWaiting;
    private bool isAttacking = false;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private HealthSystem healthSystemPlayer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
            healthSystemPlayer = player.GetComponent<HealthSystem>();
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("[EnemyAI] El agente NO está sobre el NavMesh. Rebakea el NavMesh.");
        }
    }

    private void Update()
    {
        if (!agent.isOnNavMesh) return;
        if (playerTransform == null) return;

        if (healthSystemPlayer != null && healthSystemPlayer.IsDead)
        {
            if (currentState != AIState.Patrol)
            {
                currentState = AIState.Patrol;
                if (agent.isOnNavMesh) agent.isStopped = false;
                GoNext();
            }
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case AIState.Patrol:
                UpdatePatrolState(distanceToPlayer);
                break;

            case AIState.Chase:
                UpdateChaseState(distanceToPlayer);
                break;

            case AIState.Attack:
                UpdateAttackState(distanceToPlayer);
                break;
        }
    }

    private void UpdatePatrolState(float distance)
    {
        if (distance <= radiusDetection)
        {
            currentState = AIState.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                StartCoroutine(WaitAndMove());
            }
        }
    }

    private void UpdateChaseState(float distance)
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(playerTransform.position);
        }

        if (distance <= radiusAttack)
        {
            currentState = AIState.Attack;
            return;
        }

        if (distance > radiusDetection * 1.2f)
        {
            currentState = AIState.Patrol;
            GoNext();
        }
    }

    private void UpdateAttackState(float distance)
    {
        if (distance > radiusAttack && !isAttacking)
        {
            currentState = AIState.Chase;
            return;
        }

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        if (controlPoints.Length > 0)
        {
            currentPointIndex = (currentPointIndex + 1) % controlPoints.Length;
            GoNext();
        }

        isWaiting = false;
    }

    private IEnumerator AttackCooldown()
    {
        if (agent.isOnNavMesh) agent.isStopped = true;

        yield return new WaitForSeconds(attackWindup);

        if (playerTransform != null &&
            Vector3.Distance(transform.position, playerTransform.position) <= radiusAttack)
        {
            ExecuteAttack();
        }

        yield return new WaitForSeconds(attackCooldown);

        if (agent.isOnNavMesh) agent.isStopped = false;

        isAttacking = false;
    }

    private void ExecuteAttack()
    {
        if (healthSystemPlayer == null) return;
        if (healthSystemPlayer.IsDead) return;

        healthSystemPlayer.TakeDamage(attackDamage);
    }

    private void GoNext()
    {
        if (controlPoints.Length == 0) return;
        if (!agent.isOnNavMesh) return;

        agent.SetDestination(controlPoints[currentPointIndex].position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusDetection);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }
}
