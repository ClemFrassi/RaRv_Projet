using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeOverlay : MonoBehaviour
{
    public PlayerBehaviour player;
    public Image HealthValue;
    public Text HealthText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //int life = player.Life;
        //HealthValue.fillAmount = life / GameConfig.GetInstance().LifeNumber;
        //HealthText.text = life + "/" + GameConfig.GetInstance().LifeNumber;
    }
}
