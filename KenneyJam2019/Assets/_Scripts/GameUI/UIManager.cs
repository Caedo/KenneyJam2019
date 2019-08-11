using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public RaceData raceData;
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject gameSetupPanel;
    public GameObject highscoresPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;

    [Header("Setup Panel")]
    public Text numberOfNpcsValueLabel;
    public Slider npcSlider;
    public InputField nameField;
    public Dropdown colorDropdown;
    public Text numberOfLapsValueLabel;
    public Slider lapsSlider;
    public AudioMixer mixer;

[   Header("Highscores Panel")]
    public Text highscoresEntriesLabel;   

    [Header("Transform targets")]
    public Camera mainCamera;
    public Transform mainTransform;
    public Transform startTransform;
    public Transform highscoresTransform;
    public Transform creditsTransform;
    public Transform optionsTransform;

    private readonly Dictionary<string, Color> ColorDictionary = new Dictionary<string, Color>() { { "Black", Color.black }, { "White", Color.white }, { "Red", Color.red }, { "Green", Color.green }, { "Blue", Color.blue }, { "Yellow", Color.yellow }, { "Cyan", Color.cyan }, { "Magenta", Color.magenta }, { "Duck", new Color(0.5450f, 0.2705f, 0.0745f, 1f) }
    };

    Transform target;

    // Start is called before the first frame update
    void Start() {
        mainMenuPanel.SetActive(true);
        gameSetupPanel.SetActive(false);
        highscoresPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
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
        mainMenuPanel.SetActive(false);
        gameSetupPanel.SetActive(true);

        target = startTransform;
    }

    public void ShowHighscores() {
        mainMenuPanel.SetActive(false);
        highscoresPanel.SetActive(true);

        SetHighscores();

        target = highscoresTransform;
    }

    public void ShowOptions() {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);

        target = optionsTransform;
    }

    public void ShowCredits() {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);

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
        numberOfNpcsValueLabel.text = ((int) number).ToString();
    }

    public void SetNumberOfLaps(float number) {
        numberOfLapsValueLabel.text = ((int) number).ToString();
    }

    public void StartGame() {
        string color = colorDropdown.options[colorDropdown.value].text;
        Color playerColor = ColorDictionary[color];

        PlayerData playerData = new PlayerData() {
            name = nameField.text,
            steerByAI = false,
            color = playerColor,
        };
        raceData.lapCount = (int) lapsSlider.value;

        raceData.CreateData((int) npcSlider.value, playerData);

        SceneManager.LoadScene("RaceTest");
    }

    public void BackFromGameSetup() {
        mainMenuPanel.SetActive(true);
        gameSetupPanel.SetActive(false);

        target = mainTransform;
    }

    // Highscores

    public void SetHighscores() {
        StringBuilder builder = new StringBuilder();
        for(int i = 0; i < 5; i++)
        {
            if(PlayerPrefs.HasKey("Highscore_time_" + i.ToString()))
            {
                float time = PlayerPrefs.GetFloat("Highscore_time_" + i.ToString());
                string name = PlayerPrefs.GetString("Highscore_name_" + i.ToString());
                builder.AppendFormat("{0}. {1} - {2:00.00} s\n", i + 1, name, time);
            }
        }

        highscoresEntriesLabel.text = builder.ToString();
    }

    public void BackFromHighscores() {
        mainMenuPanel.SetActive(true);
        highscoresPanel.SetActive(false);

        target = mainTransform;
    }

    // Options

    public void SetLevel(float sliderValue) {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void BackFromOptions() {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);

        target = mainTransform;
    }

    // Credits

    public void BackFromCredits() {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);

        target = mainTransform;
    }
}