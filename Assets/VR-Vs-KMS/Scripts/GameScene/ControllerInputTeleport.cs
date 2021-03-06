using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class ControllerInputTeleport : MonoBehaviour
{
    // Start is called before the first frame update
    private SteamVR_Input_Sources inputSource;

    public GameObject cameraRig;

    public bool canTeleport;


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
}
