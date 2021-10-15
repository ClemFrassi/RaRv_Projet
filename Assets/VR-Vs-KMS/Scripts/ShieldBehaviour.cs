using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private float scaleX;
    private float scaleY;
    private float scaleZ;
    private int count;
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
        Debug.Log("HIT");
        if (count >= 5)
        {
            count = 0;
            Destroy();
            
        } else
        {
            count++;
            ReduceShield();
        }
    }

    private void Destroy()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }

    public void Respawn()
    {
        transform.localScale = new Vector3(scaleX , scaleY, scaleZ);
    }

    private void ReduceShield()
    {
        Debug.Log("REDUCE");
        transform.localScale = new Vector3(scaleX - count * 0.1f, scaleY, scaleZ - count * 0.1f);
        Debug.Log("REDUCED");
    }
}
