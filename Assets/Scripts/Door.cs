using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked = false;
    private GameObject hinge;
    private Animator hingeAnimator;
    private bool open = false;

    private BoxCollider boxCollider;
    private GameObject canvas;

    private void Start()
    {
        hinge = transform.parent.gameObject;
        hingeAnimator = hinge.GetComponent<Animator>();

        boxCollider = GetComponent<BoxCollider>();
        canvas = GameObject.Find("Canvas");
    }

    public void PlayerInteract()
    {
        if (locked)
        {
            // Tell player that door is locked
            canvas.GetComponent<UI>().flashMessage("Door is Locked!", 2.0f);
        }
        else
        {
            // Toggles the door state
            open = !open;
            hingeAnimator.SetBool("Open", open);
            // if the door is open disable the box collider
            boxCollider.enabled = !open;
        }
    }
}
