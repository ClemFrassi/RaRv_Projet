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
            ready = false;    
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.CompareTag("VR") || other.gameObject.CompareTag("KMS")) && !other.gameObject.GetComponent<ShieldBehaviour>())
        {
                photonView.RPC("AddInside", RpcTarget.AllViaServer, other.gameObject.GetPhotonView().ViewID);
                Debug.Log("ADDED : " + other.name);   
        }
        
        
    }

    public void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.CompareTag("VR") || other.gameObject.CompareTag("KMS")) && !other.gameObject.GetComponent<ShieldBehaviour>())
        {
            photonView.RPC("RemoveInside", RpcTarget.AllViaServer, other.gameObject.GetPhotonView().ViewID);
            Debug.Log("REMOVED : " + other.name);
        }
    }

    [PunRPC]
    private void Explosion(PhotonMessageInfo info)
    {
        Debug.Log("EXPLOSIION");
        foreach (Collider coll in inside)
        {
            Debug.Log("NOM : " + coll.gameObject.name);
            Debug.Log("TAG : " + coll.gameObject.tag);

            if (coll.gameObject.CompareTag("VR"))
            {
                Debug.Log("NAME : " + coll.gameObject.name + " IN CHILDREN ");
                if (coll.gameObject.GetComponentInChildren<PlayerBehaviour>())
                {
                    coll.gameObject.GetComponentInChildren<PlayerBehaviour>().HitByCharge();
                }
                

            } else if (coll.gameObject.CompareTag("KMS"))
            {
                Debug.Log("NAME : " + coll.gameObject.name + " IN GAMEOBJECT ");

                if (coll.gameObject.GetComponent<PlayerBehaviour>())
                {
                    Debug.Log("HAVE PLAYERBEHAVIOUR");
                    coll.gameObject.GetComponent<PlayerBehaviour>().HitByCharge();
                    Debug.Log("HIT");
                }
                
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
        //gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }

    [PunRPC]
    private void AddInside(int id)
    {
        Collider coll = PhotonView.Find(id).GetComponent<Collider>();
        inside.Add(coll);
    }

    [PunRPC] 
    private void RemoveInside(int id)
    {
        Collider coll = PhotonView.Find(id).GetComponent<Collider>();
        inside.Remove(coll);
    }
}
