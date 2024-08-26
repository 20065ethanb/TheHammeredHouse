using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public List<TargetType> type;

    private Collider objectCollider;
    private Rigidbody objectRigidbody;

    private AudioSource audioSource;
    public AudioClip useSound;

    // Start is called before the first frame update
    void Start()
    {
        objectCollider = GetComponent<Collider>();
        objectRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PickedUp()
    {
        // Turns off gravity and collider when held
        objectCollider.enabled = false;
        objectRigidbody.useGravity = false;
    }

    public void Dropped()
    {
        // Enables gravity, collider and sets object scale
        objectCollider.enabled = true;
        objectRigidbody.useGravity = true;
        transform.localScale = Vector3.one;
    }

    public void Use(GameObject objectHit)
    {
        // Checks if object is taregt
        Target target = objectHit.GetComponent<Target>();
        if (target != null)
        {
            // Checks if it's valid to use the tool on the object
            bool validUse = type.Contains(target.type) || type.Contains(TargetType.All);
            target.ObjectInteraction(validUse);
            if (validUse)
                audioSource.PlayOneShot(useSound);
        }
    }
}
