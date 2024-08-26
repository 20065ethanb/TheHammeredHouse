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
        GameObject canvas = transform.Find("Canvas").gameObject;
        TextMeshProUGUI[] text = canvas.GetComponentsInChildren<TextMeshProUGUI>();

        int digits = 4;
        int max = (int)Mathf.Pow(10, digits);

        GameObject escapeObjects = GameObject.Find("Escape");
        Keypad[] keyPads = escapeObjects.GetComponentsInChildren<Keypad>();

        foreach (TextMeshProUGUI word in text)
        {
            int randomNumber = Random.Range(0, max);

            foreach (Keypad keypad in keyPads)
            {
                if (keypad.getKey() == 0 || Random.value > 0.5f)
                    keypad.setKey(randomNumber);
            }

            word.text = randomNumber.ToString();
        }
    }
}
