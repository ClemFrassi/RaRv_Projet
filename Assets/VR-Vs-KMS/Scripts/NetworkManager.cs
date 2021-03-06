using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static NetworkManager Instance;

    [Tooltip("The prefab to use for representing the user on a PC. Must be in Resources folder")]
    public GameObject playerPrefabPC;

    [Tooltip("The prefab to use for representing the user in VR. Must be in Resources folder")]
    public GameObject playerPrefabVR;


    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. 
    /// </summary>
    public override void OnLeftRoom()
    {
        // TODO: load the Lobby Scene
        SceneManager.LoadScene("LobbyScene");
    }

    /// <summary>
    /// Called when Other Player enters the room and Only other players
    /// </summary>
    /// <param name="other"></param>
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
                                                                        // TODO: 

    }

    /// <summary>
    /// Called when Other Player leaves the room and Only other players
    /// </summary>
    /// <param name="other"></param>
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
        // TODO: 
    }
    #endregion


    #region Public Methods

    /// <summary>
    /// Our own function to implement for leaving the Room
    /// </summary>
    public void LeaveRoom()
    {
        // TODO: 
        PhotonNetwork.LeaveRoom();
    }

    private void updatePlayerNumberUI()
    {
        // TODO: Update the playerNumberUI

    }

    void Start()
    {
        Instance = this;
        #region TO debug
        Debug.Log("device:" + UserDeviceManager.GetDeviceUsed());
        Debug.Log("prefab:" + UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(playerPrefabPC, playerPrefabVR));
        #endregion

        GameObject playerPrefab = UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(playerPrefabPC, playerPrefabVR);
        if (playerPrefab == null)
        {
            Debug.LogErrorFormat("<Color=Red><a>Missing</a></Color> playerPrefab Reference for device {0}. Please set it up in GameObject 'NetworkManager'", UserDeviceManager.GetDeviceUsed());
        }
        else
        {
            // TODO: Instantiate the prefab representing my own avatar only if it is UserMe
            if (UserManager.UserMeInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                Vector3 initialPos = UserDeviceManager.GetDeviceUsed() == UserDeviceType.HTC ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 5f, 0f);
                PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, initialPos, Quaternion.identity, 0);
                Debug.Log("Just instantiated the " + UserDeviceManager.GetDeviceUsed() + "player for Luc!");
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            // Code to leave the room by pressing CTRL + the Leave button
            if (Input.GetButtonUp("Leave"))
            {
                Debug.Log("Leave event");
                LeaveRoom();
            }
        }

    }
    #endregion
}