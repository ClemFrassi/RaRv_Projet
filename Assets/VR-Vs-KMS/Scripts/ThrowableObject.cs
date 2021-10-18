using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    // Start is called before the first frame update
    private bool explosive;
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
        explosive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(explosive)
        {

        }
    }
}
