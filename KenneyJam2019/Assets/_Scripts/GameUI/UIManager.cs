using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject MainMenuPanel;
    public GameObject GameSetupPanel;
    public GameObject OptionsPanel;
    public GameObject CreditsPanel;
    public Text NumberOfNpcsValueLabel;
    public Slider npcSlider;
    public InputField NameField;
    public Dropdown ColorDropdown;
    public Text NumberOfLapsValueLabel;
    public Slider lapsSlider;
    public AudioMixer mixer;

    public RaceData raceData;

    [Header("Transform targets")]
    public Camera mainCamera;
    public Transform mainTransform;
    public Transform startTransform;
    public Transform creditsTransform;
    public Transform optionsTransform;

    private readonly Dictionary<string, Color> ColorDictionary = new Dictionary<string, Color>() { { "Black", Color.black }, { "White", Color.white }, { "Red", Color.red }, { "Green", Color.green }, { "Blue", Color.blue }, { "Yellow", Color.yellow }, { "Cyan", Color.cyan }, { "Magenta", Color.magenta }, { "Duck", new Color(0.5450f, 0.2705f, 0.0745f, 1f) }
    };

    Transform target;

    // Start is called before the first frame update
    void Start() {
        MainMenuPanel.SetActive(true);
        GameSetupPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        Time.timeScale = 1;

        target = mainTransform;
    }

    // Update is called once per frame
    void Update() {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, target.position, 0.1f);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, target.rotation, 0.1f);
    }

    // Main menu

    public void SetupGame() {
        MainMenuPanel.SetActive(false);
        GameSetupPanel.SetActive(true);

        target = startTransform;
    }

    public void ShowOptions() {
        MainMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);

        target = optionsTransform;
    }

    public void ShowCredits() {
        MainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);

        target = creditsTransform;
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

    public void SetNumberOfLaps(float number) {
        NumberOfLapsValueLabel.text = ((int) number).ToString();
    }

    public void StartGame() {
        // int numberOfNpcs = int.Parse(NumberOfNpcsValueLabel.text);
        // string playerName = NameField.text;
        string color = ColorDropdown.options[ColorDropdown.value].text;
        Color playerColor = ColorDictionary[color];
        // int numberOfLaps = int.Parse(NumberOfLapsValueLabel.text);

        PlayerData playerData = new PlayerData() {
            name = NameField.text,
            steerByAI = false,
            color = playerColor,
        };
        raceData.lapCount = (int) lapsSlider.value;

        raceData.CreateData((int) npcSlider.value, playerData);

        SceneManager.LoadScene("RaceTest");
        //Application.LoadLevel(Application.loadedLevel);
    }

    public void BackFromGameSetup() {
        MainMenuPanel.SetActive(true);
        GameSetupPanel.SetActive(false);

        target = mainTransform;
    }

    // Options

    public void SetLevel(float sliderValue) {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void BackFromOptions() {
        MainMenuPanel.SetActive(true);
        OptionsPanel.SetActive(false);

        target = mainTransform;
    }

    // Credits

    public void BackFromCredits() {
        MainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);

        target = mainTransform;
    }
}