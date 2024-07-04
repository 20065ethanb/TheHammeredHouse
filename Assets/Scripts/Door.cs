using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<GameObject> doorLocks;

    public bool open = false;
    public float smooth = 1.0f;
    public float DoorOpenAngle = -90.0f;
    public float DoorCloseAngle = 0.0f;

    private AudioSource audioSource;
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
        Quaternion doorRotation = Quaternion.Euler(0, open ? DoorOpenAngle : DoorCloseAngle, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, doorRotation, Time.deltaTime * 5 * smooth);
    }

    public void PlayerInteract()
    {
        CheckLocks();
        if (doorLocks.Count == 0)
        {
            Interact();
        }
        else
        {
            // Tell player that door has a lock
            canvas.GetComponent<UI>().flashMessage("Door is Locked!", 2.0f);
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
        CheckLocks();
        if (doorLocks.Count == 0 && open != state)
        {
            open = state;
            // if the door is open disable the box collider
            boxCollider.enabled = !open;
            // Play sound
            audioSource.clip = open ? openDoor : closeDoor;
            audioSource.Play();
        }
    }

    private void CheckLocks()
    {
        // Removes empty spaces from the locks list
        for (int i = doorLocks.Count - 1; i >= 0; i--)
        {
            if (doorLocks[i] == null)
                doorLocks.RemoveAt(i);
        }
    }
}
