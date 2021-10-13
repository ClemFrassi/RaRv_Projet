﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSShootController : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject ChargePrefab;
    public int force = 50;

    private bool canShoot;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.yellow, 1.5f);
            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                if (canShoot)
                {
                    canShoot = false;
                    //SHOOT
                    photonView.RPC("Shoot", RpcTarget.AllViaServer, hit);
                    //IENUMERABLE
                    StartCoroutine(Reload());

                }
            }
        }
    }

    [PunRPC]
    public void Shoot(RaycastHit hit)
    {
        GameObject Charge = Instantiate(ChargePrefab, gameObject.transform.position, gameObject.transform.rotation);
        Charge.GetComponent<ChargeController>().SetTag("Antiviral");
        Charge.GetComponent<Rigidbody>().velocity = (hit.point - transform.position) * force;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(GameConfig.GetInstance().DelayShoot);
        canShoot = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
