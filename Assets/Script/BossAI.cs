using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonsterAI
{
    [SerializeField] Vector3 offsetToTarget = new Vector3();
    [SerializeField] GameObject breathPrefab;
    [SerializeField] Transform breathPosition;
    [SerializeField] bool isAttackBehavior;
    [SerializeField] bool isCloseRange;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth.isDead)
        {
            //navMeshAgent.enabled = false;
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
        
        isCloseRange = distanceToTarget < 3? true: false;
    }

    
    public override void AttackTarget()
    {
        if (isAttackBehavior) return;
        if (isCloseRange)
        {
            StopAllCoroutines();
            StartCoroutine(CloseAttackBehavior());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(AttackBehavior());
        }
        
    }

    IEnumerator CloseAttackBehavior()
    {
        isAttackBehavior = true;
        anim.SetTrigger("TailAttack");
        Debug.Log("tail attack");
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1f);
        navMeshAgent.isStopped = false;
        yield return null;
        isAttackBehavior = false;
    }

    IEnumerator AttackBehavior()
    {
        isAttackBehavior = true;
        int randomNo = Random.Range(0, 3);
        yield return new WaitForSeconds(0.2f);
        switch (randomNo)
        {
            case 0:
                anim.SetTrigger("Breath");
                Instantiate(breathPrefab, breathPosition.position, breathPosition.rotation);
                Debug.Log("Breath");
                navMeshAgent.isStopped = true;
                yield return new WaitForSeconds(1.3f);
                navMeshAgent.isStopped = false;
                break;
            
            case 1:
                //
                anim.SetTrigger("Header");
                Debug.Log("Header Attack");
                navMeshAgent.isStopped = true;
                yield return new WaitForSeconds(1.3f);
                navMeshAgent.isStopped = false;
                break;

            //case 2:
            //    //
            //    

        }
        yield return new WaitForSeconds(0.5f);
        isAttackBehavior = false;
    }
}
