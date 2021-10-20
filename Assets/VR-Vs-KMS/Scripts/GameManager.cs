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
        if (KMScontamination >= GameConfig.GetInstance().NbContaminatedPlayerToVictory)
        {
            //KMS WIN
        }

        if (VRcontamination >= GameConfig.GetInstance().NbContaminatedPlayerToVictory)
        {
            //VR WIN
        }
    }

    void EndGame()
    {
        EndGameCanvas.gameObject.SetActive(true);
        EndGameCanvas.worldCamera = Camera.current;
        EndGameCanvas.planeDistance = 1;

        if (Camera.current.CompareTag("VR"))
        {

        }

    }
}
