using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public string incorrectToolMessage = "X requires a Y to Z!";
    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectInteraction(bool valid)
    {
        if (valid)
        {
            Destroy(gameObject);
        }
        else
        {
            canvas.GetComponent<UI>().flashMessage(incorrectToolMessage, 2.0f);
        }
    }
}
