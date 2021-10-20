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

    

    void Start()
    {
        explosive = false;
        ready = false;
        exploded = false;
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
            if(!explosive)
            {
                explosive = true;
                photonView.RPC("Particle", RpcTarget.AllViaServer);
            }
            
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!explosive)
        {
            return;
        }

        if (other.gameObject.GetComponentInChildren<PlayerBehaviour>())
        {
            photonView.RPC("Explosion", RpcTarget.AllViaServer, other.GetComponent<PlayerBehaviour>().photonView.ViewID);
            explosive = false;
        }
        StartCoroutine(Delete());    

    }

    [PunRPC]
    private void Explosion(int userID, PhotonMessageInfo info)
    {
        GameObject other =  PhotonView.Find(userID).gameObject;
        other.GetComponentInChildren<PlayerBehaviour>().HitByBomb();
    }

    [PunRPC]
    private void Particle(PhotonMessageInfo info)
    {
        ParticleObject = Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
    }

    [PunRPC]
    private void Destroy(PhotonMessageInfo info)
    {
        Destroy(gameObject);
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(1);
        photonView.RPC("Destroy", RpcTarget.AllViaServer);

    }
}
