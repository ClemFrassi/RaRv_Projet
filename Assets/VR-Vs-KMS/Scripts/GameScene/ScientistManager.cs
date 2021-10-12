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
}
