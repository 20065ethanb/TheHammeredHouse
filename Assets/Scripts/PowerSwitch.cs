using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitch : MonoBehaviour
{
    public bool switchState;
    public float switchOnPosition = -0.07f;
    public float switchOffPosition = 0.07f;

    public float smooth = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        switchState = Random.value > 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject slider = transform.GetChild(0).gameObject;
        Vector3 sPos = new Vector3(switchState ? switchOnPosition : switchOffPosition, 0, 0.1278f);
        slider.transform.localPosition = Vector3.MoveTowards(slider.transform.localPosition, sPos, Time.deltaTime * 5 * smooth);
    }

    public void flipSwitch()
    {
        switchState = !switchState;
    }
}
