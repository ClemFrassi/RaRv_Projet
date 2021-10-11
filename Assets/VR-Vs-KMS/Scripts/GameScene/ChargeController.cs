using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeController : MonoBehaviour
{
    // Start is called before the first frame update
    public Material green;
    public Material red;
    void Start()
    {
        Kill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTag(string tag)
    {
        gameObject.tag = tag;
        SetColor();
    }

    public void SetColor()
    {
        if(gameObject.CompareTag("Viral"))
        {
            gameObject.GetComponent<MeshRenderer>().material = red;
        } else if(gameObject.CompareTag("Antiviral"))
        {
            gameObject.GetComponent<MeshRenderer>().material = green;
        }
    }

    void Kill()
    {
        Destroy(gameObject, 5f);
    }
}
