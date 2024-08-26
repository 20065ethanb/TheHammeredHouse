using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Keypad : MonoBehaviour
{
    private int keypadCombo = 0;

    private string accessGrantedText = "Granted";
    private string accessDeniedText = "Denied";

    private float displayResultTime = 1f;
    private float screenIntensity = 2.2f;
    
    private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); //orangy
    private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); //red
    private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); //greenish

    private AudioSource audioSource;
    public AudioClip buttonClicked;
    public AudioClip accessDenied;
    public AudioClip accessGranted;

    private Renderer panelMesh;
    private TMP_Text keypadDisplayText;

    private string currentInput;
    private bool displayingResult = false;
    private bool accessWasGranted = false;

    public GameObject FrontDoorLock;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Transform visuals = transform.Find("KeypadVisuals");
        panelMesh = visuals.Find("panel").GetComponent<Renderer>();
        keypadDisplayText = visuals.Find("DisplayCanvas").GetComponentInChildren<TMP_Text>();

        ClearInput();
        panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
    }


    //Gets value from pressedbutton
    public void AddInput(string input)
    {
        audioSource.PlayOneShot(buttonClicked);
        if (displayingResult || accessWasGranted) return;
        switch (input)
        {
            case "enter":
                CheckCombo();
                break;
            default:
                if (currentInput != null && currentInput.Length == 9) // 9 max passcode size 
                {
                    return;
                }
                currentInput += input;
                keypadDisplayText.text = currentInput;
                break;
        }

    }
    public void CheckCombo()
    {
        if (int.TryParse(currentInput, out var currentKombo))
        {
            bool granted = currentKombo == keypadCombo;
            if (!displayingResult)
            {
                StartCoroutine(DisplayResultRoutine(granted));
            }
        }
        else
        {
            Debug.LogWarning("Couldn't process input for some reason..");
        }

    }

    //mainly for animations 
    private IEnumerator DisplayResultRoutine(bool granted)
    {
        displayingResult = true;

        if (granted) AccessGranted();
        else AccessDenied();

        yield return new WaitForSeconds(displayResultTime);
        displayingResult = false;
        if (granted) yield break;
        ClearInput();
        panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);

    }

    private void AccessDenied()
    {
        keypadDisplayText.text = accessDeniedText;
        panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
        audioSource.PlayOneShot(accessDenied);
    }

    private void ClearInput()
    {
        currentInput = "";
        keypadDisplayText.text = currentInput;
    }

    private void AccessGranted()
    {
        accessWasGranted = true;
        keypadDisplayText.text = accessGrantedText;
        panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
        audioSource.PlayOneShot(accessGranted);
        Destroy(FrontDoorLock);
    }

    public int getKey()
    { return keypadCombo; }

    public void setKey(int key)
    { keypadCombo = key; }
}