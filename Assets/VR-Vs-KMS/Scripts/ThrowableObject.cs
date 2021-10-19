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
            Debug.Log("DANS LA ZONE : " + other.name);
            return;
        }

        
        if(other.gameObject.GetComponent<PlayerBehaviour>())
        {
            Debug.Log("DEGATS");
            photonView.RPC("Explosion", RpcTarget.AllViaServer, other.GetComponent<PlayerBehaviour>().photonView.ViewID);   
        }
        

    }


    private void Explosion(int userID, PhotonMessageInfo info)
    {
        Debug.Log("LAUNCHED");
        PhotonView other =  PhotonView.Find(userID);
        Debug.Log("NOM DE l'objet : " + other.name);
        Debug.Log("TAG DE l'objet : " + other.tag);
        if (other.CompareTag("KMS") || other.CompareTag("VR"))
        {
            Debug.Log("DEGATS SUR : " + other.name);
            other.GetComponent<PlayerBehaviour>().HitByCharge();
        }

        Destroy(gameObject);
    }
}
