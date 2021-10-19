using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private float scaleX;
    private float scaleY;
    private float scaleZ;
    private int count;

    public VR_Overlay Overlay;
    void Start()
    {
        count = 0;
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        scaleZ = transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        if (photonView.IsMine)
        {
            Debug.Log("HIT");
            if (count >= 4)
            {
                count = 0;
                Destroy();

            }
            else
            {
                count++;
                ReduceShield();
            }
        }
        
       
    }

    private void Destroy()
    {
        transform.localScale = new Vector3(0, 0, 0);
        Overlay.SetShieldValue(0);
    }

    public void Repair()
    {
        transform.localScale = new Vector3(scaleX , scaleY, scaleZ);
        Overlay.ResetShield();
    }

    private void ReduceShield()
    {
        transform.localScale = new Vector3(scaleX - count * 0.1f, scaleY, scaleZ - count * 0.1f);
        Overlay.SetShieldValue(5 - count + 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;

        if(hit.CompareTag("Antiviral"))
        {
            Hit();
            Destroy(hit);
        }
    }
}
