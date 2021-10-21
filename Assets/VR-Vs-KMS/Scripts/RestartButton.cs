using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject Canvas;
    void Start()
    {
        Canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Canvas.CompareTag("VR"))
        {
            gameObject.SetActive(false);
        }
    }

    public void RestartGame()
    {
        photonView.RPC("Restart", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void Restart(PhotonMessageInfo info)
    {   
        SceneManager.LoadScene("GameScene");
    }
}
