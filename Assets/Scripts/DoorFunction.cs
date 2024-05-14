using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFunction : MonoBehaviour
{
    public bool locked = false;
    private GameObject hinge;
    private Animator hingeAnimator;
    private bool open = false;

    private void Start()
    {
        hinge = transform.parent.gameObject;
        hingeAnimator = hinge.GetComponent<Animator>();
    }

    public void PlayerInteract()
    {
        if (locked)
        {
            // Tell player that door is locked
        }
        else
        {
            open = !open;
            hingeAnimator.SetBool("Open", open);
        }
    }
}
