using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        photonView.RPC("Restart", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
