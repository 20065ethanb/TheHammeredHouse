using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBox : MonoBehaviour
{
    public GameObject mainSwitch;

    private bool mainSwitchState = true;
    public float mainLeverOnAngle = 2;
    public float mainLeverOffAngle = 128;

    public List<GameObject> switches;

    private List<bool> switchStates = new List<bool>();
    public float switchOnPosition = -0.07f;
    public float switchOffPosition = 0.07f;

    public float smooth = 1.0f;

    public List<GameObject> lasers;

    private AudioSource audioSource;
    public AudioClip mainSound, secondSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        while (switchStates.Count < switches.Count)
        {
            switchStates.Add(Random.value > 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion mainSwitchRotation = Quaternion.Euler(mainSwitchState ? mainLeverOnAngle : mainLeverOffAngle, 0, 0);
        mainSwitch.transform.GetChild(0).localRotation = Quaternion.Slerp(mainSwitch.transform.GetChild(0).localRotation, mainSwitchRotation, Time.deltaTime * 5 * smooth);

        for (int i = 0; i < switches.Count; i++)
        {
            GameObject slider = switches[i].transform.GetChild(0).gameObject;
            Vector3 sliderPos = new Vector3(switchStates[i] ? switchOnPosition : switchOffPosition, 0, 0.1278f);
            slider.transform.localPosition = Vector3.MoveTowards(slider.transform.localPosition, sliderPos, Time.deltaTime * 2 * smooth);
        }

        for (int i = 0; i < lasers.Count; i++)
        {
            lasers[i].SetActive(mainSwitchState);
        }
    }

    public void flipSwitch(GameObject s)
    {
        if (s.Equals(mainSwitch))
        {
            mainSwitchState = !mainSwitchState;
            // Play sound
            audioSource.PlayOneShot(mainSound);
        }
        else if (switches.Contains(s))
        {
            int i = switches.IndexOf(s);
            switchStates[i] = !switchStates[i];
            // Play sound
            audioSource.PlayOneShot(secondSound);
        }
    }
}
