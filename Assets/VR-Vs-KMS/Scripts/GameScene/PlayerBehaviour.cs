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
    public ControllerInputTeleport VRcanTP;
    public AudioSource hit;
    public AudioSource death;
    public AudioSource respawn;
    public GameManager gameManager;
    public Camera actualcamera;
    public GameObject BlackScreen;
    public List<GameObject> VRGO;
    public bool isDead;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (photonView.IsMine)
        {
            gameManager.mainCam = actualcamera;
        }
        
        if (gameObject.tag == "KMS")
        {
            animator = GetComponentInParent<Animator>();
        }
        Life = GameConfig.GetInstance().LifeNumber;
        spawnPoints = new List<Transform>();
        SpawnerContainer = GameObject.Find("SpawnAreaContainer");
        isDead = false;
        GetSpawners();
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HitByCharge()
    {
        if (photonView.IsMine)
        {
            Hit();
            if (Life > 0)
            {
                hit.Play();
            }
            else if (Life == 0 && !isDead)
            {
                death.Play();
                if(gameObject.CompareTag("KMS")) {  
                    StartCoroutine(WaitForAnim());
                    gameManager.Contamined(0);
                }
                else if (gameObject.CompareTag("VR"))
                {
                    StartCoroutine(KillingVR());
                    gameManager.Contamined(1);
                }


            }
        }

    }

    public void Respawn()
    {
        if (photonView.IsMine)
        {
            if (gameObject.CompareTag("VR"))
            {
                VRcanShoot.canShoot = true;
                VRcanTP.canTeleport = true;
            }
            else if (gameObject.CompareTag("KMS"))
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
            Overlay.ResetLife();
            respawn.Play();
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
        return Random.Range(1, spawnPoints.Count - 1);
    }

    public void ResetLifePoints()
    {
        Life = GameConfig.GetInstance().LifeNumber;
        if (gameObject.tag == "VR")
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
        if (Life > 0)
        {
            Life--;
        }
        if (gameObject.tag == "VR" || gameObject.tag == "KMS")
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
        isDead = true;
        Scientific.GetComponent<PlayerMovements>().enabled = false;
        Scientific.GetComponent<CameraController>().enabled = false;
        BlackScreen.SetActive(true);
        Debug.Log("CAN'T MOVE + TRIGGER ANIM");
        GetComponentInParent<Animator>().SetTrigger("triggerDead");
        yield return new WaitForSeconds(5);
        Scientific.GetComponent<PlayerMovements>().enabled = true;
        Scientific.GetComponent<CameraController>().enabled = true;
        BlackScreen.SetActive(false);
        isDead = false;
        Respawn();
    }

    IEnumerator KillingVR()
    {
        isDead = true;
        BlackScreen.SetActive(true);
        foreach(GameObject GO in VRGO)
        {
            GO.SetActive(false);
        }
        yield return new WaitForSeconds(5);
        foreach (GameObject GO in VRGO)
        {
            GO.SetActive(true);
        }
        BlackScreen.SetActive(false);
        isDead = false;
        Respawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;

        if ((hit.CompareTag("Viral") && gameObject.CompareTag("KMS")) || (hit.CompareTag("Antiviral") && gameObject.CompareTag("VR")))
        {
            HitByCharge();
            Destroy(hit);
        }

        /*if (um != null && ((hit.CompareTag("KMS") && gameObject.CompareTag("Viral")) || (hit.CompareTag("VR") && gameObject.CompareTag("Antiviral"))))
        {
            Debug.Log("  It is a player !!");
            Debug.Log(um);
            um.HitByCharge();
            Destroy(gameObject);
        }*/
    }
}
