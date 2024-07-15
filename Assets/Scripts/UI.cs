using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject messageText;
    public float messageTime = 0;

    private GameObject startMenu;
    private GameObject gameUI;

    private GameObject staminaBar;
    private GameObject crosshairOn;
    private GameObject crosshairOff;
    private GameObject playerGameObject;


    // Start is called before the first frame update
    void Start()
    {
        // Get objects
        startMenu = transform.Find("StartMenu").gameObject;
        gameUI = transform.Find("GameUI").gameObject;

        staminaBar = gameUI.transform.Find("Stamina Bar").gameObject;
        crosshairOn = gameUI.transform.Find("Crosshair_On").gameObject;
        crosshairOff = gameUI.transform.Find("Crosshair_Off").gameObject;

        playerGameObject = GameObject.Find("PlayerCharacter");
        staminaBar.GetComponent<Slider>().maxValue = playerGameObject.GetComponent<PlayerController>().MAX_STAMINA;

        // Shows start menu
        startMenu.SetActive(true);
        gameUI.SetActive(false);

        // Pause time
        Time.timeScale = 0f;

        // Disable inputs
        playerGameObject.GetComponent<Inputs>().SetCursorState(false);
        playerGameObject.GetComponent<PlayerController>().enabled = false;
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

    public void BeguinGame()
    {
        // Hides menu
        startMenu.SetActive(false);
        gameUI.SetActive(true);
        // Enables time
        Time.timeScale = 1f;
        // Enables inputs
        playerGameObject.GetComponent<Inputs>().SetCursorState(true);
        playerGameObject.GetComponent<PlayerController>().enabled = true;
        // Shows text
        flashMessage("Escape the House!", 3);
    }

    public void crosshair(bool on)
    {
        crosshairOn.SetActive(on);
        crosshairOff.SetActive(!on);
    }

    public void flashMessage(string message, float showTime)
    {
        messageText.GetComponent<TextMeshProUGUI>().text = message;
        messageText.SetActive(true);
        messageTime = showTime;
    }
}
