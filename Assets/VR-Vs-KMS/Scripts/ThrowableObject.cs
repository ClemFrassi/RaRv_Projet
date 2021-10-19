using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    // Start is called before the first frame update
    private bool explosive;
    private bool ready;
    void Start()
    {
        explosive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Priming()
    {
        ready = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(ready)
        {
            explosive = true;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!explosive)
        {
            return;
        }

        if(other.CompareTag("KMS") || other.CompareTag("VR")) {
            other.gameObject.GetComponent<PlayerBehaviour>().HitByCharge();
        }

        Destroy(gameObject);
    }
}
