using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MonsterAI : MonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected float chaseRange = 5f;
    [SerializeField] protected float turnSpeed = 5f;


    protected NavMeshAgent navMeshAgent;
    protected Animator anim;
    protected float distanceToTarget = Mathf.Infinity;
    public bool isProvoked = false;
    //EnemyHealth enemyHealth;

    [SerializeField] protected PatrolPath patrolPath;
    protected int currentWaypointIndex = 0;
    [SerializeField] protected float waypointTolerance = 2;
    protected Vector3 nextPosition;
    protected float timeElpased;
    [SerializeField] protected float suspicionTime =3;
    protected MonsterHealth monsterHealth;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        monsterHealth = GetComponent<MonsterHealth>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (monsterHealth.IsDead)
        {
            navMeshAgent.enabled = false;
            return;
        }


        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
            timeElpased = 0;
        }
        if (distanceToTarget > chaseRange)
        {
            isProvoked = false;
            timeElpased += Time.deltaTime;

            if (timeElpased > suspicionTime)
            {
                PatrolBehavior();
                timeElpased = 0;
            }
            //GetComponent<Animator>().SetTrigger("Idle");
        }
        if (navMeshAgent.velocity.magnitude > 0)
        {
            anim.SetBool("isMove", true);
        }
        else anim.SetBool("isMove", false);

    }


    
    protected void PatrolBehavior()
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

    protected bool AtWaypoint()
    {
        return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
    }

    protected void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextPoint(currentWaypointIndex);
        // currentWaypointIndex는 0 - 1 - 2 - 0 - 1 - 2... 반복
    }

    protected Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypointPosition(currentWaypointIndex);
        // currentWaypointIndex에 해당하는 waypoint의 Vector3 값 반환
    }
    protected bool InAttackRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        return distanceToPlayer < chaseRange;
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    protected void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        // transform.rotation = where the target is, we need to rotate at a certain speed
    }



    protected void EngageTarget()
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

    protected void ChaseTarget()
    {
        //GetComponent<Animator>().SetBool("Attack", false);
        //GetComponent<Animator>().SetTrigger("Move");
        navMeshAgent.SetDestination(target.position);
    }
    public virtual void AttackTarget()
    {
        //anim.SetBool("isAttack", true);
        anim.SetTrigger("Attack");
        
    }

    //Gizmo 
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

}