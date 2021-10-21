using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject explosionPrefab;

    private bool explosive;
    private bool ready;
    private bool exploded;
    private GameObject ParticleObject;
    private List<Collider> inside;

    

    void Start()
    {
        explosive = false;
        ready = false;
        exploded = false;
        inside = new List<Collider>();
        gameObject.GetComponent<SphereCollider>().radius = GameConfig.GetInstance().RadiusExplosion;
        gameObject.GetComponent<SphereCollider>().enabled = false;
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
            gameObject.GetComponent<SphereCollider>().enabled = true;
            photonView.RPC("Particle", RpcTarget.AllViaServer);
            photonView.RPC("Explosion", RpcTarget.AllViaServer);
            //ready = false;    
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        inside.Add(other);
    }

    public void OnTriggerExit(Collider other)
    {
        inside.Remove(other);
    }


    private void OnTriggerStay(Collider other)
    {
        if (!explosive)
        {
            return;
        }

        if (other.gameObject.GetComponent<PlayerBehaviour>())
        {
            
        } 

    }

    [PunRPC]
    private void Explosion(PhotonMessageInfo info)
    {
        foreach (Collider coll in inside)
        {
            if (coll.gameObject.GetComponentInChildren<PlayerBehaviour>())
            {
                coll.GetComponentInChildren<PlayerBehaviour>().HitByCharge();
            }
        }
        photonView.RPC("Destroy", RpcTarget.AllViaServer);
    }

    /*private void Explosion(GameObject other)
    {
        //GameObject other = PhotonView.Find(userID).gameObject;
        Debug.Log(other.name);
        other.GetComponentInChildren<PlayerBehaviour>().HitByCharge();
    }*/

    [PunRPC]
    private void Particle(PhotonMessageInfo info)
    {
        ParticleObject = Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
    }

    [PunRPC]
    private void Destroy(PhotonMessageInfo info)
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }
}
