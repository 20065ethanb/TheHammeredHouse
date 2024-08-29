using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Paper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Gets text (on the paper)
        GameObject canvas = transform.Find("Canvas").gameObject;
        TextMeshProUGUI[] text = canvas.GetComponentsInChildren<TextMeshProUGUI>();

        // Calculate number generator parameters
        int digits = 4;
        int max = (int)Mathf.Pow(10, digits);

        // Gets keypads
        GameObject escapeObjects = GameObject.Find("Escape");
        Keypad[] keyPads = escapeObjects.GetComponentsInChildren<Keypad>();

        foreach (TextMeshProUGUI word in text)
        {
            // create random number
            int randomNumber = Random.Range(0, max);

            // has a chance to set a keypad to that number
            foreach (Keypad keypad in keyPads)
            {
                if (keypad.getKey() == 0 || Random.value > 0.5f)
                    keypad.setKey(randomNumber);
            }

            // set text to show number
            word.text = randomNumber.ToString();
        }
    }
}
