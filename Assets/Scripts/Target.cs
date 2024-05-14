using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public string incorrectToolMessage = "Wrong Tool";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectInteraction(bool valid)
    {
        if (valid)
        {
            // Do something
        }
        else
        {
            // Send incorrect message
        }
    }
}
