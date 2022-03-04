using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MonsterAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;


    NavMeshAgent navMeshAgent;
    Animator anim;
    float distanceToTarget = Mathf.Infinity;
    [SerializeField] bool isProvoked = false;
    //EnemyHealth enemyHealth;

    [SerializeField] PatrolPath patrolPath;
    int currentWaypointIndex = 0;
    [SerializeField] float waypointTolerance = 2;
    [SerializeField] float chaseDistance = 3;
    Vector3 nextPosition;

    Health playerHealth; 

    private void OnEnable()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        
    }
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //enemyHealth = GetComponent<EnemyHealth>();
        target = GameObject.FindWithTag("Player").transform;
        playerHealth.OnPlayerDeath += PatrolBehavior;
    }

    void Update()
    {
        //if (enemyHealth.IsDead())
        //{
        //    enabled = false;  // = this.enabled
        //    navMeshAgent.enabled = false;
        //}


        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
        if (distanceToTarget > chaseRange)
        {
            isProvoked = false;
            PatrolBehavior();
            //GetComponent<Animator>().SetTrigger("Idle");
        }
        if (navMeshAgent.velocity.magnitude > 0)
        {
            anim.SetBool("isMove", true);
        }
        else anim.SetBool("isMove", false);


    }

    private void PatrolBehavior()
    {
        if (patrolPath != null)
        {
            if (AtWaypoint())
            {
                CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();
        }
        navMeshAgent.SetDestination(nextPosition);
    }

    private bool AtWaypoint()
    {
        return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextPoint(currentWaypointIndex);
        // currentWaypointIndex는 0 - 1 - 2 - 0 - 1 - 2... 반복
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypointPosition(currentWaypointIndex);
        // currentWaypointIndex에 해당하는 waypoint의 Vector3 값 반환
    }
    private bool InAttackRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        return distanceToPlayer < chaseDistance;
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        // transform.rotation = where the target is, we need to rotate at a certain speed
    }



    void EngageTarget()
    {
        FaceTarget();
        if (distanceToTarget > navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }


    }

    void ChaseTarget()
    {
        //GetComponent<Animator>().SetBool("Attack", false);
        //GetComponent<Animator>().SetTrigger("Move");
        navMeshAgent.SetDestination(target.position);
    }
    void AttackTarget()
    {
        //anim.SetBool("isAttack", true);
        anim.SetTrigger("Attack");
        
    }

    //Gizmo 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    void PlayerDeath()
    {
        target = null;
        PatrolBehavior();
    }

}