﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float DefaultSpeed = 5.0f;
    public float RunningSpeed = 8.0f;
    public float CrouchedSpeed = 2.0f;
    public float Gravity = -14.0f;
    public float JumpForce = 5.0f;

    private CharacterController charCont;
    private Vector3 movement;
    private float verticalForce = 0f;
    private float speed;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        charCont = GetComponent<CharacterController>();
        Cursor.visible = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (charCont.isGrounded)
        {
            verticalForce = 0;
        }
        else
        {
            verticalForce += Gravity * Time.deltaTime;
        }

        setAnimationAndSpeed();

        if (Input.GetButton("Jump") && charCont.isGrounded)
        {
            verticalForce = JumpForce;
        }

        if (Input.GetButton("Fire2") && !animator.GetBool("isWalking"))
        {
            animator.SetBool("isTarget", true);
        }
        else
        {
            animator.SetBool("isTarget", false);
        }

        movement = new Vector3(Input.GetAxis("Horizontal") * speed, verticalForce, Input.GetAxis("Vertical") * speed);

        movement = transform.TransformDirection(movement);
        charCont.Move(movement * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(charCont.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

    private void setAnimationAndSpeed()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isWalking", true);

            if (Input.GetAxis("Vertical") != 0)
            {
                float verticalValue = Input.GetAxis("Vertical");
                speed = verticalValue > 0 ? DefaultSpeed : DefaultSpeed * 0.5f;
                animator.SetFloat("z_direction", verticalValue);
                Debug.Log(verticalValue);
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                float horizontalValue = Input.GetAxis("Horizontal");
                speed = DefaultSpeed;
                animator.SetFloat("x_direction", horizontalValue);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isWalking", false);
        }

        //if (Input.GetButton("Run"))
        //{
        //    animator.SetBool("isFastRunning", true);
        //    speed = RunningSpeed;
        //}
        //else
        //{
        //    animator.SetBool("isFastRunning", false);
        //    speed = DefaultSpeed;
        //}

        //if (Input.GetButton("Crouched"))
        //{
        //    animator.SetBool("isCrouched", true);
        //    speed = CrouchedSpeed;
        //}
        //else
        //{
        //    animator.SetBool("isCrouched", false);
        //    speed = DefaultSpeed;
        //}
    }
}
