using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public string incorrectToolMessage = "X requires a Y to Z!";
    private GameObject canvas;
    public TargetType type;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void ObjectInteraction(bool valid)
    {
        // Destroys target game objects
        if (valid)
        {
            Destroy(gameObject);
        }
        else
        {
            // wrong tool
            canvas.GetComponent<UI>().flashMessage(incorrectToolMessage, 2.0f);
        }
    }
}

public enum TargetType
{
    All,
    Wood,
    Rock,
    Lock,
    Metal,
    Glass,
    Rope
}
