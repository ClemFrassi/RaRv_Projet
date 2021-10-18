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
        if (um != null && ((hit.tag == "KMS" && gameObject.tag == "Viral") || (hit.tag == "VR" && gameObject.tag == "Antiviral")))
        {
            Debug.Log("  It is a player !!");
            Debug.Log(um);
            um.HitByCharge();
        } else if (hit.GetComponent<ShieldBehaviour>() && hit.tag == "SHIELD" && gameObject.tag == "Antiviral")
        {
            Debug.Log("It's a shield");
            hit.GetComponent<ShieldBehaviour>().Hit();
        }
        //Destroy(gameObject);
    }
}