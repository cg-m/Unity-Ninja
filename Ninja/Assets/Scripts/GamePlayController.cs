using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour {

    public static GamePlayController instance;
    [SerializeField]
    private GameObject pausePanel, gameOverPanel, pauseButton;
    [SerializeField]
    private GameObject bluePlayer, violetPlayer, orangePlayer, redPlayer;
    [SerializeField]
    private Button resumeGame, menuGame;
    [SerializeField]
    private Text scoreText;
    public AudioSource audioSource;

    public bool pauseGame, gameOver;
    // Use this for initialization
    private void Awake()
    {
        ChangeCharater();
        
    }
    void Start () {
        MakeInstance();
        audioSource.enabled = true;
        audioSource.loop = true;       
    }
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
	
	// Update is called once per frame
	void Update () {
        ChangeCharater();
        AudioControl();
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            PauseGame();
        }
        //Debug.Log("Diem: " + score);
        scoreText.text = GameMangerController.instance.score.ToString();

    }
    void AudioControl()
    {
        if (GameMangerController.instance.GetMute() == 0)
        {
            audioSource.enabled = true;
        }
        else
        {
            audioSource.enabled = false;
        }
    }
    public void PauseGame()
    {
        pauseGame = true;
        UnityEngine.Time.timeScale = 0f;
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        resumeGame.onClick.RemoveAllListeners();
        resumeGame.onClick.AddListener(() => ResumeGame());
        //menuGame.onClick.RemoveAllListeners();
       // menuGame.onClick.AddListener(() => BackToMenu());

    }
    public void ResumeGame()
    {
        pauseGame = false;
        UnityEngine.Time.timeScale = 1f;
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
        UnityEngine.Time.timeScale = 1f;
    }

    public void GameOver()
    {
        gameOver = true;
        UnityEngine.Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        pauseButton.SetActive(false);
    }
    public void BackToMenu()
    {
        Application.LoadLevel("MainMenu");
    }
    void ChangeCharater()
    {
        if (MenuController.blue)
        {
            if (violetPlayer.activeSelf)
            {
                violetPlayer.gameObject.SetActive(false);
            }
            bluePlayer.SetActive(true);

        }
         else if (MenuController.violet)
        {
            if (bluePlayer.activeSelf)
            {
                bluePlayer.gameObject.SetActive(false);
            }
            violetPlayer.SetActive(true);
        }else if (MenuController.orange)
        {
            orangePlayer.gameObject.SetActive(true);
        }else if (MenuController.red)
        {
            redPlayer.gameObject.SetActive(true);
        }
    }
    
}
