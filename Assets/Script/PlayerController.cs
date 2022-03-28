using System;
using System.Collections;
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

    Animator animator;
    Rigidbody rb;
    public GameObject sword;
    public Transform throwPos;
    public GameObject throwablePrefab;
    Health health;

    [SerializeField] int attackDamage = 10;
    [SerializeField] int powerAttackDamage = 50;
    [SerializeField] float throwVelocity = 10;
    [SerializeField] int healPoint = 50;

    [SerializeField]
    public bool PickupReady { get; private set; }

    NPCBehavior npcBehavior;
    GameManager gameManager;
    PlayerUI playerUI;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        npcBehavior = FindObjectOfType<NPCBehavior>();
        gameManager = FindObjectOfType<GameManager>();
        playerUI = FindObjectOfType<PlayerUI>();
    }

    void Update()
    {
        if (health.IsDead) return;
        Move();
        GroundCheck();
        GetAxis();
        OtherActions();
    }

    float pressTime = 0;
    public bool isInteracting;
    private void OtherActions()
    {
        if (isInteracting) return;
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
        if (Input.GetKey(KeyCode.Mouse0))
        {
            pressTime += Time.deltaTime;
            if (pressTime > 2)
            {
                PowerAttack();
                pressTime = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (gameManager.flashBangQty > 0)
            {
                StartCoroutine(ThrowItem());
            }
            else return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PickupReady)
            {
                Pickup();
            }
            else return;
        }


        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (gameManager.potionQty > 0)
            {
                DrinkBehavior();
            }
            else return;
        }
    }

    private void DrinkBehavior()
    {
        StartCoroutine(DrinkVFX());
    }

    IEnumerator DrinkVFX()
    {
        isAttacking = true;
        animator.SetTrigger("Drink");
        gameManager.UsePostion();
        health.HealHealth(healPoint);
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    void Pickup()
    {
        animator.SetTrigger("Pickup");
        PickupReady = false;
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

        animator.SetFloat("MoveSpeed", moveSpeed);
        
    }
  
    private void Attack()
    {
        animator.SetTrigger("Slash");
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
        Debug.Log("Damage!!");
        yield return new WaitForSeconds(finishTime);
        sword.GetComponent<BoxCollider>().enabled = false;
        sword.GetComponentInChildren<TrailRenderer>().enabled = false;
        
        yield return new WaitForEndOfFrame();
        GetComponentInChildren<Weapon>().Damage = originaldamage;
        attackFactor = 1f;
    }

    void PowerAttack()
    {
        animator.SetTrigger("PowerAttack");
        health.SubtractStamina(10);
        StartCoroutine(AttackVFX(0.6f, 0.6f, powerAttackDamage));
    }


    void Jump()
    {
        if (onGround)
        {
            animator.SetTrigger("Jump");
            health.SubtractStamina(10);
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
            animator.SetTrigger("Roll");
            health.SubtractStamina(10);
        }
    }

    public void RollFinished()
    {
        isRolling = false;
        animator.applyRootMotion = true;
    }


    IEnumerator ThrowItem()
    {
        gameManager.flashBangQty--;
        playerUI.UpdateLv_ItemUI();
        isAttacking = true;
        animator.SetTrigger("Throw");
        yield return new WaitForSeconds(1f);
        InstantiateThrowable();
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        yield break;
    }

    public void InstantiateThrowable()
    {
        if (throwablePrefab != null || throwPos != null)
        {
            GameObject tempThrowable;
            tempThrowable = Instantiate(throwablePrefab, throwPos.position, Quaternion.identity);

            
            Rigidbody rigidBody = tempThrowable.GetComponent<Rigidbody>();
            rigidBody.AddForce(transform.forward * throwVelocity, ForceMode.Impulse);
            rigidBody.AddTorque(Vector3.up * throwVelocity, ForceMode.Impulse);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PickupItem")
        {
            PickupReady = true;
        }
    }

}
