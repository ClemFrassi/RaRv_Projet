using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private bool explosive;
    private bool ready;
    void Start()
    {
        explosive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Priming()
    {
        Debug.Log("PRIMING");
        ready = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(ready)
        {
            Debug.Log("EXPLOSIVE");
            explosive = true;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!explosive)
        {
            return;
        }

        Debug.Log("DEGATS");
        photonView.RPC("Explosion", RpcTarget.AllViaServer, other);

    }


    private void Explosion(Collider other, PhotonMessageInfo info)
    {
        if (other.CompareTag("KMS") || other.CompareTag("VR"))
        {
            Debug.Log("DEGATS SUR : " + other.name);
            other.gameObject.GetComponent<PlayerBehaviour>().HitByCharge();
        }

        Destroy(gameObject);
    }
}
