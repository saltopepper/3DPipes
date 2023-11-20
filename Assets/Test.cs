using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.up);
        Debug.Log(transform.right); Debug.Log(transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
