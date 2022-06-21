using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//required for buttons and TextMeshPro
using TMPro; //required for TextMeshPro
using UnityEngine.SceneManagement; //required to transition between scenes

public class GameManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject hiScoreText;
    public GameObject GameOverText1;
    public GameObject GameOverText2;

    private List<Brick> bricks;
    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        PopulateBricks();
        if(GlobalManager.instance != null)
        {
            UpdateHighScore();
        }
    }
    void PopulateBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };

        bricks = new List<Brick>();
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                bricks.Add(brick);
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }
    void UpdateHighScore()
    {
        hiScoreText.GetComponent<TextMeshProUGUI>().text = GlobalManager.instance.hiScoreName + 
            " --- " + GlobalManager.instance.hiScore;
    }
    // Update is called once per frame
    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GlobalManager.instance.RestartGame();
            }
        }
    }
    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        CheckHiScore();
    }
    void CheckHiScore()
    {
        if (m_Points > GlobalManager.instance.hiScore)
        {
            GlobalManager.instance.hiScoreName = GlobalManager.instance.playerName;
            GlobalManager.instance.hiScore = m_Points;
            UpdateHighScore();
        }
    }
    public void GameOver()
    {
        foreach (Brick brick in bricks)
        {
            try
            {
                Destroy(brick.gameObject);
            }
            //bad programming practice, but in this case, it's easier than trying to keep track of each brick.
                //we're resetting the list anyway, so it's not a huge deal.
            catch
            { }
        }
        m_GameOver = true;
        GameOverText1.SetActive(true);
        GameOverText2.SetActive(true);
    }
    public void ReturnToMenu()
    {
        GlobalManager.instance.SaveHiScore();
        GlobalManager.instance.DestroySingleton();
        SceneManager.LoadScene("menu");
    }
}
