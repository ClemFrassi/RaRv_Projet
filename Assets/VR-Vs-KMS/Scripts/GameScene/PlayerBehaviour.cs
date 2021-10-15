using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviourPunCallbacks, IPunObservable
{
    // Start is called before the first frame update
    private int Life;
    public GameObject SpawnerContainer;
    private List<Transform> spawnPoints;
    public GameObject Scientific;
    public VR_Overlay Overlay;
    private Animator animator;
    public ShieldBehaviour Shield;
    public TPSShootController KMScanShoot;
    public ControllerInputShoot VRcanShoot;

    void Start()
    {
        if(gameObject.tag == "KMS")
        {
            animator = GetComponentInParent<Animator>();
        }
        Life = GameConfig.GetInstance().LifeNumber;
        spawnPoints = new List<Transform>();
        SpawnerContainer = GameObject.Find("SpawnAreaContainer");
        GetSpawners();
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HitByCharge()
    {
        if(photonView.IsMine)
        {
            Hit();
            if (Life <= 0)
            {
                if(gameObject.CompareTag("KMS")) {  
                    StartCoroutine(WaitForAnim());
                } else if (gameObject.CompareTag("VR"))
                {
                    Respawn();
                }
               
                
            }
        }
        
    }

    public void Respawn()
    {
        if(photonView.IsMine)
        {
            if(gameObject.CompareTag("VR"))
            {
                VRcanShoot.canShoot = true;
            } else if (gameObject.CompareTag("KMS"))
            {
                KMScanShoot.canShoot = true;
            }

            Scientific.SetActive(false);
            Scientific.transform.position = spawnPoints[RandomSpawn()].position;
            ResetLifePoints();
            if (gameObject.CompareTag("VR"))
            {
                ResetShield();
            }
            Scientific.SetActive(true);
        }
        
    }

    public void GetSpawners()
    {
        foreach (Transform spawn in SpawnerContainer.GetComponentsInChildren<Transform>())
        {
            spawnPoints.Add(spawn);
        }
    }

    public int RandomSpawn()
    {
        return Random.Range(1, spawnPoints.Count-1);
    }

    public void ResetLifePoints()
    {
        Life = GameConfig.GetInstance().LifeNumber;
        if(gameObject.tag == "VR")
        {
            Overlay.ResetLife();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Life);
            if (gameObject.tag == "KMS")
            {
                stream.SendNext(animator.GetFloat("x_direction"));
                stream.SendNext(animator.GetFloat("z_direction"));
            }
        }
        else
        {
            Life = (int)stream.ReceiveNext();
            if (gameObject.tag == "KMS")
            {
                animator.SetFloat("x_direction", (float)stream.ReceiveNext());
                animator.SetFloat("z_direction", (float)stream.ReceiveNext());
            }
        }
    }

    public void Hit()
    {
        Debug.Log("MODIFY");
        Life--;
        if (gameObject.tag == "VR")
        {
            Overlay.SetHealthValue(Life);
        }
    }

    public void ResetShield()
    {
        Shield.Repair();
    }

    IEnumerator WaitForAnim()
    {
        Scientific.GetComponent<PlayerMovements>().enabled = false;
        Debug.Log("CAN'T MOVE + TRIGGER ANIM");
        GetComponentInParent<Animator>().SetTrigger("triggerDead");
        yield return new WaitForSeconds(5);
        Scientific.GetComponent<PlayerMovements>().enabled = true;
        Respawn();
    }
}
