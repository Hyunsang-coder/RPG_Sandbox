using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonsterAI
{
    [SerializeField] GameObject breathPrefab;
    [SerializeField] Transform breathPosition;
    [SerializeField] bool isAttacking;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        target = GameObject.FindWithTag("Player").transform;
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
        
        //isCloseRange = distanceToTarget < 3? true: false;
    }

    
    public override void AttackTarget()
    {
        if (isAttacking) return;

        if (distanceToTarget < navMeshAgent.stoppingDistance - 1)
        {
            StartCoroutine(CloseAttackBehavior());
            return;
        }
        
        StartCoroutine(AttackBehavior());
        
    }

    IEnumerator CloseAttackBehavior()
    {
        isAttacking = true;
        anim.SetTrigger("TailAttack");
        Debug.Log("tail attack");

        yield return new WaitForSeconds(2f);

        navMeshAgent.isStopped = false;
        isAttacking = false;
    }

    IEnumerator AttackBehavior()
    {
        isAttacking = true;
        int randomNo = Random.Range(0, 3);
        yield return new WaitForSeconds(0.5f);
        switch (randomNo)
        {
            case 0:
                anim.SetTrigger("Breath");
                Instantiate(breathPrefab, breathPosition.position, breathPosition.rotation);
                Debug.Log("Breath");
                navMeshAgent.isStopped = true;
                yield return new WaitForSeconds(1.3f);
                navMeshAgent.isStopped = false;
                isAttacking = false;
                break;
            
            case 1: case 2:
                //
                anim.SetTrigger("Header");
                Debug.Log("Header Attack");
                navMeshAgent.isStopped = true;
                yield return new WaitForSeconds(1.3f);
                navMeshAgent.isStopped = false;
                isAttacking = false;
                break;
                

        }
        yield return new WaitForSeconds(0.5f);
    }


    public override IEnumerator StunnedBehavior(float stunTime)
    {
        isStunned = true;
        isAttacking = true;

        anim.SetBool("isStunned", true);

        yield return new WaitForSeconds(stunTime);

        navMeshAgent.isStopped = false;
        anim.SetBool("isStunned", false);
        isAttacking = false;
        isStunned = false;

        Debug.Log("Stunned Ended");
    }
}
