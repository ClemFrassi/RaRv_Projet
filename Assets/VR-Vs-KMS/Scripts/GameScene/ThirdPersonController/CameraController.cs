using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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
            rotationX -= Input.GetAxis("Mouse Y") * SensVertical;
            rotationX = Mathf.Clamp(rotationX, MinimumVerticalAngle, MaximumVerticalAngle);
            rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }

    /* if (Input.GetButtonDown("FlashLight"))
     {
         if (FlashLight)
         {
             FlashLight.enabled = !FlashLight.enabled;
         }
     }*/
}
}

