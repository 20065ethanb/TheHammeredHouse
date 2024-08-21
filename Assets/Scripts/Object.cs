using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public List<TargetType> type;

    private AudioSource audioSource;
    public AudioClip useSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        Target target = objectHit.GetComponent<Target>();
        if (target != null)
        {
            bool validUse = type.Contains(target.type);
            target.ObjectInteraction(validUse);
            if (validUse)
            {
                audioSource.clip = useSound;
                audioSource.Play();
            }
        }
    }
}
