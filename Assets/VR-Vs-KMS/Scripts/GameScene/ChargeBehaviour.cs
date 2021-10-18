using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        Debug.Log("Charge hit something:" + hit);

        
        PlayerBehaviour um = hit.GetComponent<PlayerBehaviour>();
       /* if (hit.GetComponent<ShieldBehaviour>() && hit.CompareTag("SHIELD") && gameObject.CompareTag("Antiviral"))
        {
            Debug.Log("It's a shield");
            hit.GetComponent<ShieldBehaviour>().Hit();
            Destroy(gameObject);
        }*/

        if (um != null && ((hit.CompareTag("KMS") && gameObject.CompareTag("Viral")) || ( hit.CompareTag("VR") && gameObject.CompareTag("Antiviral"))))
        {
            Debug.Log("  It is a player !!");
            Debug.Log(um);
            um.HitByCharge();
            Destroy(gameObject);
        }

        
        
    }
}