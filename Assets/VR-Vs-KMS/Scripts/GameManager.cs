using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject AreaContainer;
    private int VRcontamination;
    private int KMScontamination;
    public Canvas EndGameCanvas;
    public RawImage ColoredBackground;
    public Text EndText;
    public Camera mainCam;

    void Start()
    {
        //remplir la liste des zone de contamination
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Contamined(int id)
    {
        switch (id) {

            case 0 :
                VRcontamination++;
                break;

            case 1:
                KMScontamination++;
                break;
        }

        CheckScore();
    }

    void CheckScore()
    {
        if (KMScontamination >= GameConfig.GetInstance().NbContaminatedPlayerToVictory || VRcontamination >= GameConfig.GetInstance().NbContaminatedPlayerToVictory)
        {
            EndGame();
        }
    }

    void CheckContamination()
    {
       //check les zones
    }

    void EndGame()
    {
        if (mainCam.CompareTag("VR"))
        {
            EndGameCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            EndGameCanvas.planeDistance = 1;
            EndGameCanvas.worldCamera = mainCam;
            EndGameCanvas.gameObject.SetActive(true);

            if (VRcontamination == GameConfig.GetInstance().NbContaminatedPlayerToVictory /* || zone toute capturée */)
            {
                Victory();
            } else if (KMScontamination == GameConfig.GetInstance().NbContaminatedPlayerToVictory /* || zone toute capturée par KMS*/)
            {
                Defeat();
            }
        }

        if (mainCam.CompareTag("MainCamera"))
        {
            EndGameCanvas.gameObject.SetActive(true);
            EndGameCanvas.planeDistance = 1;

            if (KMScontamination == GameConfig.GetInstance().NbContaminatedPlayerToVictory /* || zone toute capturée */)
            {
                Victory();
            }
            else if (VRcontamination == GameConfig.GetInstance().NbContaminatedPlayerToVictory /* || zone toute capturée par KMS*/)
            {
                Defeat();
            }
        }

    }

    void Victory()
    {
       
        EndText.text = "VICTORY";
        ColoredBackground.color = Color.green;

    }

    void Defeat()
    {
        
        EndText.text = "DEFEAT";
        ColoredBackground.color = Color.red;
    }
}
