using Photon.Pun;
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
        this.enabled = photonView.IsMine;
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
                    Vector3 direction = hit.point - transform.position;
                    photonView.RPC("Shoot", RpcTarget.AllViaServer, direction);
                    //IENUMERABLE
                    StartCoroutine(Reload());

                }
            }
        }
    }

    [PunRPC]
    void Shoot(Vector3 direction, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject Charge = Instantiate(ChargePrefab, gameObject.transform.position, gameObject.transform.rotation);
        Charge.GetComponent<ChargeController>().SetTag("Antiviral");
        Charge.GetComponent<Rigidbody>().velocity = direction * force * lag;
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
