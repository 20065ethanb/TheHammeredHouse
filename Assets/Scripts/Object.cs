using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{

    private Rigidbody rigidbody;
    public bool isTool;
    public float rechargeTime = 0.5f;
    public GameObject target;
    private Target targetScript;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        targetScript = target.GetComponent<Target>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PickedUp()
    {
        // Something happens when the object is picked up
    }

    public void Dropped()
    {
        // Something happens when the object is dropped
    }

    public void Use(GameObject objectHit)
    {
        targetScript.ObjectInteraction(objectHit == target);
    }
}
