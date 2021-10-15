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
                    GetComponentInParent<Animator>().SetTrigger("triggerDead"); 
                }
                
                // Camera.main.GetComponent<Animator>().SetBool("isDead", true);
                Respawn();
            }
        }
        
    }

    public void Respawn()
    {
        if(photonView.IsMine)
        {
            Scientific.SetActive(false);
            Scientific.transform.position = spawnPoints[RandomSpawn()].position;
            ResetLifePoints();
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
        Overlay.ResetLife();
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
        Overlay.SetHealthValue(Life);
    }
}
