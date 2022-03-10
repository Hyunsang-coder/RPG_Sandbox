using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonsterAI
{
    bool isAttackBehavior;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        monsterHealth = GetComponent<MonsterHealth>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(monsterHealth.IsDead)
        {
            navMeshAgent.enabled = false;
            return;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
        {
            EngageTarget();
            timeElpased = 0;
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
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
        }
        
    }

    
    public override void AttackTarget()
    {
        if (isAttackBehavior) return;
        StartCoroutine(AttackBehavior());
    }

    IEnumerator AttackBehavior()
    {
        isAttackBehavior = true;
        int randomNo = Random.Range(0, 4);
        yield return new WaitForSeconds(0.5f);
        switch (randomNo)
        {
            case 0:
                anim.SetTrigger("Breath");
                Debug.Log("Breath");
                break;
            case 1:
                //
                anim.SetTrigger("TailAttack");
                Debug.Log("Tail Attack");
                break;
            case 2:
                //
                anim.SetTrigger("Header");
                Debug.Log("Header Attack");
                break;
            case 3:
                break;

        }
        yield return new WaitForSeconds(2f);
        isAttackBehavior = false;
        yield return null;
    }
}
