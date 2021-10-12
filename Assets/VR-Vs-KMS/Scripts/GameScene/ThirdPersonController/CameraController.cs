using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Animator animator;

    public enum RotationAxis
    {
        X = 1,
        Y = 2
    }

    public RotationAxis axes = RotationAxis.X;
    public float SensHorizontal = 10.0f;
    public float SensVertical = 10.0f;

    public Light FlashLight;

    public float MinimumVerticalAngle = -75.0f;
    public float MaximumVerticalAngle = 55.0f;

    private float rotationX = 0;
    private float rotationY = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxis.X)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * SensHorizontal, 0, Space.Self);
        }
        else if (axes == RotationAxis.Y)
        {
            if (Input.GetButton("Fire2"))
            {
                animator.SetBool("isTarget", true);
            }
            else
            {
                animator.SetBool("isTarget", false);
            }

            rotationX -= Input.GetAxis("Mouse Y") * SensVertical;
            rotationX = Mathf.Clamp(rotationX, MinimumVerticalAngle, MaximumVerticalAngle);
            rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
    }
}

