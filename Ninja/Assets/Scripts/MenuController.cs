using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public GameObject selectCharaterPanel, nameGame, playButton, 
        redButton, redLockButton, orangeButton, orangeLockButton,
        sound, mute, infoPanel;

    public static bool blue, violet, orange, red;

    int audioMute;

    public AudioSource audioSource;
    public Text scoreText;
    // Use this for initialization
    private int orangeUnlock = 0, redUnlock = 0;
    private const string ORANGE = "orange", RED = "red";
	void Start () {
        
        audioSource.enabled = true;
        audioSource.loop = true;
        IsGameStartedForTheFirstTime();
        SetSound();
        audioMute = GameMangerController.instance.GetMute();

    }
	
	// Update is called once per frame
	void Update () {
        scoreText.text = GameMangerController.instance.score.ToString();
        UnlockCharacter();
        AudioControl();
        SetSound();
        GameMangerController.instance.mute = audioMute;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

        }
        //SetSound();
        //Debug.Log("" + GameMangerController.instance.GetMute());
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
    public void PlayButton()
    {
        selectCharaterPanel.SetActive(true);
        nameGame.SetActive(false);
        playButton.SetActive(false);
    }
    public void BluePlayerButton()
    {
        blue = true;
        loadPlayScene();
        UnityEngine.Time.timeScale = 1f;
        violet = false;
        orange = false;
        red = false;
    }
    public void VioletPlayeButton()
    {
        violet = true;
        loadPlayScene();
        UnityEngine.Time.timeScale = 1f;
        blue = false;
        orange = false;
        red = false;
    }
    public void OrangePlayerButton()
    {
        orange = true;
        loadPlayScene();
        UnityEngine.Time.timeScale = 1f;
        blue = false;
        violet = false;
        red = false;
    }
    public void RedPlayerButton() {
        red = true;
        loadPlayScene();
        UnityEngine.Time.timeScale = 1f;
        blue = false;
        violet = false;
        orange = false;
    }
    private void SetSound()
    {
        if (audioMute == 1)
        {
            mute.SetActive(true);
            sound.SetActive(false);
        }
        else
        {
            mute.SetActive(false);
            sound.SetActive(true);
        }
    }
    public void TurnOnSound()
    {
        if(GameMangerController.instance.GetMute() == 1)
        {
            audioMute = 0;
            
        }
        
    }
    public void Mute()
    {
        if (GameMangerController.instance.GetMute() == 0)
        {
            audioMute = 1;

        }
    }
    private void UnlockCharacter()
    {
        if (GetOrangeUnlock() == 1)
        {
            orangeButton.SetActive(true);
            orangeLockButton.SetActive(false);
            //Debug.Log("Open Orange Player");
        }
        if (GetRedUnlock() == 1)
        {
            redButton.SetActive(true);
            redLockButton.SetActive(false);
            //Debug.Log("Open Red Player");
        }
    }
    public void OrangePlayerLockButton()
    {
        if (GameMangerController.instance.score < 200)
        {
            if (!infoPanel.activeSelf)
            {
                infoPanel.gameObject.SetActive(true);
            }
        }
        else
        {
            GameMangerController.instance.score -= 200;
            SetOrangeUnlock(1);
            
        }
    }
    public void RedPlayerLockButton()
    {
        if (GameMangerController.instance.score < 500)
        {
            if (!infoPanel.activeSelf)
            {
                infoPanel.gameObject.SetActive(true);
            }
        }
        else
        {
            GameMangerController.instance.score -= 500;
            SetRedUnlock(1);
            //Debug.Log("Open Orange Player");
        }
    }
    public void ExitInfoPanel()
    {
        if (infoPanel.activeSelf)
        {
            infoPanel.gameObject.SetActive(false);
        }
    }

    void IsGameStartedForTheFirstTime()
    {
        if (!PlayerPrefs.HasKey("IsGameStartedForTheFirstTime"))
        {
            PlayerPrefs.SetInt(ORANGE, 0);
            PlayerPrefs.SetInt(RED, 0);
            PlayerPrefs.SetInt("IsGameStartedForTheFirstTime", 0);
        }
    }
    void loadPlayScene()
    {
        if (GameMangerController.instance.GetLevel() == 0)
        {
            SceneManager.LoadScene("GamePlayLevel1");
        }
        else
        {
            string nameLevel = string.Concat("GamePlayLevel", GameMangerController.instance.GetLevel());
            SceneManager.LoadScene(nameLevel);
        }
    }
    void SetOrangeUnlock(int orange)
    {
        PlayerPrefs.SetInt(ORANGE, orange);
    }
    int GetOrangeUnlock()
    {
        return PlayerPrefs.GetInt(ORANGE);
    }
    void SetRedUnlock(int red)
    {
        PlayerPrefs.SetInt(RED, red);
    }
    int GetRedUnlock()
    {
        return PlayerPrefs.GetInt(RED);
    }

}
