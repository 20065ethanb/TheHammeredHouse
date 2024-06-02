using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public bool isTool;
    //public float rechargeTime = 0.5f;
    public List<GameObject> targets;

    // Start is called before the first frame update
    void Start()
    {

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
        if (objectHit.GetComponent<Target>() != null)
            objectHit.GetComponent<Target>().ObjectInteraction(targets.Contains(objectHit));
    }
}
