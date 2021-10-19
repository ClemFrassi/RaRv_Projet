using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VR_Overlay : MonoBehaviour
{
    // Start is called before the first frame update
    public Image HealthValue;
    public Text HealthText;
    public Image ShieldValue;
    public Text ShieldText;
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

    public void SetShieldValue(int value)
    {
        float val = value;
        float total = 5;
        ShieldValue.fillAmount = val / total;
        ShieldText.text = value + "/" + total;

    }

    public void ResetShield()
    {
        ShieldValue.fillAmount = 1;
        ShieldText.text = 5 + "/" + 5;
    }
}
