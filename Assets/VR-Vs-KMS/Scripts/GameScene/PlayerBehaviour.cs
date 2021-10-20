﻿using Photon.Pun;
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
    public AudioSource hit;
    public AudioSource death;
    public AudioSource respawn;
    public CapsuleCollider scientistCollider;
    public GameObject BlackScreen;
    public List<GameObject> VirusBody;

    void Start()
    {
        if (gameObject.tag == "KMS")
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
        if (photonView.IsMine)
        {
            Hit();
            if (Life > 0)
            {
                hit.Play();
            }
            else if (Life == 0)
            {
                death.Play();
                if(gameObject.CompareTag("KMS")) {  
                    StartCoroutine(WaitForAnim());
                }
                else if (gameObject.CompareTag("VR"))
                {
                    StartCoroutine(VirusKilled());
                }


            }
        }

    }

    public void HitByBomb()
    {
        if (photonView.IsMine)
        {
            Hit();
            if (Life == 0)
            {
                if (gameObject.CompareTag("KMS"))
                {
                    StartCoroutine(WaitForAnim());
                }
                else if (gameObject.CompareTag("VR"))
                {
                    Respawn();
                }


            }
            Debug.Log(gameObject.name + " killed by bomb");
        }

    }

    public void Respawn()
    {
        if (photonView.IsMine)
        {
            if (gameObject.CompareTag("VR"))
            {
                VRcanShoot.canShoot = true;
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
        scientistCollider.enabled = false;
        Scientific.GetComponent<PlayerMovements>().enabled = false;
        Scientific.GetComponent<CameraController>().enabled = false;
        Debug.Log("CAN'T MOVE + TRIGGER ANIM");
        yield return new WaitForSeconds(3);
        StartCoroutine(BlackScreenDisplaying(2));
        Scientific.GetComponent<PlayerMovements>().enabled = true;
        Scientific.GetComponent<CameraController>().enabled = true;
        scientistCollider.enabled = true;
        Respawn();
    }

    IEnumerator VirusKilled()
    {
        BlackScreen.SetActive(true);
        foreach (GameObject element in VirusBody)
        {
            element.SetActive(false);
        }
        yield return new WaitForSeconds(5);
        foreach (GameObject element in VirusBody)
        {
            element.SetActive(true);
        }
        BlackScreen.SetActive(false);
        Respawn();
    }

    IEnumerator BlackScreenDisplaying(float time)
    {
        BlackScreen.SetActive(true);
        yield return new WaitForSeconds(time);
        BlackScreen.SetActive(false);
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
