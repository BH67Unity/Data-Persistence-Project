using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//required for buttons and TextMeshPro
using System.IO;//required for JSON serialization
using UnityEngine.SceneManagement;//required for scene transitions
using UnityEditor;//required for conditional compiling of exit functionality
using TMPro; //required for TextMeshPro
[DefaultExecutionOrder(1000)]//because globabl UI is involved, this ensures the script loads last

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager instance;

    public GameObject hiScoreText;
    public TMP_InputField playerNameInput;
    public string playerName;
    public GameObject startButton;
    public GameObject exitButton;
    public GameObject resetHiScoreButton;

    public int hiScore;
    public string hiScoreName;

    void IsDuplicateSingleton()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
        DontDestroyOnLoad(instance);
    }
    // Start is called before the first frame update
    void Start()
    {
        //apply singleton pattern to global manager
        IsDuplicateSingleton();

        //link to menu UI elements
        /*
        hiScoreText = GameObject.Find("HiScore Text");
        playerNameInput = GameObject.Find("Player InputField");
        startButton = GameObject.Find("Start Button");
        exitButton = GameObject.Find("Exit Button");
        resetHiScoreButton = GameObject.Find("Reset HiScore Button");
        */

        //wire up button click events
        startButton.GetComponent<Button>().onClick.AddListener(StartClicked);
        exitButton.GetComponent<Button>().onClick.AddListener(ExitClicked);
        resetHiScoreButton.GetComponent<Button>().onClick.AddListener(ResetClicked);

        //update HiScore text
        LoadHiScore();
        UpdateHiScore();
    }
    private void UpdateHiScore()
    {
        hiScoreText.GetComponent<TextMeshProUGUI>().text = "HiScore: " + hiScoreName + " --- " + hiScore;
    }
    void StartClicked()
    {
        //get player name
        playerName = playerNameInput.GetComponent<TMP_InputField>().text;
        
        //start game
        SceneManager.LoadScene("main");
    }
    void ExitClicked()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    void ResetClicked()
    {
        //###SDTZ
        UpdateHiScore();
        //###EDTZ
        /*
        hiScore = 0;
        hiScoreName = "Mr. Null";
        UpdateHiScore();
        SaveHiScore();
        */
    }
    [System.Serializable]
    class SaveData
    {
        public int hiScore;
        public string hiScoreName;
    }
    void SaveHiScore()
    {
        //create SaveData object
        SaveData hiScoreData = new SaveData();

        //populate with current hiScore
        hiScoreData.hiScoreName = hiScoreName;
        hiScoreData.hiScore = hiScore;

        //serialize to json file
        string jsonData = JsonUtility.ToJson(hiScoreData);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", jsonData);
    }
    void LoadHiScore()
    {
        string filepath = Application.persistentDataPath + "/savedata.json";
        if(File.Exists(filepath))
        {
            string jsonData = File.ReadAllText(filepath);
            SaveData hiScoreData = JsonUtility.FromJson<SaveData>(jsonData);
            hiScoreName = hiScoreData.hiScoreName;
            hiScore = hiScoreData.hiScore;
        }
        else
        {
            ResetClicked();
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
