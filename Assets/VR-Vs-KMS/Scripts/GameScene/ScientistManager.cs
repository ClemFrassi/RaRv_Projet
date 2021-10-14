using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistManager : MonoBehaviourPunCallbacks
{
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("isLocalPlayer:" + photonView.IsMine);
        setTPSCameraRig();
        activateLocalPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Get the GameObject of the CameraRig
    /// </summary>
    public void setTPSCameraRig()
    {
        if (!photonView.IsMine) return;
        try
        {
            // Get the Camera to set as the followed camera
            cam.SetActive(true);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no TPSCameraRig found\n" + ex);
        }
    }

    protected void activateLocalPlayer()
    {
        // enable the ThirdPersonUserControl if it is a Loacl player = UserMe
        // disable the ThirdPersonUserControl if it is not a Loacl player = UserOther
        GetComponent<CameraController>().enabled = photonView.IsMine;
        GetComponent<PlayerMovements>().enabled = photonView.IsMine;
        GetComponent<CharacterController>().enabled = photonView.IsMine;
        GetComponent<Rigidbody>().isKinematic = !photonView.IsMine;
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = photonView.IsMine ? 2 : 0;
        }
    }
}
