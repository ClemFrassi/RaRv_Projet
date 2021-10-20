using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerInputShoot : MonoBehaviourPunCallbacks, IPunObservable
{
    // Start is called before the first frame update
    private SteamVR_Input_Sources inputSource;

    private GameObject selectedObject;
    private bool isGrabbingPinch;
    public GameObject ChargePrefab;

    public int force;

    public bool canShoot;

    void Awake()
    {
        inputSource = gameObject.GetComponent<SteamVR_Behaviour_Pose>().inputSource;
    }
    void Start()
    {
        canShoot = true;
        this.enabled = photonView.IsMine;
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(inputSource))
        {
            if (canShoot)
            {
                Debug.Log("Shooting + " + canShoot);
                canShoot = false;
                Debug.Log("canShoot + " + canShoot);
                //SHOOT
                photonView.RPC("Shoot", RpcTarget.AllViaServer);
                //IENUMERABLE
                StartCoroutine(Reload());
            }
        }

        if (SteamVR_Actions._default.Teleport.GetStateDown(inputSource))
        {
            if (selectedObject)
            {
                isGrabbingPinch = true;
                GrabSelectedObject();
            }
        }

        if (SteamVR_Actions._default.Teleport.GetStateUp(inputSource))
        {
            isGrabbingPinch = false;
            UngrabSelectedObject();
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(GameConfig.GetInstance().DelayShoot);
        canShoot = true;
    }

    [PunRPC]
    void Shoot(PhotonMessageInfo info)
    {   // Tips for Photon lag compensation. Il faut compenser le temps de lag pour l'envoi du message.
        // donc décaler la position de départ de la balle dans la direction
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        Debug.LogFormat("PunRPC: ThrowBall lag:{0}", lag);

        GameObject Charge = Instantiate(ChargePrefab, gameObject.transform.position, gameObject.transform.rotation);
        Charge.GetComponent<ChargeController>().SetTag("Viral");
        Charge.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * force);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrabbableObject>())
        {
            Debug.Log("OBJET SELECTIONNER");
            selectedObject = other.gameObject;
            Debug.Log(selectedObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (selectedObject == other.gameObject)
        {
            selectedObject = null;
        }
    }

    private void GrabSelectedObject()
    {
        selectedObject.GetComponent<ThrowableObject>().Priming();
        PhotonView selectedPhoton = selectedObject.GetPhotonView();
        selectedPhoton.TransferOwnership(photonView.ControllerActorNr);
        if (photonView.IsMine)
        {
            FixedJoint fx = gameObject.AddComponent<FixedJoint>();
            fx.breakForce = 20000;
            fx.breakTorque = 20000;
            fx.connectedBody = selectedObject.GetComponent<Rigidbody>();
        }
        

    }

    private void UngrabSelectedObject()
    {
        if (photonView.IsMine)
        {
            if (gameObject.GetComponent<FixedJoint>())
            {
                FixedJoint fx = gameObject.GetComponent<FixedJoint>();
                fx.connectedBody = null;
                Destroy(fx);
                selectedObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity() * 2;
            }
        }
            
    }
}
