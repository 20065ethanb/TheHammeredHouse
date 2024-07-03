using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLever : MonoBehaviour
{
    public bool switchState = true;
    public float mainLeverOnAngle = 2;
    public float mainLeverOffAngle = 128;

    public float smooth = 1.0f;

    public List<GameObject> lasers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion target = Quaternion.Euler(switchState ? mainLeverOnAngle : mainLeverOffAngle, 0, 0);
        transform.GetChild(0).localRotation = Quaternion.Slerp(transform.GetChild(0).localRotation, target, Time.deltaTime * 5 * smooth);

        for (int i = 0; i < lasers.Count; i++)
        {
            lasers[i].SetActive(switchState);
        }
    }

    public void flipSwitch()
    {
        switchState = !switchState;
    }
}
