using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VR_Overlay : MonoBehaviour
{
    // Start is called before the first frame update
    public Image HealthValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealthValue(int value)
    {
        HealthValue.fillAmount = (value * 100) / GameConfig.GetInstance().LifeNumber;

    }
}
