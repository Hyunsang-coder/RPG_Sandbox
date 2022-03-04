using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 100;
    public float maxSpeed = 5;
    float moveSpeed;
    float desiredSpeed;
    float accel = 7;
    float deccel = 100;
    bool onGround = true;
    float groundRayDist = 1;
    float horizontal;
    float vertical;

    Animator anim;
    Rigidbody rb;

    public GameObject sword;
   

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {

        Move();
        GroundCheck();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

    }

    private void Attack()
    {
        anim.SetTrigger("Slash");
        StartCoroutine(SwingVFX());
    }

    IEnumerator SwingVFX()
    {
        yield return new WaitForSeconds(0.3f);
        sword.GetComponent<BoxCollider>().enabled = true;
        sword.GetComponentInChildren<TrailRenderer>().enabled = true;
        yield return new WaitForSeconds(0.8f);
        sword.GetComponentInChildren<TrailRenderer>().enabled = false;
        sword.GetComponent<BoxCollider>().enabled = false;
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
        Vector3 moveVector = new Vector3(horizontal, 0, vertical).normalized;
        desiredSpeed = moveVector.magnitude * maxSpeed;

        float acceleration = IsMove ? accel : deccel;

        moveSpeed = Mathf.MoveTowards(moveSpeed, desiredSpeed, acceleration * Time.deltaTime);


        transform.LookAt(transform.position + moveVector);
        transform.Translate(moveVector*moveSpeed *Time.deltaTime, Space.World);
        //transform.position += moveVector * moveSpeed * Time.deltaTime;

        anim.SetFloat("MoveSpeed", moveSpeed);

    }

    void Jump()
    {
        if (onGround)
        {
            anim.SetTrigger("Jump");
        }
    }

    public void Launch()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

}
