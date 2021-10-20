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
        if (Camera.main.CompareTag("VR"))
        {
            if (VRcontamination == GameConfig.GetInstance().NbContaminatedPlayerToVictory /* || zone toute capturée */)
            {
                Victory();
            } else if (KMScontamination == GameConfig.GetInstance().NbContaminatedPlayerToVictory /* || zone toute capturée par KMS*/)
            {
                Defeat();
            }
        }

        if (Camera.main.CompareTag("MainCamera"))
        {
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
        EndGameCanvas.gameObject.SetActive(true);
        EndGameCanvas.worldCamera = Camera.current;
        EndGameCanvas.planeDistance = 1;
        EndText.text = "VICTORY";
        ColoredBackground.color = Color.green;

    }

    void Defeat()
    {
        EndGameCanvas.gameObject.SetActive(true);
        EndGameCanvas.worldCamera = Camera.current;
        EndGameCanvas.planeDistance = 1;
        EndText.text = "DEFEAT";
        ColoredBackground.color = Color.red;
    }
}
