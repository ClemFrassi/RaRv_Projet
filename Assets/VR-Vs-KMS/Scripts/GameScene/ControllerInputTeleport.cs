using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class ControllerInputTeleport : MonoBehaviourPunCallbacks, IPunObservable
{
    // Start is called before the first frame update
    private SteamVR_Input_Sources inputSource;

    public GameObject cameraRig;
    public GameObject TeleportParticles;

    private bool canTeleport;


    void Awake()
    {
        inputSource = gameObject.GetComponent<SteamVR_Behaviour_Pose>().inputSource;
    }
    void Start()
    {
        canTeleport = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions._default.Teleport.GetStateDown(inputSource))
        {
                TeleportPressed();
        }

        if (SteamVR_Actions._default.Teleport.GetStateUp(inputSource))
        {
            if (canTeleport)
            {
                TeleportReleased();
            } else
            {
                Released();
            }
        }
    }

    private void TeleportPressed()
    {
        ControllerPointer cp = gameObject.AddComponent<ControllerPointer>();
    }

    private void TeleportReleased()
    {

        ControllerPointer cp = gameObject.GetComponent<ControllerPointer>();
        if (cp.CanTeleport)
        {
            Teleport(cameraRig.transform.position, cp.TargetPosition);
            //photonView.RPC("Teleport", RpcTarget.Others, cameraRig.transform.position, cp.TargetPosition);
            cameraRig.transform.position = cp.TargetPosition;
            canTeleport = false;
            StartCoroutine(ResetTeleport());
        }
        cp.DesactivatePointer();
        Destroy(cp);
    }

    private void Released()
    {
        ControllerPointer cp = gameObject.GetComponent<ControllerPointer>();
        cp.DesactivatePointer();
        Destroy(cp);
    }

    IEnumerator ResetTeleport()
    {
        yield return new WaitForSeconds(GameConfig.GetInstance().DelayTeleport);
        canTeleport = true;
    }

    //[PunRPC]
    void Teleport(Vector3 startPosition, Vector3 targetPosition)
    {
        GameObject particles = Instantiate(TeleportParticles, startPosition, gameObject.transform.rotation);
        particles.transform.LookAt(targetPosition);
        StartCoroutine(MoveOverSeconds(particles, targetPosition, 1f));
        //particles.transform.position = Vector3.Lerp(startPosition, targetPosition, 1f);
        //Destroy(particles, 1f);
    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
