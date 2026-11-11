using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public enum AIState {Patrol,Attack,Chase}//solo puede tomar un estado a la vez
    [Header("States")]
    [SerializeField] private AIState currentState = AIState.Patrol;

    [Header("AI settings")]

    [SerializeField] private float radiusDetection;
    [SerializeField] private float radiusAttack;

    [SerializeField] private Transform[] controlPoints;

    [SerializeField] private float waitTime = 1f;

    [SerializeField] private float attackCooldown;

    private int currentPointIndex;
    
    private bool isWaiting;

    private NavMeshAgent agent;

    private Transform playerTransform;
    private bool isAttacking = false;

    private HealthSystem healthSystemPlayer;

    private void Awake()
    {

       agent = GetComponent<NavMeshAgent>(); 

    }

    void Start()
    {
        //cargar el jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
          playerTransform = player.GetComponent<Transform>();
          healthSystemPlayer = player.GetComponent<HealthSystem>();
        }
        else
        {
            Debug.Log("No encontro player");
        }

     }

    private void Update()
    {
    if (!agent.isOnNavMesh) return;

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
    //cambio de estado
      if (distance <= radiusDetection)
      {
          currentState = AIState.Chase;
          return;
      }
      //bool var3 = agent.pathPending;
      //float var2 = agent.remainingDistance;
      //float var1 = agent.stopping Distance;
      if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
      {
         if(!isWaiting)
         {
            //corrutina para el cambio de posicion y punto de control
            StartCoroutine (WaitAndMove());
         }
      }
    }

    private void UpdateChaseState(float distance)
    {
       //cambio de estado 
       agent.SetDestination(playerTransform.position);

       if(distance <= radiusAttack)
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
    //cambio de estado
    //frenar el movimiento
    //mirar al jugador
    if(distance > radiusAttack && !isAttacking)
       {
         currentState = AIState.Chase;
         return;
       }

       if (!isAttacking)
       {
           //corrutina de ataque
           StartCoroutine (AttackCooldown());
       }

    }

    private IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

          if(controlPoints.Length > 0)
          {
             currentPointIndex = (currentPointIndex + 1) % controlPoints.Length;
             //0 = 0+1 % 3 = 1
             //1 = 1+1 % 3 = 2
             //2 = 2+1 % 3 = 0
             //ir a mi otro punto
             GoNext();
          }
          isWaiting = false;
    }

    private IEnumerator AttackCooldown()
     {
        isAttacking = true;
        agent.isStopped = true;
        //ExecuteAttack();
        yield return new WaitForSeconds(attackCooldown);
        Debug.Log("VOlvio");
        isAttacking = false;
        agent.isStopped = false;
    }

    private void GoNext()
    {
         if (controlPoints.Length == 0) return;

         agent.SetDestination(controlPoints[currentPointIndex].position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,radiusDetection);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,radiusAttack);

    }



}