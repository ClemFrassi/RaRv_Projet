using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Material VirusShotMaterial;
    public Material KMSShotMaterial;

    void Awake()
    {
        GameConfig.GetInstance().LoadConfig();
        Debug.Log("AwakeDone");
    }

    void Start()
    {
        VirusShotMaterial.color = GameConfig.GetInstance().ColorShotVirus;
        KMSShotMaterial.color = GameConfig.GetInstance().ColorShotKMS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
