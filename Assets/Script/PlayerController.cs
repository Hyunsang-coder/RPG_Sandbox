using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5;
    public float rollVelocity = 3;
    public float maxSpeed = 5;
    float moveSpeed;
    float desiredSpeed;
    float accel = 5;
    float deccel = 50;
    bool onGround = true;
    float groundRayDist = 1;
    float horizontal;
    float vertical;
    bool isRolling;
    bool isAttacking;
    float attackFactor = 1;
    Animator anim;
    Rigidbody rb;
    public GameObject sword;
    Health playerHealth;

    [SerializeField] int attackDamage = 10;
    [SerializeField] int powerAttackDamage = 50;

    NPCBehavior npcBehavior;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerHealth = GetComponent<Health>();
        npcBehavior = FindObjectOfType<NPCBehavior>();
    }


    void Update()
    {
        if (playerHealth.IsDead) return;
        Move();
        GroundCheck();
        GetAxis();
        OtherActions();

    }

    private void OtherActions()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Roll();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            PowerAttack();
        }
    }

    private void GetAxis()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * groundRayDist * 0.5f, -Vector3.up);
        if (Physics.Raycast(ray, out hit, groundRayDist))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        Debug.DrawRay(transform.position + Vector3.up * groundRayDist * 0.5f, -Vector3.up * groundRayDist*0.5f, Color.red);
    }

    private bool IsMove
    {
        get { return !Mathf.Approximately(horizontal + vertical, 0); }
    }

    void Move()
    {
        if (isRolling || isAttacking) return;
        
        Vector3 moveVector = new Vector3(horizontal, 0, vertical).normalized;
        desiredSpeed = moveVector.magnitude * maxSpeed;

        float acceleration = IsMove ? accel : deccel;

        moveSpeed = Mathf.MoveTowards(moveSpeed, desiredSpeed * attackFactor, acceleration * Time.deltaTime);


        transform.LookAt(transform.position + moveVector);
        transform.Translate(moveVector * moveSpeed * Time.deltaTime , Space.World);
        //transform.position += moveVector * moveSpeed * Time.deltaTime;

        anim.SetFloat("MoveSpeed", moveSpeed);
        
    }
  
    private void Attack()
    {
        if (npcBehavior.interactionReady) return;
        anim.SetTrigger("Slash");
        StartCoroutine(AttackVFX(0.3f, 0.6f, attackDamage));
    }

    IEnumerator AttackVFX(float waitTime, float finishTime, int damage)
    {
        int originaldamage = GetComponentInChildren<Weapon>().Damage;
        GetComponentInChildren<Weapon>().Damage = damage;
        attackFactor = 0.2f;
        yield return new WaitForSeconds(waitTime);
        sword.GetComponentInChildren<TrailRenderer>().enabled = true;
        sword.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(finishTime);
        sword.GetComponent<BoxCollider>().enabled = false;
        sword.GetComponentInChildren<TrailRenderer>().enabled = false;
        
        yield return new WaitForEndOfFrame();
        GetComponentInChildren<Weapon>().Damage = originaldamage;
        attackFactor = 1f;
    }

    void PowerAttack()
    {
        anim.SetTrigger("PowerAttack");
        playerHealth.SubtractStamina(10);
        StartCoroutine(AttackVFX(0.5f, 0.8f, powerAttackDamage));
    }


    void Jump()
    {
        if (onGround)
        {
            anim.SetTrigger("Jump");
            playerHealth.SubtractStamina(10);
        }
    }

    public void Launch()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Debug.Log("launched!");
    }
    void Roll()
    {
        if (!isRolling)
        {
            isRolling = true;
            anim.SetTrigger("Roll");
            playerHealth.SubtractStamina(10);
        }
    }

    public void RollFinished()
    {
        isRolling = false;
        anim.applyRootMotion = true;
    }

    
}
