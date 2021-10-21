using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using vr_vs_kms;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject AreaContainer;
    private int VRcontamination;
    private int KMScontamination;
    public Canvas EndGameCanvas;
    public RawImage ColoredBackground;
    public Text EndText;
    public Camera mainCam;

    private int checkAreaState;
    private int victoryInt;

    public List<ContaminationArea> listOfArea;

    void Awake()
    {
        
        listOfArea = new List<ContaminationArea>();
    }

    void Start()
    {
        EndGameCanvas.gameObject.SetActive(false);

        foreach (ContaminationArea area in AreaContainer.GetComponentsInChildren<ContaminationArea>() )
        {
            listOfArea.Add(area);
        }
        checkAreaState = 3;
    }


    // Update is called once per frame
    void Update()
    {
        
        foreach (ContaminationArea area in listOfArea)
        {
            if (checkAreaState == 3)
            {
                checkAreaState = area.state;
            }

            if (checkAreaState != 3)
            {
                if (area.state != checkAreaState || area.state == 0)
                {
                    checkAreaState = 3;
                    return; 
                }
            }
            
        }

        photonView.RPC("VictoryIntContamination", RpcTarget.AllViaServer, checkAreaState);
    }

    public void Contamined(int id)
    {
        photonView.RPC("AddScore", RpcTarget.AllViaServer, id);
    }

    void CheckScore()
    {
        if (KMScontamination >= GameConfig.GetInstance().NbContaminatedPlayerToVictory || VRcontamination >= GameConfig.GetInstance().NbContaminatedPlayerToVictory)
        {
            photonView.RPC("EndGame", RpcTarget.AllViaServer);
        }
    }

    void CheckContamination()
    {
       if (victoryInt == 1 || victoryInt == 2)
        {
            photonView.RPC("EndGame", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    void EndGame(PhotonMessageInfo info)
    {
            if (mainCam.CompareTag("VR"))
            {
                EndGameCanvas.gameObject.SetActive(true);
                EndGameCanvas.tag = "VR";
                EndGameCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                EndGameCanvas.planeDistance = 1;
                EndGameCanvas.worldCamera = mainCam;



                if (VRcontamination == GameConfig.GetInstance().NbContaminatedPlayerToVictory || victoryInt == 1)
                {
                    Victory();
                }
                else if (KMScontamination == GameConfig.GetInstance().NbContaminatedPlayerToVictory || victoryInt == 2)
                {
                    Defeat();
                }
            }

            if (mainCam.CompareTag("MainCamera"))
            {
                EndGameCanvas.gameObject.SetActive(true);
                EndGameCanvas.tag = "KMS";
                EndGameCanvas.planeDistance = 1;

                if (KMScontamination == GameConfig.GetInstance().NbContaminatedPlayerToVictory || victoryInt == 2)
                {
                    Victory();
                }
                else if (VRcontamination == GameConfig.GetInstance().NbContaminatedPlayerToVictory || victoryInt == 1)
                {
                    Defeat();
                }
            }
    }

    void Victory()
    {
        EndText.text = "VICTORY";
        ColoredBackground.color = Color.green;
        Cursor.visible = true;
    }

    void Defeat()
    {
        
        EndText.text = "DEFEAT";
        ColoredBackground.color = Color.red;
        Cursor.visible = true;
    }

    [PunRPC]
    void AddScore(int id, PhotonMessageInfo info)
    {
        switch (id)
        {

            case 0:
                VRcontamination++;
                break;

            case 1:
                KMScontamination++;
                break;
        }

        CheckScore();
    }

    [PunRPC]
    void VictoryIntContamination(int id)
    {
        victoryInt = id;
        CheckContamination();
    }
}
