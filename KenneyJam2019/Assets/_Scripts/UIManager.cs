using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject GameSetupPanel;
    public GameObject OptionsPanel;
    public GameObject CreditsPanel;
    public Text NumberOfNpcsValueLabel;
    public InputField NameField;
    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        MainMenuPanel.SetActive(true);
        GameSetupPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Main menu

    public void SetupGame()
    {
        MainMenuPanel.SetActive(false);
        GameSetupPanel.SetActive(true);
    }

    public void ShowOptions()
    {
        MainMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        MainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    // Setup Game

    public void SetNumberOfNpcs(float number)
    {
        NumberOfNpcsValueLabel.text = ((int)number).ToString();
    }

    public void StartGame()
    {
        int numberOfNpcs = int.Parse(NumberOfNpcsValueLabel.text);
        string playerName = NameField.text;
        //Application.LoadLevel(Application.loadedLevel);
    }

    public void BackFromGameSetup()
    {
        MainMenuPanel.SetActive(true);
        GameSetupPanel.SetActive(false);
    }

    // Options

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void BackFromOptions()
    {
        MainMenuPanel.SetActive(true);
        OptionsPanel.SetActive(false);
    }

    // Credits

    public void BackFromCredits()
    {
        MainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);
    }
}

