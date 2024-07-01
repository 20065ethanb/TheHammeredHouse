using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked = false;

    public bool open = false;
    public float smooth = 1.0f;
    public float DoorOpenAngle = -90.0f;
    public float DoorCloseAngle = 0.0f;

    public AudioSource audioSource;
    public AudioClip openDoor, closeDoor;

    public GameObject pairedDoor;

    private BoxCollider boxCollider;
    private GameObject canvas;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        boxCollider = GetComponent<BoxCollider>();
        canvas = GameObject.Find("Canvas");
    }

    void Update()
    {
        var target = Quaternion.Euler(0, open ? DoorOpenAngle : DoorCloseAngle, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
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
        // If the door is paired toggle that door
        if (pairedDoor != null) pairedDoor.GetComponent<Door>().SetState(!open);
        // Toggles the door state
        SetState(!open);
    }

    public void SetState(bool state)
    {
        if (!(locked || open == state))
        {
            open = state;
            // if the door is open disable the box collider
            boxCollider.enabled = !open;
            // Play sound
            audioSource.clip = open ? openDoor : closeDoor;
            audioSource.Play();
        }
    }
}
