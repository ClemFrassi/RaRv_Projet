using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSShootController : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject ChargePrefab;
    public int force = 50;

    private bool canShoot;

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        this.enabled = photonView.IsMine;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
            {
                if (canShoot)
                {
                    canShoot = false;
                    //SHOOT
                    Vector3 direction = (raycastHit.point - transform.position).normalized;
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
        Charge.GetComponent<Rigidbody>().velocity = direction * force;
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
