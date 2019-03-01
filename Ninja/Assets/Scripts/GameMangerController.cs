using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMangerController : MonoBehaviour {
    public static GameMangerController instance;

    public int score = 0, mute = 0, level = 0;

    

    private const string SCORE = "Score", MUTE = "Mute", LEVEL = "Level";
	// Use this for initialization
	void Awake () {
        MakeSingleInstance();
        IsGameStartedForTheFirstTime();
        //SetScore(1000);
        score = GetScore();

        //level = GetLevel();
        SetLevel(0);
        
    }
	
    void MakeSingleInstance()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
	// Update is called once per frame
	void Update () {
        SetScore(score);
        SetMute(mute);
        SetLevel(level);
        //Debug.Log("Mute in GM" + mute);
        //Debug.Log("" + GetLevel());
    }
    void IsGameStartedForTheFirstTime()
    {
        if (!PlayerPrefs.HasKey("IsGameStartedForTheFirstTime"))
        {
            PlayerPrefs.SetInt(SCORE, 0);
            PlayerPrefs.SetInt(MUTE, 0);
            PlayerPrefs.SetInt(LEVEL, 0);
            PlayerPrefs.SetInt("IsGameStartedForTheFirstTime", 0);
        }
    }
    public void SetScore(int score)
    {
        PlayerPrefs.SetInt(SCORE, score);
    }
    public int GetScore()
    {
        return PlayerPrefs.GetInt(SCORE);
    }
    public void SetMute(int mute)
    {
        PlayerPrefs.SetInt(MUTE, mute);
    }
    public int GetMute()
    {
        return PlayerPrefs.GetInt(MUTE);
    }
    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt(LEVEL, level);
    }
    public int GetLevel()
    {
        return PlayerPrefs.GetInt(LEVEL);
    }
}
