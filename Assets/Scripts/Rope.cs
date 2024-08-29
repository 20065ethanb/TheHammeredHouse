using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public GameObject ropeLock;
    public bool visable = false;

    // Start is called before the first frame update
    void Start()
    {
        // Makes rope invisable
        UpdateChildren();
    }

    // Update is called once per frame
    void Update()
    {
        // Makes rope visable
        if (ropeLock == null && !visable)
        {
            visable = true;
            UpdateChildren();
        }
    }

    private void UpdateChildren()
    {
        // Updates rope visabltiy
        MeshRenderer[] children = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer child in children)
            child.enabled = visable;
    }
}
