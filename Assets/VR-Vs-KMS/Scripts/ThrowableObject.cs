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
        ready = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(ready)
        {
            explosive = true;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!explosive)
        {
            return;
        }

        
        if(other.gameObject.GetComponent<PlayerBehaviour>())
        {
            photonView.RPC("Explosion", RpcTarget.AllViaServer, other.GetComponent<PlayerBehaviour>().photonView.ViewID);   
        }
        

    }

    [PunRPC]
    private void Explosion(int userID, PhotonMessageInfo info)
    {
        Debug.Log("LAUNCHED");
        GameObject other =  PhotonView.Find(userID).gameObject;
        Debug.Log("NOM DE l'objet : " + other.name);
        Debug.Log("TAG DE l'objet : " + other.tag);
        if (other.CompareTag("KMS") || other.CompareTag("VR"))
        {
            Debug.Log("DEGATS SUR : " + other.name);
            other.gameObject.GetComponent<PlayerBehaviour>().HitByCharge();
        }

        Destroy(gameObject);
    }
}
