using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VR_Overlay : MonoBehaviour
{
    // Start is called before the first frame update
    public Image HealthValue;
    public Text HealthText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealthValue(int value)
    {
        float val = value;
        float total = GameConfig.GetInstance().LifeNumber;
        HealthValue.fillAmount = val / total;
        HealthText.text = value + "/" + GameConfig.GetInstance().LifeNumber;

    }

    public void ResetLife()
    {
        HealthValue.fillAmount = 1;
        HealthText.text = GameConfig.GetInstance().LifeNumber + "/" + GameConfig.GetInstance().LifeNumber;
    }
}
