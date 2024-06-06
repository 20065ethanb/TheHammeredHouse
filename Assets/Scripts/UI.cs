using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject messageText;
    public float messageTime = 0;

    public GameObject staminaBar;
    private GameObject playerGameObject;


    // Start is called before the first frame update
    void Start()
    {
        playerGameObject = GameObject.Find("PlayerCharacter");
        staminaBar.GetComponent<Slider>().maxValue = playerGameObject.GetComponent<PlayerController>().MAX_STAMINA;
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.GetComponent<Slider>().value = playerGameObject.GetComponent<PlayerController>().stamina;

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
