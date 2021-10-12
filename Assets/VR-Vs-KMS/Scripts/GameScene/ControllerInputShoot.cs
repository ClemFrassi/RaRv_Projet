using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerInputShoot : MonoBehaviour
{
    // Start is called before the first frame update
    private SteamVR_Input_Sources inputSource;
    public GameObject ChargePrefab;

    public int force;

    private bool canShoot;

    void Awake()
    {
        inputSource = gameObject.GetComponent<SteamVR_Behaviour_Pose>().inputSource;
    }
    void Start()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(inputSource))
        {
            if (canShoot)
            {
                canShoot = false;
                //SHOOT
                Shoot();
                //IENUMERABLE
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(GameConfig.GetInstance().DelayShoot);
        canShoot = true;
    }

    public void Shoot()
    {
        GameObject Charge = Instantiate(ChargePrefab, gameObject.transform.position, gameObject.transform.rotation);
        Charge.GetComponent<ChargeController>().SetTag("Viral");
        Charge.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * force);

    }
}
