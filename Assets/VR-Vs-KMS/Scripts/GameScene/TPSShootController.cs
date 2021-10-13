using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSShootController : MonoBehaviour
{
    public GameObject ChargePrefab;
    public int force = 50;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.yellow, 1.5f);
            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                Debug.Log(hit.collider.name);
                GameObject Charge = Instantiate(ChargePrefab, gameObject.transform.position, gameObject.transform.rotation);
                Charge.GetComponent<ChargeController>().SetTag("Antiviral");
                Charge.GetComponent<Rigidbody>().velocity = (hit.point - transform.position) * force;

                //Rigidbody ballRb = hit.rigidbody;
                //ballRb.AddForce((hit.transform.localPosition - hit.point) * force);
            }
        }
    }

    public void Shoot()
    {
        GameObject Charge = Instantiate(ChargePrefab, gameObject.transform.position, gameObject.transform.rotation);
        Charge.GetComponent<ChargeController>().SetTag("Antiviral");
        Charge.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * force);
    }
}
