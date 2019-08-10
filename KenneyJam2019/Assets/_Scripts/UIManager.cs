using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public GameObject MainMenuPanel;
    public GameObject GameSetupPanel;
    public GameObject OptionsPanel;
    public GameObject CreditsPanel;
    public Text NumberOfNpcsValueLabel;
    public Slider NPCNumberSlider;
    public InputField NameField;
    public AudioMixer mixer;

    public RaceData raceData;

    // Start is called before the first frame update
    void Start() {
        MainMenuPanel.SetActive(true);
        GameSetupPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        Time.timeScale = 1;
    }

    // Main menu

    public void SetupGame() {
        MainMenuPanel.SetActive(false);
        GameSetupPanel.SetActive(true);
    }

    public void ShowOptions() {
        MainMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void ShowCredits() {
        MainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Setup Game

    public void SetNumberOfNpcs(float number) {
        NumberOfNpcsValueLabel.text = ((int) number).ToString();
    }

    public void StartGame() {
        PlayerData playerData = new PlayerData() {
            name = NameField.text,
            steerByAI = false,
        };

        raceData.CreateData((int) NPCNumberSlider.value, playerData);

        SceneManager.LoadScene("RaceTest");
    }

    public void BackFromGameSetup() {
        MainMenuPanel.SetActive(true);
        GameSetupPanel.SetActive(false);
    }

    // Options

    public void SetLevel(float sliderValue) {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void BackFromOptions() {
        MainMenuPanel.SetActive(true);
        OptionsPanel.SetActive(false);
    }

    // Credits

    public void BackFromCredits() {
        MainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);
    }
}