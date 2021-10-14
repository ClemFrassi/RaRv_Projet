﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        Debug.Log("Charge hit something:" + hit);


        PlayerBehaviour um = hit.GetComponent<PlayerBehaviour>();
        if (um != null && (hit.tag == "KMS" && gameObject.tag == "Viral") || (hit.tag == "KMS" && gameObject.tag == "Antiviral"))
        {
            Debug.Log("  It is a player !!");
            um.HitByCharge();
        }
        Destroy(gameObject);
    }
}