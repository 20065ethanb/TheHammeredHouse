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
        UpdateChildren();
    }

    // Update is called once per frame
    void Update()
    {
        if (ropeLock == null && !visable)
        {
            visable = true;
            UpdateChildren();
        }
    }

    private void UpdateChildren()
    {
        MeshRenderer[] children = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer child in children)
            child.enabled = visable;
    }
}
