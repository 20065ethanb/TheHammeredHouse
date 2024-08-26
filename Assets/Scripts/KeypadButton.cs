using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    public string value;

    private float bttnspeed = 0.1f;
    private float moveDist = 0.0025f;
    private float buttonPressedTime = 0.1f;

    private Keypad keypad;

    void Start()
    {
        keypad = transform.parent.parent.GetComponentInParent<Keypad>();
    }


    public void PressButton()
    {
        if (!moving)
        {
            keypad.AddInput(value);
            StartCoroutine(MoveSmooth());
        }
    }
    private bool moving;

    private IEnumerator MoveSmooth()
    {

        moving = true;
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = transform.localPosition + new Vector3(0, 0, moveDist);

        float elapsedTime = 0;
        while (elapsedTime < bttnspeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / bttnspeed);

            transform.localPosition = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }
        transform.localPosition = endPos;
        yield return new WaitForSeconds(buttonPressedTime);
        startPos = transform.localPosition;
        endPos = transform.localPosition - new Vector3(0, 0, moveDist);

        elapsedTime = 0;
        while (elapsedTime < bttnspeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / bttnspeed);

            transform.localPosition = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }
        transform.localPosition = endPos;

        moving = false;
    }
}