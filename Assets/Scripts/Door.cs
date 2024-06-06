using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked = false;

    public bool open = false;
    public float smooth = 1.0f;
    float DoorOpenAngle = -90.0f;
    float DoorCloseAngle = 0.0f;

    public AudioSource asource;
    public AudioClip openDoor, closeDoor;

    private BoxCollider boxCollider;
    private GameObject canvas;

    private void Start()
    {
        asource = GetComponent<AudioSource>();

        boxCollider = GetComponent<BoxCollider>();
        canvas = GameObject.Find("Canvas");
    }

    void Update()
    {
        if (open)
        {
            var target = Quaternion.Euler(0, DoorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);

        }
        else
        {
            var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);

        }
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
            Interact();
        }
    }

    public void Interact()
    {
        // Toggles the door state
        open = !open;
        // if the door is open disable the box collider
        boxCollider.enabled = !open;
        // Play sound
        asource.clip = open ? openDoor : closeDoor;
        asource.Play();
    }
}
