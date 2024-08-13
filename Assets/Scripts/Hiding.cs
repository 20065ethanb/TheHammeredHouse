using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : MonoBehaviour
{
    public GameObject hideText, stopHidingText;
    public GameObject visablePlayer, hidenPlayer;
    bool interactable, hiding;

    void Start() // Interaction option starts off hiden
    {
        interactable = false;
        hiding = false;
    }
    void OnTriggerStay(Collider other) // Interaction option is true if camera is looking at it 
    {
        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(true);
            interactable = true;
        }
    }
    void OnTriggerExit(Collider other) // Turns option off when not looked at
    {
        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(false);
            interactable = false;
        }
    }
    void Update()
    {
        if (interactable == true)
        {
            if (Input.GetKeyDown(KeyCode.E)) // E to hide
            {
                hideText.SetActive(false);
                hidenPlayer.SetActive(true);
                stopHidingText.SetActive(true); // When hiding is equal to ture then visablePlayer will equal false
                hiding = true;
                visablePlayer.SetActive(false);
                interactable = false;
            }
        }
        if (hiding == true)
        {
            if (Input.GetKeyDown(KeyCode.Q)) // Q to leave hiding spot
            {
                stopHidingText.SetActive(false);
                visablePlayer.SetActive(true);
                hidenPlayer.SetActive(false);
                hiding = false;
            }
        }
    }
}
