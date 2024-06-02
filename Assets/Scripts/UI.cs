using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject messageText;
    public float messageTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (messageTime > 0)
        {
            messageTime -= Time.deltaTime;
            if (messageTime <= 0)
            {
                messageTime = 0;
                messageText.SetActive(false);
            }
        }
    }

    public void flashMessage(string message, float showTime)
    {
        messageText.GetComponent<TextMeshProUGUI>().text = message;
        messageText.SetActive(true);
        messageTime = showTime;
    }
}
