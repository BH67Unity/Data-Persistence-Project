using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//required for buttons and TextMeshPro
using System.IO;//required for JSON serialization
using UnityEngine.SceneManagement;//required for scene transitions
using UnityEditor;//required for conditional compiling of exit functionality
[DefaultExecutionOrder(1000)]//because globabl UI is involved, this ensures the script loads last

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager instance;

    public GameObject playerNameInput;
    public string playerName;
    public GameObject startButton;
    public GameObject exitButton;
    public GameObject resetHiScoreButton;

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
        playerNameInput = GameObject.Find("Player InputField");
        startButton = GameObject.Find("Start Button");
        exitButton = GameObject.Find("Exit Button");
        resetHiScoreButton = GameObject.Find("Reset HiScore Button");

        //wire up button click events
        startButton.GetComponent<Button>().onClick.AddListener(StartClicked);
        exitButton.GetComponent<Button>().onClick.AddListener(ExitClicked);
        resetHiScoreButton.GetComponent<Button>().onClick.AddListener(ResetClicked);
    }
    void StartClicked()
    {
        //get player name
        playerName = playerNameInput.GetComponent<InputField>().text;
        
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

    }
    void SaveHiScore()
    {

    }
    void LoadHiScore()
    {

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
