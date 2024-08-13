using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour
{
    public Transform insidePosition;
    public Transform outsidePosition;

    private AudioSource audioSource;
    public AudioClip closetSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        // Play sound
        audioSource.clip = closetSound;
        audioSource.Play();
    }
}
