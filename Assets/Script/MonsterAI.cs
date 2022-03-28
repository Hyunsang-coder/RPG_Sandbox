using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MonsterAI : MonoBehaviour
{
    public Transform target;
    public float chaseRange = 5f;
    public float turnSpeed = 5f;


    public NavMeshAgent navMeshAgent;
    public Animator anim;
    public float distanceToTarget = Mathf.Infinity;
    public bool isProvoked = false;
    //EnemyHealth enemyHealth;

    public PatrolPath patrolPath;
    public int currentWaypointIndex = 0;
    public float waypointTolerance = 2;
    public Vector3 nextPosition;
    public float timeElpased;
    public float suspicionTime =3;
    public EnemyHealth enemyHealth;
    public bool isStunned;

    enum EnemyState {ES_Idle, ES_Attack, ES_Chase, ES_Stunned};
    EnemyState enemyState;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        target = GameObject.FindWithTag("Player").transform;
        enemyState = EnemyState.ES_Idle;
    }

    public bool TargetInRange()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        return distanceToTarget < chaseRange? true: false;
    }

    void Update()
    {
        if (enemyHealth.isDead || isStunned)
        {
            navMeshAgent.isStopped = false;
            return;
        }

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (TargetInRange())
        {
            isProvoked = true;
            timeElpased = 0;
        }
        if (!TargetInRange())
        {
            isProvoked = false;
            timeElpased += Time.deltaTime;

            if (timeElpased > suspicionTime)
            {
                PatrolBehavior();
                timeElpased = 0;
            }
            
        }
        if (navMeshAgent.velocity.magnitude > 0)
        {
            anim.SetBool("isMove", true);
        }
        else anim.SetBool("isMove", false);

    }


    public virtual void GetStunned(float stunTime)
    {
        enemyState = EnemyState.ES_Stunned;
        if (enemyState == EnemyState.ES_Stunned)
        {
            StopAllCoroutines();
            StartCoroutine(StunnedBehavior(stunTime));
        }
    }

    public virtual IEnumerator StunnedBehavior(float stunTime)
    {
        isStunned = true;

        //anim.SetTrigger("Stunned");
        anim.SetBool("isStunned", true);
        
        yield return new WaitForSeconds(stunTime);

        navMeshAgent.isStopped = false;
        anim.SetBool("isStunned", false);
        isStunned = false;

        Debug.Log("Stunned Ended");
        yield break;
    }


    public void PatrolBehavior()
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

    public bool AtWaypoint()
    {
        return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
    }

    public void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextPoint(currentWaypointIndex);
        // currentWaypointIndex는 0 - 1 - 2 - 0 - 1 - 2... 반복
    }

    public Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypointPosition(currentWaypointIndex);
        // currentWaypointIndex에 해당하는 waypoint의 Vector3 값 반환
    }
    public bool InAttackRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        return distanceToPlayer < chaseRange;
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    public void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        // transform.rotation = where the target is, we need to rotate at a certain speed
    }



    public void EngageTarget()
    {
        //FaceTarget();
        if (distanceToTarget > navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    public void ChaseTarget()
    {
        FaceTarget();
        navMeshAgent.SetDestination(target.position);
    }
    public virtual void AttackTarget()
    {
        enemyState = EnemyState.ES_Attack;
        if (enemyState == EnemyState.ES_Attack)
        {
            FaceTarget();
            anim.SetTrigger("Attack");
        }
    }

    //Gizmo 
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    
}