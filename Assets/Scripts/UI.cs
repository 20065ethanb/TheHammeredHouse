using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject messageText;
    public float messageTime = 0;

    public AudioClip startMusic, gameMusic;
    private AudioSource audioSource;

    private GameObject startMenu;
    private GameObject gameUI;
    private GameObject gameOverMenu;
    private GameObject youWinScreen;
    private GameObject controls;

    private GameObject staminaBar;
    private GameObject crosshairOn;
    private GameObject crosshairOff;
    private GameObject playerGameObject;

    private GameObject ramesisGameObject;

    private int difficultyValue = 1;
    private TextMeshProUGUI difficultyText;
    private GameObject difficultySlider;


    // Start is called before the first frame update
    void Start()
    {
        // Get objects
        startMenu = transform.Find("StartMenu").gameObject;
        gameUI = transform.Find("GameUI").gameObject;
        gameOverMenu = transform.Find("GameOverMenu").gameObject;
        youWinScreen = transform.Find("YouWinScreen").gameObject;
        controls = transform.Find("Controls").gameObject;

        staminaBar = gameUI.transform.Find("Stamina Bar").gameObject;
        crosshairOn = gameUI.transform.Find("Crosshair_On").gameObject;
        crosshairOff = gameUI.transform.Find("Crosshair_Off").gameObject;

        playerGameObject = GameObject.Find("PlayerCharacter");
        audioSource = playerGameObject.GetComponent<AudioSource>();

        ramesisGameObject = GameObject.Find("Ramesis");

        difficultyText = startMenu.transform.Find("DifficultyText").gameObject.GetComponent<TextMeshProUGUI>();
        difficultySlider = startMenu.transform.Find("DifficultySlider").gameObject;

        // Shows start menu
        MainMenu();

        // Pause time
        Time.timeScale = 0f;

        // Disable inputs
        playerGameObject.GetComponent<Inputs>().SetCursorState(false);
        playerGameObject.GetComponent<PlayerController>().enabled = false;

        // Music
        audioSource.clip = startMusic;
        audioSource.Play();

        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        UpdateSlider();
    }

    public void MainMenu()
    {
        startMenu.SetActive(true);
        gameUI.SetActive(false);
        gameOverMenu.SetActive(false);
        youWinScreen.SetActive(false);
        controls.SetActive(false);
    }

    public void Controls()
    {
        startMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void UpdateSlider()
    {
        // Gets value
        difficultyValue = Mathf.RoundToInt(difficultySlider.GetComponent<Slider>().value);
        Color colour;
        string difficulty;
        // Checks to see what diffculty it means
        if (difficultyValue == 0)
        {
            difficulty = "Easy";
            colour = Color.green;
        }
        else if (difficultyValue == 1)
        {
            difficulty = "Medium";
            colour = Color.yellow;
        }
        else
        {
            difficulty = "Hard";
            colour = Color.red;
        }

        // Change text and colours to suit the difficulty theme
        difficultyText.text = "Difficulty: " + difficulty;
        difficultySlider.transform.Find("Background").GetComponent<Image>().color = colour;
        difficultySlider.transform.Find("Fill Area").GetComponentInChildren<Image>().color = colour;

        // sets brightness
        RenderSettings.ambientIntensity = Mathf.Pow(0.5f, difficultyValue);
        RenderSettings.fogDensity = 0.0625f * Mathf.Pow(1.5f, difficultyValue);
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

        // Music
        audioSource.clip = gameMusic;
        audioSource.Play();

        // Shows text
        flashMessage("Escape the House!", 3);

        // sets player stamina
        playerGameObject.GetComponent<PlayerController>().max_stamina *= Mathf.Pow(0.7f, difficultyValue);
        playerGameObject.GetComponent<PlayerController>().stamina = playerGameObject.GetComponent<PlayerController>().max_stamina;
        staminaBar.GetComponent<Slider>().maxValue = playerGameObject.GetComponent<PlayerController>().max_stamina;

        // sets ramesis values
        ramesisGameObject.GetComponent<RamesisController>().walkSpeed *= Mathf.Pow(1.25f, difficultyValue);
        ramesisGameObject.GetComponent<RamesisController>().runSpeed *= Mathf.Pow(1.25f, difficultyValue);
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

    public void crosshair(bool on)
    {
        crosshairOn.SetActive(on);
        crosshairOff.SetActive(!on);
    }

    public void GameOver()
    {
        // Hides UI
        gameUI.SetActive(false);
        gameOverMenu.SetActive(true);

        // Pause time
        Time.timeScale = 0f;

        // Disable inputs
        playerGameObject.GetComponent<Inputs>().SetCursorState(false);
        playerGameObject.GetComponent<PlayerController>().enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void YouWin()
    {
        // Hides UI
        gameUI.SetActive(false);
        youWinScreen.SetActive(true);

        // Pause time
        Time.timeScale = 0f;

        // Disable inputs
        playerGameObject.GetComponent<Inputs>().SetCursorState(false);
        playerGameObject.GetComponent<PlayerController>().enabled = false;
    }
}
