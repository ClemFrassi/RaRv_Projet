using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public GameObject joinGameButton;
    [SerializeField]
    public GameObject cancelButton;
    [SerializeField]
    public int RoomSize;

    //Callback function for when the first connection is established
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        joinGameButton.SetActive(true);
        cancelButton.SetActive(false);
    }

    public void JoinGame() //Paired to the Join Game button
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom(); //First tries to koin an existing room
            Debug.Log("Joining a room");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    //Callback function for when the random room join failed
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    //trying to create our own room
    private void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 1000); //creating a random name for the room
        RoomOptions rommOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, rommOps); //attempting to create a new room
        Debug.Log("Room" + randomRoomNumber);
    }

    //Callback function for when the room creatioin failed
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room... trying again");
        CreateRoom(); //Retrying to create a new room with a different name
    }

    public void QuickCancel() //Paired to the cancel button. Used to stop looking for a room
    {
        cancelButton.SetActive(false);
        joinGameButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
