using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float Speed = 5.0f;
    public float Gravity = -14.0f;
    public float JumpForce = 5.0f;

    private CharacterController charCont;
    private Vector3 movement;
    private float verticalForce = 0f;

    // Start is called before the first frame update
    void Start()
    {
        charCont = GetComponent<CharacterController>();
        Cursor.visible = false;
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

        if (Input.GetButton("Jump") && charCont.isGrounded)
        {
            verticalForce = JumpForce;
        }

        movement = new Vector3(Input.GetAxis("Horizontal") * Speed, verticalForce, Input.GetAxis("Vertical") * Speed);
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
}
